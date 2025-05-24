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
public class BattleSelectRewardCell
{
    public GameObject gameObject;
    public Transform transform;

    public Image icon;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI describeText;
    public Button selectBtn;

    public int index = -1;

    private BattleSelectRewardUI parentUI;

    public BattleClientMsg_BattleBoxSelection data;

    public void Init(GameObject gameObject, BattleSelectRewardUI parentUI)
    {
        this.parentUI = parentUI;
        this.gameObject = gameObject;
        this.transform = this.gameObject.transform;

        icon = transform.Find("iconBg/icon").GetComponent<Image>();
        nameText = transform.Find("name_text").GetComponent<TextMeshProUGUI>();
        describeText = transform.Find("content_text").GetComponent<TextMeshProUGUI>();
        selectBtn = transform.Find("selectBtn").GetComponent<Button>();

        selectBtn.onClick.RemoveAllListeners();
        selectBtn.onClick.AddListener(OnClickSelectBtn);
    }

    public void Show()
    {
        this.gameObject.SetActive(true);
    }

    public void RefreshUI(BattleClientMsg_BattleBoxSelection selectionDat, int index)
    {
        this.data = selectionDat;
        this.index = index;

        var configId = this.data.rewardConfigId;
        var rewardConfig = ConfigManager.Instance.GetById<Config.BattleReward>(configId);


        ResourceManager.Instance.GetObject<Sprite>(rewardConfig.IconResId,
            (sprite) => { icon.sprite = sprite; });

        RefreshTextShow();
    }

    public void RefreshTextShow()
    {
        var configId = this.data.rewardConfigId;
        var rewardConfig = ConfigManager.Instance.GetById<Config.BattleReward>(configId);
        var nameStr = "";
        var desStr = "";
        var isMaskSureReward = 1 == rewardConfig.MakeSureRewardOccasion;
        AttrHelper_Client.GetBattleRewardContent(this.data.battleReward,isMaskSureReward,false,
            out nameStr,
            out desStr);
        
        // var configId = this.data.rewardConfigId;
        // var rewardConfig = ConfigManager.Instance.GetById<Config.BattleReward>(configId);
        //
        // var type = (BattleRewardType)rewardConfig.Type;
        //
        // var nameStr = rewardConfig.Name;
        // var desStr = rewardConfig.Describe;
        // var intValueList = this.data.intValueList;
        // if (type == BattleRewardType.GainSkill_FixedRand)
        // {
        //     if (0 == rewardConfig.MakeSureRewardOccasion)
        //     {
        //         nameStr = "获得技能";
        //         if (intValueList.Count > 0)
        //         {
        //             var skillConfigId = intValueList[0];
        //             var skillConfig = ConfigManager.Instance.GetById<Skill>(skillConfigId);
        //             desStr = $"获得技能:{skillConfig.Name}\n{skillConfig.Describe}";
        //         }
        //     }
        // }
        // else if (type == BattleRewardType.TeamMember_Gain)
        // {
        // }
        // else if (type == BattleRewardType.TeamMember_RandAttr)
        // {
        // }
        //
        nameText.text = nameStr;
        describeText.text = desStr;
    }

    public void OnClickSelectBtn()
    {
        this.parentUI.OnSelectSelection(this);
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }
}