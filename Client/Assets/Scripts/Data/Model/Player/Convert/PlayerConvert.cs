using GameData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using ServerSimulation.Account.Models;


namespace GameData
{
    public class PlayerConvert
    {
        public static PlayerInfo ToPlayerInfo(NetProto.PlayerInfoProto netPlayer)
        {
            PlayerInfo player = new PlayerInfo()
            {
                uid = netPlayer.Uid.ToString(),
                name = netPlayer.Name,
                avatarURL = netPlayer.AvatarURL,
                level = netPlayer.Level
            };
            return player;
        }
        
        /// <summary>
        /// 将UserPlayer转换为PlayerInfo
        /// </summary>
        /// <param name="userPlayer">服务器模拟的用户玩家数据</param>
        /// <param name="userId">用户ID</param>
        /// <returns>游戏内使用的玩家信息</returns>
        public static PlayerInfo ToPlayerInfo(UserPlayer userPlayer, string userId)
        {
            if (userPlayer == null)
            {
                return null;
            }
            
            PlayerInfo player = new PlayerInfo()
            {
                uid = userId,
                name = userPlayer.Nickname,
                avatarURL = userPlayer.AvatarId,
                level = userPlayer.Level
            };
            
            return player;
        }
    }
}