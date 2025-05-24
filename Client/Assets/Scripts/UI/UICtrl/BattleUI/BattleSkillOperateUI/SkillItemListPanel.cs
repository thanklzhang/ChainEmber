// using System;
// using System.Collections;
// using System.Collections.Generic;
// using Battle_Client;
// //using Newtonsoft.Json.Bson;
//
// using UnityEngine;
// using UnityEngine.UI;
//
// namespace Battle_Client.BattleSkillOperate
// {
//     //技能书道具列表部分
//     public class SkillItemListPanel
//     {
//         public GameObject gameObject;
//         public Transform transform;
//
//         public Transform skillItemRoot;
//         public List<SkillItemCell> skillCellList;
//         public SkillItemCell currSelectSkillCell;
//
//         public BattleUI battleUI;
//         public void Init(GameObject gameObject, BattleUI battleUI)
//         {
//             this.battleUI = battleUI;
//             
//             this.gameObject = gameObject;
//             this.transform = this.gameObject.transform;
//
//             skillCellList = new List<SkillItemCell>();
//
//             skillItemRoot = transform.Find("scroll/mask/content");
//             
//            
//         }
//
//
//         public void Show()
//         {
//             EventDispatcher.AddListener< BattleItemInfo>(EventIDs.OnSkillItemInfoUpdate, OnItemInfoUpdate);
//             
//             this.gameObject.SetActive(true);
//             
//             RefreshAllUI();
//         }
//
//         //刷新全部
//         public void RefreshAllUI()
//         {
//             //find dataList
//             var items = BattleManager.Instance.GetLocalCtrlHeroSkillItems();
//
//             //fill skillCellList , show by dataList
//             int maxCount = 50;
//             for (int i = 0; i < maxCount; i++)
//             {
//                 GameObject go = null;
//                 if (i < skillItemRoot.childCount)
//                 {
//                     go = skillItemRoot.GetChild(i).gameObject;
//                 }
//                 else
//                 {
//                     go = GameObject.Instantiate(skillItemRoot.GetChild(0).gameObject, skillItemRoot, false)
//                         .gameObject;
//                 }
//                 go.SetActive(true);
//
//                 SkillItemCell cell = null;
//                 if (i < skillCellList.Count)
//                 {
//                     cell = skillCellList[i];
//                 }
//                 else
//                 {
//                     cell = new SkillItemCell();
//                     skillCellList.Add(cell);
//                     cell.Init(go, this);
//                 }
//
//                 var data = items.Find(item => item.index == i);
//
//                 cell.RefreshUI(data);
//             }
//
//           
//             
//             for (int i = skillItemRoot.childCount - 1; i >= maxCount; i--)
//             {
//                 if (i < skillCellList.Count)
//                 {
//                     var cell = skillCellList[i];
//                     cell.Hide();
//                     
//                     skillCellList.RemoveAt(i);
//                 }
//                 else
//                 {
//                     skillItemRoot.GetChild(i).gameObject.SetActive(false);
//                 }
//             }
//             
//             //List<SkillItemCell> delList = new List<SkillItemCell>();
//             // for (int i = maxCount; i < skillItemRoot.childCount; i++)
//             // {
//             //     if (i < skillCellList.Count)
//             //     {
//             //         var cell = skillCellList[i];
//             //         cell.Hide();
//             //
//             //         delList.Add(cell);
//             //     }
//             //     else
//             //     {
//             //         skillItemRoot.GetChild(i).gameObject.SetActive(false);
//             //     }
//             // }
//             //
//             // for (int i = 0; i < delList.Count; i++)
//             // {
//             //     skillCellList.Remove(delList[i]);
//             // }
//         }
//
//         public void OnItemInfoUpdate(BattleItemInfo itemInfo)
//         {
//             var myEntityGuid = BattleManager.Instance.GetLocalCtrlHeroGuid();
//
//             if (myEntityGuid == itemInfo.ownerGuid)
//             {
//                 var findCell = skillCellList.Find(cell => cell.Index == itemInfo.index);
//                 if (itemInfo.count <= 0)
//                 {
//                     findCell.RefreshUI(null);
//                 }
//                 else
//                 {
//                     findCell.RefreshUI(itemInfo);
//                 }
//             }
//         }
//
//         public void UpdateSkill(int index, int configId, int count = 1)
//         {
//             //find skillCell
//
//             //set skill
//         }
//
//         public void Update(float deltaTime)
//         {
//         }
//
//         public void OnClicktOne(SkillItemCell cell)
//         {
//            
//             // for (int i = 0; i < skillCellList.Count; i++)
//             // {
//             //     var currCell = skillCellList[i];
//             //     currCell.CancelSelect();
//             // }
//             
//             currSelectSkillCell?.CancelSelect();
//             
//             currSelectSkillCell = cell;
//             
//             currSelectSkillCell.Select();
//         }
//
//         public SkillItemCell GetCurrSelect()
//         {
//             return currSelectSkillCell;
//         }
//
//         public void Hide()
//         {
//             EventDispatcher.RemoveListener< BattleItemInfo>(EventIDs.OnSkillItemInfoUpdate, OnItemInfoUpdate);
//             this.gameObject.SetActive(false);
//         }
//
//         public void Release()
//         {
//             
//         }
//     }
// }