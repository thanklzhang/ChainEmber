using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;


public static class ExtensionMethods
{
   
    /// <summary>
    /// 根据名字递归查找子物体(DFS)
    /// </summary>
    /// <returns>The child.</returns>
    /// <param name="trans">Trans.</param>
    /// <param name="childName">Child name.</param>
    public static Transform FindDeepChild(this Transform trans, string childName)
    {
        Transform resultTrs = null;
        resultTrs = trans.Find(childName);
        if (resultTrs == null)
        {
            foreach (Transform trs in trans)
            {
                resultTrs = FindDeepChild(trs, childName);
                if (resultTrs != null)
                    return resultTrs;
            }
        }

        return resultTrs;
    }

 
    public static List<T> GetComponentsInChildrenExceptSelf<T>(this Transform transform)// where T :Transform
    {
        T []t = transform.GetComponentsInChildren<T>();
        List<T> list = new List<T>();
        list.AddRange(t);
        list.RemoveAt(0);

        return list;

       

    }
    
}