using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UIFunc
{
    public static void DoUIList<T, K>(UIListArgs<T, K> listArgs) where T : BaseUIShowObj<K>, new()
    {
        //数据层之后也换成泛型
        IList dataList = listArgs.dataList;
        IList<T> showObjList = listArgs.showObjList;
        Transform root = listArgs.root;

        for (int i = 0; i < dataList.Count; i++)
        {
            var data = dataList[i];

            GameObject go = null;
            if (i < root.childCount)
            {
                go = root.GetChild(i).gameObject;
            }
            else
            {
                var tempObj = root.GetChild(0).gameObject;
                go = GameObject.Instantiate(tempObj, root);
            }

            T showObj = default(T);
            if (i < showObjList.Count)
            {
                showObj = showObjList[i];
            }
            else
            {
                showObj = new T();
                showObjList.Add(showObj);
                showObj.Init(go, listArgs.parentObj);
            }

            go.SetActive(true);
            showObj.Refresh(data, i);
        }

        for (int i = dataList.Count; i < root.childCount; i++)
        {
            var obj = root.GetChild(i).gameObject;

            if (i < showObjList.Count)
            {
                var showObj = showObjList[i];
                showObj.Release();
            }

            obj.SetActive(false);
        }
    }

}
