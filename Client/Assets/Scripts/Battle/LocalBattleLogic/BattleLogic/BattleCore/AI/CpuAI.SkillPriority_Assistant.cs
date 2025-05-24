// using System;
// using System.Collections.Generic;
// using System.Linq;
// using Config;
//
// namespace Battle
// {
//     public partial class CpuAI : BaseAI
//     {
//         //攻击倾向（攻击技能和普攻）
//         public AIAnalyseResult GetSkillPriority_Assistant(Skill skill)
//         {
//             var brave = dimensionMgr.GetValue(CharacterDimensionType.Bravery);
//             var love = dimensionMgr.GetValue(CharacterDimensionType.Compassion);
//             var tenacity = dimensionMgr.GetValue(CharacterDimensionType.Tenacity);
//
//             BattleEntity expectTarget = null;
//             Vector3 expectPos = Vector3.zero;
//             List<SkillTagType> tags = skill.infoConfig.TagList.Select(t => (SkillTagType)t).ToList();
//
//             //选择目标筛选类型
//             var selectEntityType = (EntityRelationFilterType)skill.infoConfig.EntityRelationFilterType;
//
//             //释放目标类型
//             var releaseTargetType = (SkillReleaseTargetType)skill.infoConfig.SkillReleaseTargeType;
//
//             //技能效果目标类型
//             var effectTarget = (SkillEffectTargetType)skill.infoConfig.SkillEffectTargetType;
//
//             //是否是区域技能
//             var areaId = Skill_Tool.GetKeyAreaEft(skill.configId);
//             bool isAreaSkill = areaId > 0;
//             bool isAreaPointSkill = areaId > 0 && releaseTargetType == SkillReleaseTargetType.Point;
//             var hpRatio = this.entity.GetCurrHpRatio();
//             float assistantPriority = 0;
//             for (int i = 0; i < tags.Count; i++)
//             {
//                 var tag = tags[i];
//                 if (!skill.isNormalAttack && skill.state != ReleaseSkillState.ReadyRelease)
//                 {
//                     //技能的不能释放就跳过
//                     assistantPriority = 0.0f;
//                 }
//                 else
//                 {
//                     if (tag == SkillTagType.Healing || tag == SkillTagType.Protect ||
//                         tag == SkillTagType.Buff_Attack ||
//                         tag == SkillTagType.Buff_Defence ||
//                         tag == SkillTagType.Buff_Move)
//                     {
//                         if (selectEntityType == EntityRelationFilterType.Friend ||
//                             selectEntityType == EntityRelationFilterType.MeAndFriend)
//                         {
//                             float selectHpRatio = 0.8f;
//                             //区域
//                             if (isAreaPointSkill)
//                             {
//                                
//                                 var _info = GetAreaPriority_Assistant_Skill(areaId, skill, tag);
//                                 assistantPriority = love * _info.Item1;
//                                 var assExpectPos = _info.Item2;
//
//                                 if (selectEntityType == EntityRelationFilterType.Friend)
//                                 {
//                                     expectPos = assExpectPos;
//                                 }
//                                 else
//                                 {
//                                     var selfPriority = (1 - hpRatio) * 6.0f * (1 - love);
//                                     if (hpRatio >= selectHpRatio)
//                                     {
//                                         selfPriority = 0;
//                                     }
//
//                                     assistantPriority = Math.Max(assistantPriority, selfPriority);
//
//                                     expectPos = assistantPriority > selfPriority ? assExpectPos : this.entity.position;
//                                 }
//                             }
//                             else if (releaseTargetType == SkillReleaseTargetType.Entity)
//                             {
//                                 //单个实体
//
//                                 var _info = GetSingleTargetEntityPriority_Assistant_Skill(skill);
//
//                                 assistantPriority += _info.Item1;
//                                 var assExpectTarget = _info.Item2;
//
//                                 if (selectEntityType == EntityRelationFilterType.Friend)
//                                 {
//                                     expectTarget = assExpectTarget;
//                                 }
//                                 else
//                                 {
//                                     var selfPriority = (1 - hpRatio) * 6.0f * (1 - love);
//                                     if (hpRatio >= selectHpRatio)
//                                     {
//                                         selfPriority = 0;
//                                     }
//                                     
//                                     assistantPriority = Math.Max(assistantPriority, selfPriority);
//
//                                     expectTarget = assistantPriority > selfPriority ? assExpectTarget : this.entity;
//                                 }
//                             }
//                             else if (releaseTargetType == SkillReleaseTargetType.Point)
//                             {
//                                 //点 先简单处理 ，应该是按照技能效果来区分 例如 投掷物需要计算飞行途径的敌人数量和生命值百分比等这种 待定吧
//                                 //释放范围内所有单位点进行计算
//                                 var _info = GetSinglePointPriority_Assistant_Skill(skill);
//                                 assistantPriority += _info.Item1;
//                                 var assExpectPos = _info.Item2;
//
//                                 if (selectEntityType == EntityRelationFilterType.Friend)
//                                 {
//                                     expectPos = assExpectPos;
//                                 }
//                                 else
//                                 {
//                                    
//                                     var selfPriority = (1 - hpRatio) * 6.0f * (1 - love);
//                                     if (hpRatio >= selectHpRatio)
//                                     {
//                                         selfPriority = 0;
//                                     }
//                                     
//                                     assistantPriority = Math.Max(assistantPriority, selfPriority);
//
//                                     expectPos = assistantPriority > selfPriority ? assExpectPos : this.entity.position;
//                                 }
//                             }
//                         }
//                     }
//                 }
//             }
//
//             var analyseResult = new AIAnalyseResult()
//             {
//                 priority = assistantPriority,
//                 skill = skill,
//                 expectTarget = expectTarget,
//                 expectPos = expectPos
//             };
//
//             return analyseResult;
//         }
//     }
// }