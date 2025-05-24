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
    public class ProcessEnterState_RecvMsg : ClientRecvMsg
    {
        public override void Handle()
        {
            var arg = this.msgArg as ProcessEnterState_RecvMsg_Arg;

            // var player = BattleManager.Instance.GetLocalPlayer();
            // if (player != null && arg.playerIndex == player.playerIndex)
            // {
            //     player.SetBoxShopItemsData(arg.boxDic);
            // }

            BattleManager.Instance.OnEnterProcessState(arg.state,arg.waveIndex,arg.surplusTimeMS);
        }
    }

    public class ProcessEnterState_RecvMsg_Arg : BaseClientRecvMsgArg
    {
        public BattleProcessState state;
        public int surplusTimeMS;
        public int waveIndex;
    }
}