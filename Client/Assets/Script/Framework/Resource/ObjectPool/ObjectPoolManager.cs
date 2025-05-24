using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.U2D;
using UnityEngine.UI;

public class ObjectPoolManager : Singleton<ObjectPoolManager>
{
    //gameObject texture sprite material
    public Dictionary<Type, ObjectPoolGroup> objectPoolGroupDic = new Dictionary<Type, ObjectPoolGroup>();

    public Transform root;
    public void Init(Transform objPoolRoot)
    {
        root = objPoolRoot;
    }
    
    public void GetObject<T>(string path, Action<UnityEngine.Object> callback) where T : UnityEngine.Object
    {
        var type = typeof(T);
        ObjectPoolGroup group = null;
        if (objectPoolGroupDic.ContainsKey(type))
        {
            group = objectPoolGroupDic[type];
        }
        else
        {
            group = new ObjectPoolGroup();
            objectPoolGroupDic.Add(type, group);
        }

        group.GetObject<T>(path, callback);
    }

    public void ReturnObject<T>(string path, T obj) where T : UnityEngine.Object
    {
        Logx.Log(LogxType.Asset,"return obj : path : " + path);
        var type = typeof(T);
        ObjectPoolGroup group = null;
        if (objectPoolGroupDic.ContainsKey(type))
        {
            group = objectPoolGroupDic[type];
            group.ReturnObj(path, obj);
        }
        else
        {
            Logx.LogWarning(LogxType.Resource,"the type is not found : " + type.ToString());
        }
    }

    public void Release()
    {
        //清除都没在使用的 poolObj

        foreach (var VARIABLE in objectPoolGroupDic)
        {
            VARIABLE.Value.Release();
        }
    }

    
}