// namespace Battle
// {
//     public class Battle_SelectBoxRewardArg
//     {
//         public int releaserGuid;
//         public int index;
//     }
//
//     public class SelectBoxRewardAction : PlayerAction
//     {
//         public Battle_SelectBoxRewardArg arg;
//         public override void Handle(Battle battle)
//         {
//             //var releaserEntity = battle.FindEntity(releaserGuid);
//             ////releaserEntity.ReleaseSkill(skillId, targetGuid, targetPos);
//             //releaserEntity.AskReleaseSkill(skillId, targetGuid, targetPos);
//
//             var entityAI = battle.FindAI(arg.releaserGuid);
//             if (entityAI != null)
//             {
//                 entityAI.AskSelectBoxReward(arg);
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
