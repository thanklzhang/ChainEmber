using System;
using UnityEngine;
using ServerSimulation.Account.Models;

namespace ServerSimulation.Services
{
    /// <summary>
    /// 账号服务类，作为UI与账号系统的中间层接口
    /// </summary>
    public class AccountService
    {
        private static AccountService instance;
        public static AccountService Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AccountService();
                }
                return instance;
            }
        }

        private AccountSystem AccountSystem => ServerSimulationManager.Instance.AccountSystem;

        /// <summary>
        /// 获取当前账号
        /// </summary>
        public UserAccount GetCurrentAccount()
        {
            return AccountSystem.GetCurrentAccount();
        }
        
        /// <summary>
        /// 获取当前玩家数据
        /// </summary>
        public UserPlayer GetCurrentPlayer()
        {
            return AccountSystem.GetCurrentPlayer();
        }
        
        /// <summary>
        /// 更新玩家数据
        /// </summary>
        public bool UpdateUserPlayer(UserPlayer player)
        {
            return AccountSystem.UpdateUserPlayer(player);
        }
        
        // /// <summary>
        // /// 检查是否有本地用户
        // /// </summary>
        // public bool HasLocalUser()
        // {
        //     return AccountSystem.HasLocalUser();
        // }
        //
        /// <summary>
        /// 清除当前用户
        /// </summary>
        public void ClearCurrentUser()
        {
            AccountSystem.ClearAccount();
        }
        
        /// <summary>
        /// 更新昵称
        /// </summary>
        public bool UpdateNickname(string newNickname)
        {
            var player = GetCurrentPlayer();
            if (player == null)
            {
                Debug.LogError("[AccountService] 更新昵称失败，未找到当前玩家数据");
                return false;
            }
            
            player.Nickname = newNickname;
            player.LastUpdateTime = DateTime.Now;
            
            return UpdateUserPlayer(player);
        }
        
        /// <summary>
        /// 更新头像
        /// </summary>
        public bool UpdateAvatar(string avatarId)
        {
            var player = GetCurrentPlayer();
            if (player == null)
            {
                Debug.LogError("[AccountService] 更新头像失败，未找到当前玩家数据");
                return false;
            }
            
            player.AvatarId = avatarId;
            player.LastUpdateTime = DateTime.Now;
            
            return UpdateUserPlayer(player);
        }
        
        /// <summary>
        /// 增加经验
        /// </summary>
        public bool AddExperience(int expAmount)
        {
            var player = GetCurrentPlayer();
            if (player == null)
            {
                Debug.LogError("[AccountService] 增加经验失败，未找到当前玩家数据");
                return false;
            }
            
            player.Experience += expAmount;
            
            // 简单的升级逻辑
            bool leveledUp = false;
            while (player.Experience >= GetExpRequiredForLevel(player.Level + 1))
            {
                player.Level += 1;
                leveledUp = true;
                Debug.Log($"[AccountService] 用户 {player.Nickname} 升级到 {player.Level}!");
            }
            
            player.LastUpdateTime = DateTime.Now;
            UpdateUserPlayer(player);
            
            return leveledUp;
        }
        
        private int GetExpRequiredForLevel(int level)
        {
            // 简单的经验值计算公式，可以根据需要调整
            return level * 100;
        }
    }
} 