using System;
using System.Collections.Generic;
using UnityEngine;
using ServerSimulation.Services;

namespace ServerSimulation
{
    /// <summary>
    /// 服务管理器，统一管理所有模拟后台服务
    /// </summary>
    public class ServerServiceManager : Singleton<ServerServiceManager>
    {
        // private static ServerServiceManager instance;
        // public static ServerServiceManager Instance
        // {
        //     get
        //     {
        //         if (instance == null)
        //         {
        //             GameObject go = GameObject.Find("ServerServiceManager");
        //             if (go == null)
        //             {
        //                 go = new GameObject("ServerServiceManager");
        //                 instance = go.AddComponent<ServerServiceManager>();
        //                 DontDestroyOnLoad(go);
        //             }
        //             else
        //             {
        //                 instance = go.GetComponent<ServerServiceManager>();
        //                 if (instance == null)
        //                 {
        //                     instance = go.AddComponent<ServerServiceManager>();
        //                 }
        //             }
        //         }
        //         return instance;
        //     }
        // }

        // private void Awake()
        // {
        //     if (instance == null)
        //     {
        //         instance = this;
        //         DontDestroyOnLoad(gameObject);
        //     }
        //     else if (instance != this)
        //     {
        //         Destroy(gameObject);
        //     }
        // }

        /// <summary>
        /// 初始化所有服务
        /// </summary>
        public void InitAllServices()
        {
            Debug.Log("[ServerServiceManager] 初始化服务...");
            
            // 在这里可以初始化其他服务
            
            Debug.Log("[ServerServiceManager] 服务初始化完成");
        }

        /// <summary>
        /// 账号服务
        /// </summary>
        public AccountService AccountService => AccountService.Instance;

        /// <summary>
        /// 登录服务
        /// </summary>
        public LoginService LoginService => LoginService.Instance;

        /// <summary>
        /// 通用的服务获取方法
        /// </summary>
        /// <typeparam name="T">服务类型</typeparam>
        /// <returns>服务实例</returns>
        public T GetService<T>() where T : class
        {
            if (typeof(T) == typeof(AccountService))
            {
                return AccountService as T;
            }
            else if (typeof(T) == typeof(LoginService))
            {
                return LoginService as T;
            }
            
            // 添加更多服务类型的判断
            
            Debug.LogError($"[ServerServiceManager] 未找到服务类型: {typeof(T).Name}");
            return null;
        }
    }
} 