using System.Collections.Generic;
using System.Threading;
using UnityEngine.UIElements;

namespace Battle
{
    public class SummonEffect : SkillEffect
    {
        //public Config.BuffEffect tableConfig;
        public ISummonEffect tableConfig;

        //BuffType buffType;
        Battle battle;

        public override void OnInit()
        {
            battle = this.context.battle;
            tableConfig = BattleConfigManager.Instance.GetById<ISummonEffect>(this.configId);
        }

        public override CreateEffectInfo ToCreateEffectInfo()
        {
            CreateEffectInfo createInfo = new CreateEffectInfo();
            createInfo.guid = guid;
            createInfo.resId = tableConfig.EffectResId;
            createInfo.effectPosType = EffectPosType.Custom_Pos;
            createInfo.followEntityGuid = -1;
            if (createInfo.resId > 0)
            {
                createInfo.isAutoDestroy = true;
            }

            return createInfo;
        }

        public override void OnStart()
        {
            base.OnStart();

            StartSummon();
        }

        void StartSummon()
        {
            var releaser = context.fromSkill.releaser;
            var summonConfigId = tableConfig.SummonConfigId;

            //判断召唤物数目上限
            var count = tableConfig.SummonCount;
            var limitCount = tableConfig.MaxSummonCount;
            var currSummonCount = releaser.GetSummonEntityCount(summonConfigId);
            var overCount = currSummonCount + count - limitCount;
            if (overCount > 0)
            {
                //超过上限
                //超过的部分从之前的召唤物中删除(这里只考虑正常情况)
                var delCurrSummonCount = overCount;
                releaser.ForceSummonEntityDead(summonConfigId, delCurrSummonCount);
            }

            //召唤逻辑
            var type = (SummonBornPosType)tableConfig.SummonBornPosType;
            List<Vector3> posList = new List<Vector3>();
            if (type == SummonBornPosType.ReleaserAroundPos)
            {
                posList = battle.GetAroundEmptyPos(releaser, count);
            }

            List<EntityInit> entityInitList = new List<EntityInit>();

            for (int i = 0; i < count; i++)
            {
                //获得的空节点不一定全都成功获得（可能地图上全是单位和障碍，没有空点）
                if (i >= posList.Count)
                {
                    break;
                }

                var pos = posList[i];
                var entityConfig = BattleConfigManager.Instance.GetById<IEntityInfo>(summonConfigId);

                var dir = (pos - releaser.position).normalized;
                EntityInit entityInit = new EntityInit()
                {
                    configId = summonConfigId,
                    playerIndex = releaser.playerIndex,
                    position = pos,
                    isPlayerCtrl = false,
                    //level 和 star 对于召唤兽来说先不弄 之后再说
                    level = 1,
                    star = 1,
                    dir = dir,
                    roleType = BattleEntityRoleType.Summon,
                    summonLastTime = tableConfig.LastTime / 1000.0f,
                    beSummonMaster = releaser
                };

                entityInit.skillInitList = new List<SkillInit>();

                foreach (var skillId in entityConfig.SkillIds)
                {
                    SkillInit skill = new SkillInit()
                    {
                        configId = skillId,
                        level = 1
                    };
                    entityInit.skillInitList.Add(skill);
                }

                if (entityConfig.UltimateSkillId > 0)
                {
                    SkillInit ultimateSkill = new SkillInit()
                    {
                        configId = entityConfig.UltimateSkillId,
                        level = 1
                    };
                    entityInit.skillInitList.Add(ultimateSkill);
                }

                entityInitList.Add(entityInit);
            }

            var entityList = battle.CreateEntities(entityInitList);

            for (int i = 0; i < entityList.Count; i++)
            {
                var entity = entityList[i];
                this.context.selectEntities = new List<BattleEntity>() { entity };

                battle.AddSkillEffectGroup(tableConfig.StartEffectList,context);

                releaser.AddSummonEntity(entity);

                releaser.OnSummonEntity(new EntityTriggerEventArg()
                {
                    triggerType = EffectTriggerTimeType.OnSummonEntity,
                    oppositeTriggerEntity = entity,
                    damageSrcSkill = this.context.fromSkill
                });
            }
        }

        public override void OnUpdate(float timeDelta)
        {
            //CheckCollider();
        }


        public override void OnEnd()
        {
        }
    }

    public enum SummonBornPosType
    {
        //上下文选取点（上下文中选取的单位点或者坐标点）
        ContextSelectPos = 0,

        //释放者周围点（优先选择释放者朝向方向，如果周围点有单位，那么就不断向外寻找空地点）
        ReleaserAroundPos = 1,
    }
}