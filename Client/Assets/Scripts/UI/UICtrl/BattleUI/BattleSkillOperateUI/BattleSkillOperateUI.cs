// using System;
// using System.Collections;
// using System.Collections.Generic;
// using Battle_Client;
//
// using UnityEngine;
// using UnityEngine.UI;
//
// namespace Battle_Client.BattleSkillOperate
// {
//     //技能操作界面（技能书列表 学习技能 合成技能）
//     public class BattleSkillOperateUI
//     {
//         public GameObject gameObject;
//         public Transform transform;
//
//         private SkillInputSelectPanel selectPanel;
//         private SkillItemListPanel skillListPanel;
//         private SkillItemShopPanel shopPanel;
//
//         private Button learnBtn;
//         private Button saleBtn;
//         private Button closeBtn;
//         private BattleUI battleUI;
//
//         public void Init(GameObject gameObject, BattleUI battleUI)
//         {
//             this.battleUI = battleUI;
//             this.gameObject = gameObject;
//             this.transform = this.gameObject.transform;
//
//             selectPanel = new SkillInputSelectPanel();
//             skillListPanel = new SkillItemListPanel();
//             shopPanel = new SkillItemShopPanel();
//
//             // selectPanel.Init(null, null);
//             var skillListRoot = transform.Find("root/skillRoot");
//             skillListPanel.Init(skillListRoot.gameObject, this.battleUI);
//             // shopPanel.Init(null, null);
//
//             learnBtn = transform.Find("root/learnBtn").GetComponent<Button>();
//             saleBtn = transform.Find("root/saleBtn").GetComponent<Button>();
//             closeBtn = transform.Find("root/closeBtn").GetComponent<Button>();
//
//             learnBtn.onClick.AddListener(OnClickLearnBtn);
//             saleBtn.onClick.AddListener(OnClickSaleBtn);
//             closeBtn.onClick.AddListener(OnClickCloseBtn);
//         }
//
//         void OnClickLearnBtn()
//         {
//             var inputSelect = selectPanel.GetCurrSelect();
//             var selectSkill = skillListPanel.GetCurrSelect();
//
//             //send net
//             ItemUseArg_Client arg = new ItemUseArg_Client()
//             {
//                 itemIndex = selectSkill.Index,
//                 releaserGuid = BattleManager.Instance.GetLocalCtrlHeroGuid()
//             };
//
//
//             BattleManager.Instance.MsgSender.Send_UseSkillItem(arg);
//         }
//
//         void OnClickSaleBtn()
//         {
//         }
//
//         void OnClickCloseBtn()
//         {
//             this.Hide();
//         }
//
//         public void Show()
//         {
//             this.gameObject.SetActive(true);
//             this.skillListPanel.Show();
//         }
//
//         public void RefreshAllUI()
//         {
//             // skillListPanel.RefreshAllUI();
//         }
//
//         public void RefreshDataList()
//         {
//         }
//
//         void RefreshSkillShowList()
//         {
//         }
//
//         public void Update(float deltaTime)
//         {
//         }
//
//         public void Hide()
//         {
//             this.gameObject.SetActive(false);
//             this.skillListPanel.Hide();
//         }
//
//         public void Release()
//         {
//             skillListPanel.Release();
//         }
//     }
// }