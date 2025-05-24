using System;
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

        // 空构造函数，用于反序列化
        public UserPlayer() 
        {
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
        }
    }
} 