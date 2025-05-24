using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using LitJson;

public class AssetBundleLoadProcess : BaseLoadProcess
{
    public Dictionary<string, AssetBundleLoader> abLoaderDic = new Dictionary<string, AssetBundleLoader>();


    // public override void AddLoader(BaseLoader loader)
    // {
    //     //Logx.Log("assetBundle : AddLoader");
    //
    //     var assetBundleLoader = (AssetBundleLoader)loader;
    //
    //     //从缓存池中找到是否有相同 path 如果有则附加 finishCallback
    //     //没有的话加入到缓存池中
    //     AssetBundleLoader abLoader = null;
    //     if (!abLoaderDic.TryGetValue(assetBundleLoader.path, out abLoader))
    //     {
    //         //Logx.Log("assetBundle : AddLoader 1 assetBundleLoader.path : " + assetBundleLoader.path);
    //
    //         abLoaderDic.Add(assetBundleLoader.path, assetBundleLoader);
    //         ////Logx.Logz("AssetBundleLoadProcess AddLoader : no loader cache and start a new loader : " + assetBundleLoader.path);
    //         base.AddLoader(loader);
    //     }
    //     else
    //     {
    //         //Logx.Log("assetBundle : AddLoader 2");
    //         ////Logx.Logz("AssetBundleLoadProcess AddLoader : have loader cache " + assetBundleLoader.path);
    //         abLoader.finishLoadCallback.AddRange(assetBundleLoader.finishLoadCallback);
    //         abLoader.refCount += 1;
    //     }
    // }

    public override void OnLoadFinish(BaseLoader loader)
    {
        var assetBundleLoader = (AssetBundleLoader)loader;
        
        //通知 正在准备的所有 loader 在这里指的是正在等依赖加载的 loader
        for (int i = 0; i < this.preparingList.Count; i++)
        {
            var prepareLoader = (AssetBundleLoader)this.preparingList[i];
            prepareLoader.OnLoadOtherABFinish(assetBundleLoader.path);
        }

        base.OnLoadFinish(loader);
    }
}