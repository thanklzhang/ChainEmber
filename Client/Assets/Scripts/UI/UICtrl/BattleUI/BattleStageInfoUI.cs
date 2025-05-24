using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Battle_Client;
using Config;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleStageInfoUIArgs : UICtrlArgs
{
    public int battleConfigId;
}

public class BattleStageInfoUI
{
    public GameObject gameObject;
    public Transform transform;

    private int battleConfigId;

    TextMeshProUGUI stageNameText;

    // private GameObject bossLimitGo;
    // private RectTransform bossLimitRootRectTran;
    // Text bossLimitTimeText;
    private TextMeshProUGUI stageProgressText;

    // //runtime
    // private float currTimer;
    // private bool isHasBossCountdown;
    public void Init(GameObject gameObject, BattleUI battleUIPre)
    {
        this.gameObject = gameObject;
        this.transform = this.gameObject.transform;

        stageNameText = this.transform.Find("stageName").GetComponent<TextMeshProUGUI>();
        stageProgressText = this.transform.Find("stageProgress").GetComponent<TextMeshProUGUI>();
        // bossLimitGo = this.transform.Find("stageTimeRoot").gameObject;
        // bossLimitRootRectTran = bossLimitGo.transform.GetComponent<RectTransform>();
        // bossLimitTimeText = bossLimitGo.transform.Find("stageTime").GetComponent<Text>();
        EventDispatcher.AddListener<int, int>(EventIDs.OnProcessReadyStateEnter, OnReadyStateEnter);
    }

    public void Show()
    {
        this.gameObject.SetActive(true);
    }

    public void RefreshAllUI()
    {
        var battleTableId = BattleManager.Instance.battleConfigId;
        BattleStageInfoUIArgs args = new BattleStageInfoUIArgs();
        args.battleConfigId = battleTableId;

        this.Refresh(args);
    }

    public void Refresh(UICtrlArgs args)
    {
        var uiDataArgs = (BattleStageInfoUIArgs)args;
        battleConfigId = uiDataArgs.battleConfigId;

        this.RefreshInfo();
    }

    // public void StartBossLimitCountdown()
    // {
    //     var battleConfig = ConfigManager.Instance.GetById<Config.Battle>(battleConfigId);
    //     currTimer = battleConfig.BossLimitTime / 1000.0f;
    //     isHasBossCountdown = true;
    //
    //     this.RefreshInfo();
    //
    //
    // }

    void RefreshInfo()
    {
        var battleConfig = ConfigManager.Instance.GetById<Config.Battle>(battleConfigId);

        stageNameText.text = battleConfig.Name;

        var curr = 1;
        var max = GetMaxWave();
        stageProgressText.text = $"{curr}/{max}";

        // if (isHasBossCountdown)
        // {
        //     RefreshBossLimitTimeShow();
        // }
        //
        // this.bossLimitGo.SetActive(isHasBossCountdown);
    }

    // void RefreshBossLimitTimeShow()
    // {
    //     if (this.currTimer < 0)
    //     {
    //         this.currTimer = 0;
    //     }
    //
    //     var minutes = (int)(this.currTimer / 60.0f);
    //     var seconds = (int)(this.currTimer % 60.0f);
    //     var ms = (int)((this.currTimer - (int)currTimer) * 1000);
    //    
    //     bossLimitTimeText.text = string.Format("{0}分{1}秒{2:D3}",minutes,seconds, ms);
    //     
    //     LayoutRebuilder.ForceRebuildLayoutImmediate(bossLimitRootRectTran);
    // }

    public void Update(float deltaTime)
    {
        // if (isHasBossCountdown)
        // {
        //     this.currTimer -= deltaTime;
        //     RefreshBossLimitTimeShow();
        // }
    }

    void OnReadyStateEnter(int waveIndex, int surplusTime)
    {
        var curr = waveIndex + 1;
        var max = GetMaxWave();
        stageProgressText.text = $"{curr}/{max}";
    }

    int GetMaxWave()
    {
        return BattleTool_Client.GetMaxWave(this.battleConfigId);
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }

    public void Close()
    {
        this.Hide();
        // isHasBossCountdown = false;
    }

    public void Release()
    {
        EventDispatcher.RemoveListener<int, int>(EventIDs.OnProcessReadyStateEnter, OnReadyStateEnter);
    }
}