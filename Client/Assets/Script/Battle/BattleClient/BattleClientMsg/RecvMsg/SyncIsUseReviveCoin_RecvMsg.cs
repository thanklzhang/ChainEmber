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
    public class SyncIsUseReviveCoin_RecvMsg : ClientRecvMsg
    {
        public override void Handle()
        {
            var arg = this.msgArg as SyncIsUseReviveCoin_RecvMsg_Arg;

            var localPlayer = BattleManager.Instance.GetLocalPlayer();
            if (arg.playerIndex == localPlayer.playerIndex)
            {
                EventDispatcher.Broadcast(EventIDs.OnSyncIsUseReviveCoin);
            }
        }
    }

    public class SyncIsUseReviveCoin_RecvMsg_Arg : BaseClientRecvMsgArg
    {
        public int playerIndex;
    }
}