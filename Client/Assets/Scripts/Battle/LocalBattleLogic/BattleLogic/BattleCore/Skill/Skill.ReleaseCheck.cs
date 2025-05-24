using System.Collections.Generic;

namespace Battle
{
    public partial class Skill
    {
        //对于这个技能来说 是否能够释放
        public static bool IsCanRelease(Skill skill,int targetGuid,Vector3 targetPos)
        {
            if (null == skill)
            {
                return false;
            }

            var releaser = skill.releaser;

            //先判断释放者本身状态是否能够释放技能
            if (!releaser.IsCanReleaseSkill())
            {
                return false;
            }

            //var skill = FindSkillByConfigId(skillId);
            // if (null == skill)
            // {
            //     //TODO
            //     //人物技能没找到 试着看是否用了道具效果
            //     //武器技能 T快捷键 可能会用到
            // }

            //判断是否是被动技能
            if (1 == skill.infoConfig.IsPassiveSkill)
            {
                return false;
            }

            //判断技能的当前状态是否能够释放
            var isRedayRelease = skill.IsReadyRelease();
            if (!isRedayRelease)
            {
                //_Battle_Log.Log(string.Format("ReleaseSkill fail , not RedayRelease state : guid : {0} skillId : {1}", guid, skillId));
                return false;
            }

            //检测技能目标的关系是否符合
            var isSuitTargetType = CheckSkillTargetRelation(releaser,skill,targetGuid);
            if (!isSuitTargetType)
            {
                return false;
            }

            //检测范围
            var isInRange = IsInSkillReleaseRange(releaser,skill.infoConfig.Id, targetGuid, targetPos);
            if (!isInRange)
            {
                //_Battle_Log.Log(string.Format("ReleaseSkill fail , not in range : guid : {0} skillId : {0}", guid, skillId));
                //TryToMoveByReleaseSkill(skillId, targetGuid, targetPos);
                return false;
            }

            return true;
        }

        public static bool IsInSkillReleaseRange(BattleEntity releaser, int skillId, int targetGuid, Vector3 targetPos)
        {
            // var isCanReleaseSkill = releaser.IsCanReleaseSkill();
            //
            // if (!isCanReleaseSkill)
            // {
            //     // Logx.Log(string.Format("{0} move : no release skill ! ", this.infoConfig.Name));
            //     return false;
            // }

            bool isInRange = false;
            var skill = releaser.FindSkillByConfigId(skillId);

            if (null == skill)
            {
                BattleLog.LogWarning("the skillId is not found : " + skillId);
                return false;
            }

            if (null == skill.infoConfig)
            {
                BattleLog.LogWarning("the infoConfig is null , the skill id is : " + skillId);
                return false;
            }

            var releaseTargetType = (SkillReleaseTargetType)skill.infoConfig.SkillReleaseTargeType;
            if (releaseTargetType == SkillReleaseTargetType.Entity || releaseTargetType == SkillReleaseTargetType.Point)
            {
                //判断当前距离是否能够释放技能
                float sqrtDis = 99999999;
                if (targetGuid > 0)
                {
                    var targetEntity = releaser.GetBattle().FindEntity(targetGuid);
                    if (targetEntity != null)
                    {
                        sqrtDis = Vector3.SqrtDistance(releaser.position, targetEntity.position);
                    }
                }
                else
                {
                    targetPos = new Vector3(targetPos.x, 0, targetPos.z);
                    sqrtDis = Vector3.SqrtDistance(releaser.position, targetPos);
                }

                //_G.Log(string.Format("{0} 's state : {1}", this.infoConfig.Name, this.entityState.ToString()));

                var releaseRange = skill.GetReleaseRange();
                //_G.Log(string.Format("sqr dis : {0} relaseRange : {1}", sqrtDis, releaseRange * releaseRange));
                if (sqrtDis <= releaseRange * releaseRange)
                {
                    isInRange = true;
                }
                else
                {
                    isInRange = false;
                }
            }
            else
            {
                isInRange = true;
            }

            return isInRange;
        }
        
        
        //检测技能目标的关系是否符合（比如敌对关系等）
        public static bool CheckSkillTargetRelation(BattleEntity releaser, Skill skill, int targetGuid)
        {
            var isSuitTargetType = false;
            if ((SkillReleaseTargetType)skill.infoConfig.SkillReleaseTargeType == SkillReleaseTargetType.Entity)
            {
                if (targetGuid > 0)
                {
                  
                    var targetEntity = releaser.GetBattle().FindEntity(targetGuid);
                    if (targetEntity != null)
                    {
                        
                        var selectEntityType = (EntityRelationFilterType)skill.infoConfig.EntityRelationFilterType;
                        isSuitTargetType = releaser.IsSuitSkillSelectType(selectEntityType,targetEntity);
                    }
                }
            }
            else
            {
                isSuitTargetType = true;
            }
            return isSuitTargetType;
        }
    }
}