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
    public class SyncEntityDir_RecvMsg : ClientRecvMsg
    {
        public override void Handle()
        {
            var arg = this.msgArg as SyncEntityDir_RecvMsg_Arg;
            var guid = arg.guid;
            var dir = arg.dir;
            
            var entity = BattleEntityManager.Instance.FindEntity(guid);
            if (entity != null)
            {
                entity.SetDirTarget(dir);
            }
        }
    }

    public class SyncEntityDir_RecvMsg_Arg : BaseClientRecvMsgArg
    {
        public int guid;
        public UnityEngine.Vector3 dir;
    }
}