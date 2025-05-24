using System.Collections.Generic;

namespace Battle
{
    public enum EffectPosType
    {
        //受击挂点
        Hit_Pos = 0,

        //自定义挂点
        Custom_Pos = 1,
    }

    public class CreateEffectInfo
    {
        //唯一 guid
        public int guid;

        //资源 id
        public int resId;

        //是否自动销毁
        public bool isAutoDestroy;

        //跟随 entity 的 guid
        public int followEntityGuid;

        //创建点位置类型
        public EffectPosType effectPosType;

        //创建点(位置类型为自定义类型)
        public Vector3 createPos;

        //buff 信息(如果是 buff 的话)
        public BuffEffectInfo buffInfo;

        //是否同步消息
        public bool isSync = true;

        // //相关的 entity guid 组，如 link 效果涉及到链接的实体组
        // public List<int> groupEntityGuidList;
    }

    public partial class SkillEffectMgr
    {
        Dictionary<int, SkillEffect> skillEffectDic;
        Battle battle;

        public void Init(Battle battle)
        {
            this.battle = battle;
            skillEffectDic = new Dictionary<int, SkillEffect>();
        }


        int maxGuid = 1;

        public int GenGuid()
        {
            return maxGuid++;
        }

        public void Update(float timeDelta)
        {
            foreach (var item in skillEffectDic)
            {
                var effect = item.Value;
                effect.Update(timeDelta);
            }

            //按顺序创建或者删除效果
            for (int i = 0; i < this.eftOpList.Count; i++)
            {
                var eftOp = this.eftOpList[i];
                if (eftOp.opType == EffectOperateType.Create)
                {
                    var eft = eftOp.opEft;
                    var buffInfo = eftOp.buffInfo;

                    if (buffInfo != null)
                    {
                        buffInfo.guid = eft.guid;
                    }

                    var createInfo = eft.ToCreateEffectInfo();

                    createInfo.buffInfo = buffInfo;
                    
                    this.skillEffectDic.Add(eft.guid, eft);
                    if (createInfo.buffInfo != null)
                    {
                        //Battle_Log.LogZxy("skillEffectDic add : " + createInfo.guid);
                        // Battle_Log.LogZxy(" eft.state : " +  eft.state);
                       
                    }

                    if (eft.state == SkillEffectState.InitFinish)
                    {
                        if (createInfo.isSync)
                        {
                            if (createInfo.buffInfo != null)
                            {
                                //Battle_Log.LogZxy("buff sync : " + createInfo.guid);
                            }

                            battle?.OnCreateSkillEffect(createInfo);
                        }
                        eft.Start();
                    }
                }
                else if (eftOp.opType == EffectOperateType.Delete)
                {
                    var effect = eftOp.opEft;
                    effect.End();
                    effect.Destroy();
                    skillEffectDic.Remove(effect.guid);

                    //展示效果不通知
                    var isNotify = !effect.isAutoDestroy; //!(effect is CalculateEffect);
                    if (isNotify)
                    {
                        this.battle.OnSkillEffectDestroy(effect.guid);
                    }
                }
            }
            
            this.eftOpList.Clear();
        }

        public void DeleteSkillEffect(SkillEffect skillEffect)
        {
            eftOpList.Add(new EffectOperate()
            {
                opType = EffectOperateType.Delete,
                opEft = skillEffect
            });
        }

        public void DeleteAllBuffsFromEntity(int entityGuid,bool isOnDead = false)
        {
            var entity = battle.FindEntity(entityGuid, true);
            if (entity != null)
            {
                var buffs = entity.GetBuffs();
                foreach (var item in buffs)
                {
                    var buff = item.Value;
                    if (isOnDead)
                    {
                        if (1 == buff.tableConfig.IsDeleteOnDead)
                        {
                            buff.ForceDelete();
                        }
                    }
                    else
                    {
                        buff.ForceDelete();
                    }
                }
            }
        }

        public void DeleteBuffFromEntity(int entityGuid, int effectConfigId)
        {
            var entity = battle.FindEntity(entityGuid);
            if (entity != null)
            {
                var buff = entity.GetBuffByConfigId(effectConfigId);
                if (buff != null)
                {
                    buff.ForceDelete();
                }
            }
        }
    }

    public class BuffEffectInfo
    {
        public int guid = 0;
        public int configId = 0;
        public int targetEntityGuid = 0;
        public int linkTargetEntityGuid = 0;
        public int currCDTime = 0;
        public int maxCDTime = 0;
        public int statckCount = 0;

        //public int iconResId;
    }
}