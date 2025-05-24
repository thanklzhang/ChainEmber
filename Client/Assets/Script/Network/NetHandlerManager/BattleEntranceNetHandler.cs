using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using NetProto;
public class BattleEntranceNetHandler : NetHandler
{
    public Action applyTeamBattleAction;
    //public Action<scApplyHeroExamBattle> applyHeroExamBattleAction;
    //public Action<scEnterGame> enterGameResultAction;
    public override void Init()
    {
        //AddListener((int)ProtoIDs.ApplyHeroExamBattle, OnApplyHeroExamBattle);
        AddListener((int)ProtoIDs.ApplyEnterBattle, OnApplyEnterBattle);
        //AddListener((int)ProtoIDs.EnterGame, OnEnterGame);

    }

    //申请进入主线战斗
    public void ApplyMainTaskBattle(int chapterId, int stageId, Action action)
    {
        csApplyEnterBattle apply = new csApplyEnterBattle();
        apply.FuncId = (int)FunctionIds.MainTask;
        apply.MainTaskArgs = new MainTaskEnterBattleArgs()
        {
            ChapterId = chapterId,
            StageId = stageId
        };

        applyTeamBattleAction = action;
        NetworkManager.Instance.SendMsg(ProtoIDs.ApplyEnterBattle, apply.ToByteArray());
    }


    //申请组队战斗
    public void ApplyTeamBattle(int teamRoomId, Action action)
    {
        csApplyEnterBattle apply = new csApplyEnterBattle();
        apply.FuncId = (int)FunctionIds.Team;
        apply.TeamArgs = new TeamEnterBattleArgs()
        {
            TeamRoomId = teamRoomId
        };

        applyTeamBattleAction = action;
        NetworkManager.Instance.SendMsg(ProtoIDs.ApplyEnterBattle, apply.ToByteArray());
    }

    public void OnApplyEnterBattle(MsgPack msgPack)
    {
        scApplyEnterBattle scApply = scApplyEnterBattle.Parser.ParseFrom(msgPack.data);
        var funcId = scApply.FuncId;
    }


    ////申请英雄试炼战斗
    //public void SendApplyHeroExamBattle(Action<scApplyHeroExamBattle> action)
    //{
    //    csApplyHeroExamBattle appy = new csApplyHeroExamBattle();
    //    applyHeroExamBattleAction = action;
    //    NetworkManager.Instance.SendMsg(ProtoIDs.ApplyHeroExamBattle, appy.ToByteArray());
    //    //TransitionBattleMsg(ProtoIDs.ApplyHeroExamBattle, start);
    //}

    //public void OnApplyHeroExamBattle(MsgPack msgPack)
    //{
    //    scApplyHeroExamBattle scApply = scApplyHeroExamBattle.Parser.ParseFrom(msgPack.data);

    //    applyHeroExamBattleAction?.Invoke(scApply);
    //    applyHeroExamBattleAction = null;

    //}


}

