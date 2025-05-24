using System;
using UnityEngine;
using GameData;
using ServerSimulation.Account.Models;

namespace ServerSimulation.Services
{
    /// <summary>
    /// 登录结果回调
    /// </summary>
    /// <param name="success">是否成功</param>
    /// <param name="userId">用户ID</param>
    /// <param name="errorMessage">错误信息</param>
    public delegate void LoginCallback(bool success, string userId, string errorMessage);
    
    /// <summary>
    /// 注册结果回调
    /// </summary>
    /// <param name="success">是否成功</param>
    /// <param name="userId">用户ID</param>
    /// <param name="errorMessage">错误信息</param>
    public delegate void RegisterCallback(bool success, string userId, string errorMessage);
    
    /// <summary>
    /// 登录服务类，作为UI与登录系统的中间层接口
    /// </summary>
    public class LoginService
    {
        private static LoginService instance;
        public static LoginService Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new LoginService();
                }
                return instance;
            }
        }
        
        private LoginSystem LoginSystem => ServerSimulationManager.Instance.LoginSystem;
        
        /// <summary>
        /// 当前登录的用户ID
        /// </summary>
        public string CurrentUserId { get; private set; }
        
        /// <summary>
        /// 当前是否已登录
        /// </summary>
        public bool IsLoggedIn => !string.IsNullOrEmpty(CurrentUserId);
        
        /// <summary>
        /// 登录（如果后台没有用户会自动注册）
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="callback">登录结果回调</param>
        public void Login(string username, string password, LoginCallback callback)
        {
            Debug.Log($"[LoginService] 尝试登录: {username}");

            // 模拟网络延迟
            Timer.Instance.AddTimer(0.5f, () =>
            {
                var response = LoginSystem.Login(username, password);
                
                if (response.Success)
                {
                    // 登录成功，设置当前用户ID
                    CurrentUserId = response.UserId;
                    
                    // 更新游戏数据
                    UpdateGameDataAfterLogin(response);
                    
                    Debug.Log($"[LoginService] 登录成功: {username}, userId: {response.UserId}");
                    callback(true, response.UserId, null);
                }
                else
                {
                    Debug.Log($"[LoginService] 登录失败: {response.ErrorMessage}");
                    callback(false, null, response.ErrorMessage);
                }
            });
        }
        
        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="callback">注册结果回调</param>
        public void Register(string username, string password, RegisterCallback callback)
        {
            Debug.Log($"[LoginService] 尝试注册: {username}");

            // 模拟网络延迟
            Timer.Instance.AddTimer(0.8f, () =>
            {
                var response = LoginSystem.Register(username, password);
                
                if (response.Success)
                {
                    Debug.Log($"[LoginService] 注册成功: {username}, userId: {response.UserId}");
                    callback(true, response.UserId, null);
                }
                else
                {
                    Debug.Log($"[LoginService] 注册失败: {response.ErrorMessage}");
                    callback(false, null, response.ErrorMessage);
                }
            });
        }
        
        /// <summary>
        /// 注销登录
        /// </summary>
        public void Logout()
        {
            Debug.Log($"[LoginService] 用户注销登录: {CurrentUserId}");
            CurrentUserId = null;
        }
        
        /// <summary>
        /// 清除后台存储的用户
        /// </summary>
        public void ClearLocalUser()
        {
            LoginSystem.ClearCurrentUser();
            CurrentUserId = null;
            Debug.Log("[LoginService] 已清除后台用户数据");
        }
        
        /// <summary>
        /// 登录成功后更新游戏数据
        /// </summary>
        private void UpdateGameDataAfterLogin(LoginResponse response)
        {
            try
            {
                // 更新UserGameData
                var userData = GameDataManager.Instance.UserData;
                
                // 直接使用设备ID作为用户标识符
                userData.Uid = response.UserId;
                
                // 同步玩家数据到游戏中
                if (response.UserPlayer != null)
                {
                    // 使用转换工具将UserPlayer转换为PlayerInfo
                    userData.PlayerInfo = GameData.PlayerConvert.ToPlayerInfo(response.UserPlayer, response.UserId);
                    
                    Debug.Log($"[LoginService] 已同步玩家数据: Name={userData.PlayerInfo.name}, Level={userData.PlayerInfo.level}");
                }
                else
                {
                    Debug.Log($"[LoginService] 未收到玩家数据，仅设置了用户ID");
                }
                
                Debug.Log($"[LoginService] 已更新游戏数据, userId: {response.UserId}");
            }
            catch (Exception ex)
            {
                Debug.LogError($"[LoginService] 更新游戏数据失败: {ex.Message}");
            }
        }
    }
    
    /// <summary>
    /// 简单的定时器类，用于模拟网络延迟
    /// </summary>
    public class Timer
    {
        private static Timer instance;
        public static Timer Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Timer();
                }
                return instance;
            }
        }
        
        public void AddTimer(float delay, Action callback)
        {
            var gameObject = new GameObject("Timer");
            var timerComponent = gameObject.AddComponent<TimerComponent>();
            timerComponent.StartTimer(delay, callback);
        }
        
        private class TimerComponent : MonoBehaviour
        {
            private float timeLeft;
            private Action callback;
            
            public void StartTimer(float delay, Action callback)
            {
                this.timeLeft = delay;
                this.callback = callback;
            }
            
            void Update()
            {
                if (timeLeft > 0)
                {
                    timeLeft -= Time.deltaTime;
                    if (timeLeft <= 0)
                    {
                        callback?.Invoke();
                        Destroy(this.gameObject);
                    }
                }
            }
        }
    }
} 