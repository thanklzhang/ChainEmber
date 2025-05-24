using Battle_Client;
using System;
using System.Collections;
using System.Collections.Generic;
using Battle;
using Battle_Client.BattleSkillOperate;
using Config;
using GameData;
using UnityEngine;
using UnityEngine.UI;

// public enum EntityRelationType
// {
//     Self = 0,
//     Friend = 1,
//     Enemy = 2
// }

public partial class BattleUI : BaseUI
{
    public Action onCloseBtnClick;
    public Action onReadyStartBtnClick;
    public Action onAttrBtnClick;

    Button closeBtn;
    Button readyStartBtn;
    Button hasReadyBtn;
    Button attrBtn;

    public GameObject readyBgGo;
    public Text stateText;

    // public GameObject bossComingRootGo;

    private Transform funcBtnRoot;
    private Button heroFuncBtn;
    private Button skillFuncBtn;
    private Button itemWarehouseBtn;
    private Button boxFuncBtn;
    private Button battleRewardBtn;

    public Text boxFuncBtnCountText;
    public Text coinText;

    public Button startBattleBtn;

    //血条
    HeroStateUIMgr heroStateUIMgr;

    //飘字
    FloatWordMgr floatWordMgr;

    //属性面板
    BattleAttrUI attrUI;

    //技能显示面板
    BattleSkillUI skillUI;

    //玩家仓库道具显示面板
    BattleItemWarehouseUI itemWarehouseUI;

    //操作实体道具界面
    OpEntitiesItemUI opEntitiesItemUI;

    //本地玩家实体身上的道具显示面板
    LocalPlayerEntityItemUI localEntityItemUI;

    //队友英雄信息面板
    BattleHeroInfoUICtrl heroInfoUI;

    //buff 显示面板
    BattleBuffUI buffUI;

    //通用描述面板
    DescribeUI describeUI;

    //关卡信息界面
    BattleStageInfoUI stageInfoUI;

    //人口信息界面
    BattlePopulationUI populationUI;

    // //技能书操作UI
    // private BattleSkillOperateUI skillItemOperateUI;

    //选择奖励界面
    private BattleSelectRewardUI boxUI;

    //宝箱主界面
    protected BattleBoxMainUI boxMainUI;

    //战斗流程阶段提示界面
    private BattleProcessUI processUI;

    //战斗流程阶段目标信息界面
    private WaveTargetInfoUI processTargetInfoUI;

    //战斗回合（当前波）结算界面
    private BattleWavePassUI wavePassUI;

    //战斗获得奖励的列表界面
    private BattleRewardUI battleRewardUI;

    //观看实体信息界面
    private BattleLookEntityInfoUI lookEntityInfoUI;

    //-----------------
    //智能施法 临时功能
    private Button intelligentReleaseBtn;
    private GameObject intelligentReleaseSelectFlagGo;

    public Transform tempRoot;

    public void ShowBoxUI(bool isOpenBox = false)
    {
        this.boxMainUI.Show();
        if (isOpenBox)
        {
            this.boxMainUI.Hide();
            this.boxMainUI.OpenMyBox();
        }
    }

    protected override void OnLoadFinish()
    {
        closeBtn = this.transform.Find("closeBtn").GetComponent<Button>();
        readyBgGo = this.transform.Find("readyBg").gameObject;
        readyStartBtn = this.transform.Find("readyBg/readyStartBtn").GetComponent<Button>();
        hasReadyBtn = this.transform.Find("readyBg/hasReadyBtn").GetComponent<Button>();
        stateText = this.transform.Find("stateText").GetComponent<Text>();
        attrBtn = this.transform.Find("attrBtn").GetComponent<Button>();

        tempRoot = this.transform.Find("tempRoot");
        // bossComingRootGo = this.transform.Find("bossComingRoot").gameObject;

        //功能栏
        funcBtnRoot = transform.Find("functionBar/group");

        skillFuncBtn = funcBtnRoot.Find("skill/item").GetComponent<Button>();
        //skillFuncBtn.onClick.AddListener(() => { skillItemOperateUI.Show(); });

        boxFuncBtn = funcBtnRoot.Find("box/item").GetComponent<Button>();
        boxFuncBtn.onClick.AddListener(() =>
        {
            // var hero = BattleManager.Instance.GetLocalCtrlHero();
            // if (hero != null)
            // {
            //     hero.TryOpenBox(RewardQuality.Blue);
            // }

            if (BattleManager.Instance.processState == BattleProcessState.Ready)
            {
                // this.boxMainUI.Show();
                ShowBoxUI();
            }
        });
        boxFuncBtnCountText = boxFuncBtn.transform.Find("countBg/count").GetComponent<Text>();

        coinText = transform.Find("coinText").GetComponent<Text>();

        //战斗奖励列表
        battleRewardBtn = funcBtnRoot.Find("battleReward/item").GetComponent<Button>();
        battleRewardBtn.onClick.AddListener(() => { this.battleRewardUI.Show(); });

        //道具仓库
        itemWarehouseBtn = funcBtnRoot.Find("warehouse/item").GetComponent<Button>();
        itemWarehouseBtn.onClick.AddListener(() =>
        {
            if (BattleManager.Instance.processState == BattleProcessState.Ready)
            {
                this.opEntitiesItemUI.Show();
                this.itemWarehouseUI.Show();
            }
        });

        //
        closeBtn.onClick.AddListener(() => { onCloseBtnClick?.Invoke(); });
        readyStartBtn.onClick.AddListener(() =>
        {
            //onReadyStartBtnClick?.Invoke();

            BattleManager.Instance.MsgSender.Send_BattleReadyFinish();

            AudioManager.Instance.PlaySound((int)ResIds.audio_click_001);
        });
        attrBtn.onClick.AddListener(() => { onAttrBtnClick?.Invoke(); });

        startBattleBtn = this.transform.Find("startBattleBtn").GetComponent<Button>();
        startBattleBtn.onClick.AddListener(() =>
        {
            //send
            BattleManager.Instance.MsgSender.Send_AskEnterBattleProcess();
        });

        //英雄头顶状态UI管理
        this.heroStateUIMgr = new HeroStateUIMgr();
        var heroStateUIRoot = this.transform.Find("heroStateShow");
        this.heroStateUIMgr.Init(heroStateUIRoot.gameObject, this);

        //飘字管理
        floatWordMgr = new FloatWordMgr();
        var floatRoot = this.transform.Find("float_word_root");
        floatWordMgr.Init(floatRoot);

        //属性面板
        attrUI = new BattleAttrUI();
        var attrUIRoot = this.transform.Find("attrBar");
        attrUI.Init(attrUIRoot.gameObject, this);
        // attrUI.Hide();

        //技能面板
        var skillUIRoot = this.transform.Find("skillBar");
        skillUI = new BattleSkillUI();
        skillUI.Init(skillUIRoot.gameObject, this);

        //玩家仓库道具面板
        var itemWarehouseUIRoot = this.transform.Find("itemWarehouseUI");
        itemWarehouseUI = new BattleItemWarehouseUI();
        itemWarehouseUI.Init(itemWarehouseUIRoot.gameObject, this);

        //操作实体道具界面
        var opEntitiesItemUIRoot = this.transform.Find("opEntitiesItemUI");
        opEntitiesItemUI = new OpEntitiesItemUI();
        opEntitiesItemUI.Init(opEntitiesItemUIRoot.gameObject, this);

        //玩家实体道具面板
        var localEntityItemUIRoot = this.transform.Find("localEntityItemUI");
        localEntityItemUI = new LocalPlayerEntityItemUI();
        localEntityItemUI.Init(localEntityItemUIRoot.gameObject, this);

        //英雄信息面板
        var heroInfoUIRoot = this.transform.Find("all_player_info");
        heroInfoUI = new BattleHeroInfoUICtrl();
        heroInfoUI.Init(heroInfoUIRoot.gameObject, this);

        //buff 面板
        var buffUIRoot = this.transform.Find("buffBar");
        buffUI = new BattleBuffUI();
        buffUI.Init(buffUIRoot.gameObject, this);

        //通用描述面板
        var describeUIRoot = this.transform.Find("DescribeBar");
        describeUI = new DescribeUI();
        describeUI.Init(describeUIRoot.gameObject, this);
        describeUI.Hide();

        //关卡信息界面
        var stageUIRoot = this.transform.Find("stageInfo");
        stageInfoUI = new BattleStageInfoUI();
        stageInfoUI.Init(stageUIRoot.gameObject, this);

        //人口信息界面
        var populationUIRoot = this.transform.Find("populationInfo");
        populationUI = new BattlePopulationUI();
        populationUI.Init(populationUIRoot.gameObject, this);

        //技能操作界面
        var skillOperateUIRoot = this.transform.Find("skillOperateUI");
        // skillItemOperateUI = new BattleSkillOperateUI();
        // skillItemOperateUI.Init(skillOperateUIRoot.gameObject, this);

        //宝箱界面
        var boxUIRoot = this.transform.Find("boxUI");
        boxUI = new BattleSelectRewardUI();
        boxUI.Init(boxUIRoot.gameObject, this);

        //宝箱主界面
        var boxMainUIRoot = this.transform.Find("boxMainUI");
        boxMainUI = new BattleBoxMainUI();
        boxMainUI.Init(boxMainUIRoot.gameObject, this);
        boxMainUI.Hide();

        //关卡流程时间相关界面
        var processUIRoot = this.transform.Find("processTimeRoot");
        processUI = new BattleProcessUI();
        processUI.Init(processUIRoot.gameObject, this);

        var waveTargetInfoUIRoot = this.transform.Find("waveTargetInfo");
        processTargetInfoUI = new WaveTargetInfoUI();
        processTargetInfoUI.Init(waveTargetInfoUIRoot.gameObject, this);

        //关卡回合（当前波）结算界面
        var wavePassUIRoot = this.transform.Find("wavePassUI");
        wavePassUI = new BattleWavePassUI();
        wavePassUI.Init(wavePassUIRoot.gameObject, this);

        //战斗奖励列表界面
        var battleRewardUIRoot = this.transform.Find("battleRewardUI");
        battleRewardUIRoot.gameObject.SetActive(false);
        battleRewardUI = new BattleRewardUI();
        battleRewardUI.Init(battleRewardUIRoot.gameObject, this);

        //观看实体信息界面
        var lookEntityInfoUIRoot = this.transform.Find("lookInfoBar");
        lookEntityInfoUIRoot.gameObject.SetActive(false);
        lookEntityInfoUI = new BattleLookEntityInfoUI();
        lookEntityInfoUI.Init(lookEntityInfoUIRoot.gameObject, this);

        //智能施法----------------------
        intelligentReleaseBtn = this.transform.Find("intelligentRelease").GetComponent<Button>();
        intelligentReleaseSelectFlagGo = this.intelligentReleaseBtn.transform.Find("flag").gameObject;
        intelligentReleaseBtn.onClick.AddListener(OnClickIntelligentReleaseBtn);
        intelligentReleaseSelectFlagGo.SetActive(BattleManager.Instance.IsIntelligentRelease);
    }

    protected override void OnActive()
    {
        this.attrUI.RefreshAllUI();
        this.skillUI.RefreshAllUI();
        this.heroInfoUI.RefreshAllUI();
        this.stageInfoUI.RefreshAllUI();
        this.populationUI.RefreshAllUI();
        this.buffUI.RefreshAllUI();
        this.itemWarehouseUI.RefreshAllUI();
        this.localEntityItemUI.RefreshAllUI();
        this.opEntitiesItemUI.RefreshAllUI();
        // this.skillItemOperateUI.RefreshAllUI();
        this.boxUI.RefreshAllUI();
        this.boxMainUI.RefreshAllUI();
        this.processUI.RefreshAllUI();
        this.processTargetInfoUI.RefreshAllUI();
        this.wavePassUI.RefreshAllUI();
        this.battleRewardUI.RefreshAllUI();

        this.OnUpdateMyBoxInfo();
        this.OnUpdateBattleCurrencyInfo();

        EventDispatcher.AddListener<int, bool>(EventIDs.OnPlayerReadyState, this.OnPlayerReadyState);
        EventDispatcher.AddListener(EventIDs.OnAllPlayerLoadFinish, this.OnAllPlayerLoadFinish);
        EventDispatcher.AddListener(EventIDs.OnBattleStart, this.OnBattleStart);
        EventDispatcher.AddListener(EventIDs.OnUpdateMyBoxInfo, this.OnUpdateMyBoxInfo);
        EventDispatcher.AddListener(EventIDs.OnUpdateBattleCurrencyInfo, this.OnUpdateBattleCurrencyInfo);
        EventDispatcher.AddListener(EventIDs.OnUpdateShopBoxInfo, this.OnUpdateShopBoxInfo);
        EventDispatcher.AddListener(EventIDs.OnProcessWavePass, this.OnProcessWavePass);
        EventDispatcher.AddListener<BattleEntity_Client, AbnormalStateBean>(EventIDs.OnEntityAbnormalEffect,
            this.OnEntityAbnormalEffect);
    }

    protected override void OnUpdate(float deltaTime)
    {
        this.heroStateUIMgr.Update(deltaTime);
        this.floatWordMgr.Update(deltaTime);
        this.skillUI.Update(deltaTime);
        this.buffUI.Update(deltaTime);
        this.describeUI.Update(deltaTime);
        this.heroInfoUI.Update(deltaTime);
        this.stageInfoUI.Update(deltaTime);
        this.populationUI.Update(deltaTime);
        this.itemWarehouseUI.Update(deltaTime);
        this.localEntityItemUI.Update(deltaTime);
        this.opEntitiesItemUI.Update(deltaTime);
        // this.skillItemOperateUI.Update(deltaTime);
        this.boxUI.Update(deltaTime);
        this.boxMainUI.Update(deltaTime);
        this.processUI.Update(deltaTime);
        this.processTargetInfoUI.Update(deltaTime);
        this.wavePassUI.Update(deltaTime);
        this.battleRewardUI.Update(deltaTime);
        this.lookEntityInfoUI.Update(deltaTime);
    }

    void OnPlayerReadyState(int uid, bool isReady)
    {
        var myUid = (int)GameDataManager.Instance.UserData.Uid;

        if (myUid == uid)
        {
            SetReadyBtnShowState(isReady);
        }
    }

    void OnAllPlayerLoadFinish()
    {
        SetReadyShowState(true);

        SetStateText("wait to battle start");

        // UICtrlManager.Instance.Close<LoadingUI>

        UIManager.Instance.Close<LoadingUICtrl>();
    }

    void OnBattleStart()
    {
        SetReadyShowState(false);
        SetStateText("OnBattleStart");
    }

    public void SetReadyBtnShowState(bool isHasReady = false)
    {
        if (!isHasReady)
        {
            readyStartBtn.gameObject.SetActive(true);
            hasReadyBtn.gameObject.SetActive(false);
        }
        else
        {
            readyStartBtn.gameObject.SetActive(false);
            hasReadyBtn.gameObject.SetActive(true);
        }
    }

    public void SetReadyShowState(bool isShow)
    {
        readyBgGo.SetActive(isShow);
        // readyStartBtn.gameObject.SetActive(isShow);
        if (isShow)
        {
            SetReadyBtnShowState(false);
        }
    }

    public void SetStateText(string stateStr)
    {
        stateText.text = stateStr;
    }

    public void OnUpdateMyBoxInfo()
    {
        var player = BattleManager.Instance.GetLocalPlayer();
        if (player != null)
        {
            var boxCount = player.GetBoxTotalCount();
            boxFuncBtnCountText.text = "" + boxCount;
        }

        this.boxMainUI.RefreshMyBoxUI();
    }

    public void OnUpdateBattleCurrencyInfo()
    {
        var player = BattleManager.Instance.GetLocalPlayer();
        var coinCount = player.GetCoinCount();
        this.coinText.text = coinCount + " 战银";

        this.boxMainUI.RefreshMyBoxUI();
        this.boxMainUI.RefreshShopUI();

        this.populationUI.RefreshAllUI();
    }

    public void OnUpdateShopBoxInfo()
    {
        this.boxMainUI.RefreshShopUI();
    }

    public void OnProcessWavePass()
    {
        var arg = BattleManager.Instance.wavePassArg;
        this.wavePassUI.ShowByData(arg);
    }

    #region 飘字相关

    // internal void ShowFloatWord(string word, GameObject go, int floatStyle, Color color)
    // {
    //     floatWordMgr.ShowFloatWord(word, go, floatStyle, color);
    // }

    internal void ShowFloatWord(FloatWordBean bean)
    {
        floatWordMgr.ShowFloatWord(bean);
    }

    #endregion


    public void OnWarehouseClose()
    {
        this.opEntitiesItemUI.Close();
    }


    protected override void OnInactive()
    {
        EventDispatcher.RemoveListener<int, bool>(EventIDs.OnPlayerReadyState, this.OnPlayerReadyState);
        EventDispatcher.RemoveListener(EventIDs.OnAllPlayerLoadFinish, this.OnAllPlayerLoadFinish);
        EventDispatcher.RemoveListener(EventIDs.OnBattleStart, this.OnBattleStart);
        EventDispatcher.RemoveListener(EventIDs.OnUpdateMyBoxInfo, this.OnUpdateMyBoxInfo);
        EventDispatcher.RemoveListener(EventIDs.OnUpdateBattleCurrencyInfo, this.OnUpdateBattleCurrencyInfo);
        EventDispatcher.RemoveListener(EventIDs.OnUpdateShopBoxInfo, this.OnUpdateShopBoxInfo);
        EventDispatcher.RemoveListener(EventIDs.OnProcessWavePass, this.OnProcessWavePass);
        EventDispatcher.RemoveListener<BattleEntity_Client, AbnormalStateBean>(EventIDs.OnEntityAbnormalEffect,
            this.OnEntityAbnormalEffect);
    }

    void OnClickIntelligentReleaseBtn()
    {
        BattleManager.Instance.IsIntelligentRelease = !BattleManager.Instance.IsIntelligentRelease;

        this.intelligentReleaseSelectFlagGo.SetActive(BattleManager.Instance.IsIntelligentRelease);
    }

    protected override void OnClose()
    {
        onCloseBtnClick = null;
        onReadyStartBtnClick = null;

        this.heroStateUIMgr.Release();
        this.attrUI.Release();
        this.skillUI.Release();
        this.heroInfoUI.Release();
        this.buffUI.Release();
        this.describeUI.Release();
        this.floatWordMgr.Release();
        this.itemWarehouseUI.Release();
        this.opEntitiesItemUI.Release();
        this.localEntityItemUI.Release();
        this.stageInfoUI.Release();
        this.populationUI.Release();
        // this.skillItemOperateUI.Release();
        this.boxUI.Release();
        this.boxMainUI.Release();
        this.processUI.Release();
        this.processTargetInfoUI.Release();
        this.wavePassUI.Release();
        this.battleRewardUI.Release();
        this.lookEntityInfoUI.Release();

        this.intelligentReleaseBtn.onClick.RemoveAllListeners();
    }
}