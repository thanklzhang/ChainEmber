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
    public class UpdatePlayerTeamMembersInfo_RecvMsg : ClientRecvMsg
    {
        public override void Handle()
        {
            var arg = this.msgArg as UpdatePlayerTeamMembersInfo_RecvMsg_Arg;
            var playerIndex = arg.playerIndex;
            // var stateBean = arg.stateBean;

            var player = BattleManager.Instance.GetLocalPlayer();
            if (player != null && player.playerIndex == playerIndex)
            {
                player.UpdatePlayerTeamMembersInfo(arg.entityGuids);
            }
        }
    }

    public class UpdatePlayerTeamMembersInfo_RecvMsg_Arg : BaseClientRecvMsgArg
    {
        public int playerIndex;
        public List<int> entityGuids;
    }
}