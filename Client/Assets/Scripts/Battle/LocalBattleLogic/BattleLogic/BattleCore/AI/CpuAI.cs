using System;
using System.Collections.Generic;
using System.Linq;
using Config;

namespace Battle
{
    //电脑通用 AI
    public partial class CpuAI : BaseAI
    {
        private CharacterDimensionMgr dimensionMgr;
        public BattleEntity currTargetEntity;
        private BattleEntity recentlyDamageSourceEntity;
        private AIThinkType aiThinkType = AIThinkType.Idle;
        private List<Skill> skillList;
        
        private float decideTotalTime = 0.75f;
        private float decideTimer = 0.0f;
        
        public float recentlyDamageSourceTimer;
        public float recentlyDamageSourceTotalTime = 3.0f;
        
        private float escapeMoveTimer;
        private float escapeMoveTotalTime = 2.5f;
        
        private float checkNormalAttackRangeTimer = 0.0f;
        private float checkNormalAttackRangeTotalTime = 3.0f;

        private float autoReleaseNoTargetSkillRange = 10.0f;

        public override void OnInit()
        {
            dimensionMgr = new CharacterDimensionMgr();
            dimensionMgr.Init();

            //Test
            dimensionMgr.SetValue(CharacterDimensionType.Bravery, 1.0f);
            dimensionMgr.SetValue(CharacterDimensionType.Compassion, 0.8f);
            dimensionMgr.SetValue(CharacterDimensionType.Tenacity, 0.5f);
        }

        public override void OnUpdate(float deltaTime)
        {
            //最近收到伤害源时间更新
            recentlyDamageSourceTimer += deltaTime;
            if (recentlyDamageSourceTimer >= recentlyDamageSourceTotalTime)
            {
                recentlyDamageSourceTimer = 0;
                this.recentlyDamageSourceEntity = null;
            }

            if (currTargetEntity != null)
            {
                if (currTargetEntity.IsDead())
                {
                    currTargetEntity = null;
                }
            }

            //每隔一段时间 Decide
            decideTimer += deltaTime;
            if (decideTimer >= decideTotalTime)
            {
                Decide();
                decideTimer = 0;
                decideTotalTime = BattleRandom.GetRandFloat(0.45f, 0.5f, this.entity.GetBattle());
            }
            else
            {
                //不决定的时候就普通攻击
                if (aiThinkType != AIThinkType.Escape)
                {
                    if (currTargetEntity != null)
                    {
                        var normalAttackSkill = this.entity.GetNormalAttackSkill();
                         var isInRange = Skill.IsInSkillReleaseRange(this.entity, normalAttackSkill.configId,
                             currTargetEntity.guid, Vector3.zero);

                         if (!isInRange)
                         {
                             checkNormalAttackRangeTimer += deltaTime;
                             if (checkNormalAttackRangeTimer >= checkNormalAttackRangeTotalTime)
                             {
                                 checkNormalAttackRangeTimer = 0;
                                 currTargetEntity = null;
                             }
                         }
                         else
                         {
                             checkNormalAttackRangeTimer = 0;
                             if (normalAttackSkill.state != ReleaseSkillState.CD)
                             {
                                 entity.AskReleaseSkill(normalAttackSkill.configId, currTargetEntity.guid, Vector3.zero,Vector3.zero);
                             }
                         }
                    }
                }
            }
        }
        
        public override void OnMoveNoWay()
        {
            //如果无路可走 那么就换一个目标
            // this.currTargetEntity = null;
        }

        //AI 决策
        public void Decide()
        {
            if (currTargetEntity != null && currTargetEntity.IsDead())
            {
                currTargetEntity = null;
            }

            var brave = dimensionMgr.GetValue(CharacterDimensionType.Bravery);
            var love = dimensionMgr.GetValue(CharacterDimensionType.Compassion);
            var tenacity = dimensionMgr.GetValue(CharacterDimensionType.Tenacity);
            
            //计算技能优先级
            skillList = this.entity.GetSkillList();
            AIAnalyseResult maxSkillPriorityResult = null;
            foreach (var skill in skillList)
            {
                var result = GetSkillPriority(skill);

                if (null == maxSkillPriorityResult ||
                    result.priority >= maxSkillPriorityResult.priority)
                {
                    maxSkillPriorityResult = result;
                }
            }

            var skillPriority = 0.0f;
            BattleEntity skillTargetEntity = null;
            Vector3 skillTargetPos = Vector3.zero;
            Skill releaseSkill = null;
            if (maxSkillPriorityResult != null)
            {
                skillPriority = maxSkillPriorityResult.priority * brave;
                skillTargetEntity = maxSkillPriorityResult.expectTarget;
                skillTargetPos = maxSkillPriorityResult.expectPos;
                releaseSkill = maxSkillPriorityResult.skill;
            }
            

            //计算 move 优先级
            var moveResult = GetMovePriority();
            var movePriority = moveResult.priority;
            Vector3 moveTargetPos = moveResult.expectPos;

            bool isReleaseSkill = false;
            bool isMove = false;
            if (skillPriority > 0)
            {
                if (movePriority > 0)
                {
                    isReleaseSkill = skillPriority > movePriority;
                    isMove = !isReleaseSkill;
                }
                else
                {
                    isReleaseSkill = true;
                }
            }
            else
            {
                if (movePriority > 0)
                {
                    isMove = true;
                }
            }

            if (isReleaseSkill)
            {
                //释放技能
                if (skillTargetEntity != null)
                {
                    if (releaseSkill.isNormalAttack)
                    {
                        currTargetEntity = skillTargetEntity;
                    }

                    entity.AskReleaseSkill(releaseSkill.configId, skillTargetEntity.guid, skillTargetPos,Vector3.zero);
                }
                else
                {
                    entity.AskReleaseSkill(releaseSkill.configId, 0, skillTargetPos,Vector3.zero);
                }

                aiThinkType = AIThinkType.Skill;

                if (maxSkillPriorityResult != null && maxSkillPriorityResult.isEscape)
                {
                    aiThinkType = AIThinkType.Escape;
                }
            }
            else if (isMove)
            {
                //移动
                entity.AskMoveToPos(moveTargetPos);
                aiThinkType = AIThinkType.Escape;
            }
            else
            {
                //不动
                aiThinkType = AIThinkType.Idle;
            }
        }

        public override void OnBeHurt(float resultDamage, Skill fromSkill)
        {
            recentlyDamageSourceTimer = 0;
            recentlyDamageSourceEntity = fromSkill.releaser;
        }
    }
}