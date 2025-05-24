using System;
using System.Collections;
using System.Collections.Generic;
using Config;
using GameData;
using UnityEngine;
using UnityEngine.UI;

public class BattleResultUI : BaseUI
{
    // public Action onClickConfirmBtn;

    //当前选中的关卡相关组件--
    Text winContent;
    Button confirmBtn;

    Transform rewardRoot;

    //List<CommonItemUIArgs> optionDataList = new List<CommonItemUIArgs>();
    List<ResultOptionShowObj> optionShowList = new List<ResultOptionShowObj>();

    private List<ItemData> itemDataList;

    protected override void OnInit()
    {
        this.uiResId = (int)ResIds.BattleResultUI;
        this.uiShowLayer = UIShowLayer.Floor_0;
        this.showMode = CtrlShowMode.Float;
    }

    protected override void OnLoadFinish()
    {
        this.winContent = this.transform.Find("Panel/Result").GetComponent<Text>();
        this.confirmBtn = this.transform.Find("Panel/ConfirmBtn").GetComponent<Button>();
        this.rewardRoot = this.transform.Find("Panel/reward/scroll/mask/content");

        confirmBtn.onClick.AddListener(OnClickConfirmBtn);
    }

    private void OnClickConfirmBtn()
    {
        //回主页
    }
    
    protected override void OnOpen(UICtrlArgs args)
    {
        var resultArgs = (BattleResultUIArgs)args;
        itemDataList = resultArgs.itemDataList;
        var isWin = resultArgs.isWin;
        var showStr = "";


        if (isWin)
        {
            showStr = "胜利";
            winContent.color = new Color(1, (185.0f / 255), 0, 1);
        }
        else
        {
            showStr = "失败";

            winContent.color = new Color(1, (0.0f / 255), 0, 1);
        }


        winContent.text = showStr;

        // this.optionDataList = resultArgs.uiItem;


        this.RefreshRewardList();
    }

    void RefreshRewardList()
    {
        // UIListArgs<ResultOptionShowObj, BattleResultUIPre> args = new
        //     UIListArgs<ResultOptionShowObj, BattleResultUIPre>();
        // args.dataList = optionDataList;
        // args.showObjList = optionShowList;
        // args.root = rewardRoot;
        // args.parentObj = this;
        // UIFunc.DoUIList(args);

        optionShowList = new List<ResultOptionShowObj>();

        for (int i = 0; i < itemDataList.Count; i++)
        {
            GameObject go = null;
            var data = itemDataList[i];
            if (i < rewardRoot.childCount)
            {
                go = rewardRoot.GetChild(i).gameObject;
            }
            else
            {
                go = GameObject.Instantiate(rewardRoot.GetChild(0).gameObject,
                    rewardRoot, false);
            }

            ResultOptionShowObj showObj = new ResultOptionShowObj();
            showObj.Init(go, this);
            showObj.Show();
            showObj.Refresh(data);
            
            optionShowList.Add(showObj);
        }

        for (int i = itemDataList.Count; i < rewardRoot.childCount; i++)
        {
            var go = rewardRoot.GetChild(i).gameObject;
            go.SetActive(false);
        }
    }

    protected override void OnClose()
    {
        this.confirmBtn.onClick.RemoveAllListeners();
    }
}

public class BattleResultUIArgs : UICtrlArgs
{
    public bool isWin;
    public List<ItemData> itemDataList;
}