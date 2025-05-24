
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using LitJson;

public class LoadTaskManager : Singleton<LoadTaskManager>
{
    public BaseLoadProcess assetLoadTask;
    public BaseLoadProcess abLoadTask;

    public void Init()
    {
        abLoadTask = new AssetBundleLoadProcess();
        assetLoadTask = new AssetLoadProcess();

        abLoadTask.Init();
        assetLoadTask.Init();

        //监听加载流程中的加载情况
        abLoadTask.SetFinishCallback(OnLoadABFinish);
        assetLoadTask.SetFinishCallback(OnLoadAssetFinish);

    }

    public void Update(float timeDelta)
    {
        abLoadTask?.Update(timeDelta);
        assetLoadTask?.Update(timeDelta);
    }

    public void StartAssetBundleLoader(BaseLoader loader)
    {
        //Logx.Log(abLoadTask + " start to AddLoader");
        abLoadTask.AddLoader(loader);
    }

    public void StartAssetLoader(BaseLoader loader)
    {
        assetLoadTask.AddLoader(loader);
    }

    //加载流程中有 ab 加载完毕
    public void OnLoadABFinish(BaseLoader loader)
    {
        //AssetBundleLoader abLoader = (AssetBundleLoader)loader;

        //AssetBundleCache cache = new AssetBundleCache();
        //cache.path = abLoader.path;
        //cache.assetBundle = abLoader.assetBundle;
        //cache.finishLoadCallback = abLoader.finishLoadCallback;
        //cache.refCount = abLoader.refCount;

        //EventManager.Broadcast((int)GameEvent.LoadABTaskFinish, cache);
    }

    //加载流程中有 asset 加载完毕
    public void OnLoadAssetFinish(BaseLoader loader)
    {

        //AssetLoader assetLoader = (AssetLoader)loader;

        //AssetCache cache = new AssetCache();
        //cache.path = assetLoader.path;
        //cache.asset = assetLoader.asset;
        //cache.finishLoadCallback = assetLoader.finishLoadCallback;

        //EventManager.Broadcast((int)GameEvent.LoadAssetTaskFinish, cache);
    }




}
