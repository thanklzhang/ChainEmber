using System;
using System.Collections;
using System.Collections.Generic;
using Battle;
using Battle_Client;
using Config;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattlePopulationUIUIArgs : UICtrlArgs
{
}

public class BattlePopulationUI
{
    public GameObject gameObject;
    public Transform transform;

    private int battleConfigId;

    TextMeshProUGUI populationText;

    public Button buyBtn;
    
    // private GameObject bossLimitGo;
    // private RectTransform bossLimitRootRectTran;
    // Text bossLimitTimeText;

    private BattleUI battleUI;

    // //runtime
    // private float currTimer;
    // private bool isHasBossCountdown;
    public void Init(GameObject gameObject, BattleUI battleUI)
    {
        this.gameObject = gameObject;
        this.transform = this.gameObject.transform;

        this.battleUI = battleUI;

        populationText = this.transform.Find("countText").GetComponent<TextMeshProUGUI>();
        buyBtn = this.transform.Find("addBtn").GetComponent<Button>();
        
        buyBtn.onClick.AddListener(OnBuyBtnClick);

        EventDispatcher.AddListener<List<int>>(EventIDs.OnUpdatePlayerTeamMembersInfo, OnUpdatePlayerTeamMembersInfo);
        EventDispatcher.AddListener(EventIDs.OnUpdateBattleCurrencyInfo, this.OnUpdateBattleCurrencyInfo);
    }

    public void Show()
    {
        this.gameObject.SetActive(true);
    }

    public void RefreshAllUI()
    {
        RefreshCountShow();
    }

    void RefreshCountShow()
    {
        var player = BattleManager.Instance.GetLocalPlayer();
        var currCount = player.teamMemberGuids.Count;
        var maxCount = player.currency.GetCurrencyCount(BattleCurrency.PopulationId);
        populationText.text = $"{currCount}/{maxCount}";
    }

    public void OnBuyBtnClick()
    {
        //判断金币是否够
        var player = BattleManager.Instance.GetLocalPlayer();
        var paramConfig = BattleConfigManager.Instance.GetById<IBattleCommonParam>(1);
        var currBuyPopulation = player.buyInfo.hasBuyPopulation;
        var cost = 0;
        if (currBuyPopulation < paramConfig.BuyPopulationCostCoin.Count)
        {
            cost = paramConfig.BuyPopulationCostCoin[currBuyPopulation];
        }

        if (player.currency.GetCurrencyCount(BattleCurrency.CoinId) < cost)
        {
            Tips.ShowSkillTipText("金币不足");
            return;
        }
        
        BattleManager.Instance.MsgSender.Send_BuyPopulation();
    }
    
    public void OnUpdatePlayerTeamMembersInfo(List<int> guids)
    {
        this.RefreshAllUI();
    }

    public void OnUpdateBattleCurrencyInfo()
    {
        this.RefreshAllUI();
    }


    public void Update(float deltaTime)
    {
        // if (isHasBossCountdown)
        // {
        //     this.currTimer -= deltaTime;
        //     RefreshBossLimitTimeShow();
        // }
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }

    public void Close()
    {
        this.Hide();
    }

    public void Release()
    {
        EventDispatcher.RemoveListener<List<int>>(EventIDs.OnUpdatePlayerTeamMembersInfo,
            OnUpdatePlayerTeamMembersInfo);
        EventDispatcher.RemoveListener(EventIDs.OnUpdateBattleCurrencyInfo, this.OnUpdateBattleCurrencyInfo);
        
        buyBtn.onClick.RemoveListener(OnBuyBtnClick);
    }
}