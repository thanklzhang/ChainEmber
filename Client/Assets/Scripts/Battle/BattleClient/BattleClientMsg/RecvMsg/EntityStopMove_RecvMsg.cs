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
    public class EntityStopMove_RecvMsg : ClientRecvMsg
    {
        public override void Handle()
        {
            var arg = this.msgArg as EntityStopMove_RecvMsg_Arg;
            var Guid = arg.Guid;
            var EndPos = arg.EndPos;
            
            var guid = Guid;
            var endPos = EndPos;
            var entity = BattleEntityManager.Instance.FindEntity(guid);
            if (entity != null)
            {
                entity.StopMove(endPos);
            }
        }
    }

    public class EntityStopMove_RecvMsg_Arg : BaseClientRecvMsgArg
    {
        public int Guid;
        public UnityEngine.Vector3 EndPos;
    }
}