using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UIElements;

namespace Battle
{
    public enum SkillAffectType
    {
        No = 0,
        Positive = 1,
        Negative = 2
    }


    public class BuffEffect : SkillEffect
    {
        //public Config.BuffEffect tableConfig;
        public IBuffEffect tableConfig;

        float currLastTime;
        float intervaleTime;

        //自身跟随的目标
        BattleEntity target;

        //链接 buff 的另一个目标 
        private BattleEntity linkTarget;

        public LinkGroupEffect linkGroupEft;

        //BuffType buffType;
        Battle battle;

        int currLayerCount = 1;


        public void InitLayerCount(int layerCount)
        {
            this.currLayerCount = layerCount;
        }


        public override void OnInit()
        {
            battle = this.context.battle;
            tableConfig = BattleConfigManager.Instance.GetById<IBuffEffect>(this.configId);

            ResetLastTimer();

            if (this.context.selectEntities.Count > 0)
            {
                target = this.context.selectEntities[0];
            }
            else
            {
                BattleLog.LogError(
                    "BuffEffect : the this.context.selectEntities of count is 0 : in buff , config : " +
                    tableConfig.Id);
            }

            if (null == target)
            {
                BattleLog.LogError("BuffEffect : the target is null , config : " + tableConfig.Id);
                return;
            }

            if (target.IsDead())
            {
                this.SetWillEndState();
                return;
            }

            linkGroupEft = this.context.linkGroupEffect;

            target.AddBuff(this);

            //异常状态
            foreach (var stateType in this.tableConfig.AbnormalStateTypeList)
            {
                target.AddAbnormalState((EntityAbnormalStateType)stateType);
            }

            // Battle_Log.LogZxy("buff init : " + this.guid);
        }

        public override CreateEffectInfo ToCreateEffectInfo()
        {
            var followEntityGuid = 0;
            if (context.selectEntities.Count > 0)
            {
                followEntityGuid = context.selectEntities[0].guid;
            }
            else
            {
                BattleLog.LogError(" : the this.context.selectEntities of count is 0 : in buff , config : " +
                                   configId);
                return null;
            }

            CreateEffectInfo createInfo = new CreateEffectInfo();
            createInfo.guid = guid;
            createInfo.resId = tableConfig.EffectResId;
            createInfo.effectPosType = EffectPosType.Custom_Pos;
            // if (createInfo.effectPosType == EffectPosType.Custom_Pos)
            // {
            //     createInfo.createPos;
            // }

            createInfo.followEntityGuid = followEntityGuid;
            createInfo.isAutoDestroy = isAutoDestroy;

            // if (buffInfo != null)
            // {
            //     buffInfo.guid = guid;
            // }
            //
            // createInfo.buffInfo = buffInfo;

            return createInfo;
        }

        public override void OnStart()
        {
            base.OnStart();

            // Battle_Log.LogZxy("buff OnStart : " + this.guid);
            // SkillEffectContext context = new SkillEffectContext()
            // {
            //     selectEntities = new List<BattleEntity>() { target },
            //     battle = this.context.battle,
            //     fromSkill = this.context.fromSkill
            // };


            SkillEffectContext context = base.context.Copy();
            context.selectEntities = new List<BattleEntity>() { target };

            //触发 start effect
            TriggerStartEffect(context);

            //ResetIntervaleTime();

            //间隔的第一下直接释放
            intervaleTime = 0;

            //属性改变
            CalculateAttrValue();

            //普通攻击伤害附加
            HandleNormalAttackAddedDamage();
        }

        public void HandleNormalAttackAddedDamage()
        {
            foreach (var eftId in this.tableConfig.NormalAttackAddedEffectIds)
            {
                target.AddNormalAttackAddedDamage(this.guid, DamageCalculate.Create(target.guid, eftId));
            }
        }

        public void RemoveNormalAttackAddedDamage()
        {
            target?.RemoveNormalAttackAddedDamage(this.guid);
        }


        List<AttrOption> buffAttrs = new List<AttrOption>();

        public void ResetLastTimer()
        {
            currLastTime = tableConfig.LastTime / 1000.0f;
            if (currLastTime <= 0)
            {
                currLastTime = 999999999;
            }
        }

        public float GetMaxLastTime()
        {
            if (0 == this.tableConfig.LastTime)
            {
                return -1;
            }

            return tableConfig.LastTime / 1000.0f;
            ;
        }

        public float GetCurrLastTime()
        {
            return currLastTime;
        }

        public int GetCurrStackCount()
        {
            return this.currLayerCount;
        }

        //相同 configId 叠加
        public void AddLayer(int layer)
        {
            if (layer > 0)
            {
                //叠层的时候刷新持续时间 也可以改为配置
                ResetLastTimer();

                this.currLayerCount += layer;
                CalculateAttrValue();

                if (this.currLayerCount >= this.tableConfig.MaxLayerCount)
                {
                    this.currLayerCount = this.tableConfig.MaxLayerCount;

                    CalculateAttrValue();

                    //触发满层效果
                    var context = new SkillEffectContext();
                    context.selectEntities = new List<BattleEntity>();
                    if ((BuffEffectTargetType)this.tableConfig.MaxLayerTriggerTargetType ==
                        BuffEffectTargetType.BuffTarget)
                    {
                        context.selectEntities.Add(this.target);
                    }
                    else if ((BuffEffectTargetType)this.tableConfig.MaxLayerTriggerTargetType ==
                             BuffEffectTargetType.SkillReleaser)
                    {
                        context.selectEntities.Add(this.context.fromSkill.releaser);
                    }


                    battle.AddSkillEffectGroup(tableConfig.MaxLayerTriggerEffectList, context);


                    if (1 == this.tableConfig.IsMaxLayerRemove)
                    {
                        this.SetWillEndState();
                    }
                }
                // else
                // {
                //     this.currLayerCount = this.currLayerCount + layer;
                //     CalculateAttrValue();
                // }
            }
            else
            {
                this.currLayerCount = this.currLayerCount + layer;
                CalculateAttrValue();

                var buffInfo = this.GetBuffInfo();
                battle.OnUpdateBuffInfo(buffInfo);

                if (this.currLayerCount <= 0)
                {
                    this.SetWillEndState();
                }
            }
        }

        public static BuffEffectInfo GenNewBuffInfo(int configId, int followEntityGuid,
            int linkTargetEntityGuid)
        {
            var tableConfig = BattleConfigManager.Instance.GetById<IBuffEffect>(configId);

            var buffInfo = new BuffEffectInfo();
            buffInfo.targetEntityGuid = followEntityGuid;
            buffInfo.linkTargetEntityGuid = linkTargetEntityGuid;
            if (tableConfig.InitLayerCount > 0)
            {
                buffInfo.statckCount = tableConfig.InitLayerCount;
            }
            else
            {
                buffInfo.statckCount = 1;
            }

            buffInfo.configId = configId;

            buffInfo.maxCDTime = (int)(tableConfig.LastTime);
            buffInfo.currCDTime = buffInfo.maxCDTime;

            return buffInfo;
        }


        //创建一个用于同步数据的 buff 数据
        public BuffEffectInfo GetBuffInfo()
        {
            var buffInfo = new BuffEffectInfo();
            buffInfo.targetEntityGuid = target.guid;
            buffInfo.configId = configId;
            buffInfo.statckCount = this.GetCurrStackCount();
            buffInfo.guid = this.guid;
            buffInfo.maxCDTime = (int)(this.GetMaxLastTime() * 1000);
            buffInfo.currCDTime = (int)(this.GetCurrLastTime() * 1000);

            return buffInfo;
        }

        public void CalculateAttrValue()
        {
            if (this.tableConfig.AttrGroupConfigId <= 0)
            {
                return;
            }

            //这里先默认 n 层 就是 n 倍的属性效果
            //TODO 现在好像只有一层的效果了
            if (buffAttrs != null && buffAttrs.Count > 0)
            {
                this.target.RemoveAttrs(EntityAttrGroupType.Buff, this.guid, buffAttrs);
            }

            buffAttrs?.Clear();

            BattleEntity releaser = null;

            if (null == context.fromSkill)
            {
                releaser = context.selectEntities[0];
            }
            else
            {
                releaser = context.fromSkill.releaser;
            }

            // if (1801201 == this.tableConfig.AttrGroupConfigId)
            // {
            //     Battle_Log.Log("");
            // }

            var attrArg = new AttrHelper.AttrCreateArg();
            attrArg.guid = this.guid;
            attrArg.attrGroupConfigId = this.tableConfig.AttrGroupConfigId;
            attrArg.attrGroupType = EntityAttrGroupType.Buff;

            attrArg.releaser = releaser;
            attrArg.target = target;
            attrArg.teamLeader = target.teamLeader;

            if ((BuffAddLayerType)this.tableConfig.AddLayerType == BuffAddLayerType.AddLayerAndEffect)
            {
                attrArg.layerCount = this.currLayerCount;
            }
            else
            {
                attrArg.layerCount = 1;
            }


            // buffAttrs = AttrHelper.GetAttrOptions(attrArg);

            this.buffAttrs = this.target.AddAttrs(attrArg);
        }

        //触发 start effect
        public void TriggerStartEffect(SkillEffectContext context)
        {
            battle.AddSkillEffectGroup(tableConfig.StartEffectList, context);
        }


        public override void OnUpdate(float timeDelta)
        {
            //CheckCollider();

            CheckInterval(timeDelta);

            currLastTime -= timeDelta;
            if (currLastTime <= 0)
            {
                this.SetWillEndState();
            }
        }

        public void ResetIntervalTime()
        {
            intervaleTime = tableConfig.IntervalTime / 1000.0f;
        }

        public void CheckInterval(float timeDelta)
        {
            if (tableConfig.LastTime <= 0)
            {
                return;
            }
            //if (null == tableConfig.IntervalEffectList || tableConfig.IntervalEffectList == "")
            //{
            //    return;
            //}

            if (0 == tableConfig.IntervalEffectList.Count)
            {
                return;
            }

            intervaleTime -= timeDelta;
            if (intervaleTime <= 0)
            {
                TriggerIntervalEffect();
                ResetIntervalTime();
            }
        }

        public void TriggerIntervalEffect()
        {
            // SkillEffectContext context = new SkillEffectContext()
            // {
            //     selectEntities = new List<BattleEntity>() { target },
            //     battle = this.context.battle,
            //     fromSkill = this.context.fromSkill
            // };

            var context = this.context.Copy();
            context.selectEntities = new List<BattleEntity>() { target };


            var calculateLayer = 1;
            if ((BuffAddLayerType)this.tableConfig.AddLayerType == BuffAddLayerType.AddLayerAndEffect)
            {
                calculateLayer = this.currLayerCount;
            }

            context.damageChangeRate += (1 * (calculateLayer - 1));
            battle.AddSkillEffectGroup(tableConfig.IntervalEffectList, context);
        }

        // public void ForceDelete()
        // {
        //     this.SetWillEndState();
        // }

        public override void OnEnd()
        {
            RemoveNormalAttackAddedDamage();

            target.RemoveBuff(this);

            if (buffAttrs != null && buffAttrs.Count > 0)
            {
                this.target.RemoveAttrs(EntityAttrGroupType.Buff, this.guid, buffAttrs);
            }

            buffAttrs?.Clear();

            foreach (var stateType in this.tableConfig.AbnormalStateTypeList)
            {
                target.RemoveAbnormalState((EntityAbnormalStateType)stateType);
            }

            foreach (var item in tableConfig.EndRemoveEffectList)
            {
                var configId = item;
                //默认为技能释放者 之后拓展

                //检查 buff(这个可以直接用 entity 进行删除 buff)
                var releaser = this.context.fromSkill.releaser;
                battle.DeleteBuffFromEntity(releaser.guid, configId);

                //检查 被动
                releaser.ForceRemovePassiveEffect(configId);
            }
        }
    }
}