// namespace Battle
// {
//     public class UseSkillAction : PlayerAction
//     {
//         public int releaserGuid;
//         public int targetGuid;
//         public Vector3 targetPos;
//         public int skillId;
//         public override void Handle(Battle battle)
//         {
//             //var releaserEntity = battle.FindEntity(releaserGuid);
//             ////releaserEntity.ReleaseSkill(skillId, targetGuid, targetPos);
//             //releaserEntity.AskReleaseSkill(skillId, targetGuid, targetPos);
//
//             var entityAI = battle.FindAI(releaserGuid);
//             if (entityAI != null)
//             {
//                 entityAI.AskReleaseSkill(skillId, targetGuid, targetPos);
//             }
//             else
//             {
//                 Logx.LogWarning("the entityAI is not found : releaserGuid : " + releaserGuid);
//             }
//             
//         }
//     }
// }
//
//
