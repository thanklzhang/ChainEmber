using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.U2D;
using UnityEngine.UI;

public class ObjectPool
{
     public UnityEngine.Object assetObj;
    public Dictionary<int, ObjectPoolObj> objectDic = new Dictionary<int, ObjectPoolObj>();
    bool isLoadingAsset = false;

    public string path;

    List<Action<UnityEngine.Object>> getObjCallbackList = new List<Action<UnityEngine.Object>>();
    Type type;

    private bool isLoading;
    internal void GetObject<T>(Action<UnityEngine.Object> callback) where T : UnityEngine.Object
    {
        this.type = typeof(T);
        if (null == assetObj)
        {
            getObjCallbackList.Add(callback);

            if (!isLoading)
            {
                isLoading = true;
                
                AssetManager.Instance.Load<T>(path, OnAssetLoadFinish);
               
            }
        }
        else
        {
            //有 asset 直接取 obj
            var poolObj = GetCachePoolObj(assetObj);
            var obj = poolObj.Use();
        
            //AssetManager.Instance.ChangeRef(path, 1);
        
            callback?.Invoke(obj);
        }

        // getObjCallbackList.Add(callback);
        // AssetManager.Instance.Load<T>(path, OnAssetLoadFinish);
    }

    public bool IsGameObject()
    {
        //TODO : 根据 path 来进行断定是否是 GameObject
        if (this.type == typeof(GameObject))
        {
            return true;
        }

        return false;
    }

    public ObjectPoolObj GetCachePoolObj(UnityEngine.Object asset)
    {
        ObjectPoolObj poolObj = null;
        var isGo = IsGameObject();

        if (isGo)
        {
            //有 asset 直接取 obj
            foreach (var item in objectDic)
            {
                if (item.Value.IsUsing())
                {
                    continue;
                }

                poolObj = item.Value;
                poolObj.Use();

                break;
            }

            if (null == poolObj)
            {
                //如果都在使用 那么创建新的 Object(目前没有上限 之后会有上限)
                poolObj = CreateNew(asset);
            }
        }
        else
        {
            //由于是引用 所以第一个就是
            foreach (var item in objectDic)
            {
                poolObj = item.Value;
                poolObj.Use();
                break;
            }

            if (null == poolObj)
            {
                //如果都在使用 那么创建新的 Object(目前没有上限 之后会有上限)
                poolObj = CreateNew(asset);
            }
        }

        return poolObj;
    }

    public ObjectPoolObj CreateNew(UnityEngine.Object asset)
    {
        //path
        UnityEngine.Object obj = null;
        var isGo = IsGameObject();

        ObjectPoolObj newPoolObj = null;
        if (isGo)
        {
            obj = GameObject.Instantiate(asset) as GameObject;
            var root = ObjectPoolManager.Instance.root;
            var go = obj as GameObject;
            go.transform.SetParent(root,false);
            
            newPoolObj = new ObjectPoolObj();
            newPoolObj.SetInfo(obj.GetInstanceID(), obj);
            //newPoolObj.id = obj.GetInstanceID();
            //newPoolObj.obj = obj;
            newPoolObj.Use();
            objectDic.Add(newPoolObj.id, newPoolObj);
        }
        else
        {
            obj = asset;
            if (null == obj)
            {
                Logx.LogWarning("the assetObj is null : path : " + path);
                return null;
            }

            if (!objectDic.ContainsKey(obj.GetInstanceID()))
            {
                newPoolObj = new ObjectPoolObj();
                newPoolObj.SetInfo(obj.GetInstanceID(), obj);
                //newPoolObj.id = obj.GetInstanceID();
                //newPoolObj.obj = obj;
                newPoolObj.Use();
                objectDic.Add(newPoolObj.id, newPoolObj);
            }
            else
            {
                newPoolObj = objectDic[obj.GetInstanceID()];
            }
        }
        
        return newPoolObj;
    }

    //asset 引用计数 ， 理论上只会是 1 和 0
    // int assetRefCount = 0;
    public void OnAssetLoadFinish(UnityEngine.Object asset)
    {
        // if (0 == assetRefCount)
        // {
        //     assetRefCount = 1;
        // }
        this.assetObj = asset;
        
        isLoading = false;
        
        // this.assetObj = asset;
        for (int i = 0; i < getObjCallbackList.Count; i++)
        {
            var callback = getObjCallbackList[i];
            var poolObj = GetCachePoolObj(asset);
            var obj = poolObj.Use();
            callback?.Invoke(obj);
        }

        getObjCallbackList.Clear();
    }

    internal void ReturnObject(UnityEngine.Object obj)
    {
        var isGo = obj is GameObject;
        var insId = obj.GetInstanceID();
        // if (isGo)
        // {
        //     if (objectDic.ContainsKey(insId))
        //     {
        //         var findObj = objectDic[insId];
        //         findObj.Return();
        //     }
        //     else
        //     {
        //         Logx.LogWarning("the insId is not found : " + insId);
        //     }
        // }

        if (objectDic.ContainsKey(insId))
        {
            var findObj = objectDic[insId];
            findObj.Return();

            // AssetManager.Instance.UnloadAsset(this.path);
        }
        else
        {
            Logx.LogWarning(LogxType.Resource,"the insId is not found : " + insId);
        }

        //AssetManager.Instance.ChangeRef(path, -1);
    }

    public void Release()
    {
        if (this.isLoading)
        {
            Logx.Log(LogxType.Resource, "ObjectPool : asset is loading  , so cant release , path : " + path + " , path : "
                                        + this.path);
            return;
        }

        var delList = new List<int>();
        foreach (var v in objectDic)
        {
            var insId = v.Key;
            var obj = v.Value;
            if (!obj.IsUsing())
            {
                delList.Add(insId);
            }
        }

        foreach (var insId in delList)
        {
            var poolObj = objectDic[insId];
            poolObj.Release();
            
            
            objectDic.Remove(insId);
        }

        if (delList.Count > 0)
        {
            Logx.Log(LogxType.Resource, "ObjectPool : Release , path : " + path + " , path : "
                                        + this.path  + " , count : " + delList.Count);
        }

        if (0 == objectDic.Count)
        {
            assetObj = null;
            AssetManager.Instance.UnloadAsset(this.path);
        }
    }
}