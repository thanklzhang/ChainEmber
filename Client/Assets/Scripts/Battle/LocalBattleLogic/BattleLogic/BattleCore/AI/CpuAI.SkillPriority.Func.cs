using System;
using System.Collections.Generic;
using System.Linq;
using Config;

namespace Battle
{
    public partial class CpuAI : BaseAI
    {
        private const float minHpPercentPriority = 2.0f;
        private const float maxHpPercentPriority = 6.0f;


        private const float minSingleEntityPriority = 2.0f;
        private const float maxSingleEntityPriority = 6.0f;

        public const float singleEntityPriorityWhenNoTarget = 5.0f;


        public const float perEntityPriorityByArea = 1.5f;
        public const float limitAreaMaxTotalPriority = 10.0f;
        public const float limitAreaMinTotalPriority = 1.0f;


        // 获得当前实体的血量百分比优先级
        public static float GetHpPercentPriority(BattleEntity entity, float min = maxHpPercentPriority,
            float max = minHpPercentPriority)
        {
            return min + (1 - entity.GetCurrHpRatio()) * (max - min);
        }

        //  获得当前实体的血量百分比优先级 反向
        public static float GetHpPercentPriorityReverse(BattleEntity entity, float min = maxHpPercentPriority,
            float max = minHpPercentPriority)
        {
            return max - (1 - entity.GetCurrHpRatio()) * (max - min);
        }

        //从给出的实体列表 获取单个目标的最大优先级
        public (float, BattleEntity) GetMaxSingleEntityPriority(List<BattleEntity> selectEntities,
            float minPri = minSingleEntityPriority,
            float maxPri = maxSingleEntityPriority)
        {
            BattleEntity maxEntity = null;
            float maxPriority = 0;
            for (int j = 0; j < selectEntities.Count; j++)
            {
                var selectEntity = selectEntities[j];
                var currP = 0.0f;
                currP += GetHpPercentPriorityReverse(selectEntity, minPri, maxPri);

                if (currP > maxPriority)
                {
                    maxPriority = currP;
                    maxEntity = selectEntity;
                }
            }

            return (maxPriority, maxEntity);
        }

        // 获取单个目标优先级
        public (float, BattleEntity) GetSingleTargetEntityPriority(Skill skill)
        {
            var battle = this.entity.GetBattle();
            var selectEntityType = (EntityRelationFilterType)skill.infoConfig.EntityRelationFilterType;
            BattleEntity expectTarget = null;
            var resultPriority = 0.0f;
            if (skill.isNormalAttack && currTargetEntity != null)
            {
                //普攻的优先级
                resultPriority += GetHpPercentPriorityReverse(currTargetEntity);
                expectTarget = currTargetEntity;
            }
            else
            {
                //没有目标的时候 或者是 技能的时候
                List<BattleEntity> selectEntities = battle.GetEntitiesInCircleAtEntity(
                    skill.releaser,
                    skill.releaser.position,
                    skill.GetReleaseRange(),
                    selectEntityType);

                if (selectEntities.Count > 0)
                {
                    var _info = GetMaxSingleEntityPriority(selectEntities);
                    var maxPriority = _info.Item1;
                    resultPriority += maxPriority;
                    expectTarget = _info.Item2;
                }
                else
                {
                    if (skill.isNormalAttack)
                    {
                        //周围都没有人 随机找一个敌方    
                        var nearestEntity =
                            this.entity.GetNearestEntityFromAll(EntityRelationFilterType.Enemy);
                        if (nearestEntity != null)
                        {
                            resultPriority += singleEntityPriorityWhenNoTarget;
                            expectTarget = nearestEntity;
                        }
                    }
                }
            }

            return (resultPriority, expectTarget);
        }


        //  获取单个技能释放点优先级
        public (float, Vector3) GetSinglePointPriority(Skill skill)
        {
            Vector3 expectPos;
            var attackPriority = 0.0f;

            var battle = this.entity.GetBattle();
            var selectEntityType = (EntityRelationFilterType)skill.infoConfig.EntityRelationFilterType;
            var selectEntities = battle.GetEntitiesInCircleAtEntity(skill.releaser,
                skill.releaser.position,
                skill.GetReleaseRange(), selectEntityType);
            var _info = GetMaxSingleEntityPriority(selectEntities);
            var maxPriority = _info.Item1;
            attackPriority += maxPriority;
            expectPos = _info.Item2?.position ?? Vector3.zero;

            return (attackPriority, expectPos);
        }

        //获取区域伤害技能的总共优先级
        public (float, Vector3) GetAreaPriority(int areaId, Skill skill)
        {
            var attackPriority = 0.0f;
            var expectPos = Vector3.zero;

            //区域
            var areaConfig = BattleConfigManager.Instance.GetById<IAreaEffect>(areaId);
            if ((AreaType)areaConfig.AreaType == AreaType.Circle)
            {
                var _info = GetCircleAreaMaxEntityPriorityFromEntity(skill, skill.releaser,
                    skill.GetReleaseRange(),
                    areaConfig.RangeParam[0] / 1000.0f);
                attackPriority += _info.Item1;
                expectPos = _info.Item2;
            }
            else
            {
                //其他的先待定
                attackPriority += 1;
                expectPos = skill.releaser.position;
            }

            return (attackPriority, expectPos);
        }


        //获取单位周围圆形区域中的小圆形中最大优先级的单位点
        public (float, Vector3) GetCircleAreaMaxEntityPriorityFromEntity(Skill skill, BattleEntity srcEntity,
            float bigRadius, float smallRadius)
        {
            //取最密集的位置
            var selectEntityType = (EntityRelationFilterType)skill.infoConfig.EntityRelationFilterType;
            var battle = this.entity.GetBattle();
            //筛选圆中的单位
            var selectEntities = battle.GetEntitiesInCircleAtEntity(srcEntity,
                srcEntity.position,
                bigRadius, selectEntityType);

            //以每个选中的单位再次进行圆形检测
            BattleEntity maxEntity = null;
            float maxPriority = -1;
            for (int i = 0; i < selectEntities.Count; i++)
            {
                var selectEntity = selectEntities[i];
                var selectEntities2 = battle.GetEntitiesInCircleAtEntity(srcEntity,
                    selectEntity.position,
                    smallRadius, selectEntityType);

                var currP = GetTotalSelectEntitiesPriority(selectEntities2);
                if (currP > maxPriority)
                {
                    maxPriority = currP;
                    maxEntity = selectEntity;
                }
            }

            // Battle_Log.LogZxy("zxyzxy : maxPriority : " + maxPriority);

            return (maxPriority, maxEntity?.position ?? Vector3.zero);
        }

        //得到选取单位中的优先级总和
        public float GetTotalSelectEntitiesPriority(List<BattleEntity> selectEntities,
            float perEntityPri = perEntityPriorityByArea, float limitMaxTotalPri = limitAreaMaxTotalPriority,
            float limitMinTotalPri = limitAreaMinTotalPriority)
        {
            var resultPri = 0.0f;
            if (selectEntities.Count > 0)
            {
                var totalP = MathTool.Clamp(
                    perEntityPri * selectEntities.Count, 0, limitMaxTotalPri);

                var partMaxP = totalP / selectEntities.Count;
                var partMinP = 0;
                foreach (var _e in selectEntities)
                {
                    var resultP = GetHpPercentPriorityReverse(_e, 0, partMaxP);
                    resultPri += resultP;
                }

                if (resultPri < limitMinTotalPri) resultPri = limitMinTotalPri;
            }

            return resultPri;
        }
    }
}