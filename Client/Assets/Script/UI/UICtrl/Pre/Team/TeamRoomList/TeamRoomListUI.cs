// using System;
// using System.Collections;
// using System.Collections.Generic;
// using GameData;
// 
// using UnityEngine;
// using UnityEngine.UI;
//
//
// public class TeamRoomListUI : BaseUI
// {
// //     // //this callback
// //     // public Action event_onClickCloseBtn;
// //     // public Action event_onClickCreateBtn;
// //     //
// //     // //single item callback
// //     // public Action<int> event_onClickSingleRoomJoinBtn;
// //
// //     //ui component
// //     Button closeBtn;
// //     Transform roomRoot;
// //     Button createBtn;
// //
// //     //data
// //     public List<TeamRoomUIData> roomDataList;
// //     public List<TeamRoomUIShowObj> roomShowObjList;
// //
// //     protected override void OnInit()
// //     {
// //         roomRoot = transform.Find("scroll/mask/content");
// //         closeBtn = transform.Find("closeBtn").GetComponent<Button>();
// //         createBtn = transform.Find("createBtn").GetComponent<Button>();
// //
// //         closeBtn.onClick.AddListener(() =>
// //         {
// //             //event_onClickCloseBtn?.Invoke();
// //             UICtrlManager.Instance.Exit<TeamRoomListUICtrl>();
// //         });
// //         createBtn.onClick.AddListener(() =>
// //         {
// //             //event_onClickCreateBtn?.Invoke();
// //             var net = NetHandlerManager.Instance.GetHandler<TeamNetHandler>();
// //             net.SendCreateTeamRoom(() =>
// //             {
// //                 var creatRoomData = GameDataManager.Instance.TeamStore.currEnterRoomData;
// //                 this.OnEnterRoomInfoUI(creatRoomData);
// //             });
// //         });
// //
// //         // roomDataList = new List<TeamRoomUIData>();
// //         roomShowObjList = new List<TeamRoomUIShowObj>();
// //     }
// //
// //
// //     public void OnEnterRoomInfoUI(TeamRoomData creatRoomData)
// //     {
// //         //UICtrlManager.Instance.ShowTitleBar(TitleBarIds.TeamRoomInfo);
// //
// //         //TODO 打开房间 info
// //
// //         // this._roomListUIPre.Hide();
// //         // this._roomInfoUIPre.Show();
// //
// //         // this.RefreshRoomInfoUI(creatRoomData);
// //     }
// //
// //
// //     // public void RefreshRoomInfoUI(TeamRoomData roomData)
// //     // {
// //     //     List<TeamRoomData> roomList = GameDataManager.Instance.TeamStore.RoomList;
// //     //
// //     //     var selfUid = GameDataManager.Instance.UserStore.Uid;
// //     //
// //     //     TeamRoomInfoUIArgs args = new TeamRoomInfoUIArgs();
// //     //     args.id = roomData.id;
// //     //     //var teamStageTb = Config.ConfigManager.Instance.GetById<Config.TeamStage>(roomData.teamStageId);
// //     //     args.stageConfigId = roomData.teamStageId;
// //     //     args.roomName = roomData.roomName;
// //     //     args.playerUIDataList = new List<TeamRoomPlayerUIData>();
// //     //
// //     //     foreach (var player in roomData.playerList)
// //     //     {
// //     //         TeamRoomPlayerUIData uiPlayer = ConvertToPlayerUIData(player);
// //     //         args.playerUIDataList.Add(uiPlayer);
// //     //     }
// //     //
// //     //     _roomInfoUIPre.Refresh(args);
// //     // }
// //     //
// //     //
// //
// //     protected override void OnEnter(UICtrlArgs args)
// //     {
// //         // TeamRoomListUIArgs teamArgs = (TeamRoomListUIArgs)args;
// //         // roomDataList = teamArgs.roomDataList;
// //         // this.RefreshRoomList();
// //     }
// //
// //     protected override void OnActive()
// //     {
// //         RefreshRoomList();
// //     }
// //
// //     void RefreshRoomList()
// //     {
// //         //列表
// //         // UIListArgs<TeamRoomUIShowObj, TeamRoomListUI> args = new UIListArgs<TeamRoomUIShowObj, TeamRoomListUI>();
// //         // args.dataList = roomDataList;
// //         // args.showObjList = roomShowObjList;
// //         // args.root = roomRoot;
// //         // args.parentObj = this;
// //         // UIFunc.DoUIList(args);
// //
// //         List<TeamRoomData> roomList = GameDataManager.Instance.TeamStore.RoomList;
// //         if (null == roomList)
// //         {
// //             roomList = new List<TeamRoomData>();
// //         }
// //
// //         for (int i = 0; i < roomList.Count; i++)
// //         {
// //             var roomData = roomList[i];
// //             GameObject go = null;
// //             if (i < this.transform.childCount)
// //             {
// //                 go = this.transform.GetChild(i - 1).gameObject;
// //             }
// //             else
// //             {
// //                 go = GameObject.Instantiate(this.transform.GetChild(0).gameObject, this.roomRoot, false);
// //             }
// //
// //             TeamRoomUIShowObj showObj = null;
// //
// //             if (i < this.roomShowObjList.Count)
// //             {
// //                 showObj = this.roomShowObjList[i];
// //             }
// //             else
// //             {
// //                 showObj = new TeamRoomUIShowObj();
// //                 this.roomShowObjList.Add(showObj);
// //                 showObj.Init(go);
// //             }
// //
// //             showObj.Refresh(roomData, i);
// //         }
// //     }
// //
// //     public void OnClickSingleRoomJoinBtn(int roomId)
// //     {
// //         //event_onClickSingleRoomJoinBtn?.Invoke(roomId);
// //     }
// //
// //     protected override void OnExit()
// //     {
// //         // event_onClickCloseBtn = null;
// //         // event_onClickCreateBtn = null;
// //         // event_onClickSingleRoomJoinBtn = null;
// //
// //         foreach (var item in roomShowObjList)
// //         {
// //             item.Release();
// //         }
// //
// //         closeBtn.onClick.RemoveAllListeners();
// //         createBtn.onClick.RemoveAllListeners();
// //     }
// // }
// //
// // //-----------------------------------------
// //
// // public class TeamRoomUIData
// // {
// //     public int id;
// //     public int teamStageId;
// //     public string roomName;
// //     public int currPlayerCount;
// //     public int totalPlayerCount;
// //     public string masterName;
// //     public string masterAvatarURL;
// // }
// //
// //
// // public class TeamRoomListUIArgs : UICtrlArgs
// // {
// //     // public List<TeamRoomUIData> roomDataList;
// // }
// //
// // public class TeamRoomUIShowObj
// // {
// //     public Transform transform;
// //     public GameObject gameObject;
// //
// //     Text idText;
// //     Text roomNameText;
// //     Text stageNameText;
// //     Text roomPlayerCountText;
// //     private Text roomMasterName;
// //     private Image stageBgImg;
// //     private Image roomMasterAvatarImg;
// //
// //     Button joinBtn;
// //
// //     TeamRoomData roomData;
// //
// //     public void Init(GameObject gameObject)
// //     {
// //         this.gameObject = gameObject;
// //         this.transform = this.gameObject.transform;
// //
// //         idText = transform.Find("id").GetComponent<Text>();
// //         roomNameText = transform.Find("name").GetComponent<Text>();
// //         stageNameText = transform.Find("stageName").GetComponent<Text>();
// //         roomPlayerCountText = transform.Find("playerCount").GetComponent<Text>();
// //         roomMasterName = transform.Find("roomMasterName").GetComponent<Text>();
// //         roomMasterAvatarImg = transform.Find("roomMasterAvatarBg/avatar").GetComponent<Image>();
// //         stageBgImg = transform.Find("stagePic").GetComponent<Image>();
// //         joinBtn = transform.Find("joinBtn").GetComponent<Button>();
// //
// //         joinBtn.onClick.RemoveAllListeners();
// //         joinBtn.onClick.AddListener(() =>
// //         {
// //             //this.parentObj.OnClickSingleRoomJoinBtn(this.uiData.id);
// //         });
// //     }
// //
// //     public void Refresh(TeamRoomData data, int index)
// //     {
// //         this.roomData = data;
// //         var teamStageId = this.roomData.teamStageId;
// //         var currTeamStageTb = Config.ConfigManager.Instance.GetById<Config.TeamStage>(teamStageId);
// //
// //         idText.text = "" + this.roomData.id;
// //         roomNameText.text = this.roomData.roomName;
// //
// //         var master = data.playerList.Find((pData) => { return pData.isMaster; });
// //
// //         roomMasterName.text = master.playerInfo.name;
// //
// //         stageNameText.text = currTeamStageTb.Name;
// //
// //         var teamStageTb = Config.ConfigManager.Instance.GetById<Config.TeamStage>(roomData.teamStageId);
// //
// //         roomPlayerCountText.text = this.roomData.playerList.Count + "/" + teamStageTb.MaxPlayerCount;
// //
// //         var stageTb = ConfigManager.Instance.GetById<Config.TeamStage>(this.roomData.teamStageId);
// //         ResourceManager.Instance.GetObject<Sprite>(stageTb.IconResId, (sprite) => { stageBgImg.sprite = sprite; });
// //         var masterAvatarResId = int.Parse(master.playerInfo.avatarURL);
// //
// //         ResourceManager.Instance.GetObject<Sprite>(masterAvatarResId,
// //             (sprite) => { roomMasterAvatarImg.sprite = sprite; });
// //     }
// //
// //     public void Release()
// //     {
// //     }
// }