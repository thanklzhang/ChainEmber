using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using LitJson;
using UnityEngine.UI;
using Object = UnityEngine.Object;

//asset 资源缓存
public class AssetCache
{
    public string path;
    //internal Action<AssetCache> finishLoadCallback;

    public List<Action<UnityEngine.Object>> finishLoadCallbackList = new List<Action<Object>>();

    internal UnityEngine.Object asset;
    private int refCount;

    //TODO : 可以加入状态 未加载 加载中 加载完成
    public bool isLoading;
    // {
    //     get { return finishLoadCallbackList.Count > 0; }
    // }

    public bool isFinishLoad;

    internal int RefCount
    {
        get => refCount;
        set
        {
            if (value < 0 && refCount <= 0)
            {
                Logx.LogWarning(LogxType.Asset, "the curr refCount is less than or equal to 0 : path : " + path + " , will : " + refCount + " -> " + value);

                return;
            }

            Logx.Log(LogxType.Asset, "change ref : " + path + " : " + refCount + " -> " + value);
            refCount = value;
        }
    }

    public void Init(string path)
    {
        this.path = path;
    }

    public void Release()
    {
         // Resources.UnloadAsset(this.asset);
        this.asset = null;

        finishLoadCallbackList.Clear();
        finishLoadCallbackList = null;
    }
}