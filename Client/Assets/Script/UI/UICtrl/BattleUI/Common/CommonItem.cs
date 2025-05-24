using Battle_Client;
using System;
using System.Collections;
using System.Collections.Generic;
using Config;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CommonItem
{
    public GameObject gameObject;
    public Transform transform;

    protected Image itemIconImg;
    private TextMeshProUGUI countText;

    private BattleItemData_Client data;

    public virtual void Init(GameObject gameObject)
    {
        this.gameObject = gameObject;
        this.transform = this.gameObject.transform;
        
        this.itemIconImg = transform.Find("icon").GetComponent<Image>();

        countText = transform.Find("countText").GetComponent<TextMeshProUGUI>();
    }

    public Image GetIconImage()
    {
        return this.itemIconImg;
    }

    public void RefreshUI(BattleItemData_Client data)
    {
        this.data = data;
        
        var itemConfig = ConfigManager.Instance.GetById<Config.BattleItem>(this.data.configId);
        var iconResId = itemConfig.IconResId;
        ResourceManager.Instance.GetObject<Sprite>(iconResId,
            (sprite) => { this.itemIconImg.sprite = sprite; });

        var count = this.data.count;
        countText.text = "" + this.data.count;
        if (count > 1)
        {
            countText.gameObject.SetActive(true);
        }
        else
        {
            countText.gameObject.SetActive(false);
        }
    }

    public virtual void Release()
    {
    }
}