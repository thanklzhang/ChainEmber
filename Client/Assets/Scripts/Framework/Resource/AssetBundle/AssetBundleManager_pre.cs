// using System;
// using System.Collections;
// using System.Collections.Generic;
// using System.IO;
// using System.Linq;
// using System.Text;
// using System.Threading.Tasks;
// using UnityEngine;
// using LitJson;
// using UnityEditor.VersionControl;
//
// public class AssetBundleLoadCallback
// {
//     public ContextAssetInfo contextAssetInfo;
//     internal Action<AssetBundleCache, ContextAssetInfo> finishLoadCallback;
// }
//
// public class AssetBundleCache
// {
//     public string path;
//
//     // //透传的加载完成回调 可以认为是业务层传来的回调
//     // internal Action<AssetBundleCache> finishLoadCallback;
//     //
//     // //透传的 assetInfo
//     // public ContextAssetInfo contextAssetInfo;
//
//     public List<AssetBundleLoadCallback> finishLoadCallbacksList;
//
//     internal AssetBundle assetBundle;
//     private int refCount;
//
//     //当前 AB 是否加载完
//     public bool isFinishLoad;
//
//     //是否有 asset 正在加载
//     public bool IsLoadingAssetFromAB
//     {
//         get { return loadingAssetsDic.Count > 0; }
//     }
//
//     public Dictionary<string, int> loadingAssetsDic = new Dictionary<string, int>();
//
//
//     public void AddLoadAsset(string path)
//     {
//         if (!loadingAssetsDic.ContainsKey(path))
//         {
//             loadingAssetsDic.Add(path, 1);
//         }
//         else
//         {
//             loadingAssetsDic[path] += 1;
//         }
//     }
//
//     public void RemoveLoadAsset(string path)
//     {
//         if (loadingAssetsDic.ContainsKey(path))
//         {
//             loadingAssetsDic.Remove(path);
//         }
//         else
//         {
//             Logx.LogWarning(LogxType.Asset, "the loadingAssetsDic doesnt contain path : " + path);
//         }
//     }
//
//     //目前这个引用计数只会在 从 assetManager 加载 asset 的流程开始的时候开始计数 
//     //其他情况暂不考虑
//     public int RefCount
//     {
//         get => refCount;
//         set
//         {
//             //Logx.Logz("ab : refCount : " + path + " : " + refCount + " -> " + value);
//             refCount = value;
//         }
//     }
// }
//
// public class AssetBundleManager : Singleton<AssetBundleManager>
// {
//     //assetBundle 的缓存
//     Dictionary<string, AssetBundleCache> abCacheDic = new Dictionary<string, AssetBundleCache>();
//
//     private AssetBundleManifest manifest;
//
//     public void Init()
//     {
//         //Logx.Logzxy("AB", "abLog : init");
//         var manifestAB = AssetBundle.LoadFromFile(Const.AssetBundlePath + "/" + "StreamingAssets");
//         manifest = manifestAB.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
//
//         //EventManager.AddListener<AssetBundleCache>((int)GameEvent.LoadABTaskFinish, this.OnLoadFinish);
//     }
//
//     public string[] GetDependPaths(string path)
//     {
//         var deps = manifest.GetDirectDependencies(path);
//         return deps;
//     }
//
//
//     //assetPath : 透传的 asset 路径，加载完成会回调回去
//     public void Load(string path, Action<AssetBundleCache,ContextAssetInfo> finishCallback, bool isSync,
//         ContextAssetInfo assetInfo = null)
//     {
//         if (isSync)
//         {
//             //LoadSync(path, finishCallback, fromRequest);
//         }
//         else
//         {
//             LoadAsync(path, finishCallback, assetInfo);
//         }
//     }
//
//
//     public void LoadAsync(string path, Action<AssetBundleCache,ContextAssetInfo> finishCallback, ContextAssetInfo assetInfo = null)
//     {
//         //Logx.Logzxy("AB", "LoadAsync : start load : " + path);
//
//         path = path.ToLower();
//         var abCache = GetCacheByPath(path);
//         if (abCache != null)
//         {
//             ////Logx.Logz("LoadAsync : have ab cache");
//             //判断是否在 ab 缓存中 
//             //有的话直接拿走
//             abCache.RefCount += 1;
//             // ChangeRefCount(abCache, 1);
//
//             if (abCache.isFinishLoad)
//             {
//                 //已经加载完成
//                 // abCache.contextAssetInfo = assetInfo; //bug?
//
//                 finishCallback?.Invoke(abCache,assetInfo);
//             }
//             else
//             {
//                 //加载中
//                 //TODO : 这时候 abLoad 的 callbackList 应该没用了 可以去掉
//                 Logx.Log(LogxType.AB, "now loading , ab :  : " + abCache.path);
//
//
//                 AssetBundleLoadCallback callback = new AssetBundleLoadCallback();
//                 callback.finishLoadCallback = finishCallback;
//                 callback.contextAssetInfo = assetInfo;
//                 abCache.finishLoadCallbacksList.Add(callback);
//
//
//                 LoadTrueAssetBundle(path, finishCallback, assetInfo);
//             }
//         }
//         else
//         {
//             ////Logx.Logz("LoadAsync : no ab cache");
//             //可能没在缓存中 加载新的ab
//
//
//             AssetBundleCache newAbCache = new AssetBundleCache();
//             newAbCache.path = path;
//             // newAbCache.finishLoadCallback = finishCallback;
//             this.abCacheDic.Add(path, newAbCache);
//             newAbCache.isFinishLoad = false;
//             newAbCache.RefCount = 1;
//             
//             AssetBundleLoadCallback callback = new AssetBundleLoadCallback();
//             callback.finishLoadCallback = finishCallback;
//             callback.contextAssetInfo = assetInfo;
//             newAbCache.finishLoadCallbacksList.Add(callback);
//
//             LoadTrueAssetBundle(path, finishCallback, assetInfo);
//         }
//     }
//
//     public AssetBundleCache GetCacheByPath(string path)
//     {
//         path = path.ToLower();
//         AssetBundleCache abCache = null;
//         abCacheDic.TryGetValue(path, out abCache);
//         return abCache;
//     }
//
//     public void LoadTrueAssetBundle(string path, Action<AssetBundleCache,ContextAssetInfo> finishCallback,
//         ContextAssetInfo assetInfo = null)
//     {
//         // var abLoadList = LoadTaskManager.Instance.abLoadTask.preparingList;
//         // var loader = abLoadList.Find((abLoader) =>
//         // {
//         //     var currPath = abLoader.GetPath();
//         //     if(!path.Equals(""))
//         //     {
//         //         return path == currPath;
//         //     }
//         //
//         //     return false;
//         // });
//         //
//         // if (null == loader)
//         // {
//         //     loader = new AssetBundleLoader();
//         //     var abLoader = loader as AssetBundleLoader;
//         //     abLoader.path = path;
//         //     abLoader.finishLoadCallback = finishCallback;
//         // }
//         //
//         //
//
//
//         var loader = new AssetBundleLoader();
//         loader.path = path.ToLower();
//         // 这里应该是不用了 因为都在 abCache 中储存着 cb 了
//         // loader.finishLoadCallback = finishCallback;
//         // loader.contextAssetInfo = assetInfo;
//
//         // AssetBundle
//
//
//         //依赖
//         var deps = GetDependPaths(path).ToList();
//         for (int i = 0; i < deps.Count; i++)
//         {
//             var depPath = deps[i];
//             Load(depPath, null, false);
//         }
//         //Logx.Log("LoadTaskManager.Instance.StartAssetBundleLoader , loader.path : " + loader.path);
//         //加载任务交给加载管理器去执行
//         // Logx.Log("AssetBundleManager : LoadTrueAssetBundle : start LoadTrueAssetBundle : " + path);
//         //这里会判断 是否有相同 path 的 loader
//
//         LoadTaskManager.Instance.StartAssetBundleLoader(loader);
//     }
//
//     //有 AB 加载完成
//     public void OnLoadFinish(AssetBundleCache ab)
//     {
//         // Logx.Log("AssetBundleManager : OnLoadFinish : " + ab.path);
//         //判断缓存中是否有 没有的话添加到缓存中
//         AssetBundleCache abCache = null;
//         if (!abCacheDic.TryGetValue(ab.path, out abCache))
//         {
//             abCacheDic.Add(ab.path, ab);
//             abCache = abCacheDic[ab.path];
//         }
//         else
//         {
//             //abCache.RefCount += ab.RefCount;
//             //这里可能有问题 如果这里增加了引用计数 那么当 asset 加载好之后 ab 这边又会增加引用计数
//             //ChageRefCount(abCache, ab.RefCount);
//         }
//
//         //这个回调是加载开始的时候的时候传的回调 可以认为是相对的业务层传来的回调
//         // var finishCallback = ab.finishLoadCallback;
//         // finishCallback?.Invoke(abCache);
//
//         var callbackList = ab.finishLoadCallbacksList;
//         for (int i = 0; i < callbackList.Count; i++)
//         {
//             var callback = callbackList[i];
//             callback.finishLoadCallback?.Invoke(ab, callback.contextAssetInfo);
//         }
//     }
//
//
//     public void AddAssetBundleReference(string abPath)
//     {
//         AssetBundleCache abCache = null;
//         if (abCacheDic.TryGetValue(abPath, out abCache))
//         {
//             //abCache.RefCount += 1; 
//             ChangeRefCount(abCache, 1);
//         }
//         else
//         {
//             //Logx.Logz("the abPath is not found : " + abPath);
//         }
//     }
//
//     internal void ReduceAssetBundleReference(string abPath)
//     {
//         AssetBundleCache abCache = null;
//         if (abCacheDic.TryGetValue(abPath, out abCache))
//         {
//             if (abCache.RefCount <= 0)
//             {
//                 Logx.LogWarning("AB", "the refCount of abCache is 0 : " + abPath);
//                 return;
//             }
//
//             //abCache.RefCount -= 1;
//             ChangeRefCount(abCache, -1);
//         }
//         else
//         {
//             //Logx.Logz("the abPath is not found : " + abPath);
//         }
//     }
//
//     public void ChangeRefCount(AssetBundleCache cache, int value)
//     {
//         cache.RefCount += value;
//         var deps = this.GetDependPaths(cache.path);
//         for (int i = 0; i < deps.Length; i++)
//         {
//             var depPath = deps[i];
//             if (this.abCacheDic.ContainsKey(depPath))
//             {
//                 var currCache = this.abCacheDic[depPath];
//                 ChangeRefCount(currCache, value);
//             }
//             else
//             {
//                 //Logx.Logz("the abCacheDic doesnt contain this depPath : path and depPath are : " + cache.path + " " + depPath);
//             }
//         }
//     }
//
//     public void TrueReleaseAB(string abPath)
//     {
//         AssetBundleCache abCache = null;
//         if (abCacheDic.TryGetValue(abPath, out abCache))
//         {
//             if (abCache.RefCount > 0)
//             {
//                 Logx.LogWarning("AB", "the refCount of abCache is using(refCount > 0) : " + abPath);
//                 return;
//             }
//
//             var ab = abCache.assetBundle;
//             ab.Unload(true);
//             this.abCacheDic.Remove(abCache.path);
//         }
//     }
//
//     
//     public void Unload(string path)
//     {
//         
//     }
//
//     //清理 AB , 目前策略是：
//
//     //清理优先级按照 ab 引用计数值排序 ，引用计数越低清理优先级越高，
//     //即使 ab 有引用 ， 只要 ab 已经加载完毕  并且 没有 asset 资源在加载
//     //那么就可以清除
//     //清理数目为总数的一半 ， 优先清理 '清理优先级' 较高的
//     public void Release()
//     {
//     }
// }