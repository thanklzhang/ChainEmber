// using System;
// using System.Collections;
// using System.Collections.Generic;
// 
// using UnityEngine;
// using UnityEngine.UI;
//
//
// public class HeroListUIArgs : UIArgs
// {
//     public List<HeroCardUIData> cardList;
// }
//
// public class HeroCardShowObj : BaseUIShowObj<HeroListUIPre>
// {
//     //GameObject gameObject;
//     //Transform transform;
//
//     Text levelText;
//     Text nameText;
//     GameObject unlockFlagObj;
//
//     Button upgradeLevelBtn;
//     private Image heroIconImg;
//
//     //Action<int> clickUpgradeLevelCallback;
//
//     public HeroCardUIData uiData;
//
//
//     public override void OnInit()
//     {
//         levelText = this.transform.Find("levelBg/level").GetComponent<Text>();
//         nameText = this.transform.Find("nameBg/name").GetComponent<Text>();
//         unlockFlagObj = this.transform.Find("lockFlag").gameObject;
//         upgradeLevelBtn = this.transform.Find("upgradeLevelBtn").GetComponent<Button>();
//         heroIconImg = this.transform.Find("bg/rolePic").GetComponent<Image>();
//
//         upgradeLevelBtn.onClick.AddListener(() =>
//         {
//             this.parentObj.OnClickUpgradeLevelCallback(this.uiData.guid, 1);
//             //clickUpgradeLevelCallback?.Invoke(this.uiData.id);
//         });
//     }
//
//     //public void Refresh(HeroCardUIData uiData)
//     //{
//     //    this.uiData = uiData;
//
//     //    var id = this.uiData.id;
//     //    var heroInfoTable = ConfigManager.Instance.GetById<Config.HeroInfo>(id);
//     //    levelText.text = "" + this.uiData.level;
//     //    nameText.text = "" + heroInfoConfig.Name;
//     //    unlockFlagObj.SetActive(!this.uiData.isUnlock);
//     //    upgradeLevelBtn.gameObject.SetActive(this.uiData.isUnlock);
//     //}
//
//     public override void OnRefresh(object data, int index)
//     {
//         this.uiData = (HeroCardUIData)data;
//
//         var configId = this.uiData.configId;
//         var heroInfoTable = ConfigManager.Instance.GetById<Config.EntityInfo>(configId);
//         levelText.text = "Lv." + this.uiData.level;
//         nameText.text = "" + heroInfoConfig.Name;
//         unlockFlagObj.SetActive(!this.uiData.isUnlock);
//         // upgradeLevelBtn.gameObject.SetActive(this.uiData.isUnlock);
//
//         ResourceManager.Instance.GetObject<Sprite>(heroInfoConfig.AllBodyResId, (sprite)
//             =>
//         {
//             heroIconImg.sprite = sprite;
//         });
//     }
//
//     public override void OnRelease()
//     {
//         //clickUpgradeLevelCallback = null;
//         upgradeLevelBtn.onClick.RemoveAllListeners();
//     }
// }
//
// public class HeroListUIPre : BaseUI
// {
//     public Action onGoInfoUIBtnClick;
//     public Action onCloseBtnClick;
//
//     public Action<int, int> onClickOneHeroUpgradeLevelBtn;
//
//     Button goInfoUIBtn;
//     Button closeBtn;
//
//     Transform heroListRoot;
//
//     List<HeroCardUIData> cardDataList = new List<HeroCardUIData>();
//     List<HeroCardShowObj> showDataObjList = new List<HeroCardShowObj>();
//
//     protected override void OnInit()
//     {
//         goInfoUIBtn = this.transform.Find("cardScroll/mask/root/HeroCard/enterInfoBtn").GetComponent<Button>();
//         closeBtn = this.transform.Find("closeBtn").GetComponent<Button>();
//         heroListRoot = this.transform.Find("cardScroll/mask/root");
//
//         goInfoUIBtn.onClick.AddListener(() => { onGoInfoUIBtnClick?.Invoke(); });
//
//         closeBtn.onClick.AddListener(() => { onCloseBtnClick?.Invoke(); });
//     }
//
//     public void OnClickUpgradeLevelCallback(int guid, int level)
//     {
//         onClickOneHeroUpgradeLevelBtn?.Invoke(guid, level);
//     }
//
//     //internal void RefreshOneHero(HeroCardUIData nowHeroData)
//     //{
//     //    foreach (var item in showDataObjList)
//     //    {
//     //        if (item.uiData.id == nowHeroData.id)
//     //        {
//     //            item.Refresh(nowHeroData);
//     //            break;
//     //        }
//     //    }
//     //}
//
//     public override void Refresh(UIArgs args)
//     {
//         HeroListUIArgs heroListArgs = (HeroListUIArgs)args;
//
//         this.cardDataList = heroListArgs.cardList;
//
//         this.RefreshHeroList();
//     }
//
//     void RefreshHeroList()
//     {
//         //for (int i = 0; i < this.cardDataList.Count; i++)
//         //{
//         //    var cardData = this.cardDataList[i];
//
//         //    GameObject go = null;
//         //    if (i < heroListRoot.childCount)
//         //    {
//         //        go = heroListRoot.GetChild(i).gameObject;
//         //    }
//         //    else
//         //    {
//         //        var tempObj = heroListRoot.GetChild(0).gameObject;
//         //        go = GameObject.Instantiate(tempObj, heroListRoot);
//         //    }
//
//         //    HeroCardShowObj showObj = null;
//         //    if (i < showDataObjList.Count)
//         //    {
//         //        showObj = showDataObjList[i];
//         //    }
//         //    else
//         //    {
//         //        showObj = new HeroCardShowObj();
//         //        showDataObjList.Add(showObj);
//         //        showObj.Init(go, onClickOneHeroUpgradeLevelBtn);
//         //    }
//
//         //    go.SetActive(true);
//         //    showObj.Refresh(cardData);
//         //}
//
//         //for (int i = this.cardDataList.Count; i < heroListRoot.childCount; i++)
//         //{
//         //    var obj = heroListRoot.GetChild(i).gameObject;
//         //    obj.SetActive(false);
//         //}
//
//         UIListArgs<HeroCardShowObj, HeroListUIPre> args = new UIListArgs<HeroCardShowObj, HeroListUIPre>();
//         args.dataList = cardDataList;
//         args.showObjList = showDataObjList;
//         args.root = heroListRoot;
//         args.parentObj = this;
//         UIFunc.DoUIList(args);
//     }
//
//     protected override void OnUnload()
//     {
//         onGoInfoUIBtnClick = null;
//         onCloseBtnClick = null;
//
//         foreach (var item in showDataObjList)
//         {
//             item.Release();
//         }
//
//         showDataObjList = null;
//         cardDataList = null;
//     }
// }