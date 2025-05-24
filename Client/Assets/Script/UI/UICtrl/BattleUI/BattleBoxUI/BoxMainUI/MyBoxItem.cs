using System;
using System.Collections;
using System.Collections.Generic;
using Battle;
using Battle_Client;
using UnityEngine;
using UnityEngine.UI;

public class MyBoxItem
{
    public GameObject gameObject;
    public Transform transform;

    public Image icon;
    public Image iconBg;
    // public Text nameText;
    public Text countText;

    public Button openBtn;

    public MyBoxGroup data;

    public void Init(GameObject gameObject)
    {
        this.gameObject = gameObject;
        this.transform = this.gameObject.transform;
        iconBg = this.transform.Find("iconBg").GetComponent<Image>();

        icon = iconBg.transform.Find("icon").GetComponent<Image>();
        // nameText = this.transform.Find("name_text").GetComponent<Text>();
        countText = this.transform.Find("count_text").GetComponent<Text>();
        openBtn = this.transform.Find("openBtn").GetComponent<Button>();

        openBtn.onClick.RemoveAllListeners();
        openBtn.onClick.AddListener(OnClickOpenBtn);
    }

    public IBattleMyBoxItem GetConfig(RewardQuality quality)
    {
        var list = BattleConfigManager.Instance.GetList<IBattleMyBoxItem>();
        if ((int)quality >= list.Count)
        {
            return null;
        }

        return list[(int)quality];
    }


    public void RefreshUI(MyBoxGroup data)
    {
        this.data = data;

        var config = GetConfig(this.data.quality);
        var quality = (RewardQuality)data.quality;
        iconBg.color = ColorDefine.GetColorByQuality(quality);
        int iconResId = config.IconResId;
        if (iconResId > 0)
        {
            ResourceManager.Instance.GetObject<Sprite>(iconResId, (sprite) => { icon.sprite = sprite; });
        }

        // nameText.text = config.Name;
        countText.text = "x" + this.data.count;
    }

    public void OnClickOpenBtn()
    {
        var quality = this.data.quality;
        var player = BattleManager.Instance.GetLocalPlayer();
        if (player.GetMyBoxCount(quality) > 0)
        {
            player.TryOpenBox(quality);
        }

        // //检查货币是否够
        // var player = BattleManager.Instance.GetLocalPlayer();
        // var myCoin = player.GetCoinCount();
        // // data.costItemId
        // if (myCoin < data.costCount)
        // {
        //     Logx.Log(LogxType.BattleBox, "BoxShop : OnClickBuyBtn : " +
        //                                  "coin not enough : myCoin : " + myCoin + "," +
        //                                  "needCostCount : " + data.costCount);
        //     return;
        // }
        //
        // //send buy
        // var config = BattleConfigManager.Instance.
        //     GetById<IBattleBoxShopItem>(this.data.configId);
        // var localPlayer = BattleManager.Instance.GetLocalPlayer();
        // localPlayer.BuyBoxFromShop((RewardQuality)config.Quality,1);
    }

    public void Release()
    {
        openBtn.onClick.RemoveAllListeners();
    }
}