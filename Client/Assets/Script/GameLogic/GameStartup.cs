using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

//正常游戏流程启动
public class GameStartup : MonoBehaviour
{
    public bool isLocalBattleTest = false;
    public bool isUseAB = false;
    public bool isUseInternalAB;

    public ServerTypeUI serverTypeUI;

    public GameObject initOperateRoot;

    public GameObject updateResourceRootGo;

    UpdateResourceModule updateResModule;
    UpdateResourceUI updateResourceUI;

    void Awake()
    {
        Logx.Log(LogxType.Game, "Init");

        DontDestroyOnLoad(this.gameObject);

        serverTypeUI.startUp = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        //处理预编译变量
        HandlePreCompileVar();

        //判断初始化环境
        initOperateRoot.gameObject.SetActive(true);
        var formal = false;
        if (formal)
        {
            Logx.Log(LogxType.Game, "mode : Formal Game");
            //正式环境
            Startup();
        }
        else
        {
            if (GlobalConfig.isLocalBattleTest)
            {
                //本地战斗 供测试使用
                Logx.Log(LogxType.Game, "mode : local battle test");
                this.Startup();
            }
            else
            {
                //局域网模式 本地需要开启所有本地服务器     
                // Const.isLANServer = true;
                Logx.Log(LogxType.Game, "mode : LAN Game");

                serverTypeUI.gameObject.SetActive(true);
                //this.Startup(); 
            }
        }
    }

    //处理预编译变量
    void HandlePreCompileVar()
    {
        GlobalConfig.isLocalBattleTest = false;
        GlobalConfig.isUseAB = false;
        GlobalConfig.isUseInternalAB = false;


#if UNITY_EDITOR
        GlobalConfig.isLocalBattleTest = this.isLocalBattleTest;
        GlobalConfig.isUseAB = this.isUseAB;
        GlobalConfig.isUseInternalAB = this.isUseInternalAB;
#else
    #if IS_LOCAL_BATTLE
        Const.isLocalBattleTest = true;
    #endif
            
    #if IS_USE_AB
        Const.isUseAB = true;
    #endif
            
    #if IS_USE_INTERNAL_AB
        Const.isUseInternalAB = true;
    #endif
#endif
    }

    public void Startup()
    {
        //游戏流程启动
        StartCoroutine(_Startup());
    }

    public IEnumerator _Startup()
    {
        if (!GlobalConfig.isLocalBattleTest)
        {
            //检查游戏资源并更新
            yield return CheckResourceUpdate();
        }

        //资源更新完成
        initOperateRoot.gameObject.SetActive(false);

        //加载游戏初始化资源
        var gameInitPrefab = Resources.Load("GameInit") as GameObject;
        var go = Instantiate(gameInitPrefab);
        var gameMain = go.GetComponent<GameMain>();
        yield return gameMain.GameInit();

        //游戏初始化完毕
        if (!GlobalConfig.isLocalBattleTest)
        {
            gameMain.StartToLogin();
        }
        else
        {
            gameMain.StartLocalBattle();
        }
    }

    IEnumerator CheckResourceUpdate()
    {
        //检查资源更新
        updateResModule = new UpdateResourceModule();
        updateResourceUI = new UpdateResourceUI();
        updateResourceUI.Init(updateResourceRootGo, updateResModule);

        //if (Const.isUseAB)
        {
            updateResourceUI.Show();
        }

        //Logx.Log(LogxType.Game,"update resource start");
        UpdateResError updateResError = new UpdateResError();
        yield return updateResModule.CheckResource(updateResError);

        if (updateResError.IsError())
        {
            Logx.LogError("更新资源过程中出错 停止更新 : 原因 : " + updateResError.errInfo);
            updateResModule.TriggerStateEvent(UpdateResStateType.Error, updateResError.errInfo);
            yield break;
        }

        Logx.Log(LogxType.Game, "update resource finish");
    }

    private void OnDestroy()
    {
        this.updateResourceUI?.Release();
    }
}