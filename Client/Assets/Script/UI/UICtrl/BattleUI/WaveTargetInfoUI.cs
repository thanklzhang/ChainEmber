using System;
using System.Collections;
using System.Collections.Generic;
using Battle;
using Battle_Client;
using Config;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// public class BattlePopulationUIUIArgs : UICtrlArgs
// {
// }


//当前波的目标显示
public class WaveTargetInfoUI
{
    public GameObject gameObject;
    public Transform transform;

    private int battleConfigId;

    private TextMeshProUGUI contentTitleText;
    TextMeshProUGUI contentText;
    private Transform showRoot;

    private BattleUI battleUI;

    // //runtime
    // private float currTimer;
    // private bool isHasBossCountdown;
    public void Init(GameObject gameObject, BattleUI battleUI)
    {
        this.gameObject = gameObject;
        this.transform = this.gameObject.transform;

        this.battleUI = battleUI;


        showRoot = this.transform.Find("root");
        contentTitleText = showRoot.transform.Find("title2").GetComponent<TextMeshProUGUI>();
        contentText = showRoot.transform.Find("content").GetComponent<TextMeshProUGUI>();


        EventDispatcher.AddListener<int,int>(EventIDs.OnProcessReadyStateEnter, OnReadyStateEnter);
        EventDispatcher.AddListener<int>(EventIDs.OnProcessBattleStateEnter, OnBattleStateEnter);
        EventDispatcher.AddListener<int>(EventIDs.OnProcessBossStateEnter, OnBossStateEnter);

        EventDispatcher.AddListener<int, int>(EventIDs.OnUpdateProcessStateInfo, OnUpdateProcessStateInfo);
        
        showRoot.gameObject.SetActive(false);
    }


    public void OnUpdateProcessStateInfo(int currKillCount, int maxKillCount)
    {
        contentText.text = $"{currKillCount}/{maxKillCount}";
    }

    public void OnReadyStateEnter(int waveIndex,int time)
    {
        showRoot.gameObject.SetActive(false);
    }

    public void OnBattleStateEnter(int time)
    {
        contentTitleText.text = "进度";
        showRoot.gameObject.SetActive(true);
    }

    public void OnBossStateEnter(int time)
    {
        showRoot.gameObject.SetActive(true);
        contentTitleText.text = "击杀";
        contentText.text = "Boss";
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
        // var player = BattleManager.Instance.GetLocalPlayer();
        // var currCount = player.teamMemberGuids.Count;
        // var maxCount = player.currency.GetCurrencyCount(BattleCurrency.PopulationId);
        // populationText.text = $"{currCount}/{maxCount}";
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
        EventDispatcher.RemoveListener<int,int>(EventIDs.OnProcessReadyStateEnter, OnReadyStateEnter);
        EventDispatcher.RemoveListener<int>(EventIDs.OnProcessBattleStateEnter, OnBattleStateEnter);
        EventDispatcher.RemoveListener<int>(EventIDs.OnProcessBossStateEnter, OnBossStateEnter);

        EventDispatcher.RemoveListener<int, int>(EventIDs.OnUpdateProcessStateInfo, OnUpdateProcessStateInfo);
    }
}