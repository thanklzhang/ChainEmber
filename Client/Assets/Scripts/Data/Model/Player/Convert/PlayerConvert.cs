using GameData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


namespace GameData
{
    public class PlayerConvert
    {
        public static PlayerInfo ToPlayerInfo(NetProto.PlayerInfoProto netPlayer)
        {
            PlayerInfo player = new PlayerInfo()
            {
                uid = netPlayer.Uid,
                name = netPlayer.Name,
                avatarURL = netPlayer.AvatarURL,
                level = netPlayer.Level
            };
            return player;
        }
    }
}