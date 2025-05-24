using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GameData
{
    //这里组队主要是缓存房间数据
    public class TeamGameDataStore : BaseGameData
    {
        List<TeamRoomData> roomList = new List<TeamRoomData>();

        public List<TeamRoomData> RoomList { get => roomList; set => roomList = value; }

        public TeamRoomData currEnterRoomData;

        public void SetRoomListData(List<TeamRoomData> roomList)
        {
            this.RoomList = roomList;
        }

        public void SetCurrEnterRoomData(TeamRoomData enterRoom)
        {
            currEnterRoomData = enterRoom;
        }

        public void UpdateRoomPlayerInfoInCurrRoom(TeamRoomPlayerData player)
        {
            currEnterRoomData.UpdateRoomPlayerData(player);
        }

        public void RemovePlayer(int uid)
        {
            if (currEnterRoomData != null)
            {
                currEnterRoomData.RemovePlayer(uid);
            }
        }
    }

    public enum TeamRoomState
    {
        Ready = 0,
        Battling = 1
    }
    public class TeamRoomData
    {
        public int id;
        public int teamStageId;
        public string roomName;
        public TeamRoomState state;

        public List<TeamRoomPlayerData> playerList;

        //只更新玩家的房间数据
        public void UpdateRoomPlayerData(TeamRoomPlayerData player)
        {
            var findPlayer = playerList.Find(p => p.playerInfo.uid == player.playerInfo.uid);
            if (null == findPlayer)
            {
                //新增
                //Logx.Log("TeamGameDataStore : add new player : uid : " + player.playerInfo.uid);
                findPlayer = player;
                playerList.Add(findPlayer);
                //按照座位排序
                playerList.Sort((a, b) =>
                {
                    if (a.seat == b.seat)
                    {
                        return 0;
                    }
                    else if (a.seat < b.seat)
                    {
                        return -1;
                    }
                    else
                    {
                        return 1;
                    }
                });
            }
            else
            {
                findPlayer.selectHeroData = player.selectHeroData;
                findPlayer.isMaster = player.isMaster;
                findPlayer.isHasReady = player.isHasReady;
                findPlayer.seat = player.seat;
            }

        }

        public void RemovePlayer(int uid)
        {
            playerList.RemoveAll(p => p.playerInfo.uid == uid);
        }

    }

    public class TeamRoomPlayerData
    {
        public PlayerInfo playerInfo;
        public bool isMaster;
        public bool isHasReady;
        public HeroData selectHeroData;
        public int seat;
    }


}
