using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ServerSimulation.Account.Models;

namespace ServerSimulation.Services
{
    /// <summary>
    /// 英雄服务类，提供英雄相关的功能
    /// </summary>
    public class HeroService
    {
        private static HeroService instance;
        public static HeroService Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new HeroService();
                }
                return instance;
            }
        }

        private AccountSystem AccountSystem => ServerSimulationManager.Instance.AccountSystem;
        
        /// <summary>
        /// 获取玩家拥有的所有英雄
        /// </summary>
        public List<UserHero> GetAllHeroes()
        {
            var player = AccountSystem.GetCurrentPlayer();
            if (player == null)
            {
                Debug.LogError("[HeroService] 获取英雄列表失败，未找到当前玩家数据");
                return new List<UserHero>();
            }
            
            return player.Heroes ?? new List<UserHero>();
        }
        
        /// <summary>
        /// 获取指定ID的英雄
        /// </summary>
        public UserHero GetHero(int heroId)
        {
            var heroes = GetAllHeroes();
            return heroes.FirstOrDefault(h => h.Guid == heroId);
        }
        
        /// <summary>
        /// 为玩家添加一个新英雄
        /// </summary>
        public UserHero AddHero(int configId, int level = 1, int star = 1)
        {
            var player = AccountSystem.GetCurrentPlayer();
            if (player == null)
            {
                Debug.LogError("[HeroService] 添加英雄失败，未找到当前玩家数据");
                return null;
            }
            
            var newHero = new UserHero(configId, level, star);
            if (player.Heroes == null)
            {
                player.Heroes = new List<UserHero>();
            }
            
            player.Heroes.Add(newHero);
            player.LastUpdateTime = DateTime.Now;
            AccountSystem.UpdateUserPlayer(player);
            
            Debug.Log($"[HeroService] 玩家 {player.Nickname} 获得了新英雄，配置ID: {configId}，唯一ID: {newHero.Guid}");
            
            return newHero;
        }
        
        /// <summary>
        /// 升级英雄
        /// </summary>
        public bool LevelUpHero(int heroId)
        {
            var player = AccountSystem.GetCurrentPlayer();
            if (player == null || player.Heroes == null)
            {
                Debug.LogError("[HeroService] 升级英雄失败，未找到当前玩家数据或英雄列表");
                return false;
            }
            
            var hero = player.Heroes.FirstOrDefault(h => h.Guid == heroId);
            if (hero == null)
            {
                Debug.LogError($"[HeroService] 升级英雄失败，未找到英雄ID: {heroId}");
                return false;
            }
            
            hero.Level += 1;
            hero.Experience = 0; // 升级后经验清零
            
            player.LastUpdateTime = DateTime.Now;
            AccountSystem.UpdateUserPlayer(player);
            
            Debug.Log($"[HeroService] 英雄(ID: {heroId}, 配置ID: {hero.ConfigId})升级到{hero.Level}级");
            
            return true;
        }
        
        /// <summary>
        /// 英雄升星
        /// </summary>
        public bool UpgradeHeroStar(int heroId)
        {
            var player = AccountSystem.GetCurrentPlayer();
            if (player == null || player.Heroes == null)
            {
                Debug.LogError("[HeroService] 英雄升星失败，未找到当前玩家数据或英雄列表");
                return false;
            }
            
            var hero = player.Heroes.FirstOrDefault(h => h.Guid == heroId);
            if (hero == null)
            {
                Debug.LogError($"[HeroService] 英雄升星失败，未找到英雄ID: {heroId}");
                return false;
            }
            
            hero.Star += 1;
            
            player.LastUpdateTime = DateTime.Now;
            AccountSystem.UpdateUserPlayer(player);
            
            Debug.Log($"[HeroService] 英雄(ID: {heroId}, 配置ID: {hero.ConfigId})升到{hero.Star}星");
            
            return true;
        }
        
        /// <summary>
        /// 移除英雄
        /// </summary>
        public bool RemoveHero(int heroId)
        {
            var player = AccountSystem.GetCurrentPlayer();
            if (player == null || player.Heroes == null)
            {
                Debug.LogError("[HeroService] 移除英雄失败，未找到当前玩家数据或英雄列表");
                return false;
            }
            
            var hero = player.Heroes.FirstOrDefault(h => h.Guid == heroId);
            if (hero == null)
            {
                Debug.LogError($"[HeroService] 移除英雄失败，未找到英雄ID: {heroId}");
                return false;
            }
            
            player.Heroes.Remove(hero);
            player.LastUpdateTime = DateTime.Now;
            AccountSystem.UpdateUserPlayer(player);
            
            Debug.Log($"[HeroService] 已移除英雄(ID: {heroId}, 配置ID: {hero.ConfigId})");
            
            return true;
        }
    }
} 