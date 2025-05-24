using System;
using System.Collections;
using System.Collections.Generic;
using Battle;
using Battle_Client;
using Config;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Skill = Config.Skill;


//选项
public class BattleRewardCell
{
    public GameObject gameObject;
    public Transform transform;

    public Image icon;
    public TextMeshProUGUI nameText;

    public TextMeshProUGUI describeText;
    // public Button selectBtn;

    public int index = -1;

    private BattleRewardUI parentUI;

    public BattleReward_Client data;

    public void Init(GameObject gameObject, BattleRewardUI parentUI)
    {
        this.parentUI = parentUI;
        this.gameObject = gameObject;
        this.transform = this.gameObject.transform;

        icon = transform.Find("iconBg/icon").GetComponent<Image>();
        nameText = transform.Find("name_text").GetComponent<TextMeshProUGUI>();
        describeText = transform.Find("content_text").GetComponent<TextMeshProUGUI>();
        //selectBtn = transform.Find("selectBtn").GetComponent<Button>();

        // selectBtn.onClick.RemoveAllListeners();
        // selectBtn.onClick.AddListener(OnClickSelectBtn);
    }

    public void Show()
    {
        this.gameObject.SetActive(true);
    }

    public void RefreshUI(BattleReward_Client data, int index)
    {
        this.data = data;
        this.index = index;

        var configId = this.data.configId;
        var rewardConfig = ConfigManager.Instance.GetById<Config.BattleReward>(configId);


        ResourceManager.Instance.GetObject<Sprite>(rewardConfig.IconResId,
            (sprite) => { icon.sprite = sprite; });

        RefreshTextShow();
    }

    public void RefreshTextShow()
    {
        var nameStr = "";
        var desStr = "";
        AttrHelper_Client.GetBattleRewardContent(this.data, true, false, out nameStr,
            out desStr);

        nameText.text = nameStr;
        describeText.text = desStr;
    }

    // public string GetAttrContent()
    // {
    //     var attrGroupConfigId = this.data.intArg1;
    //     var intValueList = this.data.intListArg1;
    //     return AttrHelper_Client.GetAttrContent(attrGroupConfigId, intValueList);
    // }

    public void OnClickSelectBtn()
    {
        //this.parentUI.OnSelectSelection(this);
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }
}