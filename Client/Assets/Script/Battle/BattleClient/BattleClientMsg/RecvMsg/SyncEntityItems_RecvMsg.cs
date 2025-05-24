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
    public class SyncEntityItems_RecvMsg : ClientRecvMsg
    {
        public override void Handle()
        {
            var arg = this.msgArg as SyncEntityItems_RecvMsg_Arg;
            var entity = BattleEntityManager.Instance.FindEntity(arg.entityGuid);
            if (entity != null)
            {
                entity.UpdateItemBarCellList(arg.itemBarCellList);
            }
        }
    }

    public class SyncEntityItems_RecvMsg_Arg : BaseClientRecvMsgArg
    {
        public int entityGuid;
        public List<ItemBarCellData_Client> itemBarCellList;
    }
}