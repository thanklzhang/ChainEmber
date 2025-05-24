using System;
using System.Collections;
using System.Collections.Generic;
using Battle_Client;
using Config;
using UnityEngine;
using UnityEngine.UI;


public class BattleProcessUI
{
    public GameObject gameObject;
    public Transform transform;

    private Transform readyStateRoot;
    private Transform battleStateRoot;
    private Transform bossStateRoot;

    private Animator readyAni;
    private Animator battleAni;
    private Animator bossAni;

    private Transform readyTimeRoot;
    private Transform battleTimeRoot;
    private Transform bossTimeRoot;

    private Text readyTimeText;
    private Text battleTimeText;
    private Text bossTimeText;

    private float maxSurplusTime;
    private float surplusTimer;

    private BattleProcessState processState;

    enum BattleProcessState
    {
        Ready = 0,
        Battle = 1,
        Boss = 2
    }

    public BattleUI battleUI;
    public void Init(GameObject gameObject, BattleUI battleUI)
    {
        this.gameObject = gameObject;
        this.transform = this.gameObject.transform;
        this.battleUI = battleUI;

        readyStateRoot = this.transform.Find("readyCommingRoot");
        battleStateRoot = this.transform.Find("battleCommingRoot");
        bossStateRoot = this.transform.Find("bossComingRoot");

        readyTimeRoot = this.transform.Find("readyStateRoot");
        battleTimeRoot = this.transform.Find("battleStateRoot");
        bossTimeRoot = this.transform.Find("bossStateRoot");

        readyTimeText = readyTimeRoot.Find("stageTime").GetComponent<Text>();
        battleTimeText = battleTimeRoot.Find("stageTime").GetComponent<Text>();
        bossTimeText = bossTimeRoot.Find("stageTime").GetComponent<Text>();
        
        readyAni = readyStateRoot.GetComponentInChildren<Animator>();
        battleAni = battleStateRoot.GetComponentInChildren<Animator>();
        bossAni = bossStateRoot.GetComponentInChildren<Animator>();

        EventDispatcher.AddListener<int,int>(EventIDs.OnProcessReadyStateEnter, OnReadyStateEnter);
        EventDispatcher.AddListener<int>(EventIDs.OnProcessBattleStateEnter, OnBattleStateEnter);
        EventDispatcher.AddListener<int>(EventIDs.OnProcessBossStateEnter, OnBossStateEnter);

        readyStateRoot.gameObject.SetActive(false);
        battleStateRoot.gameObject.SetActive(false);
        bossStateRoot.gameObject.SetActive(false);
    }

    public void Show()
    {
        this.gameObject.SetActive(true);
    }

    public void RefreshAllUI()
    {
    }


    void TimePass(float deltaTime)
    {
        surplusTimer -= deltaTime * 1000.0f;
        if (surplusTimer <= 0)
        {
            surplusTimer = 0;
        }
    }

    public void Update(float deltaTime)
    {
        if (processState == BattleProcessState.Ready)
        {
            TimePass(deltaTime);
            
            TimeSpan timeSpan = TimeSpan.FromMilliseconds(surplusTimer); 
            string formattedTime = timeSpan.ToString("mm\\:ss\\:ff");
            readyTimeText.text = formattedTime;
        }
        else if (processState == BattleProcessState.Battle)
        {
            TimePass(deltaTime);
            
            TimeSpan timeSpan = TimeSpan.FromMilliseconds(surplusTimer); 
            string formattedTime = timeSpan.ToString("mm\\:ss\\:ff");
            battleTimeText.text = formattedTime;
        }
        else if (processState == BattleProcessState.Boss)
        {
            TimePass(deltaTime);
            
            TimeSpan timeSpan = TimeSpan.FromMilliseconds(surplusTimer); 
            string formattedTime = timeSpan.ToString("mm\\:ss\\:ff");
            bossTimeText.text = formattedTime;
        }
    }

    void OnReadyStateEnter(int waveIndex,int surplusTime)
    {
        processState = BattleProcessState.Ready;

        maxSurplusTime = surplusTime;
        surplusTimer = maxSurplusTime;

        readyStateRoot.gameObject.SetActive(true);
        battleStateRoot.gameObject.SetActive(false);
        bossStateRoot.gameObject.SetActive(false);

        readyAni.Play("play");

        readyTimeRoot.gameObject.SetActive(true);
        battleTimeRoot.gameObject.SetActive(false);
        bossTimeRoot.gameObject.SetActive(false);
        
        //到准备阶段自动开宝箱
        this.battleUI.ShowBoxUI(true);
    }

    void OnBattleStateEnter(int surplusTime)
    {
        processState = BattleProcessState.Battle;

        maxSurplusTime = surplusTime;
        surplusTimer = maxSurplusTime;

        readyStateRoot.gameObject.SetActive(false);
        battleStateRoot.gameObject.SetActive(true);
        bossStateRoot.gameObject.SetActive(false);

        battleAni.Play("play");

        readyTimeRoot.gameObject.SetActive(false);
        battleTimeRoot.gameObject.SetActive(true);
        bossTimeRoot.gameObject.SetActive(false);
    }


    void OnBossStateEnter(int surplusTime)
    {
        processState = BattleProcessState.Boss;

        maxSurplusTime = surplusTime;
        surplusTimer = maxSurplusTime;


        readyStateRoot.gameObject.SetActive(false);
        battleStateRoot.gameObject.SetActive(false);
        bossStateRoot.gameObject.SetActive(true);

        bossAni.Play("play");


        readyTimeRoot.gameObject.SetActive(false);
        battleTimeRoot.gameObject.SetActive(false);
        bossTimeRoot.gameObject.SetActive(true);
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
        EventDispatcher.RemoveListener<int>(EventIDs.OnProcessBattleStateEnter, OnBossStateEnter);
        EventDispatcher.RemoveListener<int>(EventIDs.OnProcessBossStateEnter, OnBossStateEnter);
    }
}