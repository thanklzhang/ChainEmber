// using System;
// using System.Collections;
// using System.Collections.Generic;
// 
// using UnityEngine;
// using UnityEngine.UI;
//
// public class MainTaskChapterUIData
// {
//     public int chapterId;
//     public MainTaskChapterState state;
//     //public bool isUnlock;
//     //public bool isFinish;
//     //public bool isHasRecvReward;
// }
//
//
// public class MainTaskUIArgs : UIArgs
// {
//     public int currChapterId;
//     public List<MainTaskChapterUIData> chapterUIDataList;
// }
//
// public class MainTaskChapterUIShowObj : BaseUIShowObj<MainTaskUIPre>
// {
//     Text nameText;
//     GameObject lockGo;
//     Button receiveBtn;
//     GameObject hasReceiveGo;
//     Button clickBtn;
//
//     MainTaskChapterUIData uiData;
//
//     public override void OnInit()
//     {
//         nameText = transform.Find("root/Text").GetComponent<Text>();
//         clickBtn = transform.Find("root/bg").GetComponent<Button>();
//
//         lockGo = transform.Find("root/lock").gameObject;
//         receiveBtn = transform.Find("root/receiveBtn").GetComponent<Button>();
//         hasReceiveGo = transform.Find("root/hasReceive").gameObject;
//
//         clickBtn.onClick.AddListener(() =>
//         {
//             this.parentObj.OnClickChapterBtn(this.uiData.chapterId);
//         });
//
//         receiveBtn.onClick.AddListener(() =>
//         {
//             this.parentObj.OnClickReceiveBtn(this.uiData.chapterId);
//         });
//     }
//
//     public override void OnRefresh(object data, int index)
//     {
//         this.uiData = (MainTaskChapterUIData)data;
//
//         var chapterId = this.uiData.chapterId;
//
//         var currChapterTb = Config.ConfigManager.Instance.GetById<Config.MainTaskChapter>(chapterId);
//         nameText.text = "" + currChapterTb.Name;
//
//         //刷新此时的状态
//         var state = this.uiData.state;
//         if (state == MainTaskChapterState.Lock)
//         {
//             lockGo.SetActive(true);
//             receiveBtn.gameObject.SetActive(false);
//             hasReceiveGo.SetActive(false);
//         }
//         else if (state == MainTaskChapterState.NoFinish)
//         {
//             lockGo.SetActive(false);
//             receiveBtn.gameObject.SetActive(false);
//             hasReceiveGo.SetActive(false);
//         }
//         else if (state == MainTaskChapterState.HasFinish)
//         {
//             lockGo.SetActive(false);
//             receiveBtn.gameObject.SetActive(true);
//             hasReceiveGo.SetActive(false);
//         }
//         else if (state == MainTaskChapterState.HasReceive)
//         {
//             lockGo.SetActive(false);
//             receiveBtn.gameObject.SetActive(false);
//             hasReceiveGo.SetActive(true);
//         }
//     }
//
//     public override void OnRelease()
//     {
//         clickBtn.onClick.RemoveAllListeners();
//         receiveBtn.onClick.RemoveAllListeners();
//     }
//
// }
//
// public class MainTaskUIPre : BaseUI
// {
//     //callback
//     public Action onClickCloseBtn;
//     public Action<int> onClickChapterBtn;
//     public Action<int> onClickReceiveBtn;
//
//     //ui component
//     Transform chapterRoot;
//     //Text currChapterNameText;
//     Button closeBtn;
//
//     //data
//     public int currChapterId;
//     public List<MainTaskChapterUIData> chapterUIDataList;
//     public List<MainTaskChapterUIShowObj> chapterUIShowObjList;
//
//     protected override void OnInit()
//     {
//         chapterRoot = transform.Find("chapterRoot/mask/content");
//         //currChapterNameText = transform.Find("currChapter").GetComponent<Text>();
//         closeBtn = transform.Find("closeBtn").GetComponent<Button>();
//
//         closeBtn.onClick.AddListener(() =>
//         {
//             onClickCloseBtn?.Invoke();
//         });
//
//         chapterUIDataList = new List<MainTaskChapterUIData>();
//         chapterUIShowObjList = new List<MainTaskChapterUIShowObj>();
//     }
//
//     public override void Refresh(UIArgs args)
//     {
//         MainTaskUIArgs mainTaskArgs = (MainTaskUIArgs)args;
//         currChapterId = mainTaskArgs.currChapterId;
//         chapterUIDataList = mainTaskArgs.chapterUIDataList;
//         this.RefreshChapterList();
//     }
//
//     void RefreshChapterList()
//     {
//         //当前章节
//         //var currChapterTb = Config.ConfigManager.Instance.GetById<Config.MainTaskChapter>(currChapterId);
//         //currChapterNameText.text = "当前进度：" + currChapterTb.Name;
//
//         //列表
//         UIListArgs<MainTaskChapterUIShowObj, MainTaskUIPre> args = new UIListArgs<MainTaskChapterUIShowObj, MainTaskUIPre>();
//         args.dataList = chapterUIDataList;
//         args.showObjList = chapterUIShowObjList;
//         args.root = chapterRoot;
//         args.parentObj = this;
//         UIFunc.DoUIList(args);
//     }
//
//     public void OnClickChapterBtn(int chapterId)
//     {
//         onClickChapterBtn?.Invoke(chapterId);
//     }
//
//     public void OnClickReceiveBtn(int chapterId)
//     {
//         onClickReceiveBtn?.Invoke(chapterId);
//     }
//
//     protected override void OnUnload()
//     {
//         //onGoInfoUIBtnClick = null;
//         onClickCloseBtn = null;
//         onClickChapterBtn = null;
//         onClickReceiveBtn = null;
//
//         foreach (var item in chapterUIShowObjList)
//         {
//             item.Release();
//         }
//
//         closeBtn.onClick.RemoveAllListeners();
//
//         //showDataObjList = null;
//         //cardDataList = null;
//
//     }
// }
