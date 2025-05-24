using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.U2D;
using UnityEngine.UI;

public enum LoadRequestType
{
    Null = 0,
    //基础资源
    GameObject = 1,
    Texture = 2,
    Material = 3,
    //封装的逻辑资源
    UI = 10,
    Model = 11
}

public class LoadResOption
{
    public string path;
    public Action<object> selfLoadFinishCallback;
    public LoadRequestType type;
}

public class LoadResGroupRequest
{
    List<LoadObjectRequest> requestList = new List<LoadObjectRequest>();

    public bool CheckFinish()
    {
        return 0 == requestList.Count;
    }

    internal void Start(List<LoadObjectRequest> loadOptions)
    {
        this.requestList = loadOptions;
        for (int i = 0; i < this.requestList.Count; i++)
        {
            var req = requestList[i];
            req.Start();
           
        }
        //for (int i = 0; i < loadOptions.Count; i++)
        //{
        //    var option = loadOptions[i];
        //    //根据 path 判断资源类型
        //    LoadObjectRequest request = null;
        //    //如果是 UI 
        //    if (option.type == LoadRequestType.UI)
        //    {
        //        request = new LoadUIRequest();
               

        //    }
        //    //如果是 GemObjects 
        //    //if (option.type == LoadRequestType.GameObject)
        //    //{
        //    //    request = new LoadGameObjectRequest(10);
        //    //}
        //    request.Init(option);

        //    request.Start();
        //    requestList.Add(request);
        //}
    }

    public void Update(float deltaTime)
    {
        for (int i = requestList.Count - 1; i >= 0; i--)
        {
            var req = requestList[i];
            if (req.CheckFinish())
            {
                req.Finish();
                requestList.Remove(req);
            }
        }
    }

    public void Finish()
    {

    }

    internal void Release()
    {
        //for requestList release

        foreach (var request in requestList)
        {
            request.Unload();
        }
    }
}
