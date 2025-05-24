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
    public class ReplaceSkillResult_RecvMsg : ClientRecvMsg
    {
        public override void Handle()
        {
            var arg = this.msgArg as ReplaceSkillResult_RecvMsg_Arg;

            var player = BattleManager.Instance.GetLocalPlayer();
            if (player != null && arg.playerIndex == player.playerIndex)
            {
                if (arg.retCodeType == ResultCodeType.AddLeaderSkillFull)
                {
                    BattleManager.Instance.OnReplaceSkillResult(arg);
                }
            }
        }
    }

    public class ReplaceSkillResult_RecvMsg_Arg : BaseClientRecvMsgArg
    {
        public int playerIndex;
        public ResultCodeType retCodeType;
        public int opSkillId;
    }
}