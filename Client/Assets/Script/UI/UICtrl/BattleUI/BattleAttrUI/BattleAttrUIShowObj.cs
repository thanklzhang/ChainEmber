// using Battle_Client;
// using System;
// using System.Collections;
// using System.Collections.Generic;
// using Battle;
//
// using UnityEngine;
// using UnityEngine.EventSystems;
// using UnityEngine.UI;
// using Vector2 = UnityEngine.Vector2;
//
//
// public class BattleAttrUIArgs : UICtrlArgs
// {
//     public List<BattleAttrUIData> battleAttrList;
// }
//
// public enum AttrValueShowType
// {
//     Int = 0,
//
//     //浮点型 保留两位
//     Float_2 = 1
// }
//
// public class BattleAttrUIData
// {
//     public EntityAttrType type;
//     public string name;
//     public int iconResId;
//     public float value;
//     public string describe;
//     public AttrValueShowType valueShowType;
// }
//
// public class BattleAttrUIShowObj : BaseUIShowObj<BattleAttrUI>
// {
//     Text nameText;
//     Text valueText;
//     Image iconImg;
//     public BattleAttrUIData uiData;
//     UIEventTrigger evetnTrigger;
//
//     public override void OnInit()
//     {
//         nameText = this.transform.Find("name_text").GetComponent<Text>();
//         valueText = this.transform.Find("value_text").GetComponent<Text>();
//         evetnTrigger = this.transform.GetComponent<UIEventTrigger>();
//
//         evetnTrigger.OnPointEnterEvent += OnPointEnter;
//         evetnTrigger.OnPointerExitEvent += OnPointExit;
//
//         //icon
//         iconImg = this.transform.Find("icon").GetComponent<Image>();
//     }
//
//     public override void OnRefresh(object data, int index)
//     {
//         this.uiData = (BattleAttrUIData)data;
//
//         nameText.text = "" + this.uiData.name;
//
//         valueText.text = "" + this.uiData.value;
//         if (this.uiData.valueShowType == AttrValueShowType.Int)
//         {
//             valueText.text = "" + (int)this.uiData.value;
//         }
//         else if (this.uiData.valueShowType == AttrValueShowType.Float_2)
//         {
//             valueText.text = string.Format("{0:F}", this.uiData.value);
//         }
//
//         var iconResId = this.uiData.iconResId;
//         //TODO：注意 iconResId
//         ResourceManager.Instance.GetObject<Sprite>(iconResId, (sprite) => { this.iconImg.sprite = sprite; });
//     }
//
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
//         var battleUIRect = parentObj.BattleUIPre.gameObject.GetComponent<RectTransform>();
//         RectTransformUtility.ScreenPointToLocalPointInRectangle(battleUIRect, screenPos, cameraUI.camera, out uiPos);
//
//         EventDispatcher.Broadcast<EntityAttrType, Vector2>(EventIDs.On_UIAttrOption_PointEnter, this.uiData.type,
//             uiPos);
//     }
//
//     public void OnPointExit(PointerEventData e)
//     {
//         EventDispatcher.Broadcast<EntityAttrType>(EventIDs.On_UIAttrOption_PointExit, this.uiData.type);
//     }
//
//
//     public override void OnRelease()
//     {
//         evetnTrigger.OnPointEnterEvent -= OnPointEnter;
//         evetnTrigger.OnPointerExitEvent -= OnPointExit;
//     }
// }