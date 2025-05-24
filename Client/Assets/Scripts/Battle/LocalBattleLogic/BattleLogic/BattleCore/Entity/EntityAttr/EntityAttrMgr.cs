using System;
using System.Collections.Generic;
using System.Linq;

namespace Battle
{
    public class EntityAttrCache
    {
        //总属性缓存（例如 ： 取 攻击力 总和 ， 这里不包括攻击力千分比加成）
        public float totalValue;
        public float totalValue_NoContinuous;

        //最终总属性缓存(这里是 攻击力 和 攻击力千分比加成 计算后的最终值 ，储存在固定属性中 ， 如果是千分比属性 那么就值计算千分比)
        public float finalValue;
        public float finalValue_NoContinuous;
    }

    public partial class EntityAttrMgr
    {
        Battle battle;
        BattleEntity battleEntity;

        //详细属性 dic
        public Dictionary<EntityAttrGroupType, EntityAttrGroup> attrGroupDic;

        //属性缓存 dic
        public Dictionary<EntityAttrType, EntityAttrCache> attrCacheDic;

        public void Init(BattleEntity battleEntity)
        {
            this.battleEntity = battleEntity;
            this.battle = this.battleEntity.GetBattle();

            attrGroupDic = new Dictionary<EntityAttrGroupType, EntityAttrGroup>();
            attrCacheDic = new Dictionary<EntityAttrType, EntityAttrCache>();

            InitAttrInfo();
        }

        //增加属性
        public void AddAttrValue(EntityAttrGroupType groupType,AttrOption attrOption)
        {
            EntityAttrGroup group = null;
            if (attrGroupDic.ContainsKey(groupType))
            {
                group = attrGroupDic[groupType];
            }
            else
            {
                group = new EntityAttrGroup();
                attrGroupDic.Add(groupType, group);
            }

            group.AddAttr(attrOption,this.battle);

            //计算增加属性的缓存
            CalculateAttrCache(attrOption.entityAttrType);
            
            //计算所有受该属性影响的持续属性
            CalculateContinuousAttr();
            
            //最后刷新下所有属性的缓存
            foreach (var attrObjKV in group.GetAttrObjs())
            {
                CalculateAttrCache(attrObjKV.Key);
            }
        }

        //计算属性 获得缓存 提高获取效率
        void CalculateAttrCache(EntityAttrType attrType)
        {
            //取出缓存
            EntityAttrCache attrCache = null;
            if (!attrCacheDic.ContainsKey(attrType))
            {
                attrCache = new EntityAttrCache();
                attrCacheDic.Add(attrType, attrCache);
            }
            else
            {
                attrCache = attrCacheDic[attrType];
            }

            //计算属性总值缓存
            var sum = 0.0f;
            var sum_NoContinuous = 0.0f;
            foreach (var item in attrGroupDic)
            {
                var _groupType = item.Key;
                var totalValue = GetTotalCalculateValue(_groupType, attrType,true);
                sum += totalValue;

                var totalValue2 = GetTotalCalculateValue(_groupType, attrType,false);
                sum_NoContinuous += totalValue2;
            }
            attrCache.totalValue = sum;
            attrCache.totalValue_NoContinuous = sum_NoContinuous;

            //计算最终值缓存
            var isPer = AttrHelper.IsPermillage(attrType);
            if (!isPer)
            {
                //固定属性
                attrCache.finalValue = CalculateFinalAttrValue(attrType,true);
                attrCache.finalValue_NoContinuous = CalculateFinalAttrValue(attrType,false);
                
                // var perType = AttrHelper.GetPermillageTypeByFixedType(attrType);
                //
                // attrCache.finalValue = CalculateFinalAttrValue(perType,true);
                // attrCache.finalValue_NoContinuous = CalculateFinalAttrValue(perType,false);
            }
            else
            {
                //千分比属性
                attrCache.finalValue = CalculateFinalAttrValue(attrType,true);
                attrCache.finalValue_NoContinuous = CalculateFinalAttrValue(attrType,false);
                
                var fixedType = AttrHelper.GetFixedTypeByPermillageType(attrType);
                EntityAttrCache attrCache2 = null;
                if (!attrCacheDic.ContainsKey(fixedType))
                {
                    attrCache2 = new EntityAttrCache();
                    attrCacheDic.Add(fixedType, attrCache2);
                }
                else
                {
                    attrCache2 = attrCacheDic[fixedType];
                }
                
                //千分比属性改变会影响最终值 因为现在最终只是储存在固定属性中 所以需要再刷新下固定属性最终缓存值
                attrCache2.finalValue = CalculateFinalAttrValue(fixedType,true);
                attrCache2.finalValue_NoContinuous = CalculateFinalAttrValue(fixedType,false);
                
            }
        }

        //获取属性总值（非缓存）
        private float
            GetTotalCalculateValue(EntityAttrGroupType groupType, EntityAttrType attrType,bool isIncludeContinuous = true)
        {
            EntityAttrGroup group = null;
            if (attrGroupDic.ContainsKey(groupType))
            {
                group = attrGroupDic[groupType];
                return group.GetTotalCalculateValue(attrType,isIncludeContinuous);
            }

            return 0;
        }


        //计算后的最终属性值
        float CalculateFinalAttrValue(EntityAttrType attrType,bool isIncludeContinuous = true)
        {
            var isPer = AttrHelper.IsPermillage(attrType);
            float fixedValue = 0;
            float resultValue = 0;
            float perValue = 0;
            
            if (!isPer)
            {
                //获取固定属性 (固定值 和 千分比 的结果)
                //固定值
                fixedValue = GetTotalAttrValue(attrType,isIncludeContinuous);
                //千分比
                EntityAttrType perType = AttrHelper.GetPermillageTypeByFixedType(attrType);
                perValue = GetTotalAttrValue(perType,isIncludeContinuous) / 1000.0f;
                resultValue = fixedValue + fixedValue * perValue;
                return resultValue;
               
            }
            else
            {
                //获取千分比属性
                perValue = GetTotalAttrValue(attrType,isIncludeContinuous);
                
                // //固定值
                // EntityAttrType fixedType = AttrHelper.GetFixedTypeByPermillageType(attrType);
                // fixedValue = GetTotalAttrValue(fixedType,isIncludeContinuous); 
                
                return perValue;
            }
            
            
        }

        //获得单一属性值的总和（缓存）
        float GetTotalAttrValue(EntityAttrType attrType,bool isIncludeContinuous = true)
        {
            var dic = attrCacheDic;

            //TODO : 减伤和增伤有待商榷
            if (dic.ContainsKey(attrType))
            {
                var cache = dic[attrType];
                if (isIncludeContinuous)
                {
                    return cache.totalValue;    
                }
                else
                {
                    return cache.totalValue_NoContinuous;
                }
            }

            return 0;
        }


        //获得计算后的最终属性值（缓存）
        public float GetFinalAttrValue(EntityAttrType attrType,bool isIncludeContinuous = true)
        {
            if (attrCacheDic.ContainsKey(attrType))
            {
                var cache = attrCacheDic[attrType];
                if (isIncludeContinuous)
                {
                    return cache.finalValue;
                }
                else
                {
                    return cache.finalValue_NoContinuous;
                }
            }

            return 0;
        }

        //获得所有属性的缓存
        public Dictionary<EntityAttrType, float> GetFinalAttrs()
        {
            Dictionary<EntityAttrType, float> dic = new Dictionary<EntityAttrType, float>();
            foreach (var kv in attrCacheDic)
            {
                dic.Add(kv.Key, kv.Value.finalValue);
            }

            return dic;
        }


        //移除属性
        public void RemoveAttrValue(EntityAttrGroupType groupType, EntityAttrType attrType,
            int id) //, bool isPermillage
        {
            EntityAttrGroup group = null;
            if (attrGroupDic.ContainsKey(groupType))
            {
                group = attrGroupDic[groupType];
                group.RemoveAttr(attrType, id);

                CalculateAttrCache(attrType);
                
                CalculateContinuousAttr();
                
                foreach (var attrObjKV in group.GetAttrObjs())
                {
                    CalculateAttrCache(attrObjKV.Key);
                }
                
            }
        }

        public void Release()
        {
        }
    }
}