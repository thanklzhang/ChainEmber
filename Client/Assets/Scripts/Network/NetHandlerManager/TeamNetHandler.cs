using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using NetProto;
using GameData;

public class TeamNetHandler : NetHandler
{
    public Action event_getTeamRoomList;
    public Action event_createTeamRoom;
    public Action event_enterTeamRoom;
    public Action event_changeReadyStateInTeamRoom;
    public Action event_leaveTeamRoom;
    public Action event_startTeamBattle;

    public override void Init()
    {
        AddListener((int)ProtoIDs.GetTeamRoomList, OnGetTeamRoomList);
        AddListener((int)ProtoIDs.CreateTeamRoom, OnCreateTeamRoom);
        AddListener((int)ProtoIDs.EnterTeamRoom, OnEnterTeamRoom);
        AddListener((int)ProtoIDs.ChangeReadyStateInTeamRoom, OnChangeReadyStateInTeamRoom);
        AddListener((int)ProtoIDs.LeaveTeamRoom, OnLeaveTeamRoom);
        AddListener((int)ProtoIDs.StartTeamBattle, OnStartTeamBattle);

        //notify
        AddListener((int)ProtoIDs.NotifyPlayerEnterTeamRoom, OnNotifyPlayerEnterTeamRoom);
        AddListener((int)ProtoIDs.NotifyChangePlayerInfoInTeamRoom, OnNotifyChangePlayerInfoInTeamRoom);
        AddListener((int)ProtoIDs.NotifyLeaveTeamRoom, OnNotiyRoomLeaveRoom);
    }

    //拉取组队房间列表数据
    public void SendGetTeamRoomList(Action action)
    {
        event_getTeamRoomList = action;
        csGetTeamRoomList csGetRoomList = new csGetTeamRoomList()
        {

        };
        NetworkManager.Instance.SendMsg(ProtoIDs.GetTeamRoomList, csGetRoomList.ToByteArray());

    }


    public void OnGetTeamRoomList(MsgPack msgPack)
    {
        scGetTeamRoomList scGet = scGetTeamRoomList.Parser.ParseFrom(msgPack.data);

        //convert
        List<TeamRoomData> localRoomList = new List<TeamRoomData>();

        foreach (var netTeamRoom in scGet.TeamRoomList)
        {
            TeamRoomData localRoom = new TeamRoomData()
            {
                id = netTeamRoom.RoomId,
                roomName = netTeamRoom.RoomName,
                state = (TeamRoomState)netTeamRoom.State,
                teamStageId = netTeamRoom.TeamStageId
            };
            localRoom.playerList = new List<TeamRoomPlayerData>();
            foreach (var netPlayer in netTeamRoom.PlayerList)
            {

                //TeamRoomPlayerData localPlayer = new TeamRoomPlayerData()
                //{
                //    playerInfo = PlayerConvert.ToPlayerInfo(netPlayer.PlayerInfo),
                //    isMaster = netPlayer.IsRoomMaster,
                //    isHasReady = netPlayer.IsHasReady,
                //    selectHeroGuid =  netPlayer.Hero
                //};
                //localRoom.playerList.Add(localPlayer);

                var teamPlayer = TeamConvert.ToTeamPlayer(netPlayer);
                localRoom.playerList.Add(teamPlayer);

            }
            localRoomList.Add(localRoom);
        }

        GameDataManager.Instance.TeamData.SetRoomListData(localRoomList);

        event_getTeamRoomList?.Invoke();
        event_getTeamRoomList = null;
    }


    public void SendCreateTeamRoom(Action action)
    {
        event_createTeamRoom = action;
        csCreateTeamRoom csCreate = new csCreateTeamRoom()
        {

        };
        NetworkManager.Instance.SendMsg(ProtoIDs.CreateTeamRoom, csCreate.ToByteArray());

    }

    public void OnCreateTeamRoom(MsgPack msgPack)
    {
        scCreateTeamRoom scCreate = scCreateTeamRoom.Parser.ParseFrom(msgPack.data);
        // Logx.Log("OnCreateTeamRoom : " + scCreate.ToString());

        var resultRoom = TeamConvert.ToTeamRoom(scCreate.TeamRoom);
        GameDataManager.Instance.TeamData.SetCurrEnterRoomData(resultRoom);

        event_createTeamRoom?.Invoke();
        event_createTeamRoom = null;
    }

    public void SendEnterTeamRoom(int roomId, Action action)
    {
        event_enterTeamRoom = action;
        csEnterTeamRoom csEnter = new csEnterTeamRoom()
        {
            TeamRoomId = roomId
        };
        NetworkManager.Instance.SendMsg(ProtoIDs.EnterTeamRoom, csEnter.ToByteArray());

    }

    public void OnEnterTeamRoom(MsgPack msgPack)
    {
        scEnterTeamRoom scEnter = scEnterTeamRoom.Parser.ParseFrom(msgPack.data);

        var resultRoom = TeamConvert.ToTeamRoom(scEnter.TeamRoom);
        GameDataManager.Instance.TeamData.SetCurrEnterRoomData(resultRoom);

        event_enterTeamRoom?.Invoke();
        event_enterTeamRoom = null;
    }

    public void SendChangeReadyStateInTeamRoom(int roomId, bool isReady, Action action)
    {
        event_changeReadyStateInTeamRoom = action;
        csChangeReadyStateInTeamRoom csChange = new csChangeReadyStateInTeamRoom()
        {
            TeamRoomId = roomId,
            IsReady = isReady
        };
        NetworkManager.Instance.SendMsg(ProtoIDs.ChangeReadyStateInTeamRoom, csChange.ToByteArray());

    }

    public void OnChangeReadyStateInTeamRoom(MsgPack msgPack)
    {
        scChangeReadyStateInTeamRoom scChange = scChangeReadyStateInTeamRoom.Parser.ParseFrom(msgPack.data);

        var err = scChange.Err;
        event_changeReadyStateInTeamRoom?.Invoke();
        event_changeReadyStateInTeamRoom = null;
    }

    public void SendStartTeamBattle(int roomId, bool isReady, Action action)
    {
        event_startTeamBattle = action;
        csStartTeamBattle csStart = new csStartTeamBattle()
        {

        };
        NetworkManager.Instance.SendMsg(ProtoIDs.StartTeamBattle, csStart.ToByteArray());

    }

    public void OnStartTeamBattle(MsgPack msgPack)
    {
        scStartTeamBattle scStart = scStartTeamBattle.Parser.ParseFrom(msgPack.data);

        var err = scStart.Err;
        event_startTeamBattle?.Invoke();
        event_startTeamBattle = null;
    }

    public void SendLeaveTeamRoom(int roomId, Action action)
    {
        event_leaveTeamRoom = action;
        csLeaveTeamRoom csLeave = new csLeaveTeamRoom()
        {
            TeamRoomId = roomId
        };
        NetworkManager.Instance.SendMsg(ProtoIDs.LeaveTeamRoom, csLeave.ToByteArray());

    }

    public void OnLeaveTeamRoom(MsgPack msgPack)
    {
        scLeaveTeamRoom scLeave = scLeaveTeamRoom.Parser.ParseFrom(msgPack.data);
        // Logx.Log("OnLeaveTeamRoom : " + scLeave.ToString());
        var err = scLeave.Err;
        event_leaveTeamRoom?.Invoke();
        event_leaveTeamRoom = null;
    }

    public void SendSelectUseHeroInTeamRoom(int teamRoomId, int currSelectHeroGuid)
    {
        NetProto.csSelectUseHeroInTeamRoom csSelect = new NetProto.csSelectUseHeroInTeamRoom();
      
        csSelect.TeamRoomId = teamRoomId;
        csSelect.HeroGuid = currSelectHeroGuid;

        NetworkManager.Instance.SendMsg(ProtoIDs.SelectUseHeroInTeamRoom, csSelect.ToByteArray());

    }

    //notify------------------------

    //(暂时不用)这个可以和下面通用一个  加个 座位号即可
    public void OnNotifyPlayerEnterTeamRoom(MsgPack msgPack)
    {
        scNotifyPlayerEnterTeamRoom sc = scNotifyPlayerEnterTeamRoom.Parser.ParseFrom(msgPack.data);
        var player = sc.Player;

        ////data handler
        //var resultPlayer = TeamConvert.ToTeamPlayer(player);
        //var room = GameDataManager.Instance.TeamStore.currEnterRoomData;
        //room.UpdatePlayerRoomData(resultPlayer);


        //dispatch event 
    }

    public void OnNotifyChangePlayerInfoInTeamRoom(MsgPack msgPack)
    {
        scNotifyChangePlayerInfoInTeamRoom sc = scNotifyChangePlayerInfoInTeamRoom.Parser.ParseFrom(msgPack.data);
        // Logx.Log("OnNotifyChangePlayerInfoInTeamRoom : " + sc.ToString());
        var player = sc.Player;

        //data handler
        var resultPlayer = TeamConvert.ToTeamPlayer(player);
        //Logx.Log("log test : isHasReady : " + resultPlayer.isHasReady);
        var room = GameDataManager.Instance.TeamData.currEnterRoomData;
        room.UpdateRoomPlayerData(resultPlayer);

        //dispatch refresh player event 
        EventDispatcher.Broadcast(EventIDs.OnPlayerChangeInfoInTeamRoom);
    }

    public void OnNotiyRoomLeaveRoom(MsgPack msgPack)
    {
        scNotifyLeaveTeamRoom leave = scNotifyLeaveTeamRoom.Parser.ParseFrom(msgPack.data);
        // Logx.Log("OnNotiyRoomLeaveRoom : " + leave.ToString());
        var uid = leave.PlayerUid;

        GameDataManager.Instance.TeamData.RemovePlayer(uid);

        EventDispatcher.Broadcast(EventIDs.OnPlayerLeaveTeamRoom, uid);
    }

    //TODO :
    //LeaveTeamRoom = 2072;
    //SelectTeamStageInTeamRoom = 2074;
    //SelectUseHeroInTeamRoom = 2075;
    //NotifyLeaveTeamRoom = 2085;
    //NotifyChangeTeamRoomInfo = 2087;

}