using System;
using System.Collections.Generic;
using System.Linq;
using Config;

namespace Battle
{
    public enum SkillActionTendencyType
    {
        Attack = 0,
        Define = 1,
        Assistant = 2,
        Displacement = 3
    }

    public partial class CpuAI : BaseAI
    {
        public AIAnalyseResult GetSkillPriority(Skill skill)
        {
            var resultPri = 0.0f;
            BattleEntity expectTarget = null;
            Vector3 expectPos = Vector3.zero;

            if (!skill.IsReadyRelease())
            {
                return new AIAnalyseResult()
                {
                    priority = 0.0f
                };
            }

            //是否是区域技能
            var areaId = Skill_Tool.GetKeyAreaEft(skill.configId);
            var isAreaSkill = areaId > 0;

            var releaseTargetType = (SkillReleaseTargetType)skill.infoConfig.SkillReleaseTargeType;
            if (releaseTargetType == SkillReleaseTargetType.Entity)
            {
                var _info = GetSingleTargetEntityPriority(skill);
                resultPri = _info.Item1;
                expectTarget = _info.Item2;
            }
            else if (releaseTargetType == SkillReleaseTargetType.Point)
            {
                if (isAreaSkill)
                {
                    //区域技能
                    var _info = GetAreaPriority(areaId, skill);
                    resultPri = _info.Item1;
                    expectPos = _info.Item2;
                }
                else
                {
                    //点 先简单处理 ，应该是按照技能效果来区分 例如 投掷物需要计算飞行途径的敌人数量和生命值百分比等这种 待定吧
                    //释放范围内所有单位点进行计算
                    var _info = GetSinglePointPriority(skill);
                    resultPri = _info.Item1;
                    expectPos = _info.Item2;
                }
            }
            else if (releaseTargetType == SkillReleaseTargetType.NoTarget)
            {
                if (isAreaSkill)
                {
                    //区域技能
                    var _info = GetAreaPriority(areaId, skill);
                    resultPri = _info.Item1;
                    expectPos = _info.Item2;
                }
                else
                {
                    //这种情况一般都是给自己加状态，或者是以自身为中心的技能
                    //周围有怪就有可以释放
                    var selectEntities = entity.GetBattle().GetEntitiesInCircleAtEntity(entity,
                        entity.position,
                        autoReleaseNoTargetSkillRange, EntityRelationFilterType.Enemy);

                    if (selectEntities.Count > 0)
                    {
                        //有怪
                        resultPri = GetTotalSelectEntitiesPriority(selectEntities);
                    }
                }
            }

            var analyseResult = new AIAnalyseResult()
            {
                priority = resultPri,
                skill = skill,
                expectTarget = expectTarget,
                expectPos = expectPos,
                isEscape = false
            };

            return analyseResult;
        }

        // public AIAnalyseResult GetSkillPriority2(Skill skill)
        // {
        //     if (!skill.isNormalAttack && skill.state != ReleaseSkillState.ReadyRelease)
        //     {
        //         //技能的不能释放就跳过
        //         return new AIAnalyseResult()
        //         {
        //             priority = 0.0f
        //         };
        //     }
        //     
        //     
        //
        //
        //     var brave = dimensionMgr.GetValue(CharacterDimensionType.Bravery);
        //     var love = dimensionMgr.GetValue(CharacterDimensionType.Compassion);
        //     var tenacity = dimensionMgr.GetValue(CharacterDimensionType.Tenacity);
        //
        //     //攻击技能（包括普攻）倾向
        //     var attackResult = GetSkillPriorityByTend(skill, SkillActionTendencyType.Attack);
        //     attackResult.priority = attackResult.priority *
        //                             MathTool.Clamp(brave, 0.0f, 1.0f);
        //
        //     //防御技能 倾向
        //     var defenceResult = GetSkillPriority_Defence(skill);
        //     defenceResult.priority = defenceResult.priority *
        //                              MathTool.Clamp(tenacity, 0.0f, 1.0f);
        //
        //     //辅助技能 倾向(这里仁爱只要符合条件，无论高低都会有释放辅助技能的优先级，只不过目标不一样
        //     //仁爱高的时候倾向给队友释放，仁爱低的时候倾向给自己释放)
        //     var assistantResult = GetSkillPriority_Assistant(skill);
        //     assistantResult.priority = assistantResult.priority;
        //
        //     //逃跑技能倾向
        //     var escapeResult = GetEscapePriority_Skill(skill);
        //     escapeResult.priority = escapeResult.priority * MathTool.Clamp(1 - tenacity, 0.0f, 1.0f);
        //
        //     // if (this.entity.playerIndex == 0 && skill.isNormalAttack)
        //     // {
        //     //     Battle_Log.LogZxy($"zxy : " +
        //     //                       $"攻击倾向:{attackResult.priority}" +
        //     //                       $"防御倾向:{defenceResult.priority}" +
        //     //                       $"辅助倾向:{assistantResult.priority}" +
        //     //                       $"逃跑技能倾向:{escapeResult.priority}");
        //     // }
        //
        //     List<AIAnalyseResult> resultList = new List<AIAnalyseResult>()
        //     {
        //         attackResult, defenceResult, assistantResult, escapeResult
        //     };
        //
        //     // 找出具有最大 priority 的 AIAnalyseResult
        //     AIAnalyseResult maxPriorityResult = resultList.OrderByDescending(r => r.priority).FirstOrDefault();
        //
        //
        //     //计算最终倾向
        //     var expectTarget = maxPriorityResult.expectTarget;
        //     var expectPos = maxPriorityResult.expectPos;
        //     var resultPriority = maxPriorityResult.priority;
        //
        //     var analyseResult = new AIAnalyseResult()
        //     {
        //         priority = resultPriority,
        //         skill = skill,
        //         expectTarget = expectTarget,
        //         expectPos = expectPos,
        //         isEscape = maxPriorityResult == escapeResult
        //     };
        //
        //     return analyseResult;
        // }
    }
}