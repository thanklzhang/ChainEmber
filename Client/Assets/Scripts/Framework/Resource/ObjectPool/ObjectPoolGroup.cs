using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.U2D;
using UnityEngine.UI;

public class ObjectPoolGroup
{
    public Dictionary<string, ObjectPool> objectPoolDic = new Dictionary<string, ObjectPool>();

    internal void GetObject<T>(string path, Action<UnityEngine.Object> callback) where T : UnityEngine.Object
    {
        ObjectPool pool = null;
        if (objectPoolDic.ContainsKey(path))
        {
            pool = objectPoolDic[path];
        }
        else
        {
            pool = new ObjectPool();
            pool.path = path;
            
            pool.path = pool.path.ToLower().Replace('\\', '/');
            
            objectPoolDic.Add(path, pool);
        }

        pool.GetObject<T>(callback);
    }

    public void ReturnObj(string path, UnityEngine.Object obj)
    {
        ObjectPool pool = null;
        if (objectPoolDic.ContainsKey(path))
        {
            pool = objectPoolDic[path];
            pool.ReturnObject(obj);
        }
        else
        {
            Logx.LogWarning(LogxType.Resource,"ObjectPoolGroup : GetObject : the path is not found : " + path);
        }
    }

    public void Release()
    {
        foreach (var VARIABLE in objectPoolDic)
        {
            VARIABLE.Value.Release();
        }
    }
}

