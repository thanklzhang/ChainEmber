// using System.Collections.Generic;
//
// namespace Battle
// {
//     public class PlayerActionMgr
//     {
//         List<PlayerAction> actionList = new List<PlayerAction>();
//         Battle battle;
//
//         public void Init(Battle battle)
//         {
//             this.battle = battle;
//         }
//
//         public void SetBattle(Battle battle)
//         {
//             this.battle = battle;
//         }
//
//         internal void AddPlayerAction(PlayerAction action)
//         {
//            // _Battle_Log.Log("add player action : " + action.ToString());
//
//             actionList.Add(action);
//         }
//
//         internal void Update()
//         {
//             //处理全部 actionList ， 这里可以固定 1 帧只处理 10 个玩家操作
//
//             for (int i = 0; i < actionList.Count; i++)
//             {
//                 var action = actionList[i];
//                 // _Battle_Log.Log("handle player action " + action.ToString() +
//                 //     "the frame : " + battle.currFrame);
//                 action.Handle(battle);
//             }
//
//             actionList.Clear();
//         }
//     }
//
// }
//
