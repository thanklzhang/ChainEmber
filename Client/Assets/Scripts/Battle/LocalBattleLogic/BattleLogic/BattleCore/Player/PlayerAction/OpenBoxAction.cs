// namespace Battle
// {
//     public class Battle_OpenBoxArg
//     {
//         public int releaserGuid;
//     }
//
//     public class OpenBoxAction : PlayerAction
//     {
//         public Battle_OpenBoxArg arg;
//         public override void Handle(Battle battle)
//         {
//             //var releaserEntity = battle.FindEntity(releaserGuid);
//             ////releaserEntity.ReleaseSkill(skillId, targetGuid, targetPos);
//             //releaserEntity.AskReleaseSkill(skillId, targetGuid, targetPos);
//
//             var entityAI = battle.FindAI(arg.releaserGuid);
//             if (entityAI != null)
//             {
//                 entityAI.AskOpenBox(arg);
//             }
//             else
//             {
//                 Logx.LogWarning("the entityAI is not found : releaserGuid : " + arg.releaserGuid);
//             }
//             
//         }
//     }
// }
//
//
