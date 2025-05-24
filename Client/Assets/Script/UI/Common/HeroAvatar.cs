using System;
using System.Collections;
using System.Collections.Generic;
using GameData;

using UnityEngine;
using UnityEngine.UI;


public class HeroAvatar
{
    public GameObject gameObject;
    public Transform transform;

    Image avatarImg;
    Text levelText;
    //Text nameText;
    Button clickBtn;
    GameObject selectGo;

    public HeroData data;

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

    public void Refresh(HeroData uiData)
    {
        this.data = uiData;

        //avatarImg
        var config = Config.ConfigManager.Instance.GetById<Config.EntityInfo>(this.data.configId);
        levelText.text = "" + this.data.level;
        
        ResourceManager.Instance.GetObject<Sprite>(config.AvatarResId, (sprite) =>
        {
            this.avatarImg.sprite = sprite;
        });

      
        //nameText.text = config.Name;

    }

    public void AddClickListener(Action<int> action)
    {
        clickBtn.onClick.RemoveAllListeners();
        clickBtn.onClick.AddListener(() =>
        {
            action?.Invoke(this.data.guid);
        });
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