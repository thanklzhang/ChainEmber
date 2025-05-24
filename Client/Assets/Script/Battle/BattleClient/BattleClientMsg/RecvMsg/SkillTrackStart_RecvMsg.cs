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
    public class SkillTrackStart_RecvMsg : ClientRecvMsg
    {
        public override void Handle()
        {
            var arg = this.msgArg as SkillTrackStart_RecvMsg_Arg;

            var create = arg.create;
            TrackBean trackBean = new TrackBean()
            {
                trackConfigId = create.trackConfigId,
                releaserGuid = create.releaserEntityGuid,
                targetPos = create.targetPos,
                targetEntityGuid = create.targetEntityGuid
            };
            
            EventDispatcher.Broadcast<TrackBean>(EventIDs.OnSkillTrackStart, trackBean);
        }
    }

    public class SkillTrackStart_RecvMsg_Arg : BaseClientRecvMsgArg
    {
        public BattleClientMsg_CreateSkillTrack create;
    }
}