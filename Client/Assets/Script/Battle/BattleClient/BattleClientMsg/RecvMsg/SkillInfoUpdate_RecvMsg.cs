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
    public class SkillInfoUpdate_RecvMsg : ClientRecvMsg
    {
        public override void Handle()
        {
            var arg = this.msgArg as SkillInfoUpdate_RecvMsg_Arg;

            var entity = BattleEntityManager.Instance.FindEntity(arg.entityGuid);
            if (entity != null)
            {
                entity.UpdateSkillInfo(arg);
            }
        }
    }

    public class SkillInfoUpdate_RecvMsg_Arg : BaseClientRecvMsgArg
    {
        public int entityGuid;
        public int skillConfigId;
        public float currCDTime;
        public float maxCDTime;
        public int exp;
        public bool isDelete;
        public int showIndex;
    }
}