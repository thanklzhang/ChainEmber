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
        /// 清除当前用户
        /// </summary>
        public void ClearCurrentUser()
        {
            accountSystem.ClearAccount();
        }
    }
} 