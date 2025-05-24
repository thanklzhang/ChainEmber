using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.U2D;
using UnityEngine.UI;
public class LoadObjectRequest
{
    //protected LoadResOption loadResOption;
    public virtual bool CheckFinish()
    {
        return true;
    }
    public virtual void Init(LoadResOption loadResOption)
    {
        //this.loadResOption = loadResOption;
    }
    public virtual void Start()
    {
        
    }
    public virtual void Finish()
    {
        
    }
    public virtual void Unload()
    {
        
    }
    
}

