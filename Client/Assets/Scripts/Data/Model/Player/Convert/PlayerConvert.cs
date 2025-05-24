using GameData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using ServerSimulation.Account.Models;
using Config;


namespace GameData
{
    public class PlayerConvert
    {
        /// <summary>
        /// 默认玩家头像ID
        /// </summary>
        private const int DEFAULT_PLAYER_AVATAR_ID = 1001;
        
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
        
        /// <summary>
        /// 获取玩家头像的资源ID
        /// </summary>
        /// <param name="avatarURL">玩家头像URL，可能是头像ID的字符串形式或网络URL</param>
        /// <returns>资源ID，如果是网络URL则返回-1</returns>
        public static int GetPlayerAvatarResId(string avatarURL)
        {
            // 检查avatarURL是否为空或空字符串
            if (string.IsNullOrEmpty(avatarURL))
            {
                // 使用默认头像ID
                return GetAvatarResIdFromConfig(DEFAULT_PLAYER_AVATAR_ID);
            }
            
            // 检查是否是网络URL
            if (IsNetworkUrl(avatarURL))
            {
                // 如果是网络URL，返回-1表示需要从网络加载
                return -1;
            }
            
            // 尝试解析avatarURL为整数
            if (int.TryParse(avatarURL, out int avatarId))
            {
                // 直接使用avatarId获取资源ID
                return GetAvatarResIdFromConfig(avatarId);
            }
            else
            {
                // 解析失败，使用默认头像ID
                Debug.LogWarning($"无法解析头像URL: {avatarURL}，使用默认头像");
                return GetAvatarResIdFromConfig(DEFAULT_PLAYER_AVATAR_ID);
            }
        }
        
        /// <summary>
        /// 判断字符串是否为网络URL
        /// </summary>
        /// <param name="url">要检查的字符串</param>
        /// <returns>是否是网络URL</returns>
        public static bool IsNetworkUrl(string url)
        {
            if (string.IsNullOrEmpty(url))
                return false;
                
            return url.StartsWith("http://") || url.StartsWith("https://") || url.StartsWith("www.");
        }
        
        /// <summary>
        /// 从PlayerAvatar配置表获取资源ID
        /// </summary>
        /// <param name="avatarId">头像ID</param>
        /// <returns>资源ID</returns>
        private static int GetAvatarResIdFromConfig(int avatarId)
        {
            PlayerAvatar avatarConfig = ConfigManager.Instance.GetById<PlayerAvatar>(avatarId);
            
            if (avatarConfig != null)
            {
                return avatarConfig.ResId;
            }
            else
            {
                Debug.LogError($"未找到头像配置，ID: {avatarId}");
                // 如果没有找到配置，返回一个默认值或者第一个可用的头像资源ID
                var avatarList = ConfigManager.Instance.GetList<PlayerAvatar>();
                if (avatarList != null && avatarList.Count > 0)
                {
                    return avatarList[0].ResId;
                }
                
                // 实在找不到任何头像配置，返回0（可能导致无法加载资源）
                return 0;
            }
        }
    }
}