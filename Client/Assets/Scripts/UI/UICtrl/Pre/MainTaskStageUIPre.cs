// using System;
// using System.Collections;
// using System.Collections.Generic;
// 
// using UnityEngine;
// using UnityEngine.UI;
//
// public class MainTaskStageUIData
// {
//     public int stageId;
//     //public bool isUnlock;
//     //public bool isFinish;
//     //public bool isHasRecvReward;
//     public MainTaskStageState state;
// }
//
//
// public class MainTaskStageUIArgs : UIArgs
// {
//     public int chapterId;
//     public int currStageId;
//     public List<MainTaskStageUIData> stageUIDataList;
// }
//
//
// public class MainTaskStageUIShowObj : BaseUIShowObj<MainTaskStageUIPre>
// {
//     Text nameText;
//     GameObject lockGo;
//     Button clickBtn;
//     GameObject selectGo;
//
//     MainTaskStageUIData uiData;
//
//     public override void OnInit()
//     {
//         nameText = transform.Find("root/Text").GetComponent<Text>();
//         lockGo = transform.Find("root/lock").gameObject;
//         selectGo = transform.Find("root/select").gameObject;
//         clickBtn = transform.Find("root/bg").GetComponent<Button>();
//
//         clickBtn.onClick.AddListener(() =>
//         {
//             this.parentObj.OnClickStageBtn(this.uiData.stageId);
//         });
//     }
//
//     public override void OnRefresh(object data, int index)
//     {
//         this.uiData = (MainTaskStageUIData)data;
//
//         var stageId = this.uiData.stageId;
//         var isUnlock = this.uiData.state != MainTaskStageState.Lock;
//
//
//         var currStageTb = Config.ConfigManager.Instance.GetById<Config.MainTaskStage>(stageId);
//         nameText.text = "" + currStageTb.Name;
//
//         lockGo.SetActive(!isUnlock);
//
//     }
//
//     public MainTaskStageUIData GetUIData()
//     {
//         return uiData;
//     }
//
//     public void SetSelectShow(bool isShow)
//     {
//         selectGo.SetActive(isShow);
//     }
//
//     public override void OnRelease()
//     {
//         clickBtn.onClick.RemoveAllListeners();
//     }
// }
//
// public class MainTaskStageUIPre : BaseUI
// {
//     public Action onClickCloseBtn;
//     public Action<int> onClickStageBtn;
//     public Action<int> onClickStartBtn;
//     public Action<int> onClickReceiveBtn;
//
//     public Button closeBtn;
//
//     //当前最大进度的关卡名
//     //public Text currStageNameText;
//
//     public Transform stageRoot;
//
//     //当前选中的关卡相关组件--
//     Text stageName;
//     Image stagePic;
//     Text stageDetailText;
//     Button startBtn;
//     Button receiveBtn;
//     GameObject hasReceveGo;
//
//
//     //---------------------
//
//     //data
//     public int currChapterId;
//     public int currStageId;
//     public List<MainTaskStageUIData> stageUIDataList;
//     public List<MainTaskStageUIShowObj> stageUIShowObjList;
//
//     protected override void OnInit()
//     {
//         closeBtn = this.transform.Find("closeBtn").GetComponent<Button>();
//
//         //currStageNameText = this.transform.Find("currProgressName").GetComponent<Text>();
//
//         stageRoot = this.transform.Find("stageRoot/mask/content");
//
//         this.stageName = this.transform.Find("stageName").GetComponent<Text>();
//         this.stageDetailText = this.transform.Find("describe").GetComponent<Text>();
//         this.startBtn = this.transform.Find("startBtn").GetComponent<Button>();
//         this.receiveBtn = this.transform.Find("receiveBtn").GetComponent<Button>();
//         this.hasReceveGo = this.transform.Find("hasReceive").gameObject;
//
//         closeBtn.onClick.AddListener(() =>
//         {
//             onClickCloseBtn?.Invoke();
//         });
//
//         this.startBtn.onClick.AddListener(() =>
//         {
//             onClickStartBtn?.Invoke(currStageId);
//         });
//
//         this.receiveBtn.onClick.AddListener(() =>
//         {
//             onClickReceiveBtn?.Invoke(currStageId);
//         });
//
//         stageUIDataList = new List<MainTaskStageUIData>();
//         stageUIShowObjList = new List<MainTaskStageUIShowObj>();
//     }
//
//     public override void Refresh(UIArgs args)
//     {
//         var resultArgs = (MainTaskStageUIArgs)args;
//         this.currChapterId = resultArgs.chapterId;
//         currStageId = resultArgs.currStageId;
//         this.stageUIDataList = resultArgs.stageUIDataList;
//
//         this.RefresStageList();
//
//         this.RefreshDetail();
//         //OnClickStageBtn(currStageId);
//
//     }
//
//     public MainTaskStageUIShowObj GetStageShowObjById(int stageId)
//     {
//         foreach (var showObj in stageUIShowObjList)
//         {
//             var uiData = showObj.GetUIData();
//             if (uiData.stageId == stageId)
//             {
//                 return showObj;
//             }
//         }
//         return null;
//     }
//
//     //刷新当前章节的所有 stage
//     public void RefresStageList()
//     {
//         //列表
//         UIListArgs<MainTaskStageUIShowObj, MainTaskStageUIPre> args = new UIListArgs<MainTaskStageUIShowObj, MainTaskStageUIPre>();
//         args.dataList = stageUIDataList;
//         args.showObjList = stageUIShowObjList;
//         args.root = stageRoot;
//         args.parentObj = this;
//         UIFunc.DoUIList(args);
//
//         //当前章节的最大关卡进度名称
//         //var stageTb = Config.ConfigManager.Instance.GetById<Config.MainTaskStage>(this.currStageId);
//         //this.currStageNameText.text = "当前关卡进度：" + stageTb.Name;
//     }
//
//     //刷新当前 stage 详情
//     public void RefreshDetail()
//     {
//         if (0 == this.currStageId)
//         {
//             return;
//         }
//         var stageTb = Config.ConfigManager.Instance.GetById<Config.MainTaskStage>(this.currStageId);
//         this.stageDetailText.text = stageTb.Describe;
//
//         this.stageName.text = stageTb.Name;
//
//         var showObj = GetStageShowObjById(currStageId);
//
//         var state = showObj.GetUIData().state;
//         if (state == MainTaskStageState.Lock)
//         {
//             //未解锁
//             startBtn.gameObject.SetActive(false);
//             receiveBtn.gameObject.SetActive(false);
//             hasReceveGo.gameObject.SetActive(false);
//         }
//         else if (state == MainTaskStageState.NoFinish)
//         {
//             //解锁状态
//             startBtn.gameObject.SetActive(true);
//             receiveBtn.gameObject.SetActive(false);
//             hasReceveGo.gameObject.SetActive(false);
//
//         }
//         else if (state == MainTaskStageState.HasFinish)
//         {
//             //完成状态
//             startBtn.gameObject.SetActive(false);
//             receiveBtn.gameObject.SetActive(true);
//             hasReceveGo.gameObject.SetActive(false);
//         }
//         else if (state == MainTaskStageState.HasReceive)
//         {
//             //已经领取状态
//             startBtn.gameObject.SetActive(false);
//             receiveBtn.gameObject.SetActive(false);
//             hasReceveGo.gameObject.SetActive(true);
//         }
//     }
//
//     //点击了一个 stage
//     public void OnClickStageBtn(int stageId)
//     {
//         var showObj = GetStageShowObjById(stageId);
//         if (showObj != null)
//         {
//             foreach (var currShowObj in stageUIShowObjList)
//             {
//                 currShowObj.SetSelectShow(false);
//             }
//
//             showObj.SetSelectShow(true);
//             onClickStageBtn?.Invoke(stageId);
//         }
//     }
//
//     protected override void OnUnload()
//     {
//         onClickCloseBtn = null;
//         onClickStageBtn = null;
//         onClickStartBtn = null;
//         onClickReceiveBtn = null;
//
//         foreach (var item in stageUIShowObjList)
//         {
//             item.Release();
//         }
//
//         this.closeBtn.onClick.RemoveAllListeners();
//         this.startBtn.onClick.RemoveAllListeners();
//         this.receiveBtn.onClick.RemoveAllListeners();
//     }
// }
