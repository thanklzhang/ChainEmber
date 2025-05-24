using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
//using Unity.Services.Core;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

public class SceneLoadManager : Singleton<SceneLoadManager>
{
    private bool isLoadFinish;
    private AsyncOperation loadReq;
    private bool isExit;
    private AsyncOperation unloadReq;
    private string currActiveScenName;

    public Action event_OnSceneLoadFinish;
    
    public void Init()
    {
      
    }

    //加载场景接口
    public IEnumerator LoadRequest(string sceneName)
    {
        Logx.Log(LogxType.SceneCtrl,"start load : " + sceneName);
        
        var isLoadFinish = false;

        yield return LoadRes(sceneName);
        // LoadRes(sceneName, () =>
        // {
        //     isLoadFinish = true;
        // });

        // while (true)
        // {
        //     yield return null;
        //
        //     if (isLoadFinish)
        //     {
        //         Logx.Log(LogxType.SceneCtrl,"load finish : " + sceneName);
        //         break;
        //     }
        // }
        
        // Scene loadedScene = SceneManager.GetSceneByName(sceneName);
        // SceneManager.SetActiveScene(loadedScene);

        isLoadFinish = true;
    }

    //加载内部场景资源
    IEnumerator LoadRes(string sceneName)
    {
        // isLoadFinish = false;
        // isExit = false;

        // event_OnSceneLoadFinish = finishAction;
        currActiveScenName = sceneName;

        if (GlobalConfig.isUseAB)
        {
            var path = (GlobalConfig.buildPath + "/" + GlobalConfig.sceneRootPath + "/" + sceneName + ".unity").ToLower();
            var abPath = AssetManager.Instance.GetABPathByAssetPath(path);
            AssetBundleManager.Instance.Load(abPath, (abCache,assetInfo) =>
            {
                if (abCache.assetBundle != null)
                {
                    loadReq = SceneManager.LoadSceneAsync(currActiveScenName);
                }
            });
        }
        else
        {
            loadReq = SceneManager.LoadSceneAsync(currActiveScenName);
        }

        while (true)
        {
            yield return null;
            
            
            //0.3 -> 0.4
            var resultProgress = 0.3f + loadReq.progress * 0.1f;
            EventSender.SendLoadingProgress(resultProgress,"加载 场景 中");
            
            if (loadReq != null && loadReq.isDone)
            {
                break;
            }
        }
    }

    
    public void Update()
    {
        if (isLoadFinish)
        {
            return;
        }
        //
        // if (loadReq != null && loadReq.isDone)
        // {
        //     isLoadFinish = true;
        // }

        if (GlobalConfig.isUseAB)
        {
            if (!isExit && unloadReq != null && unloadReq.isDone)
            {
                isExit = true;
                OnExitScene();
            }
        }

    }

    // public void OnSceneLoadFinish()
    // {
    //     Logx.Log(LogxType.SceneCtrl,"OnSceneloadFinish  : " + currActiveScenName);
    //     
    //     Scene loadedScene = SceneManager.GetSceneByName(currActiveScenName);
    //     SceneManager.SetActiveScene(loadedScene);
    //     
    //     event_OnSceneLoadFinish?.Invoke();
    // }

    private Action exitAction;
    public void Unload(string sceneName, Action action = null)
    {
        Logx.Log(LogxType.SceneCtrl,"start unload : " + sceneName);
        exitAction = action;

        //场景 Load 之后就会自动调用卸载 所以不用 unload 
        //unloadReq = SceneManager.UnloadSceneAsync(sceneName);
        
        if (GlobalConfig.isUseAB)
        {
            var path = (GlobalConfig.buildPath + "/" + GlobalConfig.sceneRootPath + "/" + sceneName + ".unity").ToLower();
            var abPath = AssetManager.Instance.GetABPathByAssetPath(path);
            AssetBundleManager.Instance.Unload(abPath);
        }
    }

    public void OnExitScene()
    {
        exitAction?.Invoke();
    }

}