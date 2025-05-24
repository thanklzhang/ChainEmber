using System;
using System.Collections;
using System.Collections.Generic;
using Config;
using UnityEngine;
using UnityEngine.UI;

public class LoadingUICtrlArg : UICtrlArgs
{
    public float progress;
    
    public float curr;
    public float max;
    public string text = "";
}

public class LoadingUICtrl : BaseUI
{
    public Transform bgTran;
    public Transform progressTran;

    public Text progressText;
    public Text progressStrText;

    private RectTransform bgRectTran;
    private RectTransform progressRectTran;


    protected override void OnInit()
    {
        this.uiResId = (int)ResIds.LoadingUI;
        this.uiShowLayer = UIShowLayer.Top_0;
    }

    protected override void OnLoadFinish()
    {
        base.OnLoadFinish();
        
        bgTran = this.transform.Find("progressBar/back");
        progressTran = this.transform.Find("progressBar/front");

        progressText = this.transform.Find("progressBar/progressText").GetComponent<Text>();
        progressStrText = this.transform.Find("progressBar/loadText").GetComponent<Text>();

        bgRectTran = bgTran.GetComponent<RectTransform>();
        progressRectTran = progressTran.GetComponent<RectTransform>();
    }

    protected override void OnOpen(UICtrlArgs args)
    {
        base.OnOpen(args);

        var _args = args as LoadingUICtrlArg;
        OnChangeProgerss(_args);
    }


    protected override void OnActive()
    {
        base.OnActive();
        
        EventDispatcher.AddListener<LoadingUICtrlArg>(EventIDs.OnChangeLoadingProgress,OnChangeProgerss);
    }

    
    protected override void OnInactive()
    {
        base.OnInactive();
        
        EventDispatcher.RemoveListener<LoadingUICtrlArg>(EventIDs.OnChangeLoadingProgress,OnChangeProgerss);
    }

    public void OnChangeProgerss(LoadingUICtrlArg arg)
    {
        // var curr = arg.curr;
        // var max = arg.max;
        // var progerss = curr / (float)max;

        if (null == arg)
        {
            arg = new LoadingUICtrlArg();
        }

        var progerss = arg.progress;
        
        var str = arg.text;
        this.SetProgress(progerss,str);
    }

    public void SetProgress(float progress,string text)
    {
        progressText.text = string.Format("{0:F2}%", progress * 100);

        progressStrText.text = text;

        var pre = progressRectTran.sizeDelta;

        progressRectTran.sizeDelta = new Vector2(bgRectTran.rect.width * progress, pre.y);
    }

   
}