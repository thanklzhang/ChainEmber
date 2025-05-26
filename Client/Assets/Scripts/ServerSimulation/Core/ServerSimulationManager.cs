using System;
using System.Collections.Generic;
using UnityEngine;

namespace ServerSimulation
{
    public class ServerSimulationManager : Singleton<ServerSimulationManager>
    {
        // private static ServerSimulationManager instance;
        //
        // public static ServerSimulationManager Instance
        // {
        //     get
        //     {
        //         if (null == instance)
        //         {
        //             // 在场景中查找或创建实例
        //             instance = FindObjectOfType<ServerSimulationManager>();
        //             if (instance == null)
        //             {
        //                 GameObject go = new GameObject("ServerSimulationManager");
        //                 instance = go.AddComponent<ServerSimulationManager>();
        //                 DontDestroyOnLoad(go);
        //             }
        //         }
        //         return instance;
        //     }
        // }

        public AccountSystem AccountSystem { get; private set; }
        public LoginSystem LoginSystem { get; private set; }
        public DataStorage DataStorage { get; private set; }
        public BattleSystem BattleSystem { get; private set; }
        
        // 是否初始化完成
        public bool IsInitialized { get; private set; }

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

        public void Init()
        {
            if (IsInitialized)
                return;

            Debug.Log("[ServerSimulation] 初始化服务器模拟系统...");

            DataStorage = new DataStorage();
            DataStorage.Init();
            
            AccountSystem = new AccountSystem(DataStorage);
            LoginSystem = new LoginSystem(AccountSystem);
            BattleSystem = new BattleSystem();
            
            IsInitialized = true;
            
            Debug.Log("[ServerSimulation] 服务器模拟系统已初始化完成");
        }
        
        public void Update()
        {
            // 模拟服务器逻辑更新（如需要）
        }
        
        public void SaveAllData()
        {
            if (!IsInitialized)
                return;
                
            DataStorage.SaveAllData();
        }

        private void OnApplicationQuit()
        {
            // 应用退出时保存数据
            SaveAllData();
        }

        private void OnApplicationPause(bool pause)
        {
            // 应用暂停时保存数据
            if (pause)
            {
                SaveAllData();
            }
        }
    }
} 