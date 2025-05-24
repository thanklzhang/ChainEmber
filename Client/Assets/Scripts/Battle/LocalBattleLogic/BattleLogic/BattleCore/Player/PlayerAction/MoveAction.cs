// namespace Battle
// {
//     public class MoveAction : PlayerAction
//     {
//         public int moveEntityGuid;
//         public Vector3 targetPos;
//         public override void Handle(Battle battle)
//         {
//             //var entity = battle.FindEntity(moveEntityGuid);
//             //if (null == entity || entity.EntityState == EntityState.Dead)
//             //{
//             //    _Battle_Log.Log("the player's entity is null , perhaps dead");
//             //    return;
//             //}
//             ////entity.StartMoveToPos(targetPos);
//             //entity.AskMoveToPos(targetPos);
//
//
//             var player = battle.FindPlayerByEntityGuid(moveEntityGuid);
//             player.EntityCtrl.AskMoveToPos(targetPos);
//             
//             // var entityAI = battle.FindAI(moveEntityGuid);
//             // entityAI?.AskMoveToPos(targetPos);
//
//
//
//         }
//     }
// }
//
//
