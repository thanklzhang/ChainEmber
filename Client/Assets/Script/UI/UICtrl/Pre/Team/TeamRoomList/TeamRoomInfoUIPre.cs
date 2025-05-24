// using System;
// using System.Collections;
// using System.Collections.Generic;
// using Battle;
// 
// using UnityEngine;
// using UnityEngine.UI;
//
//
// public class TeamRoomInfoUIPre : BaseUI
// {
//     //this callback
//     public Action event_onClickCloseBtn;
//     public Action event_onClickStartBattleBtn;
//
//     //single item callback
//     public Action<int> event_onClickSinglePlayerReadyBtn;
//     public Action<int> event_onClickSinglePlayerChangeHeroBtn;
//
//     //ui component
//     Button closeBtn;
//     Button startBattleBtn;
//     Transform playerRoot;
//
//     Text roomNameText;
//     Text stageNameText;
//     private Text stageDesText;
//
//     private Image stageIconImg;
//
//
//     //data
//     List<TeamRoomPlayerUIData> roomPlayerDataList;
//     List<TeamRoomPlayerUIShowObj> roomPlayerShowObjList;
//     string roomName;
//     string stageName;
//
//     protected override void OnInit()
//     {
//         playerRoot = transform.Find("scroll/mask/content");
//         closeBtn = transform.Find("closeBtn").GetComponent<Button>();
//         startBattleBtn = transform.Find("battleBtn").GetComponent<Button>();
//
//         stageIconImg = transform.Find("stagePic").GetComponent<Image>();
//         roomNameText = transform.Find("roomName").GetComponent<Text>();
//         stageNameText = transform.Find("stageName").GetComponent<Text>();
//         stageDesText = transform.Find("stageDes").GetComponent<Text>();
//
//         closeBtn.onClick.AddListener(() => { event_onClickCloseBtn?.Invoke(); });
//         startBattleBtn.onClick.AddListener(() => { event_onClickStartBattleBtn?.Invoke(); });
//
//         roomPlayerDataList = new List<TeamRoomPlayerUIData>();
//         roomPlayerShowObjList = new List<TeamRoomPlayerUIShowObj>();
//     }
//
//     public override void Refresh(UIArgs args)
//     {
//         TeamRoomInfoUIArgs roomInfoArgs = (TeamRoomInfoUIArgs)args;
//
//
//         var stageConfig = ConfigManager.Instance.GetById<Config.TeamStage>(roomInfoArgs.stageConfigId);
//         roomPlayerDataList = roomInfoArgs.playerUIDataList;
//         roomName = roomInfoArgs.roomName;
//         stageName = stageConfig.Name;
//         stageDesText.text = stageConfig.Describe;
//         ResourceManager.Instance.GetObject<Sprite>(stageConfig.IconResId,
//             (sprite) => { stageIconImg.sprite = sprite; });
//
//         this.RefreshRoomInfo();
//     }
//
//     void RefreshRoomInfo()
//     {
//         //玩家列表
//         UIListArgs<TeamRoomPlayerUIShowObj, TeamRoomInfoUIPre> args =
//             new UIListArgs<TeamRoomPlayerUIShowObj, TeamRoomInfoUIPre>();
//         args.dataList = roomPlayerDataList;
//         args.showObjList = roomPlayerShowObjList;
//         args.root = playerRoot;
//         args.parentObj = this;
//         UIFunc.DoUIList(args);
//
//         //房间信息
//         roomNameText.text = roomName;
//         stageNameText.text = stageName;
//     }
//
//     public void OnClickSinglePlayerReadyBtn(int uid)
//     {
//         //实际上只能是自己
//         event_onClickSinglePlayerReadyBtn?.Invoke(uid);
//     }
//
//     protected override void OnUnload()
//     {
//         event_onClickCloseBtn = null;
//         event_onClickStartBattleBtn = null;
//         event_onClickSinglePlayerReadyBtn = null;
//         event_onClickSinglePlayerChangeHeroBtn = null;
//
//         foreach (var item in roomPlayerShowObjList)
//         {
//             item.Release();
//         }
//
//         closeBtn.onClick.RemoveAllListeners();
//         startBattleBtn.onClick.RemoveAllListeners();
//     }
// }
//
// //-----------------------------------------
//
// public class TeamRoomPlayerUIData
// {
//     public int uid;
//     public string name;
//     public string avatarURL;
//     public int level;
//
//     public bool isMaster;
//     public bool isHasReady;
//     public HeroCardUIData heroUIData;
//     public bool isSelf;
// }
//
//
// public class TeamRoomInfoUIArgs : UIArgs
// {
//     public int id;
//
//     // public string stageName;
//     public int stageConfigId;
//     public string roomName;
//     public List<TeamRoomPlayerUIData> playerUIDataList;
// }
//
// public class TeamRoomPlayerUIShowObj : BaseUIShowObj<TeamRoomInfoUIPre>
// {
//     Text idText;
//     Text nameText;
//     Text levelText;
//
//     Image playerAvatarImg;
//
//     Image heroAvatarImg;
//     Text heroLevelText;
//     Text heroNameText;
//     Button changeHeroBtn;
//
//     Button readyBtn;
//     Text readyBtnText;
//     Text noReadyText;
//     Text hasReadyText;
//
//     public GameObject masterFlagGo;
//
//     Transform showRoot;
//     Transform noShowRoot;
//
//     TeamRoomPlayerUIData uiPlayerData;
//
//     public override void OnInit()
//     {
//         showRoot = transform.Find("show");
//         noShowRoot = transform.Find("noShow");
//
//         idText = showRoot.Find("id").GetComponent<Text>();
//         nameText = showRoot.Find("name").GetComponent<Text>();
//         levelText = showRoot.Find("level").GetComponent<Text>();
//         noReadyText = showRoot.Find("noReady").GetComponent<Text>();
//         hasReadyText = showRoot.Find("hasReadyText").GetComponent<Text>();
//         readyBtn = showRoot.Find("readyBtn").GetComponent<Button>();
//         readyBtnText = readyBtn.transform.Find("Text").GetComponent<Text>();
//         changeHeroBtn = showRoot.Find("changeHeroBtn").GetComponent<Button>();
//
//         playerAvatarImg = showRoot.Find("playerIcon").GetComponent<Image>();
//         heroAvatarImg = showRoot.Find("selectHeroIcon").GetComponent<Image>();
//         heroLevelText = showRoot.Find("heroLevelText").GetComponent<Text>();
//         heroNameText = showRoot.Find("heroNameText").GetComponent<Text>();
//
//         masterFlagGo = showRoot.Find("masterFlag").gameObject;
//
//         readyBtn.onClick.AddListener(() => { this.parentObj.OnClickSinglePlayerReadyBtn(this.uiPlayerData.uid); });
//
//         changeHeroBtn.onClick.AddListener(() =>
//         {
//             this.parentObj.event_onClickSinglePlayerChangeHeroBtn(this.uiPlayerData.uid);
//         });
//     }
//
//     public override void OnRefresh(object data, int index)
//     {
//         this.uiPlayerData = (TeamRoomPlayerUIData)data;
//         //var teamStageId = this.uiPlayerData.teamStageId;
//         //var currTeamStageTb = Config.ConfigManager.Instance.GetById<Config.TeamStage>(teamStageId);
//
//         idText.text = "" + uiPlayerData.uid;
//         nameText.text = uiPlayerData.name;
//         levelText.text = "" + uiPlayerData.level;
//
//         var heroConfigId = uiPlayerData.heroUIData.configId;
//         var heroConfig = ConfigManager.Instance.GetById<Config.EntityInfo>(heroConfigId);
//         heroLevelText.text = "" + uiPlayerData.heroUIData.level;
//         heroNameText.text = "" + heroConfig.Name;
//
//         var userStore = GameData.GameDataManager.Instance.UserStore;
//         var isShowChangeHeroBtn = (int)userStore.Uid == this.uiPlayerData.uid;
//         changeHeroBtn.gameObject.SetActive(isShowChangeHeroBtn);
//
//         if (uiPlayerData.isSelf)
//         {
//             if (uiPlayerData.isHasReady)
//             {
//                 noReadyText.gameObject.SetActive(false);
//                 hasReadyText.gameObject.SetActive(false);
//                 readyBtn.gameObject.SetActive(true);
//
//                 readyBtnText.text = "取消";
//             }
//             else
//             {
//                 noReadyText.gameObject.SetActive(false);
//                 hasReadyText.gameObject.SetActive(false);
//                 readyBtn.gameObject.SetActive(true);
//
//                 readyBtnText.text = "准备";
//             }
//         }
//         else
//         {
//             if (uiPlayerData.isHasReady)
//             {
//                 noReadyText.gameObject.SetActive(false);
//                 hasReadyText.gameObject.SetActive(true);
//                 readyBtn.gameObject.SetActive(false);
//             }
//             else
//             {
//                 noReadyText.gameObject.SetActive(true);
//                 hasReadyText.gameObject.SetActive(false);
//                 readyBtn.gameObject.SetActive(false);
//             }
//         }
//
//         //玩家图标
//         var playerAvatarResId = int.Parse(uiPlayerData.avatarURL);
//         ResourceManager.Instance.GetObject<Sprite>(playerAvatarResId, (sprite) => { playerAvatarImg.sprite = sprite; });
//         
//         //英雄图标
//         ResourceManager.Instance.GetObject<Sprite>( heroConfig.AllBodyResId, (sprite) => { heroAvatarImg.sprite = sprite; });
//
//         var isMaster = this.uiPlayerData.isMaster;
//         masterFlagGo.SetActive(isMaster);
//         
//         
//     }
//
//     public override void OnRelease()
//     {
//         readyBtn.onClick.RemoveAllListeners();
//         changeHeroBtn.onClick.RemoveAllListeners();
//     }
// }