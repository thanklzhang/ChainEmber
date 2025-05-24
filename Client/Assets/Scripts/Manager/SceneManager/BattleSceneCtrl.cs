using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using Battle_Client;
using Config;

//using Unity.Services.Core;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;
public enum BattleType
{
    Remote = 0,
    LocalButRemoteResult = 1,
    PureLocal = 2
}
public class BattleSceneCtrl : BaseSceneCtrl
{
    public override void Init()
    {
        //sceneName = Config.ConfigManager.Instance.GetById<Config.ResourceConfig>((int)ResIds.LoginScene).Name;
    }
  
    public override void StartLoad(Action action = null)
    {
        CoroutineManager.Instance.StartCoroutine(_StartLoad());
    }

    public IEnumerator _StartLoad()
    {
        UIManager.Instance.Open<LoadingUICtrl>();
        
        EventSender.SendLoadingProgress(0 / 1.0f,"");
        
        //loading 界面开始
        // currProgress = 0;
        // maxProgress = 1;
        // SetLoadingProgress();
        // CtrlManager.Instance.GlobalCtrlPre.LoadingUIPre.Show();

        //加载战斗所需资源
        yield return BattleManager.Instance.StartLoad();

        Logx.Log(LogxType.Game,"BattleSceneCtrl : all laod finish");
        
        //加载完成
        this.LoadFinish();

    }


    public void LoadFinish()
    {
        //加载结束 关闭 loading 界面
        
        EventSender.SendLoadingProgress(1.0f,"加载完成");
        
        
        // UICtrlManager.Instance.Close<LoadingUICtrl>();
        
        BattleManager.Instance.MsgSender.Send_PlayerLoadProgress(1000);
        
        AudioManager.Instance.PlayBGM((int)ResIds.bgm_battle_001);
        
        
    }

    // public override void Exit(Action action)
    // {
    //     SceneLoadManager.Instance.Unload(sceneName,() =>
    //     {
    //         base.Exit(action);
    //     });
    // }
}