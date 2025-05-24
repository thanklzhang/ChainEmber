// using Google.Protobuf;
// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Text;
// using System.Threading.Tasks;
// using UnityEngine;
// using NetProto;
// using GameData;
//
// public class MainTaskNetHandler : NetHandler
// {
//     public Action syncMainAction;
//     public Action finishStageAction;
//     public Action receiveRewardAction;
//     public override void Init()
//     {
//
//         AddListener((int)ProtoIDs.SyncMainTask, OnSyncMainTask);
//         AddListener((int)ProtoIDs.ApplyMainTaskBattle, OnApplyMainTaskBattle);
//         AddListener((int)ProtoIDs.FinishMainTaskStage, OnFinishStage);
//         AddListener((int)ProtoIDs.ReceiveMainTaskRward, OnReceiveRward);
//
//     }
//
//     //同步主线所有数据
//     public void SendSyncMainTask(Action action)
//     {
//         syncMainAction = action;
//         csSyncMainTask syncMainTask = new csSyncMainTask()
//         {
//
//         };
//         NetworkManager.Instance.SendMsg(ProtoIDs.SyncMainTask, syncMainTask.ToByteArray());
//
//     }
//
//     public void OnSyncMainTask(MsgPack msgPack)
//     {
//         scSyncMainTask mainTaskData = scSyncMainTask.Parser.ParseFrom(msgPack.data);
//
//         //convert
//         List<MainTaskChapterData> chapterList = new List<MainTaskChapterData>();
//         //chapter
//         foreach (var serChapter in mainTaskData.Chapters)
//         {
//             MainTaskChapterData chapterData = new MainTaskChapterData();
//             chapterData.id = serChapter.Id;
//             chapterData.state = (MainTaskChapterState)serChapter.State;
//
//             //stage
//             List<MainTaskStageData> stageList = new List<MainTaskStageData>();
//             foreach (var serStage in serChapter.Stages)
//             {
//                 MainTaskStageData stageData = new MainTaskStageData();
//                 stageData.id = serStage.Id;
//                 stageData.state = (MainTaskStageState)serStage.State;
//                 stageList.Add(stageData);
//             }
//             chapterData.SetStageList(stageList);
//
//             chapterList.Add(chapterData);
//         }
//
//
//         GameDataManager.Instance.MainTaskStore.SetMainTaskData(chapterList);
//         syncMainAction?.Invoke();
//         syncMainAction = null;
//
//         EventDispatcher.Broadcast(EventIDs.OnRefreshAllMainTaskData);
//
//     }
//
//     ////申请 主线 战斗
//     //public void SendApplyMainTaskBattle(int chapterId, int stageId, Action action)
//     //{
//     //    csApplyMainTaskBattle apply = new csApplyMainTaskBattle();
//     //    apply.ChapterId = chapterId;
//     //    apply.StageId = stageId;
//     //    NetworkManager.Instance.SendMsg(ProtoIDs.ApplyMainTaskBattle, apply.ToByteArray());
//     //}
//
//     public void OnApplyMainTaskBattle(MsgPack msgPack)
//     {
//         scApplyMainTaskBattle resp = scApplyMainTaskBattle.Parser.ParseFrom(msgPack.data);
//         var err = resp.Err;
//         if (0 == err)
//         {
//
//         }
//         else
//         {
//             LogNetErrStr(msgPack.cmdId, err);
//         }
//     }
//
//     //完成关卡
//     public void SendFinishStage(int chapterId, int stageId, Action action)
//     {
//         finishStageAction = action;
//         csFinishMainTaskStage finishStageTask = new csFinishMainTaskStage()
//         {
//             ChapterId = chapterId,
//             StageId = stageId
//         };
//         NetworkManager.Instance.SendMsg(ProtoIDs.FinishMainTaskStage, finishStageTask.ToByteArray());
//
//     }
//
//     public void OnFinishStage(MsgPack msgPack)
//     {
//         scFinishMainTaskStage finishStageData = scFinishMainTaskStage.Parser.ParseFrom(msgPack.data);
//         var err = finishStageData.Err;
//         if (0 == err)
//         {
//             var chapterId = finishStageData.ChapterId;
//             var resultStageId = finishStageData.StageId;
//             //GameDataManager.Instance.MainTaskStore.SetStageState(chapterId, resultStageId, MainTaskStageState.HasFinish);
//         }
//         else
//         {
//             LogNetErrStr(msgPack.cmdId, err);
//         }
//         finishStageAction?.Invoke();
//         finishStageAction = null;
//
//     }
//
//     //领取奖励 , type : 0:stage , 1:chapter
//     public void SendReceiveRward(int type, int id, Action action)
//     {
//         receiveRewardAction = action;
//         csReceiveMainTaskRward csRecv = new csReceiveMainTaskRward()
//         {
//             Type = type,
//         };
//         if (0 == type)
//         {
//             //stage
//             csRecv.StageId = id;
//         }
//         else
//         {
//             //chapter
//             csRecv.ChapterId = id;
//         }
//         NetworkManager.Instance.SendMsg(ProtoIDs.ReceiveMainTaskRward, csRecv.ToByteArray());
//
//     }
//
//     public void OnReceiveRward(MsgPack msgPack)
//     {
//         scReceiveMainTaskRward receiveRwardData = scReceiveMainTaskRward.Parser.ParseFrom(msgPack.data);
//         var err = receiveRwardData.Err;
//         if (0 == err)
//         {
//             if (0 == receiveRwardData.Type)
//             {
//                 //关卡
//                 var chapterId = receiveRwardData.ChapterId;
//                 var resultStageId = receiveRwardData.StageId;
//                 //GameDataManager.Instance.MainTaskStore.SetStageState(chapterId, resultStageId, MainTaskStageState.HasReceive);
//             }
//             else
//             {
//                 //chapter
//             }
//
//             CtrlManager.Instance.ShowTips("领取奖励成功");
//         }
//         else
//         {
//             LogNetErrStr(msgPack.cmdId, err);
//         }
//         receiveRewardAction?.Invoke();
//         receiveRewardAction = null;
//
//         EventDispatcher.Broadcast(EventIDs.OnRefreshAllMainTaskData);
//     }
//
//
//
// }
//
