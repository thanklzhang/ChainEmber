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
    public class EntityDead_RecvMsg : ClientRecvMsg
    {
        public override void Handle()
        {
            var arg = this.msgArg as EntityDead_RecvMsg_Arg;
            var entityGuid = arg.entityGuid;
            var entity = BattleEntityManager.Instance.FindEntity(entityGuid);
            if (entity != null)
            {
                entity.Dead(arg.isTrueDead);
            }
        }
    }

    public class EntityDead_RecvMsg_Arg : BaseClientRecvMsgArg
    {
        public int entityGuid;
        public bool isTrueDead;
    }
}