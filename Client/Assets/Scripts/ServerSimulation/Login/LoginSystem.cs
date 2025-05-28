using System;
using ServerSimulation.Account;
using ServerSimulation.Account.Models;
using UnityEngine;

namespace ServerSimulation
{
    public class LoginResponse
    {
        public bool Success;
        public string UserId;
        public string Username;
        public UserPlayer UserPlayer;
        public string ErrorMessage;
        public DateTime LoginTime;
    }

    public class LoginSystem
    {
        private AccountSystem accountSystem;

        public LoginSystem(AccountSystem accountSystem)
        {
            this.accountSystem = accountSystem;
        }

        /// <summary>
        /// 登录（如果后台没有对应账号则会自动注册）
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <returns>登录结果</returns>
        public LoginResponse Login(string username, string password)
        {
            Debug.Log($"[LoginSystem] 尝试登录: {username}");
            
            // 获取当前账户
            var account = accountSystem.GetCurrentAccount();
            
            // 有后台用户且用户名匹配，尝试登录
            if (account != null && account.Username == username)
            {
                Debug.Log($"[LoginSystem] 登录类型: 已有用户");
                // 如果密码为空，说明是自动登录，跳过密码检查
                if (!string.IsNullOrEmpty(password) && account.Password != password)
                {
                    return new LoginResponse
                    {
                        Success = false,
                        ErrorMessage = "密码错误"
                    };
                }
                
                // 更新最后登录时间
                account.LastLoginDate = DateTime.Now;
                accountSystem.SaveAccount();
                
                // 获取玩家数据
                var userPlayer = accountSystem.GetCurrentPlayer();
                
                return new LoginResponse
                {
                    Success = true,
                    UserId = account.UserId,
                    Username = account.Username,
                    UserPlayer = userPlayer,
                    LoginTime = DateTime.Now
                };
            }
            else
            {
                Debug.Log($"[LoginSystem] 登录类型: 新用户（自动注册）");
                // 没有后台用户或用户名不匹配，自动注册并登录
                return Register(username, password);
            }
            
            
        }
        
        /// <summary>
        /// 注册新账户
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <returns>注册结果</returns>
        public LoginResponse Register(string username, string password)
        {
            Debug.Log($"[LoginSystem] 尝试注册新账号: {username}");
            
            if (string.IsNullOrEmpty(username) || username.Length < 3)
            {
                return new LoginResponse
                {
                    Success = false,
                    ErrorMessage = "用户名长度不能少于3个字符"
                };
            }
            
            if (string.IsNullOrEmpty(password))
            {
                // 如果密码为空，使用默认密码
                password = "default";
            }
            else if (password.Length < 6)
            {
                return new LoginResponse
                {
                    Success = false,
                    ErrorMessage = "密码长度不能少于6个字符"
                };
            }
            
            var account = accountSystem.CreateOrUpdateAccount(username, password);
            var userPlayer = accountSystem.GetCurrentPlayer();
            
            // 为新用户添加初始英雄
            AddInitialHeroes(userPlayer);
            
            // 保存更新后的玩家数据
            accountSystem.UpdateUserPlayer(userPlayer);
            
            return new LoginResponse
            {
                Success = true,
                UserId = account.UserId,
                Username = account.Username,
                UserPlayer = userPlayer,
                LoginTime = DateTime.Now
            };
        }
        
        /// <summary>
        /// 为新玩家添加初始英雄
        /// </summary>
        private void AddInitialHeroes(UserPlayer player)
        {
            if (player == null)
            {
                Debug.LogError("[LoginSystem] 添加初始英雄失败，玩家数据为空");
                return;
            }
            
            // 如果玩家已经有英雄了，则不添加初始英雄
            if (player.Heroes != null && player.Heroes.Count > 0)
            {
                Debug.Log($"[LoginSystem] 玩家 {player.Nickname} 已经有英雄，不添加初始英雄");
                return;
            }
            
            Debug.Log($"[LoginSystem] 为新玩家 {player.Nickname} 添加初始英雄");
            
            // 初始英雄配置ID列表，可以根据游戏需求自行配置
            int[] initialHeroIds = { 1000001,1000002,1000003,1000007 }; // 假设1001, 1002是初始英雄的配置ID
            
            // 确保Heroes列表已初始化
            if (player.Heroes == null)
            {
                player.Heroes = new System.Collections.Generic.List<UserHero>();
            }
            
            // 添加所有初始英雄
            foreach (var configId in initialHeroIds)
            {
                var newHero = new UserHero(configId);
                player.Heroes.Add(newHero);
                Debug.Log($"[LoginSystem] 玩家 {player.Nickname} 获得了新英雄，配置ID: {configId}，唯一ID: {newHero.Guid}");
            }
            
            Debug.Log($"[LoginSystem] 玩家 {player.Nickname} 获得了 {initialHeroIds.Length} 个初始英雄");
        }
        
        /// <summary>
        /// 清除当前用户
        /// </summary>
        public void ClearCurrentUser()
        {
            accountSystem.ClearAccount();
        }
    }
} 