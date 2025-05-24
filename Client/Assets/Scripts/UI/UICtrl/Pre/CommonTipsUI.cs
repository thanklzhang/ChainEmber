using System;
using System.Collections;
using System.Collections.Generic;
using Config;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class CommonTipsUI : BaseUI
{
    //ui component
    Transform root;

    List<TipsOption> tipsList;

    protected override void OnInit()
    {
        
        this.uiResId = (int)ResIds.TipsUI;
        this.uiShowLayer = UIShowLayer.Top_0; 
        
     

    }

    protected override void OnLoadFinish()
    {
        root = transform.Find("root");

        tipsList = new List<TipsOption>();

        root.GetChild(0).gameObject.SetActive(false);
    }

    protected override void OnOpen(UICtrlArgs args)
    {
        TipsUIArgs tipsArgs = (TipsUIArgs)args;
        var tipStr = tipsArgs.tipStr;
        this.ShowTips(tipStr);
    }

    void ShowTips(string tipStr)
    {
        TipsOption canUseOption = null;
        for (int i = 0; i < tipsList.Count; i++)
        {
            var tipOption = tipsList[i];
            if (!tipOption.isAni)
            {
                canUseOption = tipOption;
                break;
            }
        }

        if (null == canUseOption)
        {
            canUseOption = new TipsOption();
            var tempGo = GameObject.Instantiate(root.GetChild(0).gameObject, root, false);
            tempGo.SetActive(true);
            canUseOption.Init(tempGo);
            tipsList.Add(canUseOption);
        }

        canUseOption.Refresh(tipStr);
        canUseOption.StartShow();
    }

    protected override void OnUpdate(float timeDelta)
    {
        for (int i = 0; i < tipsList.Count; i++)
        {
            var tipOption = tipsList[i];
            tipOption.Update(timeDelta);
        }
    }

    protected override void OnClose()
    {

    }
}

public class TipsUIArgs : UICtrlArgs
{
    public string tipStr;
}

public class TipsOption
{
    GameObject gameObject;
    Transform transform;

    GameObject showGo;
    TextMeshProUGUI tipsText;
    public bool isAni;

    float currTotalTimer;

    float step1LastTime = 0.8f;
    float step1Timer;


    public void Init(GameObject rootGo)
    {
        this.gameObject = rootGo;
        this.transform = this.gameObject.transform;

        this.showGo = this.transform.Find("show").gameObject;
        this.tipsText = this.transform.Find("show/contentText").GetComponent<TextMeshProUGUI>();

        this.showGo.SetActive(false);


    }

    public void StartShow()
    {
        isAni = true;
        step1Timer = 0;
        currTotalTimer = 0;
        this.showGo.SetActive(true);
    }

    public void EndShow()
    {
        isAni = false;
        this.showGo.SetActive(false);
        currTotalTimer = 0;
    }

    public void Update(float timeDelta)
    {
        if (!isAni)
        {
            return;
        }

        if (currTotalTimer > 2.00f)
        {
            EndShow();
        }
        else
        {
            currTotalTimer += timeDelta;
        }



        //if (step1Timer >= step1LastTime)
        //{

        //}

        //this.showGo.transform.localPosition += new Vector3(0,,0);

    }

    public void Refresh(string tipsStr)
    {
        this.tipsText.text = tipsStr;
    }
}
