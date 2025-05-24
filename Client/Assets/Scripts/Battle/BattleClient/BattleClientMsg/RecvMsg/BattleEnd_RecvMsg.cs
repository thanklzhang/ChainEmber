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
    public class BattleEnd_RecvMsg : ClientRecvMsg
    {
        public override void Handle()
        {
            var arg = this.msgArg as BattleEnd_RecvMsg_Arg;

            //BattleManager.Instance.BattleState = BattleState.End;

            //EventDispatcher.Broadcast<bool>(EventIDs.OnBattleEnd, isWin);
        }
    }

    public class BattleEnd_RecvMsg_Arg : BaseClientRecvMsgArg
    {
     
    }
}