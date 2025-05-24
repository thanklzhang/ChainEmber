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
    public class EntityReleaseSkill_RecvMsg : ClientRecvMsg
    {
        public override void Handle()
        {
            var arg = this.msgArg as EntityReleaseSkill_RecvMsg_Arg;
            var entityGuid = arg.entityGuid;
            var skillConfig = arg.skillConfig;
            
            var guid = entityGuid;
            var entity = BattleEntityManager.Instance.FindEntity(guid);
            if (entity != null)
            {
                entity.ReleaseSkill(skillConfig);
            }
        }
    }

    public class EntityReleaseSkill_RecvMsg_Arg : BaseClientRecvMsgArg
    {
        public int entityGuid;
        public int skillConfig;
    }
}