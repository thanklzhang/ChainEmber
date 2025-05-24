using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using LitJson;

using GameData;
using PlotDesigner.Runtime;
using Battle_Client;
using BattleClient.Debug;
using Config;
using UnityEditor;


public class GameMain : MonoBehaviour
{
    public static GameMain Instance;
    
    public GameObject tempModelAsset;
    public Transform gameObjectRoot;
    bool isLoadFinish;
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        Instance = this;
    }

    public IEnumerator GameInit()
    {
        Logx.Log(LogxType.Game, "game init start ...");

        NetworkManager.Instance.Init();
        NetMsgManager.Instance.Init();
        NetHandlerManager.Instance.Init();

        //TODO 此时热更完成

        //游戏中不依赖下载资源的初始化
        var uiRoot = transform.Find("Canvas");
        if (null == uiRoot)
        {
            Debug.LogError("the uiRoot with 'Canvas' name is null");
        }

        // UIManager.Instance.Init(uiRoot);

        var cameraRoot = transform.Find("CameraRoot");
        CameraManager.Instance.Init(cameraRoot);

        UIManager.Instance.Init(uiRoot);


        if (GlobalConfig.isUseAB)
        {
            AssetBundleManager.Instance.Init();
        }

        AssetManager.Instance.Init();
        LoadTaskManager.Instance.Init();


        BattleEntityManager.Instance.Init();
        BattleSkillEffectManager_Client.Instance.Init();
        PlotManager.Instance.Init();

        var audioRoot = transform.Find("AudioRoot");
        AudioManager.Instance.Init(audioRoot);

        // yield break;

        //读取表数据 这里可能换成异步操作
        ConfigManager.Instance.Init();
        yield return ConfigManager.Instance.LoadAllConfigData();

        //全局 UI
        GlobalUIMgr.Instance.Init();
        yield return UIManager.Instance.LoadGlobalCtrlReq();

        GameDataManager.Instance.Init();
        // ServiceManager.Instance.Init();

        SceneLoadManager.Instance.Init();
        SceneCtrlManager.Instance.Init();

        //对象池
        var objPoolRoot = transform.Find("ObjectPoolRoot");
        ObjectPoolManager.Instance.Init(objPoolRoot);

        gameObjectRoot = transform.Find("GameObjectRoot");

        InitHelper();

        //全局 ctrl
        // yield return CtrlManager.Instance.EnterGlobalCtrl();

        OperateViewManager.Instance.Init();
        OperateViewManager.Instance.StartLoad();

        BattleManager.Instance.Init();

        Logx.Log(LogxType.Game, "game init finish");

        isLoadFinish = true;
    }

    //有互相引用的初始化 在 Init 结束后 在这里初始化
    void InitByRelation()
    {
        
    }

    public void InitHelper()
    {
        AttrInfoHelper.Instance.Init();
    }

    public void StartToLogin()
    {
        Logx.Log(LogxType.Game, "enter login logic");
        SceneCtrlManager.Instance.Enter<LoginSceneCtrl>();
    }

    public void StartLocalBattle()
    {
        //TODO: 纯本地战斗 里面的英雄是配置的 结算也是本地的
        //int battleConfigId = 5900001;
        int battleConfigId = 5900010;

        Logx.Log(LogxType.Game, "create local battle test");

        BattleManager.Instance.CreatePureLocalBattle(battleConfigId);
    }

    public int currBattleFrameNum = 0;

    // Update is called once per frame
    void Update()
    {
        LoadTaskManager.Instance.Update(Time.deltaTime);
        ResourceManager.Instance.Update(Time.deltaTime);
        UIManager.Instance.Update(Time.deltaTime);

        
        BattleManager.Instance.Update(Time.deltaTime);
        BattleEntityManager.Instance.Update(Time.deltaTime);
        BattleSkillEffectManager_Client.Instance.Update(Time.deltaTime);

        PlotManager.Instance.Update(Time.deltaTime);

        NetworkManager.Instance.Update();

        SceneLoadManager.Instance.Update();

        currBattleFrameNum = currBattleFrameNum + 1;
    }

    private void FixedUpdate()
    {
        BattleManager.Instance.FixedUpdate(Time.fixedDeltaTime);
    }

    private void LateUpdate()
    {
        UIManager.Instance.LateUpdate(Time.deltaTime);
        BattleManager.Instance.LateUpdate(Time.deltaTime);
    }

    private void Release()
    {
        BattleManager.Instance.Release();
    }

    private void OnDestroy()
    {
    }
}