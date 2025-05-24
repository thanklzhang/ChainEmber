// namespace Battle
// {
//     public class UseItemAction : PlayerAction
//     {
//         public Battle_ItemUseArg arg;
//         public override void Handle(Battle battle)
//         {
//             //var releaserEntity = battle.FindEntity(releaserGuid);
//             ////releaserEntity.ReleaseSkill(skillId, targetGuid, targetPos);
//             //releaserEntity.AskReleaseSkill(skillId, targetGuid, targetPos);
//
//             var entityAI = battle.FindAI(arg.releaserGuid);
//             if (entityAI != null)
//             {
//                 entityAI.AskUseItem(arg);
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
