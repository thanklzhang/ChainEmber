using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace ServerSimulation.Account.Models
{
    [Serializable]
    public class UserPlayer
    {
        public string UserId;
        public string Nickname;
        public int Level;
        public int Experience;
        public string AvatarId;
        public DateTime CreateTime;
        public DateTime LastUpdateTime;
        public List<UserHero> Heroes; // 玩家拥有的英雄列表
        public PlayerBag Bag; // 玩家背包

        // 空构造函数，用于反序列化
        public UserPlayer() 
        {
            Heroes = new List<UserHero>();
            Bag = new PlayerBag();
        }
        
        //当用户第一次创建账号时，初始化一些默认数据
        public UserPlayer(string userId, string nickname = "")
        {
            UserId = userId;
            Nickname = string.IsNullOrEmpty(nickname) ? "玩家" + userId.Substring(0, 5) : nickname;
            Level = 1;
            Experience = 0;
            AvatarId = "default";
            CreateTime = DateTime.Now;
            LastUpdateTime = DateTime.Now;
            Heroes = new List<UserHero>(); // 初始化英雄列表
            Bag = new PlayerBag();
            Bag.AddGold(500);
            // 同步初始金币到BagData
            ServerSimulation.Services.AccountService.Instance?.SyncCurrencyToBag(PlayerBag.GOLD_ID, Bag.GetGold());
        }
    }
} 