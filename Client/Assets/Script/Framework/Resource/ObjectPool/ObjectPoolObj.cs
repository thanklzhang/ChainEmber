using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.U2D;
using UnityEngine.UI;

public class ObjectPoolObj
{
    public int id;
    UnityEngine.Object obj;

    bool isUsing;

    internal bool IsUsing()
    {
        return isUsing;
    }

    //internal UnityEngine.Object GetObj()
    //{
    //    return obj;
    //}

    internal void SetInfo(int id, UnityEngine.Object obj)
    {
        this.id = id;
        this.obj = obj;
    }

    internal UnityEngine.Object Use()
    {
        if (obj is GameObject)
        {
            var go = obj as GameObject;
            go.SetActive(true);
        }

        this.isUsing = true;
        return obj;
    }

    internal void Return()
    {
        if (obj is GameObject)
        {
            var go = obj as GameObject;
            go.SetActive(false);
            var root = ObjectPoolManager.Instance.root;
            go.transform.SetParent(root,false);
        }

        this.isUsing = false;
    }

    public void Release()
    {
        if (this.isUsing)
        {
            return;
        }

        if (obj is GameObject)
        {
            GameObject.Destroy(obj);
        }

        obj = null;


    }
}
