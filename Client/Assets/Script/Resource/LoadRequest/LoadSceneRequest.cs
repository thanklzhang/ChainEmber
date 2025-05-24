using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.U2D;
using UnityEngine.UI;

public class LoadSceneRequest : LoadObjectRequest
{
    //string[] pathList;
    //HashSet<string> currNeedLoadSet = new HashSet<string>();
    private string sceneName;
    // private int count;

    bool isFinish;

    public Action selfFinishCallback;

    // HashSet<GameObject> currGameObjects = new HashSet<GameObject>();

    public LoadSceneRequest(string sceneName)
    {
        this.sceneName = sceneName;
        // this.count = count;
    }

    public override void Start()
    {
        // SceneLoadManager.Instance.Load(sceneName,OnLoadFinish);
    }

    public void OnLoadFinish()
    {
        //obj 就算已经有一个实例了
        // currGameObjects.Add(obj);
        // for (int i = 0; i < count - 1; i++)
        // {
        //     var insObj = GameObject.Instantiate(obj);
        //     currGameObjects.Add(insObj);
        // }

        selfFinishCallback?.Invoke();
        isFinish = true;
    }

    public override bool CheckFinish()
    {
        return isFinish;
    }

    public override void Finish()
    {

    }

    public override void Unload()
    {
        SceneLoadManager.Instance.Unload(sceneName);
    }

}

