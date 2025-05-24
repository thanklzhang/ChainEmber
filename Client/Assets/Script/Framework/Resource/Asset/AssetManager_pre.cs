// using System;
// using System.Collections;
// using System.Collections.Generic;
// using System.IO;
// using System.Linq;
// using System.Text;
// using System.Threading.Tasks;
// 
// using UnityEngine;
// using LitJson;
// using UnityEngine.UI;
// using Object = UnityEngine.Object;
//
//
// //当所属的 AB 加载中的时候 , 透传的 asset 信息 , 用于 AB 加载后利用这个透传信息找到 asset 信息
// public class ContextAssetInfo
// {
//     public string assetPath;
//     public Action<UnityEngine.Object> finishCallback;
// }
//
// public class AssetManager : Singleton<AssetManager>
// {
//     //asset 资源缓存
//     public Dictionary<string, AssetCache> assetCacheDic = new Dictionary<string, AssetCache>();
//     
//     //asset 和 ab 的路径对应 , 1 对 1
//     public Dictionary<string, string> assetToAbDic = new Dictionary<string, string>();
//     
//     //ab 和 asset 的路径对应 , 1 对多
//     public Dictionary<string, List<string>> abToAssetsDic = new Dictionary<string, List<string>>();
//
//     public void Init()
//     {
//         //Logx.Logzxy("Asset", "init");
//
//       
//         if (Const.isUseAB)
//         {
//             var assetFileStr = File.ReadAllText(Const.AppStreamingAssetPath + "/" + "AssetToAbFileData.json");
//             
//             //读取 asset 和 ab 对应关系表
//             this.assetToAbDic = JsonMapper.ToObject<Dictionary<string, string>>(assetFileStr);
//
//             //读取 ab 和 asset  对应关系表
//             foreach (var kv in this.assetToAbDic)
//             {
//                 var assetPath = kv.Key;
//                 var abPath = kv.Value;
//
//
//                 if (abToAssetsDic.ContainsKey(abPath))
//                 {
//                     abToAssetsDic[abPath].Add(assetPath);
//                 }
//                 else
//                 {
//                     abToAssetsDic.Add(abPath, new List<string>()
//                     {
//                         assetPath
//                     });
//                 }
//             }
//         }
//
//
//         //EventManager.AddListener<AssetCache>((int)GameEvent.LoadAssetTaskFinish, this.OnLoadAssetFinish);
//     }
//
//     //
//     // public string GetABPathByAssetPath(string assetPath)
//     // {
//     //     string abPath = "";
//     //     if (!assetToAbDic.TryGetValue(assetPath, out abPath))
//     //     {
//     //         Logx.LogWarning("AssetManager", "the abPath is not found by assetPath : " + assetPath);
//     //     }
//     //
//     //     return abPath;
//     // }
//
//     public void Load<T>(string assetPath, Action<UnityEngine.Object> finishCallback, bool isSync = false)
//         where T : UnityEngine.Object
//     {
//         Logx.Log(LogxType.Resource, "res : start load : " + assetPath);
//         if (isSync)
//         {
//             //LoadSync(assetPath, finishCallback);
//         }
//         else
//         {
//             LoadAsync<T>(assetPath, finishCallback);
//         }
//     }
//
//     //--------------------------------
//     public void LoadAsync<T>(string assetPath, Action<UnityEngine.Object> finishCallback) where T : UnityEngine.Object
//     {
//         assetPath = assetPath.ToLower().Replace('\\', '/');
//         if (Const.isUseAB && !this.assetToAbDic.ContainsKey(assetPath))
//         {
//             Logx.LogError("Asset", "LoadAsync : the asset doesnt exist in assetToAbDic : " + assetPath);
//             return;
//         }
//
//         //判断缓存
//         AssetCache assetCache = null;
//         if (assetCacheDic.TryGetValue(assetPath, out assetCache))
//         {
//             assetCache.RefCount += 1;
//
//             if (assetCache.isFinishLoad)
//             {
//                 if (Const.isUseAB)
//                 {
//                     AddAssetBundleReferenceByAssetPath(assetPath);
//                 }
//
//                 //已经加载完成
//                 finishCallback?.Invoke(assetCache.asset);
//             }
//             else
//             {
//                 //还在加载中
//                 Logx.Log(LogxType.Resource, "now loading , asset path : " + assetPath);
//                 var abPath = this.assetToAbDic[assetPath];
//
//                 ContextAssetInfo assetInfo = new ContextAssetInfo();
//                 assetInfo.assetPath = assetPath;
//                 assetInfo.finishCallback = finishCallback;
//
//                 Logx.Log(LogxType.Resource, "ab : start load ab , abPath : " + abPath);
//                 AssetBundleManager.Instance.Load(abPath,
//                     (abCache, cbAssetInfo) => { this.LoadAssetBundleByAssetFinish<T>(abCache, cbAssetInfo); }, false,
//                     assetInfo);
//             }
//         }
//         else
//         {
//             Logx.Log(LogxType.Resource, "start load new resource , asset path : " + assetPath);
//
//             if (Const.isUseAB)
//             {
//                 var abPath = this.assetToAbDic[assetPath];
//
//
//                 assetCache = new AssetCache();
//                 assetCache.path = assetPath;
//                 // assetCache.finishLoadCallback = finishCallback;
//                 assetCache.isFinishLoad = false;
//                 assetCache.RefCount = 1;
//
//                 this.assetCacheDic.Add(assetPath, assetCache);
//
//                 // AssetBundleCache abCache = new AssetBundleCache();
//
//                 //透传 assetInfo 给 ab
//                 ContextAssetInfo assetInfo = new ContextAssetInfo();
//                 assetInfo.assetPath = assetPath;
//                 assetInfo.finishCallback = finishCallback;
//
//                 // abCache.contextAssetInfo = assetInfo;
//
//                 Logx.Log(LogxType.Resource, "ab : start load ab , abPath : " + abPath);
//                 AssetBundleManager.Instance.Load(abPath, (abCache, cbAssetInfo) =>
//                 {
//                     // Logx.Log(LogxType.Resource,"finish load new resource , asset path : " + assetPath);
//                     this.LoadAssetBundleByAssetFinish<T>(abCache, cbAssetInfo);
//                 }, false, assetInfo);
//             }
// //             else
// //             {
// // #if UNITY_EDITOR
// //                 //TODO Editor
// //                 // //var obj =UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(assetPath);
// //                 // AssetBundleCache abCache = new AssetBundleCache();
// //                 // abCache.contextAssetInfo = new ContextAssetInfo();
// //                 // abCache.contextAssetInfo.assetPath = assetPath;
// //                 // abCache.contextAssetInfo.finishCallback = finishCallback;
// //                 //
// //                 // this.LoadAssetBundleByAssetFinish<T>(abCache);
// // #endif
// //             }
//         }
//     }
//
//
//     public void ChangeRef(string path, int count)
//     {
//         AssetCache cache = null;
//         if (this.assetCacheDic.TryGetValue(path, out cache))
//         {
//             cache.RefCount += count;
//         }
//     }
//
//     public void AddAssetBundleReferenceByAssetPath(string assetPath)
//     {
//         string abPath = "";
//         if (!assetToAbDic.TryGetValue(assetPath, out abPath))
//         {
//             Logx.LogWarning("Asset",
//                 "AddAssetBundleReferenceByAssetPath : the abPath is not found by assetPath : " + assetPath);
//             return;
//         }
//
//         AssetBundleManager.Instance.AddAssetBundleReference(abPath);
//     }
//
//     //根据 asset 加载的 AB 加载完成
//     public void LoadAssetBundleByAssetFinish<T>(AssetBundleCache abCache, ContextAssetInfo contextAssetInfo)
//         where T : UnityEngine.Object
//     {
//         // Logx.Log("res : load finish : " + assetPath);
//         AssetCache assetCache = null;
//
//         if (null == abCache)
//         {
//             Logx.LogWarning(LogxType.Resource, "the abcache is null");
//             return;
//         }
//
//         if (null == contextAssetInfo)
//         {
//             Logx.LogWarning(LogxType.Resource, "the abCache.contextAssetInfo is null");
//             return;
//         }
//
//         if (string.IsNullOrEmpty(contextAssetInfo.assetPath))
//         {
//             Logx.LogWarning(LogxType.Resource, "the abCache.contextAssetInfo.assetPath is null or empty");
//             return;
//         }
//
//         var assetPath = contextAssetInfo.assetPath;
//         var finishCallback = contextAssetInfo.finishCallback;
//
//         Logx.Log(LogxType.Resource, "LoadAssetBundleByAssetFinish : asset : " + assetPath);
//
//         if (assetCacheDic.TryGetValue(assetPath, out assetCache))
//         {
//             if (assetCache.isFinishLoad)
//             {
//                 //已经有 asset 了 那么直接回调即可
//                 finishCallback?.Invoke(assetCache.asset);
//             }
//             else
//             {
//                 //还在加载中
//                 var loader = new AssetLoader();
//                 loader.path = assetPath;
//                 loader.finishLoadCallbackList.Add(finishCallback);
//
//                 if (typeof(T) == typeof(Sprite))
//                 {
//                     loader.resType = typeof(Sprite);
//                 }
//
//                 //这个函数里面会判断附加 loader 的逻辑 
//                 Logx.Log(LogxType.Resource, "start StartAssetLoader : asset : " + assetPath);
//                 LoadTaskManager.Instance.StartAssetLoader(loader);
//             }
//
//
//             // assetCache.RefCount += 1;
//             // if (Const.isUseAB)
//             // {
//             //     AddAssetBundleReferenceByAssetPath(assetPath);
//             // }
//         }
//         else
//         {
//             Logx.LogWarning(LogxType.Resource, "LoadAssetBundleByAssetFinish : the asset havent cache : " + assetPath);
//
// //             //理论上不会走到这里 因为在加载 asset 的时候就有 assetCache 了
// //             if (Const.isUseAB)
// //             {
// //                 var loader = new AssetLoader();
// //                 loader.path = assetPath;
// //                 loader.finishLoadCallback = (backAssetCache) =>
// //                 {
// //                     //触发业务层回调
// //                     finishCallback?.Invoke(backAssetCache.asset);
// //                 };
// //                 if (typeof(T) == typeof(Sprite))
// //                 {
// //                     loader.resType = typeof(Sprite);
// //                 }
// //
// //                 LoadTaskManager.Instance.StartAssetLoader(loader);
// //             }
// //             else
// //             {
// // #if UNITY_EDITOR
// //                 T obj = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(assetPath);
// //
// //                 AssetCache ac = new AssetCache()
// //                 {
// //                     asset = obj,
// //                     finishLoadCallback = (backAssetCache) =>
// //                     {
// //                         //触发业务层回调
// //                         finishCallback?.Invoke(backAssetCache.asset);
// //                     },
// //                     path = assetPath
// //                 };
// //
// //                 OnLoadAssetFinish(ac);
// //
// //
// // #endif
// //             }
//         }
//     }
//
//
//     //Asset 加载完成
//     internal void OnLoadAssetFinish(AssetCache assetCache)
//     {
//         Logx.Log(LogxType.Resource, "OnLoadAssetFinish : " + assetCache?.path);
//         //加载完成 放到缓存中
//         AssetCache currAssetCache = null;
//         if (!assetCacheDic.TryGetValue(assetCache.path, out currAssetCache))
//         {
//             // assetCacheDic.Add(assetCache.path, assetCache);
//             // //新的 cache
//             // currAssetCache = assetCacheDic[assetCache.path];
//
//             Logx.LogWarning(LogxType.Resource,
//                 "LoadAssetBundleByAssetFinish : the asset havent cache : " + assetCache.path);
//         }
//         else
//         {
//             // currAssetCache.RefCount += assetCache.RefCount;
//
//             currAssetCache.asset = assetCache.asset;
//             currAssetCache.isFinishLoad = true;
//
//             for (int i = 0; i < assetCache.RefCount; i++)
//             {
//                 AddAssetBundleReferenceByAssetPath(currAssetCache.path);
//             }
//
//             var callbacks = assetCache.finishLoadCallbackList;
//
//             for (int i = 0; i < callbacks.Count; i++)
//             {
//                 var callback = callbacks[i];
//                 callback?.Invoke(currAssetCache.asset);
//             }
//
//             assetCache.finishLoadCallbackList.Clear();
//         }
//
//
//         //会触发 LoadAssetBundleFinish 中的业务回调
//     }
//
//     //卸载 目前策略是只改变引用计数
//     public void Unload(string assetPath)
//     {
//         AssetCache cache = null;
//         if (!assetCacheDic.TryGetValue(assetPath, out cache))
//         {
//             Logx.LogWarning("AssetManager", "Unload : the cache doesnt exist : " + assetPath);
//             return;
//         }
//
//         if (cache.RefCount <= 0)
//         {
//             Logx.LogWarning("AssetManager", "Unload : the ref of cache has less than 0 : " + assetPath);
//             return;
//         }
//
//         if (!cache.isFinishLoad)
//         {
//             Logx.LogWarning("AssetManager", "Unload : the asset is loading : " + assetPath);
//             return;
//         }
//
//         cache.RefCount -= 1;
//
//         var abPath = this.assetToAbDic[assetPath];
//         // var abCache = AssetBundleManager.Instance.GetCacheByPath(abPath);
//         AssetBundleManager.Instance.ReduceAssetBundleReference(abPath);
//     }
//
//     // 采用如下计数方式：
//     // asset 计算自己的 并会改变 ab 计数
//     // 也就是说 多个 asset 的总量 等于 对应的 ab 计数
//     // 实际上只有 ab 计数也可以 但是为了之后 资源分析 等 在 asset 层也做一个计数
//
//     //TODO ： 真正的清理 是从整体来清理的
//     public void Release(string assetPath)
//     {
//         AssetCache cache = null;
//         if (!assetCacheDic.TryGetValue(assetPath, out cache))
//         {
//             Logx.LogWarning("AssetManager", "Release : the cache doesnt exist : " + assetPath);
//             return;
//         }
//
//         if (cache.RefCount > 0)
//         {
//             Logx.LogWarning("AssetManager", "Release : the refCount of cache is more than 0 : " + assetPath);
//             return;
//         }
//
//         // cache.RefCount -= 1;
//         //if (0 == cache.RefCount)
//         //{
//         //    string abPath = "";
//         //    if (!assetToAbDic.TryGetValue(assetPath, out abPath))
//         //    {
//         //        Logx.LogWarning("AssetManager", "Release : the abPath is not found by assetPath : " + assetPath);
//         //        return;
//         //    }
//
//         //    AssetBundleManager.Instance.ReduceAssetBundleReference(abPath);
//         //}
//
//         string abPath = "";
//         if (!assetToAbDic.TryGetValue(assetPath, out abPath))
//         {
//             Logx.LogWarning("AssetManager", "Release : the abPath is not found by assetPath : " + assetPath);
//             return;
//         }
//
//         Logx.Log(LogxType.Asset, "AssetManager : asset release , path : " + cache.path);
//         // if (!cache.asset is GameObject)
//         // {
//         //     Resources.UnloadAsset(cache.asset);
//         // }
//         // else
//         // {
//         //     GameObject.DestroyImmediate(cache.asset);
//         // }
//
//         cache.asset = null;
//         Resources.UnloadUnusedAssets();
//         this.assetCacheDic.Remove(cache.path.ToLower());
//
//
//         //AssetBundleManager.Instance.ReduceAssetBundleReference(abPath);
//     }
//
//     //void Release()
//     //{
//     //    //EventManager.RemoveListener<AssetCache>((int)GameEvent.LoadAssetTaskFinish, this.OnLoadAssetFinish);
//     //}
// }