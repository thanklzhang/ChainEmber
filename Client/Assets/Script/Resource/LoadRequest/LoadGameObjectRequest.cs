using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.U2D;
using UnityEngine.UI;

public class LoadGameObjectRequest : LoadObjectRequest
{
    //string[] pathList;
    //HashSet<string> currNeedLoadSet = new HashSet<string>();
    private string path;
    private int count;

    bool isFinish;

    public Action<HashSet<GameObject>> selfFinishCallback;

    HashSet<GameObject> currGameObjects = new HashSet<GameObject>();

    public LoadGameObjectRequest(string path, int count)
    {
        this.path = path;
        this.count = count;
    }

    public override void Start()
    {
        ResourceManager.Instance.GetObject<GameObject>(this.path, OnLoadFinish);
    }

    public void OnLoadFinish(GameObject obj)
    {
        //obj 就算已经有一个实例了
        currGameObjects.Add(obj);
        for (int i = 0; i < count - 1; i++)
        {
            var insObj = GameObject.Instantiate(obj);
            currGameObjects.Add(insObj);
        }

        selfFinishCallback?.Invoke(currGameObjects);
        isFinish = true;
    }

    public override bool CheckFinish()
    {
        return isFinish;
    }


    //public void Start(string[] pathList)
    //{
    //    this.pathList = pathList;
    //    for (int i = 0; i < pathList.Length; i++)
    //    {
    //        var currPath = pathList[i];
    //        currNeedLoadSet.Add(currPath);

    //        AssetManager.Instance.Load(currPath, (asset) =>
    //        {
    //            this.OnOneResLoadFinish(currPath, asset);
    //        });
    //    }
    //}

    //public void OnOneResLoadFinish(string currPath, UnityEngine.Object asset)
    //{
    //    if (currNeedLoadSet.Contains(currPath))
    //    {
    //        currNeedLoadSet.Remove(currPath);
    //    }
    //    else
    //    {
    //        Logx.LogError("the currPath is not correct : " + currPath);
    //    }


    //}

    public override void Finish()
    {

    }

    public override void Unload()
    {
        foreach (var obj in currGameObjects)
        {
            ResourceManager.Instance.ReturnObject(this.path, obj);
        }
        //for (int i = this.pathList.Length - 1; i >= 0; i--)
        //{
        //    var path = this.pathList[i];
        //    AssetManager.Instance.Release(path);
        //}
    }

}

