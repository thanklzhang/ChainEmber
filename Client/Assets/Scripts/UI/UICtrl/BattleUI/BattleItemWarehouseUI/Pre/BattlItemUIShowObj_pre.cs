// using Battle_Client;
// using System;
// using System.Collections;
// using System.Collections.Generic;
//
// using UnityEngine;
// using UnityEngine.EventSystems;
// using UnityEngine.UI;
//
// public class BattleItemUIShowObj : BaseUIShowObj<BattleItemUI>
// {
//     protected Transform itemRoot;
//     protected Image icon;
//     protected GameObject canUseMaskGo;
//     protected GameObject cdRootGo;
//     protected Image cdImg;
//     protected Text cdTimeText;
//     protected UIEventTrigger evetnTrigger;
//     private Text countText;
//     public BattleItemInfo uiData;
//
//     float currCDTimer = 0;
//
//     
//     public override void OnInit()
//     {
//         itemRoot = this.transform.Find("item");
//         canUseMaskGo = this.itemRoot.Find("cantUse").gameObject;
//         cdRootGo = this.itemRoot.Find("CDRoot").gameObject;
//         cdTimeText = this.itemRoot.Find("CDRoot/CDShow/cd_text").GetComponent<Text>();
//         cdImg = this.itemRoot.Find("CDRoot/CDShow").GetComponent<Image>();
//         icon = this.itemRoot.Find("icon").GetComponent<Image>();
//         countText = this.itemRoot.Find("count_text").GetComponent<Text>();
//
//         evetnTrigger = icon.GetComponent<UIEventTrigger>();
//
//         canUseMaskGo.SetActive(false);
//         cdRootGo.SetActive(false);
//         
//         evetnTrigger.OnPointEnterEvent += OnPointEnter;
//         evetnTrigger.OnPointerExitEvent += OnPointExit;
//     }
//
//     public override void OnRefresh(object data, int index)
//     {
//         if (null == data)
//         {
//             return;
//         }
//         
//         this.uiData = (BattleItemInfo)data;
//         RefreshUI(this.uiData);
//
//     }
//
//     void RefreshUI(BattleItemInfo data)
//     {
//         this.uiData = data;
//         
//         var isHaveItem = this.uiData.count > 0;
//
//         if (isHaveItem)
//         {
//             //道具图标
//             var itemId = this.uiData.configId;
//             var itemConfig = Config.ConfigManager.Instance.GetById<Config.BattleItem>(itemId);
//             ResourceManager.Instance.GetObject<Sprite>(itemConfig.IconResId, (sprite) => { this.icon.sprite = sprite; });
//
//             countText.text = "" + this.uiData.count;
//             
//             if (this.uiData.currCDTime <= 0)
//             {
//                 //可以使用技能了
//                 cdRootGo.SetActive(false);
//                 canUseMaskGo.SetActive(false);
//             }
//             else
//             {
//                 //在 cd 中 ，刷新 cd 时间
//                 cdRootGo.SetActive(true);
//                 //mask 是代表不能用 并不是只有 cd 这个因素 尽管现在只有 cd
//                 canUseMaskGo.SetActive(true);
//             }
//             
//             itemRoot.gameObject.SetActive(true);
//             
//         }
//         else
//         {
//             itemRoot.gameObject.SetActive(false);
//         }
//         
//     }
//
//     internal void UpdateInfo(BattleItemInfo itemInfo)
//     {
//         currCDTimer = itemInfo.currCDTime;
//         
//         RefreshUI(itemInfo);
//         
//     }
//
//
//     public void Update(float deltaTime)
//     {
//         if (currCDTimer < 0)
//         {
//             currCDTimer = 0;
//         }
//
//         if (currCDTimer > 0)
//         {
//             currCDTimer -= deltaTime;
//
//             float showTime = 0.0f;
//             var showStr = "";
//             if (currCDTimer >= 0.50f)
//             {
//                 showTime = (float)Math.Ceiling(currCDTimer);
//
//                 showStr = string.Format("{0}", (int)showTime);
//             }
//             else
//             {
//                 showTime = currCDTimer;
//                 showStr = string.Format("{0:F}", showTime);
//             }
//
//
//             //Debug.LogError("currCDTimer " + currCDTimer + " " + uiData.maxCDTime);
//             cdTimeText.text = showStr;
//             //Debug.Log("currCDTimer" + currCDTimer);
//             //Debug.Log("uiData.maxCDTime" + uiData.maxCDTime);
//             cdImg.fillAmount = currCDTimer / uiData.maxCDTime;
//         }
//     }
//
//     internal int GetItemIndex()
//     {
//         return this.uiData.index;
//     }
//
//     public void OnPointEnter(PointerEventData e)
//     {
//         //转换成点在 BattleUI 中的 localPosition
//
//         var camera3D = CameraManager.Instance.GetCamera3D();
//         var cameraUI = CameraManager.Instance.GetCameraUI();
//
//         var screenPos = e.position;
//
//         Vector2 uiPos;
//         var battleUIRect = parentObj.BattleUI.gameObject.GetComponent<RectTransform>();
//         RectTransformUtility.ScreenPointToLocalPointInRectangle(battleUIRect, screenPos, cameraUI.camera, out uiPos);
//
//         EventDispatcher.Broadcast<int, Vector2>(EventIDs.On_UIItemOption_PointEnter, this.uiData.configId, uiPos);
//     }
//
//     public void OnPointExit(PointerEventData e)
//     {
//         EventDispatcher.Broadcast<int>(EventIDs.On_UIItemOption_PointExit, this.uiData.configId);
//     }
//
//
//     public override void OnRelease()
//     {
//         evetnTrigger.OnPointEnterEvent -= OnPointEnter;
//         evetnTrigger.OnPointerExitEvent -= OnPointExit;
//     }
// }
//
//
// // public class BattleSkillUIArgs : UICtrlArgs
// // {
// //     public List<BattleSkillUIData> battleSkillList;
// // }
//
// // public class BattleItemUIData
// // {
// //     public int index;
// //     public int configId;
// //     public int count;
// //
// //     public float currCDTime;
// //     public float maxCDTime;
// //     
// //     // public int skillId;
// //     // public int iconResId;
// //     
// // }