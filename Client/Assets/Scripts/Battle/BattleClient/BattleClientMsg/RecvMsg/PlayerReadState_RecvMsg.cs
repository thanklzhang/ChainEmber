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
    public class PlayerReadyState_RecvMsg : ClientRecvMsg
    {
        
        public override void Handle()
        {
            var arg = this.msgArg as PlayerReadyState_RecvMsg_Arg;
            var uid = arg.playerIndex;
            var isReady = arg.isReady;
            EventDispatcher.Broadcast<int, bool>(EventIDs.OnPlayerReadyState, uid, isReady);
            
        }
    }

    public class PlayerReadyState_RecvMsg_Arg : BaseClientRecvMsgArg
    {
        public int playerIndex;
        public bool isReady;
    }
}