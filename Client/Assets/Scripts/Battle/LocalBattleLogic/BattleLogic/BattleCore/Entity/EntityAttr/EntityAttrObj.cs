using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battle
{
    //实体的一种属性对象 包含多个影响该属性的因素
    public class EntityAttrObj
    {
        public EntityAttrType attrType;
        public Dictionary<int, EntityAttrPart> attrPartDic;

        public void Init(EntityAttrType type)
        {
            attrType = type;
            attrPartDic = new Dictionary<int, EntityAttrPart>();
        }

        internal void Add(AttrOption _attrOption,Battle battle)
        {
            if (!this.attrPartDic.ContainsKey(_attrOption.guid))
            {
                EntityAttrPart part = new EntityAttrPart();
                part.Init(_attrOption,battle);
                this.attrPartDic.Add(_attrOption.guid, part);
            }
            else
            {
                //如果 guid 一样，那么暂时先作为替换来处理
                // Battle_Log.LogWarning("AttrOption : Add : the is exist : " + _attrOption.guid);
                var part = this.attrPartDic[_attrOption.guid];   
                var value = _attrOption.initValue;
                part.SetValue(value);


            }
        }

        public void Remove(int id)
        {
            if (this.attrPartDic.ContainsKey(id))
            {
                this.attrPartDic.Remove(id);
            }
            else
            {
                BattleLog.LogWarning("AttrOption : Add : the is not exist : " + id);
            }
        }

        public float GetTotalCalculateValue(bool isIncludeContinuous = true)
        {
            var sum = 0.0f;
            foreach (var kv in attrPartDic)
            {
                var item = kv.Value;
                bool isAdd = false;
                if (isIncludeContinuous)
                {
                    isAdd = true;
                }
                else
                {
                    if (!item.isContinuous)
                    {
                        isAdd = true;
                    }
                }
                if (isAdd)
                {
                    sum += item.value;
                }
            }

            return sum;
        }
    }
}