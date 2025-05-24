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
    public class SkillTrackEnd_RecvMsg : ClientRecvMsg
    {
        public override void Handle()
        {
            var arg = this.msgArg as SkillTrackEnd_RecvMsg_Arg;

            EventDispatcher.Broadcast<int, int>(
                EventIDs.OnSkillTrackEnd,
                arg.entityGuid, arg.skillTrackConfigId);
        }
    }

    public class SkillTrackEnd_RecvMsg_Arg : BaseClientRecvMsgArg
    {
        public int entityGuid;
        public int skillTrackConfigId;
    }
}