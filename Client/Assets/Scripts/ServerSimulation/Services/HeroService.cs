// using System;
// using System.Collections.Generic;
// using System.Linq;
// using UnityEngine;
// using ServerSimulation.Account.Models;
// using GameData;
//
// namespace ServerSimulation.Services
// {
//     /// <summary>
//     /// 英雄服务类，提供英雄相关的功能
//     /// </summary>
//     public class HeroService
//     {
//         private static HeroService instance;
//         public static HeroService Instance
//         {
//             get
//             {
//                 if (instance == null)
//                 {
//                     instance = new HeroService();
//                 }
//                 return instance;
//             }
//         }
//
//         private AccountSystem AccountSystem => ServerSimulationManager.Instance.AccountSystem;
//         
//         /// <summary>
//         /// 获取玩家拥有的所有英雄
//         /// </summary>
//         public List<UserHero> GetAllHeroes()
//         {
//             var player = AccountSystem.GetCurrentPlayer();
//             if (player == null)
//             {
//                 Debug.LogError("[HeroService] 获取英雄列表失败，未找到当前玩家数据");
//                 return new List<UserHero>();
//             }
//             
//             return player.Heroes ?? new List<UserHero>();
//         }
//         
//         /// <summary>
//         /// 获取指定ID的英雄
//         /// </summary>
//         public UserHero GetHero(int heroId)
//         {
//             var heroes = GetAllHeroes();
//             return heroes.FirstOrDefault(h => h.Guid == heroId);
//         }
//         
//         /// <summary>
//         /// 为玩家添加一个新英雄
//         /// </summary>
//         public void AddHero(int configId, Action<UserHero> callback, int level = 1, int star = 1)
//         {
//             // 模拟网络请求延迟
//            // ServerSimulation.Timer.Instance.StartTimer(0.2f, () => {
//                 // 调用AccountSystem的方法添加英雄
//                 var newHero = AccountSystem.AddHero(configId, level, star);
//                 
//                 if (newHero != null)
//                 {
//                     Debug.Log($"[HeroService] 玩家获得了新英雄，配置ID: {configId}，唯一ID: {newHero.Guid}");
//                     
//                     // 同步更新客户端数据
//                     SyncHeroDataToClient(newHero);
//                     
//                     // 调用回调通知操作完成
//                     callback?.Invoke(newHero);
//                 }
//                 else
//                 {
//                     Debug.LogError($"[HeroService] 添加英雄失败，配置ID: {configId}");
//                     callback?.Invoke(null);
//                 }
//             //});
//         }
//         
//         /// <summary>
//         /// 升级英雄
//         /// </summary>
//         public void LevelUpHero(int heroId, Action<bool, UserHero> callback)
//         {
//             // 模拟网络请求延迟
//             //ServerSimulation.Timer.Instance.StartTimer(0.2f, () => {
//                 // 获取升级前的英雄数据，用于日志
//                 var heroBeforeUpgrade = GetHero(heroId);
//                 int oldLevel = heroBeforeUpgrade?.Level ?? 0;
//                 
//                 // 调用AccountSystem的方法升级英雄
//                 bool success = AccountSystem.LevelUpHero(heroId);
//                 
//                 if (success)
//                 {
//                     // 获取升级后的英雄数据
//                     var updatedHero = GetHero(heroId);
//                     Debug.Log($"[HeroService] 英雄(ID: {heroId}, 配置ID: {updatedHero.ConfigId})升级从{oldLevel}到{updatedHero.Level}级");
//                     
//                     // 同步更新客户端数据
//                     SyncHeroDataToClient(updatedHero);
//                     
//                     // 调用回调通知操作完成
//                     callback?.Invoke(true, updatedHero);
//                 }
//                 else
//                 {
//                     Debug.LogError($"[HeroService] 升级英雄失败，ID: {heroId}");
//                     callback?.Invoke(false, null);
//                 }
//             //});
//         }
//         
//         /// <summary>
//         /// 英雄升星
//         /// </summary>
//         public void UpgradeHeroStar(int heroId, Action<bool, UserHero> callback)
//         {
//             // 模拟网络请求延迟
//             //ServerSimulation.Timer.Instance.StartTimer(0.2f, () => {
//                 // 获取升星前的英雄数据，用于日志
//                 var heroBeforeUpgrade = GetHero(heroId);
//                 int oldStar = heroBeforeUpgrade?.Star ?? 0;
//                 
//                 // 调用AccountSystem的方法升级英雄星级
//                 bool success = AccountSystem.UpgradeHeroStar(heroId);
//                 
//                 if (success)
//                 {
//                     // 获取升星后的英雄数据
//                     var updatedHero = GetHero(heroId);
//                     Debug.Log($"[HeroService] 英雄(ID: {heroId}, 配置ID: {updatedHero.ConfigId})升星从{oldStar}到{updatedHero.Star}星");
//                     
//                     // 同步更新客户端数据
//                     SyncHeroDataToClient(updatedHero);
//                     
//                     // 调用回调通知操作完成
//                     callback?.Invoke(true, updatedHero);
//                 }
//                 else
//                 {
//                     Debug.LogError($"[HeroService] 英雄升星失败，ID: {heroId}");
//                     callback?.Invoke(false, null);
//                 }
//             //});
//         }
//         
//         /// <summary>
//         /// 移除英雄
//         /// </summary>
//         public void RemoveHero(int heroId, Action<bool> callback)
//         {
//             // 模拟网络请求延迟
//             //ServerSimulation.Timer.Instance.StartTimer(0.2f, () => {
//                 // 获取英雄信息用于日志
//                 var hero = GetHero(heroId);
//                 int configId = hero?.ConfigId ?? 0;
//                 
//                 // 调用AccountSystem的方法移除英雄
//                 bool success = AccountSystem.RemoveHero(heroId);
//                 
//                 if (success)
//                 {
//                     Debug.Log($"[HeroService] 已移除英雄(ID: {heroId}, 配置ID: {configId})");
//                     
//                     // 同步更新客户端数据（移除英雄）
//                     SyncHeroesListToClient();
//                     
//                     // 调用回调通知操作完成
//                     callback?.Invoke(true);
//                 }
//                 else
//                 {
//                     Debug.LogError($"[HeroService] 移除英雄失败，ID: {heroId}");
//                     callback?.Invoke(false);
//                 }
//             //});
//         }
//         
//         /// <summary>
//         /// 同步单个英雄数据到客户端
//         /// </summary>
//         private void SyncHeroDataToClient(UserHero userHero)
//         {
//             if (userHero == null)
//             {
//                 Debug.LogError("[HeroService] 同步英雄数据失败，英雄数据为空");
//                 return;
//             }
//             
//             // 获取客户端的HeroGameData
//             var heroGameData = GameDataManager.Instance.HeroData;
//             if (heroGameData == null)
//             {
//                 Debug.LogError("[HeroService] 同步英雄数据失败，HeroData为空");
//                 return;
//             }
//             
//             // 创建客户端英雄数据
//             HeroData clientHeroData = new HeroData
//             {
//                 guid = userHero.Guid,
//                 configId = userHero.ConfigId,
//                 level = userHero.Level
//             };
//             
//             // 更新或添加英雄数据
//             var existingHero = heroGameData.GetDataByGuid(clientHeroData.guid);
//             if (existingHero != null)
//             {
//                 // 更新现有英雄数据
//                 existingHero.configId = clientHeroData.configId;
//                 existingHero.level = clientHeroData.level;
//             }
//             else
//             {
//                 // 添加新英雄数据
//                 heroGameData.HeroList.Add(clientHeroData);
//                 
//                 // 更新字典
//                 if (!heroGameData.HeroDic.ContainsKey(clientHeroData.guid))
//                 {
//                     heroGameData.HeroDic.Add(clientHeroData.guid, clientHeroData);
//                 }
//             }
//             
//             // 触发英雄数据更新事件
//             EventDispatcher.Broadcast(EventIDs.OnRefreshHeroListData);
//             
//             Debug.Log($"[HeroService] 已同步英雄数据到客户端: ID={userHero.Guid}, 等级={userHero.Level}");
//         }
//         
//         /// <summary>
//         /// 同步所有英雄列表到客户端
//         /// </summary>
//         private void SyncHeroesListToClient()
//         {
//             var player = AccountSystem.GetCurrentPlayer();
//             if (player == null || player.Heroes == null)
//             {
//                 Debug.LogError("[HeroService] 同步英雄列表失败，未找到当前玩家数据");
//                 return;
//             }
//             
//             // 获取客户端的HeroGameData
//             var heroGameData = GameDataManager.Instance.HeroData;
//             if (heroGameData == null)
//             {
//                 Debug.LogError("[HeroService] 同步英雄列表失败，HeroData为空");
//                 return;
//             }
//             
//             // 使用HeroGameData封装的方法同步数据
//             heroGameData.SyncHeroDataFromBackend(player.Heroes);
//             
//             Debug.Log($"[HeroService] 已同步 {player.Heroes.Count} 个英雄数据到客户端");
//         }
//     }
// } 