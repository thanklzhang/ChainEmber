// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Text;
// using System.Threading.Tasks;
// using Battle;
// using Battle_Client;
// using GameData;
// using NetProto;
// using UnityEngine;
//
// namespace Battle_Client
// {
//     //战斗客户端消息接收器
//     public class BattleClient_MsgReceiver_Impl : IBattleClientMsgReceiver
//     {
//         public void On_PlayerReadyState(AllPlayerLoadFinish_RecvMsg_Arg arg)
//         {
//             BattleManager.Instance.RecvBattleMsg<PlayerReadState_RecvMsg>(arg);
//         }
//
//         // public void On_PlayerReadyState(int uid, bool isReady)
//         // {
//         //     //EventDispatcher.Broadcast<int, bool>(EventIDs.OnPlayerReadyState, uid, isReady);
//         //
//         //     var arg = new PlayerReadState_RecvMsg_Arg()
//         //     {
//         //         playerIndex = uid,
//         //         isReady = isReady
//         //         
//         //     };
//         //     BattleManager.Instance.RecvBattleMsg<PlayerReadState_RecvMsg>(arg);
//         // }
//
//         public void On_AllPlayerLoadFinish(AllPlayerLoadFinish_RecvMsg_Arg arg)
//         {
//             BattleManager.Instance.RecvBattleMsg<AllPlayerLoadFinish_RecvMsg>(arg);
//             // EventDispatcher.Broadcast(EventIDs.OnAllPlayerLoadFinish);
//         }
//
//         public void On_StartBattle()
//         {
//             EventDispatcher.Broadcast(EventIDs.OnBattleStart);
//
//             //已经加载好的实体统一走一遍创建流程
//             BattleEntityManager.Instance.NotifyCreateAllEntities();
//
//             BattleManager.Instance.BattleState = Battle_Client.BattleState.Running;
//         }
//
//         public void On_CreateEntities(List<BattleClientMsg_Entity> entityList)
//         {
//             for (int i = 0; i < entityList.Count; i++)
//             {
//                 var msgEntity = entityList[i];
//                 var entity = BattleEntityManager.Instance.CreateEntity(msgEntity);
//                 // EventDispatcher.Broadcast<BattleEntity_Client>(EventIDs.OnCreateEntity, entity);
//             }
//         }
//
//         public void On_CreateSkillEffect(CreateEffectInfo createEffectInfo)
//         {
//             //var lastTime = lastTimeInt / 1000.0f;
//             BattleSkillEffect_Client_Manager.Instance.CreateSkillEffect(createEffectInfo);
//         }
//
//         public void On_DestroySkillEffect(int effectGuid)
//         {
//             //如果是 buff 先处理 UI 显示
//             var effect = BattleSkillEffect_Client_Manager.Instance.FindSkillEffect(effectGuid);
//             if (effect != null)
//             {
//                 var targetEntityGuid = effect.GetFollowEntityGuid();
//
//                 if (targetEntityGuid > 0)
//                 {
//                     //var entity = BattleEntityManager.Instance.FindEntity(targetEntityGuid);
//                     //if (entity != null)
//                     //{
//
//                     //}
//                     BuffEffectInfo_Client buffUIData = new BuffEffectInfo_Client()
//                     {
//                         targetEntityGuid = targetEntityGuid,
//                         guid = effectGuid,
//                         isRemove = true
//                     };
//                     EventDispatcher.Broadcast(EventIDs.OnBuffInfoUpdate, buffUIData);
//                 }
//             }
//
//
//             BattleSkillEffect_Client_Manager.Instance.DestorySkillEffect(effectGuid);
//         }
//
//         public void On_EntityDead(int entityGuid)
//         {
//             var entity = BattleEntityManager.Instance.FindEntity(entityGuid);
//             if (entity != null)
//             {
//                 entity.Dead();
//             }
//         }
//
//         //public void On_EntityStartMove(int Guid, UnityEngine.Vector3 EndPos, UnityEngine.Vector3 dir, float MoveSpeed)
//         //{
//         //    var guid = Guid;
//         //    var targetPos = EndPos;
//         //    var moveSpeed = MoveSpeed;
//         //    var entity = BattleEntityManager.Instance.FindEntity(guid);
//         //    if (entity != null)
//         //    {
//         //        entity.StartMove(targetPos, dir, moveSpeed);
//         //    }
//         //}
//
//
//         public void On_EntityStartMoveByPath(int Guid, List<UnityEngine.Vector3> EndPos, float MoveSpeed)
//         {
//             var guid = Guid;
//             var targetPos = EndPos;
//             var moveSpeed = MoveSpeed;
//             var entity = BattleEntityManager.Instance.FindEntity(guid);
//             if (entity != null)
//             {
//                 entity.StartMoveByPath(EndPos, moveSpeed);
//             }
//         }
//
//         public void On_EntityStopMove(int Guid, UnityEngine.Vector3 EndPos)
//         {
//             var guid = Guid;
//             var endPos = EndPos;
//             var entity = BattleEntityManager.Instance.FindEntity(guid);
//             if (entity != null)
//             {
//                 entity.StopMove(endPos);
//             }
//         }
//
//
//         public void On_EntitySyncDir(int guid, UnityEngine.Vector3 dir)
//         {
//             var entity = BattleEntityManager.Instance.FindEntity(guid);
//             if (entity != null)
//             {
//                 entity.SetToward(dir);
//             }
//         }
//
//         public void On_EntityUseSkill(int entityGuid, int skillConfig)
//         {
//             var guid = entityGuid;
//             var entity = BattleEntityManager.Instance.FindEntity(guid);
//             if (entity != null)
//             {
//                 entity.ReleaseSkill(skillConfig);
//             }
//         }
//
//         public void On_PlayPlot(string plotName)
//         {
//             var name = plotName;
//
//             BattleEntityManager.Instance.SetAllEntityShowState(false);
//             CameraManager.Instance.GetCameraUI().SetUICameraShowState(false);
//
//             PlotManager.Instance.StartPlot(name);
//         }
//
//         public void On_PlotEnd()
//         {
//             BattleEntityManager.Instance.SetAllEntityShowState(true);
//             CameraManager.Instance.GetCameraUI().SetUICameraShowState(true);
//
//             PlotManager.Instance.ClosePlot();
//         }
//
//         public void On_SetEntitiesShowState(List<int> Guids, bool isShow)
//         {
//             var entityGuids = Guids;
//             BattleEntityManager.Instance.SetEntitiesShowState(isShow, entityGuids);
//         }
//
//         public void On_SkillEffectStartMove(int EffectGuid, UnityEngine.Vector3 TargetPos, int TargetGuid,
//             float moveSpeed)
//         {
//             var guid = EffectGuid;
//             var skillEffect = BattleSkillEffect_Client_Manager.Instance.FindSkillEffect(guid);
//             if (skillEffect != null)
//             {
//                 skillEffect.StartMove(TargetPos, TargetGuid, moveSpeed);
//             }
//         }
//
//
//         public void On_SyncEntityAttr(int entityGuid, List<BattleClientMsg_BattleAttr> atts)
//         {
//             var entity = BattleEntityManager.Instance.FindEntity(entityGuid);
//             if (entity != null)
//             {
//                 entity.SyncAttr(atts);
//             }
//         }
//
//         public void On_SyncEntityValue(int entityGuid, List<BattleClientMsg_BattleValue> values)
//         {
//             var entity = BattleEntityManager.Instance.FindEntity(entityGuid);
//             if (entity != null)
//             {
//                 entity.SyncValue(values);
//             }
//         }
//
//         public void On_BattleEnd(bool isWin)
//         {
//             //BattleManager.Instance.BattleState = BattleState.End;
//
//             //EventDispatcher.Broadcast<bool>(EventIDs.OnBattleEnd, isWin);
//         }
//
//         public void On_SkillInfoUpdate(int entityGuid, int skillConfigId, float currCDTime, float maxCDTime)
//         {
//             var entity = BattleEntityManager.Instance.FindEntity(entityGuid);
//             if (entity != null)
//             {
//                 entity.UpdateSkillInfo(skillConfigId, currCDTime, maxCDTime);
//             }
//         }
//         
//         public void On_ItemInfoUpdate(int entityGuid, int index,int configId, int count,float currCDTime, float maxCDTime)
//         {
//             var entity = BattleEntityManager.Instance.FindEntity(entityGuid);
//             if (entity != null)
//             {
//                 entity.UpdateItemInfo(index, configId,count,currCDTime, maxCDTime);
//             }
//         }
//         
//         public void On_SkillItemInfoUpdate(int entityGuid, int index,int configId, int count,float currCDTime, float maxCDTime)
//         {
//             var entity = BattleEntityManager.Instance.FindEntity(entityGuid);
//             if (entity != null)
//             {
//                 entity.UpdateSkillItemInfo(index, configId,count,currCDTime, maxCDTime);
//             }
//         }
//
//         public void On_BuffInfoUpdate(BuffEffectInfo buffInfo)
//         {
//             var effect = BattleSkillEffect_Client_Manager.Instance.FindSkillEffect(buffInfo.guid);
//
//             if (effect != null)
//             {
//                 effect.SetBuffInfo(buffInfo);
//             }
//         }
//
//         public void On_SkillTrackStart(BattleClientMsg_CreateSkillTrack create)
//         {
//             TrackBean trackBean = new TrackBean()
//              {
//                  trackConfigId = create.trackConfigId,
//                  releaserGuid = create.releaserEntityGuid,
//                  targetPos = create.targetPos,
//                  targetEntityGuid = create.targetEntityGuid
//              };
//             
//              EventDispatcher.Broadcast<TrackBean>(EventIDs.OnSkillTrackStart, trackBean);
//         }
//
//         public void On_SkillTrackEnd(int entityGuid, int skillTrackConfigId)
//         {
//             EventDispatcher.Broadcast<int, int>(EventIDs.OnSkillTrackEnd, entityGuid, skillTrackConfigId);
//         }
//
//         public void On_BoxInfoUpdate(int entityGuid,List<BattleClientMsg_BattleBox> boxList)
//         {
//             //目前开箱子只是自己开
//             var hero = BattleManager.Instance.GetLocalCtrlHero();
//             if (hero != null && entityGuid == hero.guid)
//             {
//                 hero.SetBoxList(boxList);
//             }
//         }
//
//         public void On_OpenBox(BattleClientMsg_BattleBox box)
//         {
//             var entity = BattleEntityManager.Instance.FindEntity(box.openEntityGuid);
//             if (entity != null)
//             {
//                  var hero = BattleManager.Instance.GetLocalCtrlHero();
//                  if (hero != null)
//                  {
//                      if (hero.guid == entity.guid)
//                      {
//                          hero.OnOpenBox(box);
//                      }
//                  }
//             }
//         }
//
//         public void On_SelectBoxReward(int index)
//         {
//             throw new NotImplementedException();
//         }
//     }
// }