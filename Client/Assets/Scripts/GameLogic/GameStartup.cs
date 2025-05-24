using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

//正常游戏流程启动
public class GameStartup : MonoBehaviour
{
    public GameMain gameMain;
    public bool isLocalBattleTest = false;
    
    void Awake()
    {
        Logx.Log(LogxType.Game, "Init");
        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        GlobalConfig.isLocalBattleTest = isLocalBattleTest;;
        this.Startup();
    }

    public void Startup()
    {
        //游戏流程启动
        StartCoroutine(_Startup());
    }

    public IEnumerator _Startup()
    {
        //加载游戏初始化资源
        // var gameInitPrefab = Resources.Load("GameMain") as GameObject;
        // var go = Instantiate(gameInitPrefab);
        // var gameMain = go.GetComponent<GameMain>();
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

   
    private void OnDestroy()
    {
      
    }
}