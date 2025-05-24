using System;
using System.Collections;
using System.Collections.Generic;
using Config;
using GameData;

using UnityEngine;
using UnityEngine.UI;


public class TitleBarUI : BaseUI
{
    public Transform optionRoot;
    public Button closeBtn;
    public Text nameText;
    public GameObject bgGo;
    public GameObject lineGo;

    List<TitleOptionUIData> optionDataList = new List<TitleOptionUIData>();
    List<TitleOptionShowObj> optionShowList = new List<TitleOptionShowObj>();

    public Action clickCloseBtnAction;

    protected override void OnInit()
    {
        this.uiResId = (int)ResIds.TitleBarUI;
        this.uiShowLayer = UIShowLayer.Middle_0;
    }

    protected override void OnLoadFinish()
    {
        this.optionRoot = this.transform.Find("root");
        this.closeBtn = transform.Find("close").GetComponent<Button>();
        nameText = transform.Find("funcName").GetComponent<Text>();
        bgGo = transform.Find("bg").gameObject;
        lineGo = transform.Find("line01").gameObject;

        this.closeBtn.onClick.RemoveAllListeners();
        this.closeBtn.onClick.AddListener(() => { clickCloseBtnAction?.Invoke(); });
    }

    private Config.TitleBar config;

    protected override void OnOpen(UICtrlArgs args)
    {
        clickCloseBtnAction = null;
        TitleBarUIArgs titleBarListArgs = (TitleBarUIArgs)args;

        var titleBarId = titleBarListArgs.titleBarId;
        config = ConfigManager.Instance.GetById<Config.TitleBar>(titleBarId);

        //资源
        this.RefreshOptionList();

        nameText.text = config.TitleName;

        closeBtn.gameObject.SetActive(1 == config.IsShowCloseBtn);
        bgGo.SetActive(1 == config.IsShowBg);
        lineGo.SetActive(1 == config.IsShowLine);

        if (null == clickCloseBtnAction)
        {
            clickCloseBtnAction = DefaultClose;
        }
    }

    public void DefaultClose()
    {
        UIManager.Instance.CloseTopFixedUI();
    }

    private List<TitleOptionShowObj> showObjList;

    void RefreshOptionList()
    {
        var resIdList = config.ResList;

        showObjList = new List<TitleOptionShowObj>();
        for (int i = 0; i < resIdList.Count; i++)
        {
            var itemId = resIdList[i];

            GameObject go = null;
            if (i < optionRoot.childCount)
            {
                go = optionRoot.GetChild(i).gameObject;
            }
            else
            {
                go = GameObject.Instantiate(optionRoot.GetChild(0).gameObject, optionRoot, false);
            }

            go.SetActive(true);

            TitleOptionShowObj showObj = new TitleOptionShowObj();
            showObj.Init(go);


            var count = GameDataManager.Instance.BagData.GetCountByConfigId(itemId);
            showObj.RefreshUI(itemId, count, i);
        }

        // UIListArgs<TitleOptionShowObj, TitleBarUIPre> args = new
        //     UIListArgs<TitleOptionShowObj, TitleBarUIPre>();
        // args.dataList = optionDataList;
        // args.showObjList = optionShowList;
        // args.root = optionRoot;
        // args.parentObj = this;
        // UIFunc.DoUIList(args);
    }

    protected override void OnClose()
    {
        clickCloseBtnAction = null;
        this.closeBtn.onClick.RemoveAllListeners();
    }
}


public class TitleBarUIArgs : UICtrlArgs
{
    public int titleBarId;

    // public string titleName;
    // public List<TitleOptionUIData> optionList;
    // public bool isShowCloseBtn;
    // public bool isShowBg;
    // public bool isShowLine;
}

public class TitleOptionUIData
{
    public int configId;
    public int count;
}

public class TitleOptionShowObj
{
    Text nameText;
    Text countText;

    Image iconImg;
    // public TitleOptionUIData uiData;

    int currIconResId;
    Sprite currIconSprite;

    private GameObject gameObject;
    private Transform transform;

    public void Init(GameObject go)
    {
        gameObject = go;
        transform = gameObject.transform;

        nameText = this.transform.Find("name").GetComponent<Text>();
        countText = this.transform.Find("count").GetComponent<Text>();
        iconImg = this.transform.Find("icon").GetComponent<Image>();
    }

    public void RefreshUI(int itemId, int count, int index)
    {
        // this.uiData = (TitleOptionUIData)data;
        //
        // var configId = this.uiData.configId;
        var itemTb = ConfigManager.Instance.GetById<Config.Item>(itemId);
        nameText.text = itemTb.Name;
        countText.text = "" + count;

        currIconResId = itemTb.IconResId;
        ResourceManager.Instance.GetObject<Sprite>(currIconResId, (sprite) =>
        {
            //TODO
            //注意 这里界面关闭了还会再次执行
            //这里应该判断是否界面界面关闭了等状态
            currIconSprite = sprite;
            iconImg.sprite = sprite;
        });
    }

    public void Release()
    {
        if (currIconSprite != null)
        {
            ResourceManager.Instance.ReturnObject<Sprite>(currIconResId, currIconSprite);
            iconImg.sprite = null;
        }
    }
}