// using System;
// using System.Collections;
// using System.Collections.Generic;
// using Battle_Client;
// using Config;
// using UnityEngine;
// using UnityEngine.EventSystems;
// using UnityEngine.UI;
//
// namespace Battle_Client.BattleSkillOperate
// {
//     //技能书道具格子
//     public class SkillItemCell
//     {
//         public GameObject gameObject;
//         public Transform transform;
//
//         public Image icon;
//         public Text count;
//         // public int index = -1;
//         private bool isSelect;
//
//         protected UIEventTrigger evetnTrigger;
//         private SkillItemListPanel parentUI;
//
//         private GameObject selectFlagGo;
//         private Button iconBtn;
//         
//         public void Init(GameObject gameObject, SkillItemListPanel parentUI)
//         {
//             this.parentUI = parentUI;
//             this.gameObject = gameObject;
//             this.transform = this.gameObject.transform;
//
//             selectFlagGo = transform.Find("select").gameObject;
//             iconBtn = transform.Find("icon").GetComponent<Button>();
//             
//             iconBtn.onClick.RemoveAllListeners();
//             iconBtn.onClick.AddListener(OnClick);
//
//             icon = transform.Find("icon").GetComponent<Image>();
//             evetnTrigger = icon.GetComponent<UIEventTrigger>();
//             
//             evetnTrigger.OnPointEnterEvent += OnPointEnter;
//             evetnTrigger.OnPointerExitEvent += OnPointExit;
//         }
//
//
//         public void UpdateSkill(int skillConfigId, int count = 1)
//         {
//             //change skill
//
//             //show
//         }
//
//         public void Show()
//         {
//             this.gameObject.SetActive(true);
//         }
//
//         private void OnClick()
//         {
//             parentUI.OnClicktOne(this);
//         }
//
//         public void Select()
//         {
//             this.isSelect = true;
//             selectFlagGo.SetActive(this.isSelect);
//         }
//
//         public void CancelSelect()
//         {
//             this.isSelect = false;
//             selectFlagGo.SetActive(this.isSelect);
//         }
//
//         public int Index
//         {
//             get
//             {
//                 if (uiData != null)
//                 {
//                     return uiData.index;
//                 }
//                 return -1;
//             }
//         }
//         private BattleItemInfo uiData;
//         public void RefreshUI(BattleItemInfo itemInfo)
//         {
//             uiData = itemInfo;
//             //show by data
//             //by skillConfigId
//
//             bool isHaveSkill = itemInfo != null;
//             if (isHaveSkill)
//             {
//                 var config = ConfigManager.Instance.GetById<Config.BattleItem>(itemInfo.configId);
//                 ResourceManager.Instance.GetObject<Sprite>(config.IconResId, (sprite) => { icon.sprite = sprite; });
//                 icon.gameObject.SetActive(true);
//                 //this.Show();
//             }
//             else
//             {
//                 icon.gameObject.SetActive(false);
//                 //this.Hide();
//             }
//         }
//
//         public void Update(float deltaTime)
//         {
//         }
//
//         public void OnPointEnter(PointerEventData e)
//         {
//             //转换成点在 BattleUI 中的 localPosition
//
//             var camera3D = CameraManager.Instance.GetCamera3D();
//             var cameraUI = CameraManager.Instance.GetCameraUI();
//
//             var screenPos = e.position;
//
//             Vector2 uiPos;
//             var battleUIRect = parentUI.battleUI.gameObject.GetComponent<RectTransform>();
//             RectTransformUtility.ScreenPointToLocalPointInRectangle(battleUIRect, screenPos, cameraUI.camera, out uiPos);
//
//             EventDispatcher.Broadcast<int, Vector2>(EventIDs.On_UISkillItemOption_PointEnter, this.uiData.configId, uiPos);
//         }
//
//         public void OnPointExit(PointerEventData e)
//         {
//             EventDispatcher.Broadcast<int>(EventIDs.On_UISkillItemOption_PointExit, this.uiData.configId);
//         }
//         public void Hide()
//         {
//             this.gameObject.SetActive(false);
//         }
//
//         public void Release()
//         {
//         }
//     }
// }