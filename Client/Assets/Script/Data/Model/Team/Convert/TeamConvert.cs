using GameData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


namespace GameData
{
    public class TeamConvert
    {
        public static TeamRoomData ToTeamRoom(NetProto.TeamRoom netRoom)
        {
            TeamRoomData room = new TeamRoomData()
            {
                id = netRoom.RoomId,
                roomName = netRoom.RoomName,
                state = (TeamRoomState)netRoom.State,
                teamStageId = netRoom.TeamStageId
            };

            room.playerList = new List<TeamRoomPlayerData>();

            foreach (var netRoomPlayer in netRoom.PlayerList)
            {
                TeamRoomPlayerData player = ToTeamPlayer(netRoomPlayer);
                room.playerList.Add(player);

            }

            return room;
        }

        public static TeamRoomPlayerData ToTeamPlayer(NetProto.TeamRoomPlayer netPlayer)
        {
            TeamRoomPlayerData player = new TeamRoomPlayerData()
            {
                isMaster = netPlayer.IsRoomMaster,
                selectHeroData = HeroConvert.ToHeroData(netPlayer.Hero),
                isHasReady = netPlayer.IsHasReady,
                playerInfo = PlayerConvert.ToPlayerInfo(netPlayer.PlayerInfo)
            };
            return player;
        }
    }
}