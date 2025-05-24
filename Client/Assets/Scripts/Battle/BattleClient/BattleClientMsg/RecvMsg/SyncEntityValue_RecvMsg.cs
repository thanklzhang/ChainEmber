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
    public class SyncEntityValue_RecvMsg : ClientRecvMsg
    {
        public override void Handle()
        {
            var arg = this.msgArg as SyncEntityValue_RecvMsg_Arg;

            var entity = BattleEntityManager.Instance.FindEntity(arg.entityGuid);
            if (entity != null)
            {
                entity.SyncStateValue(arg.values);
            }
        }
    }

    public class SyncEntityValue_RecvMsg_Arg : BaseClientRecvMsgArg
    {
        public int entityGuid;
        public List<BattleClientMsg_BattleStateValue> values;
    }
}