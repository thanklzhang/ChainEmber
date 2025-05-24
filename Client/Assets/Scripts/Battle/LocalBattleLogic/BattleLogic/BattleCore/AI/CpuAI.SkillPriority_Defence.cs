// using System;
// using System.Collections.Generic;
// using System.Linq;
// using Config;
//
// namespace Battle
// {
//     public partial class CpuAI : BaseAI
//     {
//         //防御倾向（坚韧）
//         public AIAnalyseResult GetSkillPriority_Defence(Skill skill)
//         {
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
//             //bool isArea = areaId > 0 && releaseTargetType == SkillReleaseTargetType.Point;
//             bool isAreaSkill = areaId > 0;
//             bool isAreaPointSkill = areaId > 0 && releaseTargetType == SkillReleaseTargetType.Point;
//             float defencePriority = 0;
//             for (int i = 0; i < tags.Count; i++)
//             {
//                 var tag = tags[i];
//                 if (!skill.isNormalAttack && skill.state != ReleaseSkillState.ReadyRelease)
//                 {
//                     //技能的不能释放就跳过
//                     defencePriority = 0.0f;
//                 }
//                 else
//                 {
//                     if (tag == SkillTagType.Defence || tag == SkillTagType.Buff_Defence)
//                     {
//                         if (selectEntityType == EntityRelationFilterType.Self ||
//                             selectEntityType == EntityRelationFilterType.MeAndFriend ||
//                             selectEntityType == EntityRelationFilterType.All)
//                         {
//                             if (isAreaPointSkill)
//                             {
//                                 // var _info = GetAreaPriority_Hurt_Skill(areaId, skill);
//                                 // defencePriority = _info.Item1;
//                                 // expectPos = _info.Item2;
//
//                                 defencePriority = 4.0f;
//                                 //相当于自己的位置 防止相同点的方向位置
//                                 expectPos = this.entity.position + new Vector3(0.1f, 0.0f, 0.1f);
//
//                             }
//                             else if (releaseTargetType == SkillReleaseTargetType.NoTarget)
//                             {
//                                 // //对自己释放的无目标技能
//                                 // var _info = GetSingleTargetEntityPriority_Hurt_Skill(skill);
//                                 // defencePriority += _info.Item1;
//                                 // expectTarget = _info.Item2;
//                                 
//                                 defencePriority = 4.0f;
//                                 //相当于自己的位置 防止相同点的方向位置
//                                 expectTarget = this.entity;
//                                 
//                             }
//                             else if (releaseTargetType == SkillReleaseTargetType.Point)
//                             {
//                                 // //点 对自己点释放
//                                 // var _info = GetSinglePointPriority_Hurt_Skill(skill);
//                                 // defencePriority += _info.Item1;
//                                 // expectPos = _info.Item2;
//                                 
//                                 defencePriority = 4.0f;
//                                 //相当于自己的位置 防止相同点的方向位置
//                                 expectPos = this.entity.position + new Vector3(0.1f, 0.0f, 0.1f);
//                             }
//                         }
//                     }
//                 }
//             }
//
//             var analyseResult = new AIAnalyseResult()
//             {
//                 priority = defencePriority,
//                 skill = skill,
//                 expectTarget = expectTarget,
//                 expectPos = expectPos
//             };
//
//             return analyseResult;
//         }
//     }
// }