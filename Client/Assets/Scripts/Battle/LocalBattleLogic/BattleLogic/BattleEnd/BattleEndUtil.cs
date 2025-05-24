using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NetProto;
using Battle;

using Google.Protobuf.Collections;

public class BattleEndUtil
{
    //构建申请战斗结束的通用参数(前后端通用)
    public static NetProto.ApplyBattleEndArg MakeApplyBattleArgProto(Battle.Battle battle, 
        int teamIndex)//,BattleEndType endType
    {
        //Logx.Log("MakeApplyBattleArgProto : battle room id : " + battle.RoomId + " , winTeam : " + winTeam);

        var applyBattleEndArg = new ApplyBattleEndArg();
        var allPlayers = battle.GetAllPlayers();
        foreach (var player in allPlayers)
        {
            var uid = player.uid;
            var playerEndInfo = new PlayerBattleEndInfo()
            {
                Uid = (int)uid
            };
            var team = player.team;
            if (team == teamIndex)
            {
                //playerEndInfo.IsWin = endType == BattleEndType.Win ? 1 : 0;
                applyBattleEndArg.PlayerEndInfoList.Add(playerEndInfo);
            }
        }

        applyBattleEndArg.RoomId = battle.roomId;
        //batleEnd.StageId = stageId;
        applyBattleEndArg.BattleTableId = battle.battleConfigId;

        return applyBattleEndArg;
    }
    
}