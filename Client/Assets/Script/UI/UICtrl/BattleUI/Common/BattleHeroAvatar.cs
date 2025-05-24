using System;
using System.Collections;
using System.Collections.Generic;
using Battle_Client;
using GameData;
using UnityEngine;
using UnityEngine.UI;

public class BattleHeroShowData
{
    public int configId;
    public int level = 1;
    public int star = 1;
}

public class BattleHeroAvatar
{
    public GameObject gameObject;
    public Transform transform;

    Image avatarImg;

    Text levelText;

    //Text nameText;
    Button clickBtn;
    GameObject selectGo;

    // public BattleEntity_Client data;
    public BattleHeroShowData data;

    public void Init(GameObject gameObject)
    {
        this.gameObject = gameObject;
        this.transform = this.gameObject.transform;

        avatarImg = this.transform.Find("show/selectHeroIcon").GetComponent<Image>();
        //nameText = this.transform.Find("show/heroNameText").GetComponent<Text>();
        levelText = this.transform.Find("show/heroLevelText").GetComponent<Text>();
        clickBtn = this.transform.Find("show/clickBtn").GetComponent<Button>();
        selectGo = this.transform.Find("show/select").gameObject;
    }


    public void Show()
    {
        this.gameObject.SetActive(true);
    }

    // public void Refresh(BattleEntity_Client uiData)
    // {
    //     this.data = uiData;
    //
    //     //avatarImg
    //     var config = Config.ConfigManager.Instance.GetById<Config.EntityInfo>(this.data.configId);
    //     levelText.text = "" + this.data.level;
    //     
    //     ResourceManager.Instance.GetObject<Sprite>(config.AvatarResId, (sprite) =>
    //     {
    //         this.avatarImg.sprite = sprite;
    //     });
    //
    //   
    //     //nameText.text = config.Name;
    //
    // }

    public void Refresh(BattleHeroShowData data)
    {
        this.data = data;

        //avatarImg
        var config = Config.ConfigManager.Instance.GetById<Config.EntityInfo>(this.data.configId);
        levelText.text = "" + this.data.level;

        ResourceManager.Instance.GetObject<Sprite>(config.AvatarResId, (sprite) => { this.avatarImg.sprite = sprite; });


        //nameText.text = config.Name;
    }

    public void AddClickListener(Action<BattleHeroAvatar> action)
    {
        clickBtn.onClick.RemoveAllListeners();
        clickBtn.onClick.AddListener(() => { action?.Invoke(this); });
    }

    public void SetSelectState(bool isShow)
    {
        selectGo.SetActive(isShow);
    }

    public void RemoveClickListener()
    {
        clickBtn.onClick.RemoveAllListeners();
    }

    public void Update()
    {
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }

    public void Release()
    {
        RemoveClickListener();
    }
}