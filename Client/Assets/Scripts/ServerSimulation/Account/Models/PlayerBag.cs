// using System;
// using System.Collections.Generic;
// using UnityEngine;
//
// namespace ServerSimulation.Account.Models
// {
//     [Serializable]
//     public class PlayerBagItem
//     {
//         public int itemId;
//         public int count;
//     }
//
//     [Serializable]
//     public class PlayerBag
//     {
//         // 货币列表，itemId为货币类型
//         public List<PlayerBagItem> ItemList = new List<PlayerBagItem>();
//
//         public const int GOLD_ID = 22000001;
//
//         /// <summary>
//         /// 增加货币
//         /// </summary>
//         public void AddItem(int itemId, int amount)
//         {
//             var item = ItemList.Find(x => x.itemId == itemId);
//             if (item != null)
//                 item.count += amount;
//             else
//                 ItemList.Add(new PlayerBagItem { itemId = itemId, count = amount });
//         }
//
//         /// <summary>
//         /// 消耗货币，返回是否成功
//         /// </summary>
//         public bool ConsumeItem(int itemId, int amount)
//         {
//             var item = ItemList.Find(x => x.itemId == itemId);
//             if (item != null && item.count >= amount)
//             {
//                 item.count -= amount;
//                 return true;
//             }
//             return false;
//         }
//
//         /// <summary>
//         /// 查询货币数量
//         /// </summary>
//         public int GetItemCount(int itemId)
//         {
//             var item = ItemList.Find(x => x.itemId == itemId);
//             return item != null ? item.count : 0;
//         }
//
//         // 单独为金币提供的接口
//         public void AddGold(int amount)
//         {
//             AddItem(GOLD_ID, amount);
//         }
//         public bool ConsumeGold(int amount)
//         {
//             return ConsumeItem(GOLD_ID, amount);
//         }
//         public int GetGold()
//         {
//             return GetItemCount(GOLD_ID);
//         }
//     }
// } 