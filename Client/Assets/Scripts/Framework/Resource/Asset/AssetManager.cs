using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using LitJson;

//当所属的 AB 加载中的时候 , 透传的 asset 信息 , 用于 AB 加载后利用这个透传信息找到 asset 信息
public class ContextAssetInfo
{
    public string path;

    //public Action<UnityEngine.Object> finishCallback;
    public Type type;
}

public class AssetInfo
{
    public string path;
    public UnityEngine.Object asset;
}

public class AssetManager : Singleton<AssetManager>
{
    //asset 资源缓存
    public Dictionary<string, AssetCache> assetCacheDic = new Dictionary<string, AssetCache>();

    //asset 和 ab 的路径对应 , 1 对 1
    public Dictionary<string, string> assetToAbDic = new Dictionary<string, string>();

    //ab 和 asset 的路径对应 , 1 对多
    public Dictionary<string, List<string>> abToAssetsDic = new Dictionary<string, List<string>>();

    public void Init()
    {
        if (GlobalConfig.isUseAB)
        {
            var assetFileStr = File.ReadAllText(GlobalConfig.AppStreamingAssetPath + "/" + "AssetToAbFileData.json");

            //读取 asset 和 ab 对应关系表
            this.assetToAbDic = JsonMapper.ToObject<Dictionary<string, string>>(assetFileStr);

            //读取 ab 和 asset  对应关系表
            foreach (var kv in this.assetToAbDic)
            {
                var assetPath = kv.Key;
                var abPath = kv.Value;


                if (abToAssetsDic.ContainsKey(abPath))
                {
                    abToAssetsDic[abPath].Add(assetPath);
                }
                else
                {
                    abToAssetsDic.Add(abPath, new List<string>()
                    {
                        assetPath
                    });
                }
            }
        }
    }


    //根据 asset 获取 相应所在的 ab
    public string GetABPathByAssetPath(string assetPath)
    {
        string abPath = "";
        if (!assetToAbDic.TryGetValue(assetPath, out abPath))
        {
            Logx.LogWarning("AssetManager", "the abPath is not found by assetPath : " + assetPath);
        }

        return abPath;
    }

    public void Load<T>(string assetPath, Action<UnityEngine.Object> finishCallback, bool isSync = false)
        where T : UnityEngine.Object
    {
        Logx.Log(LogxType.Resource, "res : start load : " + assetPath);
        if (isSync)
        {
            //LoadSync(assetPath, finishCallback);
        }
        else
        {
            LoadAsync<T>(assetPath, finishCallback);
        }
    }

    //--------------------------------
    public void LoadAsync<T>(string assetPath, Action<UnityEngine.Object> finishCallback) where T : UnityEngine.Object
    {
        assetPath = assetPath.ToLower().Replace('\\', '/');
        if (GlobalConfig.isUseAB && !this.assetToAbDic.ContainsKey(assetPath))
        {
            Logx.LogError("Asset", "LoadAsync : the asset doesnt exist in assetToAbDic : " + assetPath);
            return;
        }

        //判断缓存
        AssetCache assetCache = null;
        if (assetCacheDic.TryGetValue(assetPath, out assetCache))
        {
            //有缓存
            assetCache.RefCount += 1;

            if (assetCache.isFinishLoad)
            {
                //有缓存 并且 asset 已经加载完毕
                finishCallback?.Invoke(assetCache.asset);
            }
            else
            {
                //有缓存 但是还在加载中
                Logx.Log(LogxType.Resource, "now loading , asset path : " + assetPath);
                var abPath = this.assetToAbDic[assetPath];

                ContextAssetInfo assetInfo = new ContextAssetInfo();
                assetInfo.path = assetPath;
                // assetInfo.finishCallback = finishCallback;

                assetCache.finishLoadCallbackList.Add(finishCallback);

                Logx.Log(LogxType.Resource, "ab : start load ab , abPath : " + abPath);
                if (GlobalConfig.isUseAB)
                {
                    AssetBundleManager.Instance.Load(abPath, LoadAssetBundleByAssetFinish, false, assetInfo);
                }
                else
                {
#if UNITY_EDITOR
                    T obj = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(assetPath);
                    AssetInfo info = new AssetInfo()
                    {
                        path = assetPath,
                        asset = obj
                    };

                    OnLoadAssetFinish(info);
#endif
                }
            }
        }
        else
        {
            Logx.Log(LogxType.Resource, "start load new resource , asset path : " + assetPath);

            //新的 assetCache
            assetCache = new AssetCache();
            assetCache.Init(assetPath);
            assetCache.RefCount = 1;

            this.assetCacheDic.Add(assetPath, assetCache);

            assetCache.finishLoadCallbackList.Add(finishCallback);

            if (GlobalConfig.isUseAB)
            {
                //透传 assetInfo 给 ab
                ContextAssetInfo assetInfo = new ContextAssetInfo();
                assetInfo.path = assetPath;
                // assetInfo.finishCallback = finishCallback;
                assetInfo.type = typeof(T);


                var abPath = this.assetToAbDic[assetPath];
                Logx.Log(LogxType.Resource, "ab : start load ab , abPath : " + abPath);

                AssetBundleManager.Instance.Load(abPath, LoadAssetBundleByAssetFinish, false, assetInfo);
            }
            else
            {
#if UNITY_EDITOR
                T obj = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(assetPath);
                AssetInfo info = new AssetInfo()
                {
                    path = assetPath,
                    asset = obj
                };

                OnLoadAssetFinish(info);
#endif
            }
        }
    }

    //根据 asset 加载的 AB 加载完成
    public void LoadAssetBundleByAssetFinish(AssetBundleCache abCache, ContextAssetInfo contextAssetInfo)
        // where T : UnityEngine.Object
    {
        AssetCache assetCache = null;

        if (null == abCache)
        {
            Logx.LogWarning(LogxType.Resource, "the abcache is null");
            return;
        }

        if (null == contextAssetInfo)
        {
            Logx.LogWarning(LogxType.Resource, "the abCache.contextAssetInfo is null");
            return;
        }

        if (string.IsNullOrEmpty(contextAssetInfo.path))
        {
            Logx.LogWarning(LogxType.Resource, "the abCache.contextAssetInfo.assetPath is null or empty");
            return;
        }

        var assetPath = contextAssetInfo.path;
        // var finishCallback = contextAssetInfo.finishCallback;
        var assetType = contextAssetInfo.type;

        Logx.Log(LogxType.Resource, "LoadAssetBundleByAssetFinish : asset : " + assetPath);

        if (!assetCacheDic.TryGetValue(assetPath, out assetCache))
        {
            //理论上不会走到这里 因为在加载 asset 的时候就有 assetCache 了 , 而且引用计数 > 0 也不会被清理
            Logx.LogWarning(LogxType.Resource, "LoadAssetBundleByAssetFinish : the asset havent cache : " + assetPath);
            return;
        }

        if (assetCache.isFinishLoad)
        {
            //理论上不会走到这里  因为如果加载完成的话 在之前刚加载的时候就会回调退出的
            Logx.LogWarning(LogxType.Resource,
                "LoadAssetBundleByAssetFinish : the asset is isFinishLoad , : assetPath : " + assetPath);
            return;
        }

        // assetCache.finishLoadCallbackList.Add(finishCallback);

        if (!assetCache.isLoading)
        {
            assetCache.isLoading = true;
            //assest 进入加载流程
            var loader = new AssetLoader();
            loader.path = assetPath;

            if (assetType == typeof(Sprite))
            {
                loader.resType = typeof(Sprite);
            }

            Logx.Log(LogxType.Resource, "start StartAssetLoader : asset : " + assetPath);
            LoadTaskManager.Instance.StartAssetLoader(loader);
        }
    }


    //Asset 加载完成
    internal void OnLoadAssetFinish(AssetInfo assetInfo)
    {
        Logx.Log(LogxType.Resource, "OnLoadAssetFinish : " + assetInfo.path);

        AssetCache assetCache = null;
        if (!assetCacheDic.TryGetValue(assetInfo.path, out assetCache))
        {
            //理论上不会走到这里 因为开始加载的时候就有 cache
            Logx.LogWarning(LogxType.Asset,
                "LoadAssetBundleByAssetFinish : the asset havent cache : " + assetInfo.path);
            return;
        }

        //加载完成 放到缓存中
        assetCache.asset = assetInfo.asset;
        assetCache.isFinishLoad = true;

        TriggerAssetFinishLoadCallbacks(assetCache);
    }

    public void TriggerAssetFinishLoadCallbacks(AssetCache assetCache)
    {
        var callbacks = assetCache.finishLoadCallbackList;

        for (int i = 0; i < callbacks.Count; i++)
        {
            var callback = callbacks[i];
            callback?.Invoke(assetCache.asset);

            if (GlobalConfig.isUseAB)
            {
                //修改 asset 和 ab 之间的加载引用计数
                var abPath = "";
                if (assetToAbDic.TryGetValue(assetCache.path, out abPath))
                {
                    AssetBundleManager.Instance.ChangeLoadingRefCount(-1, abPath, true);
                }
                else
                {
                    Logx.LogWarning(LogxType.Asset,
                        "LoadAssetBundleByAssetFinish : the abPath is not found , abPath : " + abPath +
                        " , assetCache.path : " + assetCache.path);
                }
            }
        }

        callbacks.Clear();
    }

    //卸载资源 如果该资源被 Load ， 那么一定要进行 Unload , 外界保证引用计数的正确性 
    public void UnloadAsset(string assetPath)
    {
        AssetCache cache = null;
        if (!assetCacheDic.TryGetValue(assetPath, out cache))
        {
            Logx.LogWarning("AssetManager", "Unload : the cache doesnt exist : " + assetPath);
            return;
        }

        if (cache.RefCount <= 0)
        {
            Logx.LogWarning("AssetManager", "Unload : the ref of cache has less than 0 : " + assetPath);
            return;
        }

        if (!cache.isFinishLoad)
        {
            Logx.LogWarning("AssetManager", "Unload : the asset is loading : " + assetPath);
            return;
        }

        cache.RefCount -= 1;
    }

    //TODO ： 真正的清理 是从整体来清理的 , 根据 LRU 来排序进行清理 , 清理数目为当前一半 
    //test : 先将 ref 次数为 0 的全部清理
    public void ReleaseUnusedAssets()
    {
        Logx.Log(LogxType.Asset, "start ReleaseUnusedAssets -------> ");

        List<AssetCache> delList = new List<AssetCache>();
        foreach (var kv in this.assetCacheDic)
        {
            var assetCache = kv.Value;
            if (0 == assetCache.RefCount)
            {
                delList.Add(assetCache);
            }
        }

        foreach (var assetCache in delList)
        {
            var path = assetCache.path;
            assetCache.Release();
            this.assetCacheDic.Remove(assetCache.path.ToLower());

            Logx.Log(LogxType.Asset, "release asset : path : " + path);
        }

        Resources.UnloadUnusedAssets();

        Logx.Log(LogxType.Asset, "finish ReleaseUnusedAssets <------- ");
    }
}