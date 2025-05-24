using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using Config;

//using Unity.Services.Core;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

public class LobbySceneCtrl : BaseSceneCtrl
{
    private Action loadFinishAction;
    public override void Init()
    {
        sceneName = Config.ConfigManager.Instance.GetById
            <Config.ResourceConfig>((int)ResIds.LobbyScene).Name;
    }

    public override void StartLoad(Action action = null)
    {
        loadFinishAction = action;
        //open loading UI
        CoroutineManager.Instance.StartCoroutine(_StartLoad());
    }

    public IEnumerator _StartLoad()
    {
        //加载场景
        yield return SceneLoadManager.Instance.LoadRequest(sceneName);
        
        //加载 UI 并打开
        yield return UIManager.Instance.EnterRequest<LobbyUI>();
        
        this.LoadFinish();
        
    }

    public void LoadFinish()
    {
        //close loading UI
        
        loadFinishAction ?.Invoke();
    }

    // public override void Exit(Action action)
    // {
    //     SceneLoadManager.Instance.Unload(sceneName,() =>
    //     {
    //         base.Exit(action);
    //     });
    // }
}