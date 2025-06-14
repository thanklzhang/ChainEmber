﻿using System;
using System.Collections;
using System.Collections.Generic;
using Config;
using GameData;
using ServerSimulation;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class BattleEmbattleUI : BaseUI
{
    // Action event_ClickCloseBtn;
    // Action<int> event_ClickConfirmBtn;
    // Action<int> event_ClickOneHeroOption;

    Button confirmBtn;
    private Button closeBtn;

    private TextMeshProUGUI currHeroNameText;
    private Image currHeroAvatarImg;
    private TextMeshProUGUI currHeroDesText;

    Transform heroRoot;

    Button clickBtn;

    // public List<HeroCardUIData> heroCardUIDataList;
    public List<EmbattleHeroShowObj> heroShowObjList;

    int currSelectHeroGuid;

    protected override void OnInit()
    {
        this.uiResId = (int)ResIds.BattleEmbattleUI;
        this.uiShowLayer = UIShowLayer.Floor_0;
        this.showMode = CtrlShowMode.Float;
    }

    protected override void OnLoadFinish()
    {
        heroRoot = this.transform.Find("scroll/mask/content");
        confirmBtn = this.transform.Find("confirmBtn").GetComponent<Button>();

        closeBtn = this.transform.Find("closeBtn").GetComponent<Button>();

        currHeroNameText = this.transform.Find("selectRoleNameText").GetComponent<TextMeshProUGUI>();
        currHeroAvatarImg = this.transform.Find("selectRolePic").GetComponent<Image>();
        currHeroDesText = this.transform.Find("describeText").GetComponent<TextMeshProUGUI>();


        confirmBtn.onClick.AddListener(() =>
        {
            // event_ClickConfirmBtn?.Invoke(currSelectHeroGuid);
            UIManager.Instance.Close<BattleEmbattleUI>();
            StartBattle();

            //startBattle
        });

        closeBtn.onClick.AddListener(() =>
        {
            //event_ClickCloseBtn?.Invoke();
            UIManager.Instance.Close<BattleEmbattleUI>();
        });

        heroShowObjList = new List<EmbattleHeroShowObj>();
    }

    public void StartBattle()
    {
        BattleService.Instance.ApplyCreateBattle(currSelectHeroGuid);
    }

    private List<HeroData> heroDataList;
    protected override void OnOpen(UICtrlArgs args)
    {
        // BattleEmbattleUIArgs selectHeroUIArgs = (BattleEmbattleUIArgs)args;
        
        // heroDataList = selectHeroUIArgs.heroDataList;
        
        // this.event_ClickConfirmBtn = selectHeroUIArgs.event_ClickConfirmBtn;
        // this.event_ClickOneHeroOption = selectHeroUIArgs.event_ClickOneHeroOption;

        // currSelectHeroGuid = selectHeroUIArgs.currSelectHeroGuid;

        this.RefreshInfo();
    }

    void RefreshInfo()
    {
        //选择英雄项
        
        heroDataList = HeroService.Instance.heroListData.heroList;
        currSelectHeroGuid = heroDataList[0].guid;
        
        heroShowObjList = new List<EmbattleHeroShowObj>();
        for (int i = 0; i < this.heroDataList.Count; i++)
        {
            var data = this.heroDataList[i];
            GameObject go = null;
            EmbattleHeroShowObj heroOption = null;
            if (i < this.heroRoot.childCount)
            {
                go = this.heroRoot.GetChild(i).gameObject;
            }
            else
            {
                var prefab = this.heroRoot.GetChild(0).gameObject;
                go = GameObject.Instantiate(prefab, this.heroRoot, false);
            }

            
            heroOption = new EmbattleHeroShowObj();
            heroShowObjList.Add(heroOption);
            go.SetActive(true);
            heroOption.Init(go);
            heroOption.AddClickListener((guid) =>
            {
                SelectHero(data.guid);
                // event_ClickOneHeroOption?.Invoke(guid);
                
            });

            heroOption.Refresh(data);
        }

        for (int i = this.heroDataList.Count; i < this.heroRoot.childCount; i++)
        {
            var go = this.heroRoot.GetChild(i).gameObject;

            go.SetActive(false);
        }


        SelectHero(currSelectHeroGuid);
    }

    public void SelectHero(int guid)
    {
        EmbattleHeroShowObj currShowObj = null;
        currSelectHeroGuid = guid;
        foreach (var showObj in heroShowObjList)
        {
            if (showObj.avatar.data.guid == guid)
            {
                showObj.SetSelectState(true);
                currShowObj = showObj;
            }
            else
            {
                showObj.SetSelectState(false);
            }
        }

        var heroConfigId = currShowObj.data.configId;
        var heroTb = ConfigManager.Instance.GetById<Config.EntityInfo>(heroConfigId);

        currHeroNameText.text = heroTb.Name;
        ResourceManager.Instance.GetObject<Sprite>(heroTb.AllBodyResId,
            (sprite) => { currHeroAvatarImg.sprite = sprite; });

        currHeroDesText.text = heroTb.Describe;
    }

    protected override void OnClose()
    {
        // event_ClickConfirmBtn = null;
        // event_ClickCloseBtn = null;
        this.confirmBtn.onClick.RemoveAllListeners();
        this.closeBtn.onClick.RemoveAllListeners();

        foreach (var item in heroShowObjList)
        {
            item.Release();
        }

        heroShowObjList = null;
    }
}

//-----------------------------------------


// public class BattleEmbattleUIArgs : UICtrlArgs
// {
//     public List<HeroData> heroDataList;
//     public Action<int> event_ClickConfirmBtn;
//     public Action<int> event_ClickOneHeroOption;
//     public int currSelectHeroGuid;
// }

public class EmbattleHeroShowObj
{
    public HeroAvatar avatar;
    public GameObject gameObject;
    public Transform transform;

    public HeroData data;
    private TextMeshProUGUI nameText;

    public void Init(GameObject go)
    {
        gameObject = go;
        transform = gameObject.transform;

        //填充 avatar
        var avatarRootGo = this.transform.Find("HeroAvatar").gameObject;
        avatar = new HeroAvatar();
        avatar.Init(avatarRootGo);

        //自身
        nameText = this.transform.Find("nameText").GetComponent<TextMeshProUGUI>();
    }

    public void AddClickListener(Action<int> eventClickOneHeroOption)
    {
        avatar.AddClickListener(eventClickOneHeroOption);
    }

    public void Refresh(HeroData data)
    {
        this.data = data;
        avatar.Refresh(data);

        var config = Config.ConfigManager.Instance.GetById<Config.EntityInfo>(data.configId);
        nameText.text = config.Name;
    }

    public void SetSelectState(bool isShow)
    {
        avatar.SetSelectState(isShow);
    }

    public void Release()
    {
        avatar.Release();
    }
}