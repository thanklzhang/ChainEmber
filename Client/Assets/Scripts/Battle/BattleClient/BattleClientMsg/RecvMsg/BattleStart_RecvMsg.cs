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
    public class BattleStart_RecvMsg : ClientRecvMsg
    {
        public override void Handle()
        {
            var arg = this.msgArg as BattleStart_RecvMsg_Arg;
            
            EventDispatcher.Broadcast(EventIDs.OnBattleStart);

            //已经加载好的实体统一走一遍创建流程
            BattleEntityManager.Instance.NotifyCreateAllEntities();

            BattleManager.Instance.BattleState = Battle_Client.BattleState.Running;
        }
    }

    public class BattleStart_RecvMsg_Arg : BaseClientRecvMsgArg
    {
    }
}