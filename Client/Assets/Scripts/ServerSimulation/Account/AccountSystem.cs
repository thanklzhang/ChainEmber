// using System.Collections.Generic;
// using ServerSimulation.Account.Models;
// using UnityEngine;
// using System;
// using System.Linq;
// using GameData;
//
// namespace ServerSimulation
// {
//     [Serializable]
//     public class AccountSystemData
//     {
//         public UserAccount CurrentAccount;
//         public UserPlayer CurrentPlayer;
//     }
//     
//     public class AccountSystem
//     {
//         private DataStorage dataStorage;
//         private UserAccount currentAccount;
//         private UserPlayer currentPlayer;
//         
//         // 存储账号数据的键名
//         private readonly string ACCOUNT_DATA_KEY = "account_system_data";
//
//         public AccountSystem(DataStorage dataStorage)
//         {
//             this.dataStorage = dataStorage;
//             LoadAccount();
//         }
//
//         // 创建或更新当前账号
//         public UserAccount CreateOrUpdateAccount(string username, string password)
//         {
//             var account = new UserAccount(username, password);
//             currentAccount = account;
//             
//             // 如果没有玩家数据，同时创建
//             if (currentPlayer == null)
//             {
//                 currentPlayer = new UserPlayer(account.UserId);
//             }
//             else
//             {
//                 // 更新玩家数据的用户ID
//                 currentPlayer.UserId = account.UserId;
//             }
//             
//             SaveAccount();
//             return account;
//         }
//
//         // 获取当前账号
//         public UserAccount GetCurrentAccount()
//         {
//             return currentAccount;
//         }
//         
//         // 获取当前玩家数据
//         public UserPlayer GetCurrentPlayer()
//         {
//             return currentPlayer;
//         }
//         
//         // 更新玩家数据
//         public bool UpdateUserPlayer(UserPlayer player)
//         {
//             if (player == null)
//             {
//                 Debug.LogWarning("[AccountSystem] 更新玩家数据失败，玩家数据为空");
//                 return false;
//             }
//             
//             player.LastUpdateTime = DateTime.Now;
//             currentPlayer = player;
//             SaveAccount();
//             return true;
//         }
//         
//         /// <summary>
//         /// 升级英雄等级
//         /// </summary>
//         /// <param name="heroId">英雄唯一ID</param>
//         /// <returns>操作是否成功</returns>
//         public bool LevelUpHero(int heroId)
//         {
//             if (currentPlayer == null || currentPlayer.Heroes == null)
//             {
//                 Debug.LogError("[AccountSystem] 升级英雄失败，未找到当前玩家数据或英雄列表");
//                 return false;
//             }
//             
//             var hero = currentPlayer.Heroes.FirstOrDefault(h => h.Guid == heroId);
//             if (hero == null)
//             {
//                 Debug.LogError($"[AccountSystem] 升级英雄失败，未找到英雄ID: {heroId}");
//                 return false;
//             }
//             
//             hero.Level += 1;
//             hero.Experience = 0; // 升级后经验清零
//             
//             currentPlayer.LastUpdateTime = DateTime.Now;
//             SaveAccount();
//             
//             Debug.Log($"[AccountSystem] 英雄(ID: {heroId}, 配置ID: {hero.ConfigId})升级到{hero.Level}级");
//             
//             return true;
//         }
//         
//         /// <summary>
//         /// 升级英雄星级
//         /// </summary>
//         /// <param name="heroId">英雄唯一ID</param>
//         /// <returns>操作是否成功</returns>
//         public bool UpgradeHeroStar(int heroId)
//         {
//             if (currentPlayer == null || currentPlayer.Heroes == null)
//             {
//                 Debug.LogError("[AccountSystem] 英雄升星失败，未找到当前玩家数据或英雄列表");
//                 return false;
//             }
//             
//             var hero = currentPlayer.Heroes.FirstOrDefault(h => h.Guid == heroId);
//             if (hero == null)
//             {
//                 Debug.LogError($"[AccountSystem] 英雄升星失败，未找到英雄ID: {heroId}");
//                 return false;
//             }
//             
//             hero.Star += 1;
//             
//             currentPlayer.LastUpdateTime = DateTime.Now;
//             SaveAccount();
//             
//             Debug.Log($"[AccountSystem] 英雄(ID: {heroId}, 配置ID: {hero.ConfigId})升到{hero.Star}星");
//             
//             return true;
//         }
//         
//         /// <summary>
//         /// 添加英雄到玩家账户
//         /// </summary>
//         /// <param name="configId">英雄配置ID</param>
//         /// <param name="level">初始等级</param>
//         /// <param name="star">初始星级</param>
//         /// <returns>添加的英雄对象</returns>
//         public UserHero AddHero(int configId, int level = 1, int star = 1)
//         {
//             if (currentPlayer == null)
//             {
//                 Debug.LogError("[AccountSystem] 添加英雄失败，未找到当前玩家数据");
//                 return null;
//             }
//             
//             var newHero = new UserHero(configId, level, star);
//             if (currentPlayer.Heroes == null)
//             {
//                 currentPlayer.Heroes = new List<UserHero>();
//             }
//             
//             currentPlayer.Heroes.Add(newHero);
//             currentPlayer.LastUpdateTime = DateTime.Now;
//             SaveAccount();
//             
//             Debug.Log($"[AccountSystem] 玩家 {currentPlayer.Nickname} 获得了新英雄，配置ID: {configId}，唯一ID: {newHero.Guid}");
//             
//             return newHero;
//         }
//         
//         /// <summary>
//         /// 从玩家账户移除英雄
//         /// </summary>
//         /// <param name="heroId">英雄唯一ID</param>
//         /// <returns>操作是否成功</returns>
//         public bool RemoveHero(int heroId)
//         {
//             if (currentPlayer == null || currentPlayer.Heroes == null)
//             {
//                 Debug.LogError("[AccountSystem] 移除英雄失败，未找到当前玩家数据或英雄列表");
//                 return false;
//             }
//             
//             var hero = currentPlayer.Heroes.FirstOrDefault(h => h.Guid == heroId);
//             if (hero == null)
//             {
//                 Debug.LogError($"[AccountSystem] 移除英雄失败，未找到英雄ID: {heroId}");
//                 return false;
//             }
//             
//             currentPlayer.Heroes.Remove(hero);
//             currentPlayer.LastUpdateTime = DateTime.Now;
//             SaveAccount();
//             
//             Debug.Log($"[AccountSystem] 已移除英雄(ID: {heroId}, 配置ID: {hero.ConfigId})");
//             
//             return true;
//         }
//
//         // 加载账号和玩家数据
//         private void LoadAccount()
//         {
//             var data = dataStorage.LoadData<AccountSystemData>(ACCOUNT_DATA_KEY);
//             if (data != null)
//             {
//                 currentAccount = data.CurrentAccount;
//                 currentPlayer = data.CurrentPlayer;
//                 
//                 Debug.Log($"[AccountSystem] 已加载账号: {currentAccount?.Username}");
//             }
//             else
//             {
//                 currentAccount = null;
//                 currentPlayer = null;
//                 Debug.Log("[AccountSystem] 未找到账号数据");
//             }
//         }
//
//         // 保存账号和玩家数据
//         public void SaveAccount()
//         {
//             var data = new AccountSystemData
//             {
//                 CurrentAccount = currentAccount,
//                 CurrentPlayer = currentPlayer
//             };
//             
//             dataStorage.SaveData(ACCOUNT_DATA_KEY, data);
//             
//             Debug.Log($"[AccountSystem] 已保存账号: {currentAccount?.Username}");
//         }
//         
//         // 清除账号数据
//         public void ClearAccount()
//         {
//             currentAccount = null;
//             currentPlayer = null;
//             dataStorage.DeleteData(ACCOUNT_DATA_KEY);
//             Debug.Log("[AccountSystem] 已清除账号数据");
//         }
//     }
// } 