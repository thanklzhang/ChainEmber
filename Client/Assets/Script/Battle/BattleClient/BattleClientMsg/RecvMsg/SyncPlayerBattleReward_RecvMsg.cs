using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Battle;
using Battle_Client;
using GameData;
using NetProto;
using UnityEngine;
using UnityEngine.UI;

namespace Battle_Client
{
    public class SyncPlayerBattleReward_RecvMsg : ClientRecvMsg
    {
        public override void Handle()
        {
            var arg = this.msgArg as SyncPlayerBattleReward_RecvMsg_Arg;
            var playerIndex = arg.playerIndex;
            // var stateBean = arg.stateBean;

            var player = BattleManager.Instance.GetLocalPlayer();
            if (player != null && player.playerIndex == playerIndex)
            {
                player.AddBattleReward(arg.battleReward);
            }
        }
    }

    public class SyncPlayerBattleReward_RecvMsg_Arg : BaseClientRecvMsgArg
    {
        public int playerIndex;
        public BattleReward_Client battleReward;
    }
}