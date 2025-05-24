using System;
using System.Collections.Generic;
using System.Linq;
using Config;

namespace Battle
{
    public partial class CpuAI : BaseAI
    {
        public float GetTotalPriority_Attack(List<BattleEntity> selectEntities, float maxPri,
            float minPri)
        {
            var total = 0.0f;
            for (int j = 0; j < selectEntities.Count; j++)
            {
                var selectEntity = selectEntities[j];
                var currP = 0.0f;

                var maxP = 6.0f;
                var minP = 2.0f;
                var hpRatio = selectEntity.GetCurrHpRatio();
                total += minP + (1 - hpRatio) * (maxP - minP);
            }

            return total;
        }

        //获得选择实体中最高优先级的单体优先级（技能进攻向）
        public (float, BattleEntity) GetMaxSingleEntityPriority_Attack(List<BattleEntity> selectEntities, float maxPri,
            float minPri,float selectMaxHpRatio = 0.8f)
        {
            BattleEntity maxEntity = null;
            float maxPriority = 0;
            var maxP = maxPri;
            var minP = minPri;
            for (int j = 0; j < selectEntities.Count; j++)
            {
                var selectEntity = selectEntities[j];
                var currP = 0.0f;

                var hpRatio = selectEntity.GetCurrHpRatio();
                currP += minP + (1 - hpRatio) * (maxP - minP);

                if (currP > maxPriority)
                {
                    maxPriority = currP;
                    maxEntity = selectEntity;
                }
            }

            return (maxPriority, maxEntity);
        }

        //获得选择实体中最高优先级的单体优先级（技能辅助向）
        public (float, BattleEntity) GetMaxSingleEntityPriority_Assistant(List<BattleEntity> selectEntities,
            float maxPri,
            float minPri,float selectMaxHpratio = 0.8f)
        {
            BattleEntity maxEntity = null;
            float maxPriority = 0;
            var maxP = maxPri;
            var minP = minPri;
            
            for (int j = 0; j < selectEntities.Count; j++)
            {
                var selectEntity = selectEntities[j];
                var currP = 0.0f;

              
                var hpRatio = selectEntity.GetCurrHpRatio();
                currP += minP + (1 - hpRatio) * (maxP - minP);
                if (hpRatio > selectMaxHpratio)
                {
                    currP = 0;
                }

                if (currP > maxPriority)
                {
                    maxPriority = currP;
                    maxEntity = selectEntity;
                }
            }

            return (maxPriority, maxEntity);
        }

        //获取圆形区域中的小圆形中最大优先级的单位点（技能进攻向）
        public (float, Vector3) GetCircleAreaMaxPriority_Skill_Attack(Skill skill, float smallCircleRadius)
        {
            //取最密集的位置
            var selectEntityType = (EntityRelationFilterType)skill.infoConfig.EntityRelationFilterType;
            var battle = this.entity.GetBattle();
            //筛选圆中的单位
            var selectEntities = battle.GetEntitiesInCircleAtEntity(skill.releaser,
                skill.releaser.position,
                skill.infoConfig.ReleaseRange / 1000.0f, selectEntityType);

            //以每个选中的单位再次进行圆形检测
            BattleEntity maxEntity = null;
            float maxPriority = -1;
            for (int j = 0; j < selectEntities.Count; j++)
            {
                var selectEntity = selectEntities[j];
                var selectEntities2 = battle.GetEntitiesInCircleAtEntity(skill.releaser,
                    selectEntity.position,
                    smallCircleRadius, selectEntityType);

                var currP = GetTotalSelectEntitiesPriority_Skill_Attack(selectEntities2);
                if (currP > maxPriority)
                {
                    maxPriority = currP;
                    maxEntity = selectEntity;
                }
            }

            // Battle_Log.LogZxy("zxyzxy : maxPriority : " + maxPriority);

            return (maxPriority, maxEntity?.position ?? Vector3.zero);
        }


        //获取圆形区域中的小圆形中最大优先级的单位点（技能辅助向）
        public (float, Vector3) GetCircleAreaMaxPriority_Skill_Assistant(Skill skill, float smallCircleRadius,
            SkillTagType tag)
        {
            //取最密集的位置
            var selectEntityType = (EntityRelationFilterType)skill.infoConfig.EntityRelationFilterType;
            var battle = this.entity.GetBattle();
            //筛选圆中的单位
            var selectEntities = battle.GetEntitiesInCircleAtEntity(skill.releaser,
                skill.releaser.position,
                skill.infoConfig.ReleaseRange / 1000.0f, selectEntityType);

            //以每个选中的单位再次进行圆形检测
            BattleEntity maxEntity = null;
            float maxPriority = -1;
            for (int j = 0; j < selectEntities.Count; j++)
            {
                var selectEntity = selectEntities[j];
                var selectEntities2 = battle.GetEntitiesInCircleAtEntity(skill.releaser,
                    selectEntity.position,
                    smallCircleRadius, selectEntityType);

                var currP = GetTotalSelectEntitiesPriority_Skill_Assistant(selectEntities2, tag);
                if (currP > maxPriority)
                {
                    maxPriority = currP;
                    maxEntity = selectEntity;
                }
            }

            // Battle_Log.LogZxy("zxyzxy : maxPriority : " + maxPriority);

            return (maxPriority, maxEntity?.position ?? Vector3.zero);
        }


        /// <summary>
        /// 得到选取单位中的优先级总和（技能进攻向）
        /// </summary>
        /// <param name="selectEntities">选取的单位组</param>
        /// <param name="totalPriFactor">总共优先级的因子（单位数目 * 该因子 = 作为总共优先级）</param>
        /// <param name="limitMaxTotalPri">限制优先级的最高上限</param>
        /// <param name="partMinPriFactor">每一个单位的最小优先级因子（该因子 * 每个单位总共优先级 = 每个单位最小优先级）</param>
        /// <returns></returns>
        public float GetTotalSelectEntitiesPriority_Skill_Attack(List<BattleEntity> selectEntities,
            float totalPriFactor = 1.5f, float limitMaxTotalPri = 10.0f, float partMinPriFactor = 0.3f)
        {
            var currP = 0.0f;
            if (selectEntities.Count > 0)
            {
                var totalP = totalPriFactor * selectEntities.Count;
                var maxP = limitMaxTotalPri;
                totalP = MathTool.Clamp(totalP, 0, maxP);
                var partMaxP = totalP / selectEntities.Count;
                var partMinP = partMaxP * partMinPriFactor;
                foreach (var _e in selectEntities)
                {
                    var hpRatio = _e.GetCurrHpRatio();
                    var resultP = partMinP + (1.0f - hpRatio) * (partMaxP - partMinP);
                    currP += resultP;
                }
            }

            return currP;
        }

        /// <summary>
        /// 得到选取单位中的优先级总和（技能辅助向）
        /// </summary>
        /// <param name="selectEntities">选取的单位组</param>
        /// <param name="totalPriFactor">总共优先级的因子（单位数目 * 该因子 = 作为总共优先级）</param>
        /// <param name="limitMaxTotalPri">限制优先级的最高上限</param>
        /// <param name="partMinPriFactor">每一个单位的最小优先级因子（该因子 * 每个单位总共优先级 = 每个单位最小优先级）</param>
        /// <returns></returns>
        public float GetTotalSelectEntitiesPriority_Skill_Assistant(List<BattleEntity> selectEntities, SkillTagType tag,
            float totalPriFactor = 1.5f, float limitMaxTotalPri = 10.0f, float partMinPriFactor = 0.3f,float selectMaxHpRatio = 0.8f)
        {
            var currP = 0.0f;
            if (selectEntities.Count > 0)
            {
                var totalP = totalPriFactor * selectEntities.Count;
                var maxP = limitMaxTotalPri;
                totalP = MathTool.Clamp(totalP, 0, maxP);
                var partMaxP = totalP / selectEntities.Count;
                var partMinP = partMaxP * partMinPriFactor;
                foreach (var _e in selectEntities)
                {
                    var hpRatio = _e.GetCurrHpRatio();
                    if (hpRatio <= selectMaxHpRatio)
                    {
                        var resultP = partMinP + (1.0f - hpRatio) * (partMaxP - partMinP);
                        currP += resultP;
                    }

                  
                }
            }

            return currP;
        }


        //获取区域伤害技能的总共优先级
        public (float, Vector3) GetAreaPriority_Hurt_Skill(int areaId, Skill skill)
        {
            var attackPriority = 0.0f;
            var expectPos = Vector3.zero;

            //区域
            var areaConfig = BattleConfigManager.Instance.GetById<IAreaEffect>(areaId);
            if ((AreaType)areaConfig.AreaType == AreaType.Circle)
            {
                var _info = GetCircleAreaMaxPriority_Skill_Attack(skill,
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

        //获取区域辅助技能的总共优先级
        public (float, Vector3) GetAreaPriority_Assistant_Skill(int areaId, Skill skill, SkillTagType tag)
        {
            var attackPriority = 0.0f;
            var expectPos = Vector3.zero;

            //区域
            var areaConfig = BattleConfigManager.Instance.GetById<IAreaEffect>(areaId);
            if ((AreaType)areaConfig.AreaType == AreaType.Circle)
            {
                var _info = GetCircleAreaMaxPriority_Skill_Assistant(skill,
                    areaConfig.RangeParam[0] / 1000.0f, tag);
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

        //获取单个目标的伤害技能优先级
        public (float, BattleEntity) GetSingleTargetEntityPriority_Hurt_Skill(Skill skill)
        {
            var battle = this.entity.GetBattle();
            var selectEntityType = (EntityRelationFilterType)skill.infoConfig.EntityRelationFilterType;
            BattleEntity expectTarget = null;
            var attackPriority = 0.0f;
            var expectPos = Vector3.zero;

            if (skill.isNormalAttack && currTargetEntity != null)
            {
                if (currTargetEntity != null)
                {
                    var hpRatio = currTargetEntity.GetCurrHpRatio();
                    var maxP = 6.0f;
                    var minP = 2.0f;
                    attackPriority += minP + (1 - hpRatio) * (maxP - minP);
                    expectTarget = currTargetEntity;
                }
            }
            else
            {
                //没有目标的时候 或者是 技能的时候（非普攻）

                List<BattleEntity> selectEntities = battle.GetEntitiesInCircleAtEntity(
                    skill.releaser,
                    skill.releaser.position,
                    skill.GetReleaseRange(),
                    selectEntityType);
                if (selectEntities.Count > 0)
                {
                    var _info = GetMaxSingleEntityPriority_Attack(selectEntities, 6.0f, 2.0f);

                    var maxPriority = _info.Item1;
                    //普通攻击和其他技能相比 优先级降低一些 让其他技能优先释放
                    if (skill.isNormalAttack) maxPriority *= 0.8f;
                    attackPriority += maxPriority;
                    expectTarget = _info.Item2;
                }
                else
                {
                    //周围都没有人 随机找一个敌方
                    var nearestEntity =
                        this.entity.GetNearestEntityFromAll(EntityRelationFilterType.Enemy);
                    if (nearestEntity != null)
                    {
                        float maxPriority = 5.0f;
                        attackPriority += maxPriority;
                        expectTarget = nearestEntity;
                    }
                }
            }

            return (attackPriority, expectTarget);
        }


        //获取单个目标的辅助技能优先级
        public (float, BattleEntity) GetSingleTargetEntityPriority_Assistant_Skill(Skill skill)
        {
            var battle = this.entity.GetBattle();
            var selectEntityType = (EntityRelationFilterType)skill.infoConfig.EntityRelationFilterType;
            BattleEntity expectTarget = null;
            var attackPriority = 0.0f;
            var expectPos = Vector3.zero;

            List<BattleEntity> selectEntities = battle.GetEntitiesInCircleAtEntity(
                skill.releaser,
                skill.releaser.position,
                skill.GetReleaseRange(),
                selectEntityType);
            if (selectEntities.Count > 0)
            {
                var _info = GetMaxSingleEntityPriority_Assistant(selectEntities, 6.0f, 2.0f);

                var maxPriority = _info.Item1;
                attackPriority += maxPriority;
                expectTarget = _info.Item2;
            }

            return (attackPriority, expectTarget);
        }


        //获得单一点的伤害技能优先级
        public (float, Vector3) GetSinglePointPriority_Hurt_Skill(Skill skill)
        {
            Vector3 expectPos;
            var attackPriority = 0.0f;

            var battle = this.entity.GetBattle();
            var selectEntityType = (EntityRelationFilterType)skill.infoConfig.EntityRelationFilterType;
            var selectEntities = battle.GetEntitiesInCircleAtEntity(skill.releaser,
                skill.releaser.position,
                skill.releaser.AttackRange, selectEntityType);
            var _info = GetMaxSingleEntityPriority_Attack(selectEntities, 6.0f, 2.0f);

            var maxPriority = _info.Item1;
            //普通攻击和其他技能相比 优先级降低一些 让其他技能优先释放
            if (skill.isNormalAttack) maxPriority *= 0.8f;
            attackPriority += maxPriority;
            expectPos = _info.Item2?.position ?? Vector3.zero;

            return (attackPriority, expectPos);
        }
        
        //获得单一点的辅助技能优先级
        public (float, Vector3) GetSinglePointPriority_Assistant_Skill(Skill skill)
        {
            Vector3 expectPos;
            var attackPriority = 0.0f;

            var battle = this.entity.GetBattle();
            var selectEntityType = (EntityRelationFilterType)skill.infoConfig.EntityRelationFilterType;
            var selectEntities = battle.GetEntitiesInCircleAtEntity(skill.releaser,
                skill.releaser.position,
                skill.releaser.AttackRange, selectEntityType);
            var _info = GetMaxSingleEntityPriority_Attack(selectEntities, 6.0f, 2.0f);

            var maxPriority = _info.Item1;
            attackPriority += maxPriority;
            expectPos = _info.Item2?.position ?? Vector3.zero;

            return (attackPriority, expectPos);
        }

        //获得自己释放的逃跑技能优先级
        public (float, Vector3) GetSinglePosPriority_Escape_Skill_Self(Skill skill)
        {
            Vector3 expectPos;
            var escapePriority = 0.0f;
            var battle = this.entity.GetBattle();
            var escapeMaxP = 6.0f;
            var hpRatio = skill.releaser.GetCurrHpRatio();

            if (recentlyDamageSourceEntity != null)
            {
                //TODO 计算离开方向的点 为远离伤害来源者方向点 , recentlyDamageSourceEntity 持续一段时间会置空
                var reflectPos = MathTool.GetMidpointReflection(recentlyDamageSourceEntity.position,
                    skill.releaser.position);
                escapePriority += (1 - hpRatio) * escapeMaxP;
                expectPos = reflectPos;
            }
            else
            {
                if (skill.releaser.teamLeader != null)
                {
                    //往队长处跑
                    escapePriority += (1 - hpRatio) * escapeMaxP;
                    expectPos = skill.releaser.teamLeader.position;
                }
                else
                {
                    expectPos = MathTool.GetRandPosAroundCircle(battle.rand, skill.GetReleaseRange(),
                        skill.releaser.position);
                }
            }

            return (escapePriority, expectPos);
        }

        //获得对自己释放的移速增益的技能的优先级
        public (float, BattleEntity, Vector3) GetSingleTargetPriority_Escape_Skill_Self(Skill skill)
        {
            var escapePriority = 0.0f;
            BattleEntity expectTarget = null;
            var expectPos = Vector3.zero;

            var escapeMaxP = 4.0f;
            var hpRatio = skill.releaser.GetCurrHpRatio();
            escapePriority += (1 - hpRatio) * escapeMaxP;

            //不管是什么目标类型 都是跟自己有关
            expectPos = skill.releaser.position;
            expectTarget = skill.releaser;

            return (escapePriority, expectTarget, expectPos);
        }
    }
}