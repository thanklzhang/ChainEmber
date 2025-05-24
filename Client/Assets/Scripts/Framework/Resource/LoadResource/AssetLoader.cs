using UnityEngine;
public class AssetLoader : BaseLoader
{
    public string path;
    public AssetBundleRequest assetRequest;

    string abPath;
    
    AssetBundle assetBundle;


    public override void OnPrepare()
    {
        abPath = AssetManager.Instance.GetABPathByAssetPath(path);
        if (null == abPath)
        {
            Logx.LogWarning(LogxType.Asset,"the abPath(have this asset) is nil , assetPath : " + path);
            return;
        }
    }
    public override bool IsPrepareFinish()
    {
        var assetBundle = AssetBundleManager.Instance.GetCacheByPath(abPath);
        if (null == assetBundle)
        {
            Logx.LogWarning(LogxType.Asset,"the assetBundle is nil , abPath : " + abPath);
            return false;
        }

        return assetBundle.isFinishLoad;
    }

    public override void OnPrepareFinish()
    {
      

    }

    public override void OnStartLoad()
    {
        Logx.Log(LogxType.Asset,"AssetLoader : OnStartLoad : path : " + this.path);
        
        var assetBundleCache = AssetBundleManager.Instance.GetCacheByPath(this.abPath);
        if (null == assetBundleCache)
        {
            Logx.LogWarning(LogxType.Asset, "the assetBundleCache is null : " + this.abPath);
            return;
        }

        if (null == assetBundleCache.assetBundle)
        {
            Logx.LogWarning(LogxType.Asset, "the assetBundleCache.assetBundle is null : " + this.abPath);
            return;
        }

        var ab = assetBundleCache.assetBundle;

        //开始加载
        if (resType != null)
        {
            assetRequest = ab.LoadAssetAsync(path, resType);
        }
        else
        {
            assetRequest = ab.LoadAssetAsync(path);
        }
       
    }

    public override bool IsLoadFinish()
    {
        if (null == assetRequest)
        {
            return false;
        }

        return assetRequest.progress >= 1;
    }
    
    internal override void OnLoadFinish()
    {
        Logx.Log(LogxType.Resource, "assetLoader OnLoadFinish : " + this.path);
        
        AssetInfo assetInfo = new AssetInfo();
        assetInfo.path = path;
        assetInfo.asset = assetRequest.asset;
        AssetManager.Instance.OnLoadAssetFinish(assetInfo);
    }

   
}