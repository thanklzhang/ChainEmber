// using Battle_Client;
// using System;
// using System.Collections;
// using System.Collections.Generic;
// using Battle;
// using Config;
// using TMPro;
// using UnityEngine;
// using UnityEngine.EventSystems;
// using UnityEngine.UI;
// using Vector2 = UnityEngine.Vector2;
//
// //TODO 待和实体道具栏合并
// public class WarehouseItemUIShowObj_pre
// {
//     public GameObject gameObject;
//     public Transform transform;
//
//     private Transform itemTran;
//
//     // private Image itemIconImg;
//     // private TextMeshProUGUI countText;
//     private GameObject lockFlagGo;
//     private UIEventTrigger evetnTrigger;
//
//     //runtime
//     public int index;
//     public bool isUnlock;
//     public BattleItemData_Client itemData;
//     private BattleItemWarehouseUI parentUI;
//
//     private CommonItem itemShowObj;
//
//     private DragScript dragScript;
//     private DropScript dropScript;
//
//     public void Init(GameObject gameObject, BattleItemWarehouseUI parentUI)
//     {
//         this.gameObject = gameObject;
//         this.transform = this.gameObject.transform;
//         this.parentUI = parentUI;
//
//         itemTran = this.transform.Find("CommonItem");
//         itemShowObj = new CommonItem();
//         itemShowObj.Init(itemTran.gameObject);
//
//         // this.itemIconImg = itemTran.Find("icon").GetComponent<Image>();
//         //
//         // countText = itemTran.Find("countText").GetComponent<TextMeshProUGUI>();
//         lockFlagGo = transform.Find("lock").gameObject;
//         dragScript = itemTran.gameObject.GetComponent<DragScript>();
//         if (null == dragScript)
//         {
//             dragScript = itemTran.gameObject.AddComponent<DragScript>();
//         }
//
//         dragScript.target = itemTran;
//
//         dragScript.onBeginDragBeforeAction += OnBeginDrag_Before;
//         dragScript.onBeginDragAction += OnBeginDrag;
//         dragScript.onDragAction += OnDrag;
//         dragScript.onEndDragAction += OnEndDrag;
//
//         var itemIconImg = itemShowObj.GetIconImage();
//         evetnTrigger = itemIconImg.GetComponent<UIEventTrigger>();
//         evetnTrigger.OnPointEnterEvent += OnPointEnter;
//         evetnTrigger.OnPointerExitEvent += OnPointExit;
//
//         dropScript = this.gameObject.GetComponent<DropScript>();
//         dropScript.OnDropAction += OnDropAction;
//     }
//     //作为拖动开始点------------------
//
//     public void OnBeginDrag_Before(PointerEventData eventData)
//     {
//         dragScript.transferData = this;
//     }
//
//     public void OnBeginDrag(PointerEventData eventData)
//     {
//     }
//
//     public void OnDrag(PointerEventData eventData)
//     {
//     }
//
//     public void OnEndDrag(PointerEventData eventData)
//     {
//         //this.parentUI.OnItemEndDrag(this,eventData);
//     }
//
//
//     //作为拖动结束点-------------------------
//     public void OnDropAction(PointerEventData eventData)
//     {
//         GameObject dropped = eventData.pointerDrag;
//
//         var dragScript = dropped.GetComponent<DragScript>();
//
//         if (null == dragScript)
//         {
//             return;
//         }
//
//         //TODO 可以通用
//         if (dragScript.transferData is ItemCellUIShowObj)
//         {
//             //从实体道具栏拖过来的
//             var showObj = dragScript.transferData as ItemCellUIShowObj;
//
//             ItemMoveArg srcMoveArg = new ItemMoveArg();
//             srcMoveArg.locationType = ItemLocationType.EntityItemBar;
//             srcMoveArg.itemIndex = showObj.index;
//             //这里如果还是用的 ItemUIShowObj 那么需要传入 entityGuid
//             srcMoveArg.entityGuid = showObj.entityGuid; //BattleManager.Instance.GetLocalCtrlHeroGuid();
//
//             ItemMoveArg desMoveArg = new ItemMoveArg();
//             desMoveArg.locationType = ItemLocationType.Warehouse;
//             desMoveArg.itemIndex = this.index;
//             //desMoveArg.entityGuid = BattleManager.Instance.GetLocalCtrlHeroGuid();
//
//
//             BattleManager.Instance.MsgSender.Send_MoveItemTo(srcMoveArg, desMoveArg);
//         }
//         else if (dragScript.transferData is WarehouseItemUIShowObj)
//         {
//             //从玩家仓库拖过来的
//             var showObj = dragScript.transferData as WarehouseItemUIShowObj;
//
//             ItemMoveArg srcMoveArg = new ItemMoveArg();
//             srcMoveArg.locationType = ItemLocationType.Warehouse;
//             srcMoveArg.itemIndex = showObj.index;
//
//             ItemMoveArg desMoveArg = new ItemMoveArg();
//             desMoveArg.locationType = ItemLocationType.Warehouse;
//             desMoveArg.itemIndex = this.index;
//             // desMoveArg.entityGuid = BattleManager.Instance.GetLocalCtrlHeroGuid();
//
//             BattleManager.Instance.MsgSender.Send_MoveItemTo(srcMoveArg, desMoveArg);
//         }
//     }
//
//     public void RefreshUI(BattleItemData_Client data, int index, bool isUnlock)
//     {
//         this.index = index;
//         this.itemData = data;
//         this.isUnlock = isUnlock;
//
//         RefreshItemShow();
//     }
//
//     void RefreshItemShow()
//     {
//         if (this.isUnlock)
//         {
//             this.lockFlagGo.SetActive(false);
//             
//             if (itemData != null)
//             {
//                
//                 this.itemTran.gameObject.SetActive(true);
//
//                 // var itemConfig = ConfigManager.Instance.GetById<Config.BattleItem>(this.data.configId);
//                 // var iconResId = itemConfig.IconResId;
//                 // ResourceManager.Instance.GetObject<Sprite>(iconResId,
//                 //     (sprite) => { this.itemIconImg.sprite = sprite; });
//                 //
//                 // var count = this.data.count;
//                 // countText.text = "" + this.data.count;
//                 // if (count > 1)
//                 // {
//                 //     countText.gameObject.SetActive(true);
//                 // }
//                 // else
//                 // {
//                 //     countText.gameObject.SetActive(false);
//                 // }
//                 itemShowObj.RefreshUI(itemData);
//             }
//             else
//             {
//                 this.itemTran.gameObject.SetActive(false);
//             }
//         }
//         else
//         {
//             this.itemTran.gameObject.SetActive(false);
//             this.lockFlagGo.SetActive(true);
//         }
//     }
//
//     public void Update(float deltaTime)
//     {
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
//         var battleUIRect = parentUI.battleUI.gameObject.GetComponent<RectTransform>();
//         RectTransformUtility.ScreenPointToLocalPointInRectangle(battleUIRect, screenPos, cameraUI.camera, out uiPos);
//
//         EventDispatcher.Broadcast<int, Vector2>(EventIDs.On_UIItemOption_PointEnter, this.itemData.configId, uiPos);
//     }
//
//     public void OnPointExit(PointerEventData e)
//     {
//         EventDispatcher.Broadcast<int>(EventIDs.On_UIItemOption_PointExit, this.itemData.configId);
//     }
//
//
//     public void Release()
//     {
//         evetnTrigger.OnPointEnterEvent -= OnPointEnter;
//         evetnTrigger.OnPointerExitEvent -= OnPointExit;
//
//         dragScript.onBeginDragBeforeAction -= OnBeginDrag_Before;
//         dragScript.onBeginDragAction -= OnBeginDrag;
//         dragScript.onDragAction -= OnDrag;
//         dragScript.onEndDragAction -= OnEndDrag;
//
//         dropScript.OnDropAction -= OnDropAction;
//
//         itemShowObj.Release();
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