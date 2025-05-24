using System.Collections.Generic;
using ServerSimulation.Account.Models;
using UnityEngine;
using System;

namespace ServerSimulation
{
    [Serializable]
    public class AccountSystemData
    {
        public UserAccount CurrentAccount;
        public UserPlayer CurrentPlayer;
    }
    
    public class AccountSystem
    {
        private DataStorage dataStorage;
        private UserAccount currentAccount;
        private UserPlayer currentPlayer;
        
        // 存储账号数据的键名
        private readonly string ACCOUNT_DATA_KEY = "account_system_data";

        public AccountSystem(DataStorage dataStorage)
        {
            this.dataStorage = dataStorage;
            LoadAccount();
        }

        // 创建或更新当前账号
        public UserAccount CreateOrUpdateAccount(string username, string password)
        {
            var account = new UserAccount(username, password);
            currentAccount = account;
            
            // 如果没有玩家数据，同时创建
            if (currentPlayer == null)
            {
                currentPlayer = new UserPlayer(account.UserId);
            }
            else
            {
                // 更新玩家数据的用户ID
                currentPlayer.UserId = account.UserId;
            }
            
            SaveAccount();
            return account;
        }

        // 获取当前账号
        public UserAccount GetCurrentAccount()
        {
            return currentAccount;
        }
        
        // 获取当前玩家数据
        public UserPlayer GetCurrentPlayer()
        {
            return currentPlayer;
        }
        
        // 更新玩家数据
        public bool UpdateUserPlayer(UserPlayer player)
        {
            if (player == null)
            {
                Debug.LogWarning("[AccountSystem] 更新玩家数据失败，玩家数据为空");
                return false;
            }
            
            player.LastUpdateTime = DateTime.Now;
            currentPlayer = player;
            SaveAccount();
            return true;
        }

        // 加载账号和玩家数据
        private void LoadAccount()
        {
            var data = dataStorage.LoadData<AccountSystemData>(ACCOUNT_DATA_KEY);
            if (data != null)
            {
                currentAccount = data.CurrentAccount;
                currentPlayer = data.CurrentPlayer;
                
                Debug.Log($"[AccountSystem] 已加载账号: {currentAccount?.Username}");
            }
            else
            {
                currentAccount = null;
                currentPlayer = null;
                Debug.Log("[AccountSystem] 未找到账号数据");
            }
        }

        // 保存账号和玩家数据
        public void SaveAccount()
        {
            var data = new AccountSystemData
            {
                CurrentAccount = currentAccount,
                CurrentPlayer = currentPlayer
            };
            
            dataStorage.SaveData(ACCOUNT_DATA_KEY, data);
            
            Debug.Log($"[AccountSystem] 已保存账号: {currentAccount?.Username}");
        }
        
        // 清除账号数据
        public void ClearAccount()
        {
            currentAccount = null;
            currentPlayer = null;
            dataStorage.DeleteData(ACCOUNT_DATA_KEY);
            Debug.Log("[AccountSystem] 已清除账号数据");
        }
    }
} 