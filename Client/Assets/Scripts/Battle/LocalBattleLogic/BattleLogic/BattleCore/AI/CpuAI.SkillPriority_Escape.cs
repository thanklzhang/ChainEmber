// using System;
// using System.Collections.Generic;
// using System.Linq;
// using Config;
//
// namespace Battle
// {
//     public partial class CpuAI : BaseAI
//     {
//         //逃跑技能倾向
//          public AIAnalyseResult GetEscapePriority_Skill(Skill skill)
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
//             
//             float escapePriority = 0;
//             for (int i = 0; i < tags.Count; i++)
//             {
//                 var tag = tags[i];
//                 if (tag == SkillTagType.Displacement)
//                 {
//                     if (selectEntityType == EntityRelationFilterType.Self)
//                     {
//                         if (releaseTargetType == SkillReleaseTargetType.Point)
//                         {
//                             var _info = GetSinglePosPriority_Escape_Skill_Self(skill);
//                             escapePriority = _info.Item1;
//                             expectPos = _info.Item2;
//                         }
//                     }
//                 }
//                 else if (tag == SkillTagType.Buff_Move)
//                 {
//                     if (selectEntityType == EntityRelationFilterType.Self)
//                     {
//                         var _info = GetSingleTargetPriority_Escape_Skill_Self(skill);
//                         //不管是什么目标类型 都是跟自己有关
//                         escapePriority = _info.Item1;
//                         expectTarget = _info.Item2;
//                         expectPos = _info.Item3;
//                         
//                     }
//                 }
//             }
//
//             var analyseResult = new AIAnalyseResult()
//             {
//                 priority = escapePriority,
//                 skill = skill,
//                 expectTarget = expectTarget,
//                 expectPos = expectPos
//             };
//
//             return analyseResult;
//         }
//
//       
//     }
// }