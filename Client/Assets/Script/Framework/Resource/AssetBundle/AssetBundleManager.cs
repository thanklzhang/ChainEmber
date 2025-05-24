using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class AssetBundleLoadCallback
{
    //透传的 assetInfo
    public ContextAssetInfo contextAssetInfo;   
    //加载 ab 的完成回调
    internal Action<AssetBundleCache, ContextAssetInfo> finishLoadABCallback;
}

public class AssetBundleInfo
{
    public string path;
    public AssetBundle assetBundle;
}

public class AssetBundleManager : Singleton<AssetBundleManager>
{
    //assetBundle 的缓存
    Dictionary<string, AssetBundleCache> abCacheDic = new Dictionary<string, AssetBundleCache>();

    private AssetBundleManifest manifest;

    public void Init()
    {
        Logx.Log(LogxType.AB, "AssetBundleManager : init");
        var manifestAB = AssetBundle.LoadFromFile(GlobalConfig.AssetBundlePath + "/" + "StreamingAssets");
        manifest = manifestAB.LoadAsset<AssetBundleManifest>("AssetBundleManifest");

        //EventManager.AddListener<AssetBundleCache>((int)GameEvent.LoadABTaskFinish, this.OnLoadFinish);
    }

    public string[] GetDependPaths(string path)
    {
        var deps = manifest.GetDirectDependencies(path);
        return deps;
    }


    //assetPath : 透传的 asset 路径，加载完成会回调回去
    public void Load(string path, Action<AssetBundleCache,ContextAssetInfo> finishCallback, bool isSync = false,
        ContextAssetInfo assetInfo = null)
    {
        if (isSync)
        {
            //LoadSync(path, finishCallback, fromRequest);
        }
        else
        {
            LoadAsync(path, finishCallback, assetInfo);
        }
    }


    public void LoadAsync(string path, Action<AssetBundleCache,ContextAssetInfo> finishCallback, ContextAssetInfo assetInfo = null)
    {
       
        Logx.Log(LogxType.AB, "LoadAsync ab : start load : " + path);

        path = path.ToLower();
        var abCache = GetCacheByPath(path);
        if (abCache != null)
        { 
            abCache.ChangeLoadingRefCount(1);
            
            AssetBundleLoadCallback callback = new AssetBundleLoadCallback();
            callback.finishLoadABCallback = finishCallback;
            callback.contextAssetInfo = assetInfo;
                 
            abCache.finishLoadCallbacksList.Add(callback);
                
            LoadTrueAssetBundle(abCache);//, finishCallback, assetInfo
        }
        else
        {
            //没在缓存中 加载新的ab
            
            //先加一个 ab 缓存
            abCache = new AssetBundleCache();
            abCache.Init(path);
            
            abCache.ChangeLoadingRefCount(1);
            
            this.abCacheDic.Add(path, abCache);
            
            //填充加载回调 list
            AssetBundleLoadCallback callback = new AssetBundleLoadCallback();
            callback.finishLoadABCallback = finishCallback;
            callback.contextAssetInfo = assetInfo;
            
            abCache.finishLoadCallbacksList.Add(callback);
            
            LoadTrueAssetBundle(abCache);//, finishCallback, assetInfo
        }
    }

    public AssetBundleCache GetCacheByPath(string path)
    {
        path = path.ToLower();
        AssetBundleCache abCache = null;
        abCacheDic.TryGetValue(path, out abCache);
        return abCache;
    }

    //开始加载 AB 资源
    public void LoadTrueAssetBundle(AssetBundleCache abCache)//, Action<AssetBundleCache,ContextAssetInfo> finishCallback,ContextAssetInfo assetInfo = null
    {
        //加载依赖
        var path = abCache.path;
        var deps = GetDependPaths(path).ToList();
        for (int i = 0; i < deps.Count; i++)
        {
            var depPath = deps[i];
            Load(depPath, null, false);
        }
        
        //Logx.Log("LoadTaskManager.Instance.StartAssetBundleLoader , loader.path : " + loader.path);

        if (abCache.isFinishLoad)
        {
            TriggerABFinishLoadCallbacks(abCache);
            
        }
        else
        {
            if (!abCache.isLoading)
            {
                abCache.isLoading = true;
                //加载任务交给加载管理器去执行
                var loader = new AssetBundleLoader();
                loader.path = path.ToLower();
        
                LoadTaskManager.Instance.StartAssetBundleLoader(loader);
            }
            else
            {
                //正在加载
            }
        }

    }

    //AB 加载完成
    public void OnLoadFinish(AssetBundleInfo loadFinishABInfo)
    {
        Logx.Log( LogxType.AB,"AssetBundleManager : OnLoadFinish : " + loadFinishABInfo.path);
    
        AssetBundleCache abCache = null;
        if (!abCacheDic.TryGetValue(loadFinishABInfo.path, out abCache))
        {
            //理论上不会走到这里 因为加载的时候就有 cache 了 , 即使清理也是清理已经加载完成的 , 正在加载中是不会被清理的
            Logx.LogWarning(LogxType.AB,"the abCache is not found : ab.path : " + loadFinishABInfo.path);
            return;  
        }
        
        abCache.isFinishLoad = true;
        abCache.assetBundle = loadFinishABInfo.assetBundle;

        TriggerABFinishLoadCallbacks(abCache);
    }

    public void TriggerABFinishLoadCallbacks(AssetBundleCache abCache)
    {
        //触发所有回调
        var callbackList = abCache.finishLoadCallbacksList;
        for (int i = 0; i < callbackList.Count; i++)
        {
            var callback = callbackList[i];
            callback.finishLoadABCallback?.Invoke(abCache, callback.contextAssetInfo);
        }
        
        callbackList.Clear();
    }

    public void ChangeLoadingRefCount(int count,string abPath,bool isToUp)
    {
        AssetBundleCache abCache = AssetBundleManager.Instance.GetCacheByPath(abPath);
        if (abCache != null)
        {
            abCache.ChangeLoadingRefCount(count);
            if (isToUp)
            {
                var deps = GetDependPaths(abPath).ToList();
                for (int i = 0; i < deps.Count; i++)
                {
                    var depPath = deps[i];
                    ChangeLoadingRefCount(count,depPath,isToUp);
                }
            }
            
        }
        else
        {
            Logx.LogWarning(LogxType.Asset,
                "the abCache is not found : abPath : " + abPath);
        }
    }

    public void Unload(string path)
    {
        
    }

    //清理 AB , 目前策略是：
    //清理所有 '没在加载' 并且 '没有从该 ab 中加载 asset ' 的 ab 

    //TODO:
    //清理优先级按照 ab 引用计数值排序 ，引用计数越低清理优先级越高，
    //即使 ab 有引用 ， 只要 ab 已经加载完毕  并且 没有 asset 资源在加载
    //那么就可以清除
    //清理数目为总数的一半 ， 优先清理 '清理优先级' 较高的
    public void ReleaseAssetBundles()
    {
        Logx.Log(LogxType.AB, "start ReleaseAssetBundles -------> ");
        
        var delList = new List<AssetBundleCache>();
        foreach (var kv in abCacheDic)
        {
            var abCache = kv.Value;

            if (abCache.isFinishLoad)
            {
                if (!abCache.IsInLoadingRef)
                {
                    delList.Add(abCache);
                }
            }
        }

        foreach (var abCache in delList)
        {
            var path = abCache.path;
            abCache.Release();
            this.abCacheDic.Remove(abCache.path);
            
            Logx.Log(LogxType.Asset, "release ab : path : " + path);
        }
        
        Logx.Log(LogxType.AB, "finish ReleaseAssetBundles <------- ");
    }
}