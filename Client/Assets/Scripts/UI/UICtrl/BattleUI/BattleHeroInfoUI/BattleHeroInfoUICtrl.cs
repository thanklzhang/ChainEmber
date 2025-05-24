using System;
using System.Collections;
using System.Collections.Generic;
using Battle;
using Battle_Client;
using Config;
using UnityEngine;
using UnityEngine.UI;

public class BattleHeroInfoUIArgs
{
    public int battleConfigId;
    public List<BattleHeroInfoUIData> battleHeroInfoUIDataList;
    public BattleHeroInfoUIData bossData;
}

public class BattleHeroInfoUICtrl
{
    //cmp
    public GameObject gameObject;
    public Transform transform;

    Transform heroRoot;

    private GameObject bossLimitGo;
    private RectTransform bossLimitRootRectTran;
    Text bossLimitTimeText;


    //runtime

    public BattleUI BattleUIPre;

    private List<BattleHeroInfoUIData> dataList;
    private List<BattleHeroInfoShowObj> showObjList;

    private BattleHeroInfoUIData bossData;
    private BattleHeroInfoShowObj bossShowObj;
    private int battleConfigId;
    private float currTimer;
    private bool isHasBossCountdown;

    public void Init(GameObject gameObject, BattleUI battleUIPre)
    {
        this.gameObject = gameObject;
        this.transform = this.gameObject.transform;

        this.BattleUIPre = battleUIPre;

        heroRoot = this.transform.Find("root");


        dataList = new List<BattleHeroInfoUIData>();
        showObjList = new List<BattleHeroInfoShowObj>();

        bossData = null;
        bossShowObj = new BattleHeroInfoShowObj();

        var bossRootGo = this.transform.Find("bossInfo").gameObject;
        bossShowObj.Init(bossRootGo, this);

        //限时 boss 击杀倒计时
        bossLimitGo = this.transform.Find("stageTimeRoot").gameObject;
        bossLimitRootRectTran = bossLimitGo.transform.GetComponent<RectTransform>();
        bossLimitTimeText = bossLimitGo.transform.Find("stageTime").GetComponent<Text>();


        EventDispatcher.AddListener<BattleEntity_Client>(EventIDs.OnCreateEntity, OnCreateEntity);
        EventDispatcher.AddListener<BattleEntity_Client, int>(EventIDs.OnChangeEntityBattleData,
            OnChangeEntityBattleData);
    }

    public void Show()
    {
        this.gameObject.SetActive(true);
    }

    public void RefreshAllUI()
    {
        this.RefreshData();

        this.RefreshHeroInfoList();
        this.RefreshBossInfo();
    }

    public void RefreshData()
    {
        battleConfigId = BattleManager.Instance.battleConfigId;

        var playerList = BattleManager.Instance.GetAllPlayerList();
        dataList = new List<BattleHeroInfoUIData>();

        foreach (var player in playerList)
        {
            if (player.ctrlHeroGuid <= 0)
            {
                //电脑
                continue;
            }

            BattleHeroInfoUIData uiData = new BattleHeroInfoUIData();
            uiData.heroGuid = player.ctrlHeroGuid;

            var heroEntity = BattleEntityManager.Instance.FindEntity(player.ctrlHeroGuid);

            if (null == heroEntity)
            {
                Logx.LogWarning("the heroEntity is null : uiData.heroGuid : " + uiData.heroGuid);
                return;
            }

            uiData.heroConfigId = heroEntity.configId;

            uiData.level = heroEntity.level;
            uiData.currHealth = heroEntity.CurrHealth;
            uiData.maxHealth = heroEntity.attr.GetValue(EntityAttrType.MaxHealth);
            uiData.playerIndex = heroEntity.playerIndex;

            dataList.Add(uiData);
        }

        dataList.Sort((a, b) =>
        {
            int myHeroGuid = BattleManager.Instance.GetLocalCtrlHeroGuid();
            if (b.heroGuid == myHeroGuid)
            {
                return 1;
            }

            return -1;
        });
    }

    void RefreshHeroInfoList()
    {
        UIListArgs<BattleHeroInfoShowObj, BattleHeroInfoUICtrl> args =
            new UIListArgs<BattleHeroInfoShowObj, BattleHeroInfoUICtrl>();
        args.dataList = dataList;
        args.showObjList = showObjList;
        args.root = heroRoot;
        args.parentObj = this;
        UIFunc.DoUIList(args);
    }

    void RefreshBossInfo()
    {
        // boss 血条
        bossShowObj.Refresh(this.bossData);

        //boss 倒计时
        if (isHasBossCountdown)
        {
            RefreshBossLimitTimeShow();
        }

        this.bossLimitGo.SetActive(isHasBossCountdown);
    }

    void RefreshBossLimitTimeShow()
    {
        if (this.currTimer < 0)
        {
            this.currTimer = 0;
        }

        var minutes = (int)(this.currTimer / 60.0f);
        var seconds = (int)(this.currTimer % 60.0f);
        var ms = (int)((this.currTimer - (int)currTimer) * 1000);

        bossLimitTimeText.text = string.Format("{0:D2}分{1:D2}秒{2:D3}", minutes, seconds, ms);

        LayoutRebuilder.ForceRebuildLayoutImmediate(bossLimitRootRectTran);
    }

    public void StartBossLimitCountdown()
    {
        var battleConfig = ConfigManager.Instance.GetById<Config.Battle>(battleConfigId);
        currTimer = battleConfig.BossLimitTime / 1000.0f;
        isHasBossCountdown = true;

        this.RefreshBossInfo();
    }

    public void RefreshSingleHeroInfo(BattleHeroInfoUIData info, int fromEntityGuid)
    {
        var findEntity = BattleEntityManager.Instance.FindEntity(info.heroGuid);
        if (null == findEntity)
        {
            Logx.LogWarning("the entity is not found : " + info.heroGuid);
            return;
        }

        var entityConfig = Config.ConfigManager.Instance.GetById<EntityInfo>(findEntity.configId);
        var isBoss = 1 == entityConfig.IsBoss;

        if (isBoss)
        {
            if (bossData != null)
            {
                if (info.heroGuid == bossData.heroGuid)
                {
                    bossShowObj.UpdateInfo(info);
                }
            }
            else
            {
                bossShowObj.Refresh(info);
            }
        }
        else
        {
            var heroInfo = FindHeroInfo(info.heroGuid);
            if (heroInfo != null)
            {
                heroInfo.UpdateInfo(info);
            }
        }
    }

    public void Update(float deltaTime)
    {
        foreach (var item in showObjList)
        {
            item.Update(deltaTime);
        }

        if (isHasBossCountdown)
        {
            this.currTimer -= deltaTime;
            RefreshBossLimitTimeShow();
        }
    }

    public BattleHeroInfoShowObj FindHeroInfo(int heroGuid)
    {
        foreach (var heroInfo in showObjList)
        {
            if (heroInfo.GetHeroGuid() == heroGuid)
            {
                return heroInfo;
            }
        }

        return null;
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }

    public void Close()
    {
        this.Hide();

        isHasBossCountdown = false;
    }

    void RefreshBattleSingleHeroInfo(BattleEntity_Client entity, int fromEntityGuid)
    {
        BattleHeroInfoUIData info = new BattleHeroInfoUIData();


        var entityConfig = Config.ConfigManager.Instance.GetById<EntityInfo>(entity.configId);
        var isBoss = 1 == entityConfig.IsBoss;

        if (0 == entity.playerIndex || isBoss)
        {
            // 只有自己和队友和 boss 才显示界面他的信息
            info.heroGuid = entity.guid;
            info.currHealth = entity.CurrHealth;
            info.maxHealth = entity.MaxHealth;
            info.heroConfigId = entity.configId;

            RefreshSingleHeroInfo(info,fromEntityGuid);
        }
    }


    public void OnCreateEntity(BattleEntity_Client entity)
    {
        RefreshBattleSingleHeroInfo(entity, 0);
    }

    public void OnChangeEntityBattleData(BattleEntity_Client entity, int fromEntityGuid)
    {
        RefreshBattleSingleHeroInfo(entity, fromEntityGuid);
    }

    public void Release()
    {
        foreach (var item in showObjList)
        {
            item.Release();
        }

        dataList = null;
        showObjList = null;

        EventDispatcher.RemoveListener<BattleEntity_Client>(EventIDs.OnCreateEntity, OnCreateEntity);
        EventDispatcher.RemoveListener<BattleEntity_Client, int>(EventIDs.OnChangeEntityBattleData,
            OnChangeEntityBattleData);
    }
}