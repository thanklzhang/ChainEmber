//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;

//public class HeroListUI : NormalUI
//{
//    Transform heroRoot;
//    GameObject heroObjPrefab;
//    List<HeroShowItem> heroShowItems;
   
//    public Action<int> onClickUpdateHeroBtnEvent;
//    public Action<int> onClickEnterHeroInfoBtnEvent;

//    protected override void OnInit()
//    {
//        this.name = UIName.HeroListUI;
//        this.resPath = "Assets/BuildRes/Prefabs/UI/HeroListUI.prefab";
//        this.type = UIType.Normal;
//    }


//    protected override void OnLoadFinish()
//    {
//        heroRoot = this.transform.Find("root");
//        heroObjPrefab = this.transform.Find("root/HeroCard").gameObject;
//        heroObjPrefab.SetActive(false);
        
//    }

//    protected override void OnOpen()
//    {

//    }

//    protected override void OnActive()
//    {

//    }
//    //提供给外部的接口--------------------------------
//    public void CreateHeroList(List<Hero> heroDataList)
//    {
//        heroShowItems = new List<HeroShowItem>();
//        for (int i = 0; i < heroDataList.Count; i++)
//        {
//            var obj = GameObject.Instantiate(heroObjPrefab, heroRoot);
//            HeroShowItem showItem = new HeroShowItem();
//            showItem.Init(obj, this);
//            showItem.Refresh(heroDataList[i]);
//            showItem.Show();
//            heroShowItems.Add(showItem);
//        }
//    }

//    public void RefreshLevelData(int heroId, int level)
//    {
//        var hero = heroShowItems.Find((dataObj) =>
//        {
//            return dataObj.heroData.id == heroId;
//        });
//        hero.SetLevelText(level);
//    }
//    //----------------------------------------------------
//    protected override void OnClose()
//    {

//    }

//}


//public class HeroShowItem
//{
//    public Hero heroData;

//    public GameObject gameObject;
//    public Transform transform;

//    public Image iconImg;
//    public Text nameText;
//    public Button updateBtn;

//    public Text idText;
//    public Text levelText;
//    Button enterInfoBtn;
//    public HeroListUI parent;

//    internal void Init(GameObject obj, HeroListUI parent)
//    {
//        this.parent = parent;

//        this.gameObject = obj;
//        this.transform = this.gameObject.transform;

//        this.idText = this.transform.Find("id").GetComponent<Text>();
//        this.levelText = this.transform.Find("level").GetComponent<Text>();
//        enterInfoBtn = this.transform.Find("enterInfoBtn").GetComponent<Button>();
//        this.updateBtn = this.transform.Find("upgradeBtn").GetComponent<Button>();
//        this.updateBtn.onClick.AddListener(OnClickUpdateBtn);
//        this.enterInfoBtn.onClick.AddListener(OnClickEnterHeroInfoBtn);

//    }

//    internal void Refresh(Hero heroData)
//    {
//        this.heroData = heroData;

//        this.idText.text = "id:" + this.heroData.id;
//        this.levelText.text = "Lv." + this.heroData.level;
//    }

//    public void SetLevelText(int level)
//    {
//        this.levelText.text = "" + level;
//    }


//    private void OnClickEnterHeroInfoBtn()
//    {
//        this.parent.onClickEnterHeroInfoBtnEvent?.Invoke(this.heroData.id);
//    }

//    public void OnClickUpdateBtn()
//    {
//        //升级
//        this.parent.onClickUpdateHeroBtnEvent?.Invoke(this.heroData.id);
//    }

//    internal void Show()
//    {
//        this.gameObject.SetActive(true);
//    }

//    internal void Hide()
//    {
//        this.gameObject.SetActive(true);
//    }
//}
