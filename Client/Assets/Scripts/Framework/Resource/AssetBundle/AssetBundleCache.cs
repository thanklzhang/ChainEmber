using System.Collections.Generic;
using UnityEngine;

public class AssetBundleCache
{
    public string path;

    //加载完成回调 list
    public List<AssetBundleLoadCallback> finishLoadCallbacksList = new List<AssetBundleLoadCallback>();

    internal AssetBundle assetBundle;

    public bool isLoading;
    // {
    //     get
    //     {
    //         return finishLoadCallbacksList.Count > 0;
    //     }
    // }

    //当前 AB 是否加载完
    public bool isFinishLoad;

    //是否在 asset 加载中被引用 , 父子结构叠加计数
    public bool IsInLoadingRef
    {
        get { return LoadingRefCount > 0; }
    }

    private int loadingRefCount;

    public int LoadingRefCount
    {
        get { return loadingRefCount; }
        set
        {
            Logx.Log(LogxType.AB, "LoadingRefCount : change ref : " + path + " : " + loadingRefCount + " -> " + value);
            loadingRefCount = value;
        }
    }

    public void ChangeLoadingRefCount(int count)
    {
        LoadingRefCount += count;

        if (LoadingRefCount < 0)
        {
            Logx.LogWarning(LogxType.AB, "the loadingRefCount is less than 0 , ab path : " + path);
        }
    }

    public void Init(string path)
    {
        this.path = path;
    }

    public void Release()
    {
        assetBundle.Unload(false);
        assetBundle = null;
        finishLoadCallbacksList?.Clear();
        finishLoadCallbacksList = null;
    }
}