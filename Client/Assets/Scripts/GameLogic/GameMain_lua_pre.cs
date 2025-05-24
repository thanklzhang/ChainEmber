//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using System.IO;
//using System.Linq;
//using UnityEngine.UI;
//using UnityEngine.SceneManagement;
//using LitJson;
//
//using GameData;
//using PlotDesigner.Runtime;
//using Battle_Client;
//using XLua;

//public class GameMain : MonoBehaviour
//{
//    public static GameMain Instance;

//    public GameObject tempModelAsset;

//    bool isLoadFinish;

//    private void Awake()
//    {
//        DontDestroyOnLoad(this.gameObject);

//        Instance = this;
//    }

//    private void Start()
//    {

//    }

//    public IEnumerator GameInit(Action finishCallback)
//    {
//        Logx.Log("!!!game startup ... ");

//        Logx.Log("!!!start to init game");

//        NetworkManager.Instance.Init();
//        NetMsgManager.Instance.Init();
//        // NetHandlerManager.Instance.Init();

//        //TODO 此时热更完成

//        //游戏中不依赖下载资源的初始化
//        var uiRoot = transform.Find("Canvas");
//        if (null == uiRoot)
//        {
//            Debug.LogError("the uiRoot with 'Canvas' name is null");
//        }

//        //UIManager.Instance.Init(uiRoot);

//        //var cameraRoot = transform.Find("CameraRoot");
//        //CameraManager.Instance.Init(cameraRoot);

//        //CtrlManager.Instance.Init();

//        if (Const.isUseAB)
//        {
//            AssetBundleManager.Instance.Init();
//        }

//        AssetManager.Instance.Init();
//        LoadTaskManager.Instance.Init();


//        //BattleEntityManager.Instance.Init();
//        //BattleSkillEffectManager.Instance.Init();
//        PlotManager.Instance.Init();


//        ////读取表数据
//        //ConfigManager.Instance.Init();
//        //yield return ConfigManager.Instance.LoadAllTableData();

//        //GameDataManager.Instance.Init();
//        ////////////ServiceManager.Instance.Init();

//        ////全局 ctrl
//        //yield return CtrlManager.Instance.EnterGlobalCtrl();

//        Logx.Log("!!!finish init game");

//        //lua start
//        LuaEnv luaEnv = new LuaEnv();
//        luaEnv.AddLoader(MyLoader);
//        luaEnv.DoString("require 'main.lua'");
//        //

//        yield return null;

//        ////test
//        //PlotManager.Instance.Test();
//        isLoadFinish = true;
//        finishCallback?.Invoke();

//    }

//    //test loader
//    static byte[] MyLoader(ref string filePath)
//    {
//        //PC
//        Debug.Log("Application.dataPath : " + Application.dataPath);
//        var path = Application.dataPath.Replace("/Assets", "") + "/LuaProject/" + filePath + "";
//        return System.Text.Encoding.UTF8.GetBytes(File.ReadAllText(path));
//    }

//    public void StartToLogin()
//    {
//        //CtrlManager.Instance.Enter<LoginCtrl>();
//    }

//    public void StartLocalBattle()
//    {
//        ////TODO: 纯本地战斗 里面的英雄是配置的 结算也是本地的
//        //int battleConfigId = 5900001;
//        //BattleManager.Instance.CreatePureLocalBattle(battleConfigId);
//    }

//    public int currBattleFrameNum = 0;

//    // Update is called once per frame
//    void Update()
//    {

//        LoadTaskManager.Instance.Update(Time.deltaTime);
//        ResourceManager.Instance.Update(Time.deltaTime);
//        //CtrlManager.Instance.Update(Time.deltaTime);

//        //BattleManager.Instance.Update(Time.deltaTime);
//        //BattleEntityManager.Instance.Update(Time.deltaTime);
//        //BattleSkillEffectManager.Instance.Update(Time.deltaTime);

//        PlotManager.Instance.Update(Time.deltaTime);
//        NetworkManager.Instance.Update();

//        currBattleFrameNum = currBattleFrameNum + 1;

//    }

//    private void FixedUpdate()
//    {
//        //BattleManager.Instance.FixedUpdate(Time.fixedDeltaTime);
//    }

//    private void LateUpdate()
//    {
//        //CtrlManager.Instance.LateUpdate(Time.deltaTime);
//    }

//    private void OnDestroy()
//    {

//    }

//}
