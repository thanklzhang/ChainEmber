using System;
using System.Collections;
using System.Collections.Generic;
using Config;
using GameData;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeroListUI : BaseUI
{
    private Transform heroRoot;
    private Button closeBtn;
    // private Text titleText;

    private List<HeroData> heroDataList;
    private List<HeroListItemShowObj> heroShowObjList;

    protected override void OnInit()
    {
        this.uiResId = (int)ResIds.HeroListUI;
        this.uiShowLayer = UIShowLayer.Floor_0;
        this.showMode = CtrlShowMode.Fixed;
    }

    protected override void OnLoadFinish()
    {
        heroRoot = this.transform.Find("cardScroll/mask/root");
        closeBtn = this.transform.Find("closeBtn").GetComponent<Button>();
        // titleText = this.transform.Find("titleText").GetComponent<Text>();

        closeBtn.onClick.AddListener(() =>
        {
            UIManager.Instance.Close<HeroListUI>();
        });

        heroShowObjList = new List<HeroListItemShowObj>();
    }

    protected override void OnOpen(UICtrlArgs args)
    {
        HeroListUIArgs heroListUIArgs = (HeroListUIArgs)args;
        
        // Get hero data from game data
        HeroGameData heroGameData = GameData.GameDataManager.Instance.HeroData;
        heroDataList = heroGameData.HeroList;
        
        // titleText.text = "英雄列表";
        
        UIManager.Instance.Open<TitleBarUI>(new TitleBarUIArgs()
        {
            titleBarId = 2
        });
        
        
        RefreshHeroList();
    }

    private void RefreshHeroList()
    {
        // Clear existing list
        heroShowObjList = new List<HeroListItemShowObj>();
        
        for (int i = 0; i < heroDataList.Count; i++)
        {
            var data = heroDataList[i];
            GameObject go = null;
            HeroListItemShowObj heroItem = null;
            
            if (i < heroRoot.childCount)
            {
                go = heroRoot.GetChild(i).gameObject;
            }
            else
            {
                var prefab = heroRoot.GetChild(0).gameObject;
                go = GameObject.Instantiate(prefab, heroRoot, false);
            }
            
            go.SetActive(true);
            heroItem = new HeroListItemShowObj();
            heroShowObjList.Add(heroItem);
            heroItem.Init(go);
            heroItem.Refresh(data);
            
            // 添加点击事件打开英雄详情界面
            heroItem.AddClickListener(() => {
                UIManager.Instance.Open<HeroInfoUI>(new HeroInfoUIArgs(heroItem.data));
            });
        }
        
        // Hide unused items
        for (int i = heroDataList.Count; i < heroRoot.childCount; i++)
        {
            var go = heroRoot.GetChild(i).gameObject;
            go.SetActive(false);
        }
    }

    protected override void OnClose()
    {
        closeBtn.onClick.RemoveAllListeners();
        
        foreach (var item in heroShowObjList)
        {
            item.Release();
        }
        
        heroShowObjList = null;
    }
}

// Arguments for opening the HeroListUI
public class HeroListUIArgs : UICtrlArgs
{
    // Add any specific arguments needed for this UI
}

// Class to manage each hero item in the list
public class HeroListItemShowObj
{
    public GameObject gameObject;
    public Transform transform;
    public HeroData data;
    
    // private HeroAvatar avatar;
    private TextMeshProUGUI nameText;
    private TextMeshProUGUI levelText;
    // private TextMeshProUGUI descriptionText;
    private Image bodyImg;
    private Button clickBtn;
    
    public void Init(GameObject go)
    {
        gameObject = go;
        transform = gameObject.transform;
        
        // Initialize UI components
        bodyImg = transform.Find("bg/rolePic").GetComponent<Image>();
        // avatar = new HeroAvatar();
        // avatar.Init(avatarRootGo);
        // var config = ConfigManager.Instance.GetById<EntityInfo>(data.configId);
        nameText = transform.Find("nameBg/name").GetComponent<TextMeshProUGUI>();
        levelText = transform.Find("levelBg/level").GetComponent<TextMeshProUGUI>();
        // descriptionText = transform.Find("descriptionText").GetComponent<TextMeshProUGUI>();
        
        // 添加点击按钮
        clickBtn = gameObject.GetComponent<Button>();
        
        // // 如果没有Image组件（用于按钮响应），添加一个透明的
        // if (clickBtn.GetComponent<Image>() == null)
        // {
        //     Image btnImage = gameObject.AddComponent<Image>();
        //     btnImage.color = new Color(0, 0, 0, 0.01f); // 几乎透明
        // }
    }
    
    public void Refresh(HeroData data)
    {
        this.data = data;
        
        var config = ConfigManager.Instance.GetById<EntityInfo>(data.configId);
        nameText.text = config.Name;
        levelText.text = "Lv." + data.level;
        // descriptionText.text = config.Describe;
        ResourceManager.Instance.GetObject<Sprite>(config.AllBodyResId, (sprite) =>
        {
            bodyImg.sprite = sprite;
        });
    }
    
    public void AddClickListener(Action callback)
    {
        if (clickBtn != null)
        {
            clickBtn.onClick.RemoveAllListeners();
            clickBtn.onClick.AddListener(() => {
                callback?.Invoke();
            });
        }
    }
    
    public void Release()
    {
        // avatar.Release();
        if (clickBtn != null)
        {
            clickBtn.onClick.RemoveAllListeners();
        }
    }
}
