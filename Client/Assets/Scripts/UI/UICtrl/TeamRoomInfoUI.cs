using System;
using System.Collections;
using System.Collections.Generic;
using Battle;
using Config;
using GameData;
using NetProto;

using UnityEngine;
using UnityEngine.UI;


public class TeamRoomInfoUI : BaseUI
{
    //this callback
    public Action event_onClickCloseBtn;
    public Action event_onClickStartBattleBtn;

    //single item callback
    public Action<int> event_onClickSinglePlayerReadyBtn;
    public Action<int> event_onClickSinglePlayerChangeHeroBtn;

    //ui component
    Button closeBtn;
    Button startBattleBtn;
    Transform playerRoot;

    Text roomNameText;
    Text stageNameText;
    private Text stageDesText;

    private Image stageIconImg;


    //data
    // List<TeamRoomPlayerUIData> roomPlayerDataList;
    List<TeamRoomPlayerUIShowObj> roomPlayerShowObjList;
    string roomName;
    string stageName;

    protected override void OnInit()
    {
        this.uiResId = (int)ResIds.TeamRoomInfoUI;
        this.uiShowLayer = UIShowLayer.Floor_0;
    }

    protected override void OnLoadFinish()
    {
        playerRoot = transform.Find("scroll/mask/content");
        closeBtn = transform.Find("closeBtn").GetComponent<Button>();
        startBattleBtn = transform.Find("battleBtn").GetComponent<Button>();

        stageIconImg = transform.Find("stagePic").GetComponent<Image>();
        roomNameText = transform.Find("roomName").GetComponent<Text>();
        stageNameText = transform.Find("stageName").GetComponent<Text>();
        stageDesText = transform.Find("stageDes").GetComponent<Text>();

        closeBtn.onClick.AddListener(() => { event_onClickCloseBtn?.Invoke(); });
        startBattleBtn.onClick.AddListener(OnClickStartBattleBtn);

        // roomPlayerDataList = new List<TeamRoomPlayerUIData>();
        roomPlayerShowObjList = new List<TeamRoomPlayerUIShowObj>();
    }

    public void OnClickStartBattleBtn()
    {
        var net = NetHandlerManager.Instance.GetHandler<BattleEntranceNetHandler>();
        var enterRoomData = GameDataManager.Instance.TeamData.currEnterRoomData;
        var tamRoomId = enterRoomData.id;
        net.ApplyTeamBattle(tamRoomId, () => { });
    }

    public TeamRoomData currRoomData;

    private void OnPlayerChangeInfoInTamRoom()
    {
        // var enterRoomData = GameDataManager.Instance.TeamStore.currEnterRoomData;
        // var player = enterRoomData.playerList.Find(p => p.playerInfo.uid == this.playerData.playerInfo.uid);
        // this.Refresh(player);
        this.RefreshRoomInfo();
    }

    protected override void OnActive()
    {
        // TeamRoomInfoUIArgs roomInfoArgs = (TeamRoomInfoUIArgs)args;
        EventDispatcher.AddListener(EventIDs.OnPlayerChangeInfoInTeamRoom, OnPlayerChangeInfoInTamRoom);

        UIManager.Instance.Open<TitleBarUI>(new TitleBarUIArgs()
        {
            titleBarId = 4
        });
        currRoomData = GameDataManager.Instance.TeamData.currEnterRoomData;


        var stageConfig = ConfigManager.Instance.GetById<Config.TeamStage>(currRoomData.teamStageId);
        // roomPlayerDataList = roomInfoArgs.playerUIDataList;
        roomName = currRoomData.roomName;
        stageName = stageConfig.Name;
        stageDesText.text = stageConfig.Describe;
        ResourceManager.Instance.GetObject<Sprite>(stageConfig.IconResId,
            (sprite) => { stageIconImg.sprite = sprite; });

        this.RefreshRoomInfo();
    }

    public void RefreshRoomInfo()
    {
        //玩家列表
        // UIListArgs<TeamRoomPlayerUIShowObj, TeamRoomInfoUIPre> args =
        //     new UIListArgs<TeamRoomPlayerUIShowObj, TeamRoomInfoUIPre>();
        // args.dataList = roomPlayerDataList;
        // args.showObjList = roomPlayerShowObjList;
        // args.root = playerRoot;
        // args.parentObj = this;
        // UIFunc.DoUIList(args);

        roomPlayerShowObjList = new List<TeamRoomPlayerUIShowObj>();
        for (int i = 0; i < currRoomData.playerList.Count; i++)
        {
            var data = currRoomData.playerList[i];

            GameObject go = null;
            if (i < playerRoot.childCount)
            {
                go = playerRoot.GetChild(i).gameObject;
            }
            else
            {
                go = GameObject.Instantiate(playerRoot.GetChild(0).gameObject, playerRoot, false);
            }

            TeamRoomPlayerUIShowObj showObj = new TeamRoomPlayerUIShowObj();
            showObj.Init(go, this);
            showObj.Refresh(data);
        }


        //房间信息
        roomNameText.text = roomName;
        stageNameText.text = stageName;
    }

    public void OnClickSinglePlayerReadyBtn(int uid)
    {
        //实际上只能是自己
        event_onClickSinglePlayerReadyBtn?.Invoke(uid);
    }


    protected override void OnInactive()
    {
        EventDispatcher.RemoveListener(EventIDs.OnPlayerChangeInfoInTeamRoom, OnPlayerChangeInfoInTamRoom);
    }


    protected override void OnClose()
    {
        event_onClickCloseBtn = null;
        event_onClickStartBattleBtn = null;
        event_onClickSinglePlayerReadyBtn = null;
        event_onClickSinglePlayerChangeHeroBtn = null;

        foreach (var item in roomPlayerShowObjList)
        {
            item.Release();
        }

        closeBtn.onClick.RemoveAllListeners();
        startBattleBtn.onClick.RemoveAllListeners();
    }
}

//-----------------------------------------

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


// public class TeamRoomInfoUIArgs : UIArgs
// {
//     public int id;
//
//     // public string stageName;
//     public int stageConfigId;
//     public string roomName;
//     public List<TeamRoomPlayerUIData> playerUIDataList;
// }

public class TeamRoomPlayerUIShowObj // : BaseUIShowObj<TeamRoomInfoUIPre>
{
    Text idText;
    Text nameText;
    Text levelText;

    Image playerAvatarImg;

    Image heroAvatarImg;
    Text heroLevelText;
    Text heroNameText;
    Button changeHeroBtn;

    Button readyBtn;
    Text readyBtnText;
    Text noReadyText;
    Text hasReadyText;

    public GameObject masterFlagGo;

    Transform showRoot;
    Transform noShowRoot;

    TeamRoomPlayerData playerData;


    private Transform transform;
    private GameObject gameObject;
    private TeamRoomInfoUI contextCtrl;

    public void Init(GameObject gameObject, TeamRoomInfoUI contextCtrl)
    {
        this.gameObject = gameObject;
        this.transform = this.gameObject.transform;

        this.contextCtrl = contextCtrl;

        showRoot = transform.Find("show");
        noShowRoot = transform.Find("noShow");

        idText = showRoot.Find("id").GetComponent<Text>();
        nameText = showRoot.Find("name").GetComponent<Text>();
        levelText = showRoot.Find("level").GetComponent<Text>();
        noReadyText = showRoot.Find("noReady").GetComponent<Text>();
        hasReadyText = showRoot.Find("hasReadyText").GetComponent<Text>();
        readyBtn = showRoot.Find("readyBtn").GetComponent<Button>();
        readyBtnText = readyBtn.transform.Find("Text").GetComponent<Text>();
        changeHeroBtn = showRoot.Find("changeHeroBtn").GetComponent<Button>();

        playerAvatarImg = showRoot.Find("playerIcon").GetComponent<Image>();
        heroAvatarImg = showRoot.Find("selectHeroIcon").GetComponent<Image>();
        heroLevelText = showRoot.Find("heroLevelText").GetComponent<Text>();
        heroNameText = showRoot.Find("heroNameText").GetComponent<Text>();

        masterFlagGo = showRoot.Find("masterFlag").gameObject;

        readyBtn.onClick.RemoveAllListeners();
        readyBtn.onClick.AddListener(OnClickReadyBtn);

        changeHeroBtn.onClick.AddListener(OnClickChangeHeroBtn);
    }

    public void OnClickReadyBtn()
    {
        var net = NetHandlerManager.Instance.GetHandler<TeamNetHandler>();
        var enterRoomData = GameDataManager.Instance.TeamData.currEnterRoomData;
        var player = enterRoomData.playerList.Find(p => p.playerInfo.uid == this.playerData.playerInfo.uid);
        var roomId = enterRoomData.id;
        var opReady = !player.isHasReady;

        net.SendChangeReadyStateInTeamRoom(roomId, opReady, OnChangeReadyStateCallback);
    }

    private int currSelectHeroGuid;

    public void OnClickChangeHeroBtn()
    {
        var uid = this.playerData.playerInfo.uid;
        if (uid == (int)GameData.GameDataManager.Instance.UserData.Uid)
        {
            SelectHeroUIArgs args = new SelectHeroUIArgs();

            args.heroDataList = new List<HeroData>();
            var heroList = GameData.GameDataManager.Instance.HeroData.HeroList;
            var enterRoomData = GameDataManager.Instance.TeamData.currEnterRoomData;
            var playerInfo = enterRoomData.playerList.Find(p => p.playerInfo.uid == uid);
            for (int i = 0; i < heroList.Count; i++)
            {
                var hero = heroList[i];

                // HeroData uiData = hero;
                args.heroDataList.Add(hero);

                if (playerInfo.selectHeroData.guid == hero.guid)
                {
                    currSelectHeroGuid = hero.guid;
                }
            }

            // //默认选择第一个
            // if (0 == currSelectHeroGuid)
            // {
            //     currSelectHeroGuid = heroList[0].guid;
            // }

            args.event_ClickConfirmBtn = (curr) =>
            {
                //send net
                // CtrlManager.Instance.GlobalCtrlPre.HideSelectHeroUI();

                NetProto.csSelectUseHeroInTeamRoom csSelect = new NetProto.csSelectUseHeroInTeamRoom();
                var enterRoomData = GameDataManager.Instance.TeamData.currEnterRoomData;

                var net = NetHandlerManager.Instance.GetHandler<TeamNetHandler>();
                currSelectHeroGuid = curr;
                // CtrlManager.Instance.GlobalCtrlPre.SelectHero(currSelectHeroGuid);

                net.SendSelectUseHeroInTeamRoom(enterRoomData.id, currSelectHeroGuid);
            };

            args.event_ClickOneHeroOption = (guid) =>
            {
                currSelectHeroGuid = guid;
                this.contextCtrl?.RefreshRoomInfo();

                // CtrlManager.Instance.GlobalCtrlPre.SelectHero(currSelectHeroGuid);
                contextCtrl?.RefreshRoomInfo();
            };

            args.currSelectHeroGuid = currSelectHeroGuid;

            // CtrlManager.Instance.GlobalCtrlPre.ShowSelectHeroUI(args);
            UIManager.Instance.Open<SelectHeroUI>(args);
        }
    }


    public void OnChangeReadyStateCallback()
    {
        // var enterRoomData = GameDataManager.Instance.TeamStore.currEnterRoomData;
        // var player = enterRoomData.playerList.Find(p => p.playerInfo.uid == this.playerData.playerInfo.uid);
        // this.Refresh(player);
    }


    public void Refresh(TeamRoomPlayerData data)
    {
        this.playerData = data;
        //var teamStageId = this.uiPlayerData.teamStageId;
        //var currTeamStageTb = Config.ConfigManager.Instance.GetById<Config.TeamStage>(teamStageId);

        idText.text = "" + playerData.playerInfo.uid;
        nameText.text = playerData.playerInfo.name;
        levelText.text = "" + playerData.playerInfo.level;

        var heroConfigId = playerData.selectHeroData.configId;
        var heroConfig = ConfigManager.Instance.GetById<Config.EntityInfo>(heroConfigId);
        heroLevelText.text = "" + playerData.selectHeroData.level;
        heroNameText.text = "" + heroConfig.Name;

        var userStore = GameData.GameDataManager.Instance.UserData;
        var isSelf = (int)userStore.Uid == this.playerData.playerInfo.uid;
        changeHeroBtn.gameObject.SetActive(isSelf);

        if (isSelf)
        {
            if (playerData.isHasReady)
            {
                noReadyText.gameObject.SetActive(false);
                hasReadyText.gameObject.SetActive(false);
                readyBtn.gameObject.SetActive(true);

                readyBtnText.text = "取消";
            }
            else
            {
                noReadyText.gameObject.SetActive(false);
                hasReadyText.gameObject.SetActive(false);
                readyBtn.gameObject.SetActive(true);

                readyBtnText.text = "准备";
            }
        }
        else
        {
            if (playerData.isHasReady)
            {
                noReadyText.gameObject.SetActive(false);
                hasReadyText.gameObject.SetActive(true);
                readyBtn.gameObject.SetActive(false);
            }
            else
            {
                noReadyText.gameObject.SetActive(true);
                hasReadyText.gameObject.SetActive(false);
                readyBtn.gameObject.SetActive(false);
            }
        }

        //玩家图标
        var playerAvatarResId = int.Parse(playerData.playerInfo.avatarURL);
        ResourceManager.Instance.GetObject<Sprite>(playerAvatarResId, (sprite) => { playerAvatarImg.sprite = sprite; });

        //英雄图标
        ResourceManager.Instance.GetObject<Sprite>(heroConfig.AllBodyResId,
            (sprite) => { heroAvatarImg.sprite = sprite; });

        var isMaster = this.playerData.isMaster;
        masterFlagGo.SetActive(isMaster);
    }


    public void Release()
    {
        readyBtn.onClick.RemoveAllListeners();
        changeHeroBtn.onClick.RemoveAllListeners();
    }
}