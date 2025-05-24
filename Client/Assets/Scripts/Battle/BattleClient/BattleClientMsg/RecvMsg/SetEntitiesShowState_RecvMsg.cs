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
    public class SetEntitiesShowState_RecvMsg : ClientRecvMsg
    {
        public override void Handle()
        {
            var arg = this.msgArg as SetEntitiesShowState_RecvMsg_Arg;
            var Guids = arg.Guids;
            var isShow = arg.isShow;

            var entityGuids = Guids;
            BattleEntityManager.Instance.SetEntitiesShowState(isShow, entityGuids);
        }
    }

    public class SetEntitiesShowState_RecvMsg_Arg : BaseClientRecvMsgArg
    {
        public List<int> Guids;
        public bool isShow;
    }
}