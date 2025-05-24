using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class CoroutineManager : MonoBehaviour
{
    //public void StartAction(IEnumerator ie)
    //{
    //    StartCoroutine(ie);
    //}
    public static CoroutineManager Instance;

    public void Awake()
    {
        Instance = this;
    }

}

