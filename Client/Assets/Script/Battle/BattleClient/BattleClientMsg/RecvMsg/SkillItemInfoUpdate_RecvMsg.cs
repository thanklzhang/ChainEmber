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
    public class SkillItemInfoUpdate_RecvMsg : ClientRecvMsg
    {
        public override void Handle()
        {
            var arg = this.msgArg as SkillItemInfoUpdate_RecvMsg_Arg;

            var entity = BattleEntityManager.Instance.FindEntity(arg.entityGuid);
            if (entity != null)
            {
                // entity.UpdateSkillItemInfo(arg.index, arg.configId, arg.count,
                //     arg.currCDTime, arg.maxCDTime);
            }
        }
    }

    public class SkillItemInfoUpdate_RecvMsg_Arg : BaseClientRecvMsgArg
    {
        public int entityGuid;
        public int index;
        public int configId;
        public int count;
        public float currCDTime;
        public float maxCDTime;
    }
}