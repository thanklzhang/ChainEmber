// using System;
// using UnityEngine;
// using ServerSimulation.Account.Models;
// using NetProto;
// using GameData;
//
// namespace ServerSimulation.Services
// {
//     /// <summary>
//     /// 账号服务类，作为UI与账号系统的中间层接口
//     /// </summary>
//     public class AccountService
//     {
//         private static AccountService instance;
//         public static AccountService Instance
//         {
//             get
//             {
//                 if (instance == null)
//                 {
//                     instance = new AccountService();
//                 }
//                 return instance;
//             }
//         }
//
//         private AccountSystem AccountSystem => ServerSimulationManager.Instance.AccountSystem;
//
//         /// <summary>
//         /// 获取当前账号
//         /// </summary>
//         public UserAccount GetCurrentAccount()
//         {
//             return AccountSystem.GetCurrentAccount();
//         }
//         
//         /// <summary>
//         /// 获取当前玩家数据
//         /// </summary>
//         public UserPlayer GetCurrentPlayer()
//         {
//             var player = AccountSystem.GetCurrentPlayer();
//             // 每次获取玩家时自动同步所有货币到BagData
//             // SyncAllCurrencyToBag(player);
//             return player;
//         }
//         
//         /// <summary>
//         /// 更新玩家数据
//         /// </summary>
//         public bool UpdateUserPlayer(UserPlayer player)
//         {
//             return AccountSystem.UpdateUserPlayer(player);
//         }
//         
//         // /// <summary>
//         // /// 检查是否有本地用户
//         // /// </summary>
//         // public bool HasLocalUser()
//         // {
//         //     return AccountSystem.HasLocalUser();
//         // }
//         //
//         /// <summary>
//         /// 清除当前用户
//         /// </summary>
//         public void ClearCurrentUser()
//         {
//             AccountSystem.ClearAccount();
//         }
//         
//         /// <summary>
//         /// 更新昵称
//         /// </summary>
//         public bool UpdateNickname(string newNickname)
//         {
//             var player = GetCurrentPlayer();
//             if (player == null)
//             {
//                 Debug.LogError("[AccountService] 更新昵称失败，未找到当前玩家数据");
//                 return false;
//             }
//             
//             player.Nickname = newNickname;
//             player.LastUpdateTime = DateTime.Now;
//             
//             return UpdateUserPlayer(player);
//         }
//         
//         /// <summary>
//         /// 更新头像
//         /// </summary>
//         public bool UpdateAvatar(string avatarId)
//         {
//             var player = GetCurrentPlayer();
//             if (player == null)
//             {
//                 Debug.LogError("[AccountService] 更新头像失败，未找到当前玩家数据");
//                 return false;
//             }
//             
//             player.AvatarId = avatarId;
//             player.LastUpdateTime = DateTime.Now;
//             
//             return UpdateUserPlayer(player);
//         }
//         
//         /// <summary>
//         /// 增加经验
//         /// </summary>
//         public bool AddExperience(int expAmount)
//         {
//             var player = GetCurrentPlayer();
//             if (player == null)
//             {
//                 Debug.LogError("[AccountService] 增加经验失败，未找到当前玩家数据");
//                 return false;
//             }
//             
//             player.Experience += expAmount;
//             
//             // 简单的升级逻辑
//             bool leveledUp = false;
//             while (player.Experience >= GetExpRequiredForLevel(player.Level + 1))
//             {
//                 player.Level += 1;
//                 leveledUp = true;
//                 Debug.Log($"[AccountService] 用户 {player.Nickname} 升级到 {player.Level}!");
//             }
//             
//             player.LastUpdateTime = DateTime.Now;
//             UpdateUserPlayer(player);
//             
//             return leveledUp;
//         }
//         
//         private int GetExpRequiredForLevel(int level)
//         {
//             // 简单的经验值计算公式，可以根据需要调整
//             return level * 100;
//         }
//
//         /// <summary>
//         /// 获取当前玩家金币数量
//         /// </summary>
//         public int GetGold()
//         {
//             var player = GetCurrentPlayer();
//             if (player == null)
//             {
//                 Debug.LogError("[AccountService] 获取金币失败，未找到当前玩家数据");
//                 return 0;
//             }
//             return player.Bag.GetGold();
//         }
//
//         /// <summary>
//         /// 增加金币
//         /// </summary>
//         public bool AddGold(int amount)
//         {
//             var player = GetCurrentPlayer();
//             if (player == null)
//             {
//                 Debug.LogError("[AccountService] 增加金币失败，未找到当前玩家数据");
//                 return false;
//             }
//             player.Bag.AddGold(amount);
//             player.LastUpdateTime = DateTime.Now;
//             var result = UpdateUserPlayer(player);
//             // 同步到BagData
//             SyncCurrencyToBag(PlayerBag.GOLD_ID, player.Bag.GetGold());
//             return result;
//         }
//
//         /// <summary>
//         /// 消耗金币
//         /// </summary>
//         public bool ConsumeGold(int amount)
//         {
//             var player = GetCurrentPlayer();
//             if (player == null)
//             {
//                 Debug.LogError("[AccountService] 消耗金币失败，未找到当前玩家数据");
//                 return false;
//             }
//             var result = player.Bag.ConsumeGold(amount);
//             if (result)
//             {
//                 player.LastUpdateTime = DateTime.Now;
//                 UpdateUserPlayer(player);
//                 // 同步到BagData
//                 SyncCurrencyToBag(PlayerBag.GOLD_ID, player.Bag.GetGold());
//             }
//             return result;
//         }
//
//         /// <summary>
//         /// 同步货币到BagData
//         /// </summary>
//         public void SyncCurrencyToBag(int itemId, int count)
//         {
//             var bagData = GameDataManager.Instance.BagData;
//             if (bagData != null)
//             {
//                 var item = new BagItemData { configId = itemId, count = count };
//                 bagData.UpdateItem(item);
//             }
//         }
//
//         /// <summary>
//         /// 同步所有货币到BagData
//         /// </summary>
//         public void SyncAllCurrencyToBag(UserPlayer player = null)
//         {
//             if (player == null)
//                 player = GetCurrentPlayer();
//             if (player == null || player.Bag == null) return;
//             foreach (var item in player.Bag.ItemList)
//             {
//                 SyncCurrencyToBag(item.itemId, item.count);
//             }
//         }
//     }
// } 