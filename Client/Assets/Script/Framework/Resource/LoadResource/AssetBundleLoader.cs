using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using LitJson;

public class AssetBundleLoader : BaseLoader
{
    public string path;

    public List<Action<AssetBundleCache, ContextAssetInfo>> finishLoadCallback;

    public List<string> deps;

    AssetBundleCreateRequest abCreateReq;

    public override void OnPrepare()
    {
        deps = AssetBundleManager.Instance.GetDependPaths(this.path).ToList();
        var finishList = new List<string>();
        foreach (var dep in deps)
        {
            var abCache = AssetBundleManager.Instance.GetCacheByPath(dep);
            if (abCache != null && abCache.isFinishLoad)
            {
                finishList.Add(dep);
            }
        }

        foreach (var finishPath in finishList)
        {
            deps.Remove(finishPath);
        }
    
    }

    internal void OnLoadOtherABFinish(string path)
    {
        Logx.Log("OnLoadOtherABFinish : " + this.path + " by " + path);
        if (path.Contains("login.ab"))
        {
            Logx.Log("");
        }

        //判断是否是当前 ab 所依赖的 ab
        //是的话 deps 移除该路径 相当于完成了一个加载依赖
       
        if (deps.Contains(path))
        {
            deps.Remove(path);
        }
    }
    
    public override bool IsPrepareFinish()
    {
        return 0 == deps.Count;
    }
    
    public override void OnPrepareFinish()
    {
      
    }

    public override void OnStartLoad()
    {
        var resultPath = GetABLoadPath(this.path);
        abCreateReq = AssetBundle.LoadFromFileAsync(resultPath);
    }

    public override bool IsLoadFinish()
    {
        if (null == abCreateReq)
        {
            return false;
        }

        //Logx.Log("abCreateReq progress : " + abCreateReq.progress);
        return abCreateReq.progress >= 1;
    }
    internal override void OnLoadFinish()
    {
        var loadFinishInfo = new AssetBundleInfo()
        {
            path = this.path,
            assetBundle = abCreateReq.assetBundle
        };

        AssetBundleManager.Instance.OnLoadFinish(loadFinishInfo);
    }


    string GetABLoadPath(string path)
    {
        return GlobalConfig.AssetBundlePath + "/" + path; // + Const.ExtName;
    }

    public override string GetPath()
    {
        return this.path;
    }
}