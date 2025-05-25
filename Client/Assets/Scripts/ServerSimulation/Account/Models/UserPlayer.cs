using System;
using System.Collections.Generic;
using UnityEngine;

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

        // 空构造函数，用于反序列化
        public UserPlayer() 
        {
            Heroes = new List<UserHero>();
        }
        
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
        }
    }
} 