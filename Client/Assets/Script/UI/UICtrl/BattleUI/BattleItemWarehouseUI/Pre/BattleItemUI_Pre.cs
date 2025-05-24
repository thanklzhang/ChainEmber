// using System;
// using System.Collections;
// using System.Collections.Generic;
// using Battle_Client;
//
// using UnityEngine;
// using UnityEngine.UI;
//
// // 主角身上的装备栏
// public class BattleItemUI
// {
//     public GameObject gameObject;
//     public Transform transform;
//
//     Transform itemListRoot;
//
//     Text tipText;
//     float tipShowTimer;
//     float tipMaxShowTime = 1.6f;
//
//     List<BattleItemInfo> itemDataList = new List<BattleItemInfo>();
//     List<BattleItemUIShowObj> itemShowObjList = new List<BattleItemUIShowObj>();
//   
//     public BattleUI BattleUI;
//
//     public void Init(GameObject gameObject, BattleUI battleUIPre)
//     {
//         this.gameObject = gameObject;
//         this.transform = this.gameObject.transform;
//
//         this.BattleUI = battleUIPre;
//
//         itemListRoot = this.transform.Find("group");
//         this.tipText = this.transform.Find("itemTipText").GetComponent<Text>();
//      
//         EventDispatcher.AddListener<BattleItemInfo>(EventIDs.OnItemInfoUpdate, OnItemInfoUpdate);
//         EventDispatcher.AddListener<string>(EventIDs.OnItemTips,OnItemTips);
//         
//     }
//     public void OnItemInfoUpdate( BattleItemInfo itemInfo)
//     {
//         //var entity = BattleEntityManager.Instance.FindEntity(entityGuid);
//
//         var myEntityGuid = BattleManager.Instance.GetLocalCtrlHeroGuid();
//
//         if (myEntityGuid == itemInfo.ownerGuid)
//         {
//             this.UpdateItemInfo(itemInfo);
//         }
//     }
//
//     void OnItemTips(string str)
//     {
//         this.SetItemTipText(str);
//     }
//
//     public void Show()
//     {
//         this.gameObject.SetActive(true);
//     }
//
//     public void RefreshAllUI()
//     {
//         RefreshDataList();
//         RefreshShowList();
//     }
//
//     public void RefreshDataList()
//     {
//
//         var items = BattleManager.Instance.GetLocalCtrlHeroItems();
//         
//         // for (int i = 0; i < items.Count; i++)
//         // {
//         //     var itemData = items[i];
//         //     BattleItemUIData item = new BattleItemUIData()
//         //     {
//         //         //skill.iconResId
//         //         configId = itemData.configId,
//         //         maxCDTime = itemData.maxCDTime,
//         //     };
//         //     
//         //     // var skillConfig = Config.ConfigManager.Instance.GetById<Skill>(itemData.configId);
//         //     dataList.Add(item);
//         // }
//         this.itemDataList = items;
//        
//     
//     }
//
//     void RefreshShowList()
//     {
//         UIListArgs<BattleItemUIShowObj, BattleItemUI> args = new UIListArgs<BattleItemUIShowObj, BattleItemUI>();
//         args.dataList = itemDataList;
//         args.showObjList = itemShowObjList;
//         args.root = itemListRoot;
//         args.parentObj = this;
//         UIFunc.DoUIList(args);
//     }
//
//     public void UpdateItemInfo(BattleItemInfo itemInfo)
//     {
//         var itemShowObj = FindItem(itemInfo.index);
//         if (itemShowObj != null)
//         {
//             itemShowObj.UpdateInfo(itemInfo);
//         }
//         else
//         {
//             //Logx.LogWarning("BattleSkillUI : UpdateSkillInfo : the skillId is not found : " + skillId);
//         }
//     }
//
//     public void SetItemTipText(string str)
//     {
//         this.tipText.text = str;
//         this.tipText.gameObject.SetActive(true);
//         this.tipShowTimer = this.tipMaxShowTime;
//     }
//
//     public void Update(float deltaTime)
//     {
//         foreach (var item in itemShowObjList)
//         {
//             item.Update(deltaTime);
//         }
//
//         if (this.tipShowTimer > 0)
//         {
//             this.tipShowTimer -= deltaTime;
//             if (this.tipShowTimer <= 0)
//             {
//                 this.tipText.gameObject.SetActive(false);
//             }
//         }
//     }
//
//     public BattleItemUIShowObj FindItem(int index)
//     {
//         foreach (var item in itemShowObjList)
//         {
//             if (item.GetItemIndex() == index)
//             {
//                 return item;
//             }
//         }
//         
//         return null;
//     }
//
//     public void Hide()
//     {
//         this.gameObject.SetActive(false);
//     }
//
//     public void Close()
//     {
//         this.Hide();
//     }
//
//     public void Release()
//     {
//         EventDispatcher.RemoveListener< BattleItemInfo>(EventIDs.OnItemInfoUpdate, OnItemInfoUpdate);
//         EventDispatcher.RemoveListener<string>(EventIDs.OnItemTips,OnItemTips);
//         
//         foreach (var item in itemShowObjList)
//         {
//             item.Release();
//         }
//
//         itemShowObjList = null;
//         itemDataList = null;
//     }
//
// }
