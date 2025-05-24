// using System.Collections;
// using Battle;
// using Battle_Client;
// using System.Collections.Generic;
// using GameData;
// using UnityEngine;
// using Vector3 = Battle.Vector3;
//
// namespace Battle_Client
// {
//     //战斗逻辑中接受玩家消息的接受器
//      public class LocalBattleLogic_MsgReceiver : IPlayerMsgReceiver
//      {
//          Battle.Battle battle;
//     
//          public LocalBattleLogic_MsgReceiver(Battle.Battle battle)
//          {
//              this.battle = battle;
//          }
//     
//          
//          
//          public void On_PlayerLoadProgress(int uid, int progress)
//          {
//              GameMain.Instance.StartCoroutine(DelayLoadProgress(progress));
//
//              
//              //battle.SetPlayerProgress(uid, progress);
//          }
//          
//          IEnumerator DelayLoadProgress(int progress)
//          {
//              var hero = BattleManager.Instance.GetLocalCtrlHero();
//
//              yield return new WaitForSeconds(0.5f);
//              // var myUid = GameDataManager.Instance.UserData.Uid;
//              //  
//              // // PlayerLoadProgress_BattleMsgArg msgArg = new PlayerLoadProgress_BattleMsgArg();
//              // // msg.progress = progress;
//              // //
//              var arg = new PlayerLoadProgress_MsgArg()
//              {
//                  progress = progress
//              };
//
//              battle.OnRecvBattleMsg<PlayerLoadProgress_BattleMsg>(
//                  hero.playerIndex, arg);
//              //battle.PlayerMsgReceiver.On_PlayerLoadProgress((long)myUid, progress);
//          }
//     
//          public void On_BattleReadyFinish(long uid)
//          {
//              GameMain.Instance.StartCoroutine(DelayBattleReadyFinish());
//              // battle.PlayerReadyFinish(uid);
//          }
//          IEnumerator DelayBattleReadyFinish()
//          {
//              yield return new WaitForSeconds(0.1f);
//              var myUid = GameDataManager.Instance.UserData.Uid;
//
//              var hero = BattleManager.Instance.GetLocalCtrlHero();
//
//              var arg = new BattleReadyFinish_MsgArg()
//              {
//              };
//              battle.OnRecvBattleMsg<ReadyFinish_BattleMsg>(
//                  hero.playerIndex, arg);
//              //battle.PlayerMsgReceiver.On_BattleReadyFinish((long)myUid);
//          }
//          
//          public void On_ClientPlotEnd(long uid)
//          {
//              //battle.PlayerPlotEnd(uid);
//              
//              var hero = BattleManager.Instance.GetLocalCtrlHero();
//              battle.OnRecvBattleMsg<ClientPlotEnd_BattleMsg>(
//                  hero.playerIndex, null);
//              
//          }
//     
//          public void On_MoveEntity(int entityGuid, Vector3 targetPos)
//          {
//              // var entity = battle.FindEntity(entityGuid);
//              // var player = battle.FindPlayerByUid(entity.uid);
//              // player.EntityCtrl.AskMoveToPos();
//              
//              // MoveAction action = new MoveAction()
//              // {
//              //     moveEntityGuid = entityGuid,
//              //     targetPos = targetPos
//              // };
//              // battle.AddPlayerAction(action);
//              
//              var pos = new Battle.Vector3(targetPos.x, targetPos.y, targetPos.z);
//
//              var hero = BattleManager.Instance.GetLocalCtrlHero();
//
//              var arg = new MoveEntity_MsgArg()
//              {
//                  moveEntityGuid = entityGuid,
//                  targetPos = pos,
//              };
//              battle.OnRecvBattleMsg<MoveEntity_BattleMsg>(
//                  hero.playerIndex, arg);
//          }
//     
//          public void On_UseSkill(int releaserGuid, int skillId, int targetGuid, Vector3 targetPos)
//          {
//              // UseSkillAction action = new UseSkillAction()
//              // {
//              //     releaserGuid = releaserGuid,
//              //     skillId = skillId,
//              //     targetGuid = targetGuid,
//              //     targetPos = new Vector3(targetPos.x, targetPos.y, targetPos.z)
//              // };
//              // battle.AddPlayerAction(action);
//              
//              var pos = new Battle.Vector3(targetPos.x, targetPos.y, targetPos.z);
//              //battle.PlayerMsgReceiver.On_UseSkill(releaserGuid, skillId, targetGuid, pos);
//
//              var hero = BattleManager.Instance.GetLocalCtrlHero();
//
//              var msg = new UseSkill_BattleMsgArg()
//              {
//                  releaserGuid = releaserGuid,
//                  skillId = skillId,
//                  targetGuid = targetGuid,
//                  targetPos = pos,
//              };
//              battle.OnRecvBattleMsg<UseSkill_BattleMsg>(
//                  hero.playerIndex, msg);
//          }
//     
//          public void On_UseItem(Battle_ItemUseArg itemUseArg)
//          {
//              // Logx.Log(LogxType.BattleItem,"local battle logic : On_UseItem : itemIndex : " + itemUseArg.itemIndex);
//              // UseItemAction useItemAction = new UseItemAction()
//              // {
//              //     arg = itemUseArg
//              // };
//              // battle.AddPlayerAction(useItemAction);
//              
//              var pos = new Battle.Vector3(itemUseArg.targetPos.x, itemUseArg.targetPos.y, itemUseArg.targetPos.z);
//              var hero = BattleManager.Instance.GetLocalCtrlHero();
//
//              var arg = new Battle_ItemUseArg()
//              {
//                  itemIndex = itemUseArg.itemIndex,
//                  releaserGuid = itemUseArg.releaserGuid,
//                  targetGuid = itemUseArg.targetGuid,
//                  targetPos = pos,
//              };
//
//              // var useItemMsg = new Battle_ItemUseArg()
//              // {
//              //     itemUseArg = arg
//              // };
//              battle.OnRecvBattleMsg<UseItem_BattleMsg>(
//                  hero.playerIndex, arg);
//              
//          }
//     
//          public void On_UseSkillItem(Battle_ItemUseArg itemUseArg)
//          {
//              // Logx.Log(LogxType.BattleItem,"local battle logic : On_UseSkillItem : itemIndex : " + itemUseArg.itemIndex);
//              // UseSkillItemAction useItemAction = new UseSkillItemAction()
//              // {
//              //     arg = itemUseArg
//              // };
//              // battle.AddPlayerAction(useItemAction);
//              
//              var pos = new Battle.Vector3(itemUseArg.targetPos.x, itemUseArg.targetPos.y, itemUseArg.targetPos.z);
//              var arg = new Battle_ItemUseArg()
//              {
//                  itemIndex = itemUseArg.itemIndex,
//                  releaserGuid = itemUseArg.releaserGuid,
//                  targetGuid = itemUseArg.targetGuid,
//                  targetPos = pos,
//              };
//              // battle.PlayerMsgReceiver.On_UseSkillItem(arg);
//
//              var hero = BattleManager.Instance.GetLocalCtrlHero();
//
//              // var useSkillItemMsg = new UseSkillItem_BattleMsg()
//              // {
//              //     itemUseArg = arg
//              // };
//              battle.OnRecvBattleMsg<UseSkillItem_BattleMsg>(
//                  hero.playerIndex, arg);
//              
//          }
//     
//          public void On_OpenBox(Battle_OpenBoxArg arg)
//          {
//              // Logx.Log(LogxType.BattleItem,"local battle logic : On_OpenBox ");
//              // var action = new OpenBoxAction()
//              // {
//              //     arg = arg
//              // };
//              // battle.AddPlayerAction(action);
//              
//              // Battle_OpenBoxArg arg = new Battle_OpenBoxArg();
//              // arg.releaserGuid = BattleManager.Instance.GetLocalCtrlHeroGuid();
//
//              var hero = BattleManager.Instance.GetLocalCtrlHero();
//
//              // var arg = new Battle_OpenBoxArg()
//              // {
//              //     msgArg = arg
//              // };
//
//              battle.OnRecvBattleMsg<OpenBox_BattleMsg>(
//                  hero.playerIndex, arg);
//          }
//     
//          public void On_SelectBoxReward(Battle_SelectBoxRewardArg arg)
//          {
//              // Logx.Log(LogxType.BattleItem,"local battle logic : On_SelectBoxReward ");
//              // var action = new SelectBoxRewardAction()
//              // {
//              //     arg = arg
//              // };
//              // battle.AddPlayerAction(action);
//              var hero = BattleManager.Instance.GetLocalCtrlHero();
//
//              // Battle_SelectBoxRewardArg arg = new Battle_SelectBoxRewardArg();
//              // arg.releaserGuid = BattleManager.Instance.GetLocalCtrlHeroGuid();
//              // arg.index = index;
//
//              // SelectBoxReward_BattleMsg msg = new SelectBoxReward_BattleMsg()
//              // {
//              //     arg = arg
//              // };
//
//              battle.OnRecvBattleMsg<SelectBoxReward_BattleMsg>(hero.playerIndex, arg);
//
//          }
//     }
// }