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
    public class AbnormalEffect_RecvMsg : ClientRecvMsg
    {
        public override void Handle()
        {
            var arg = this.msgArg as AbnormalEffect_RecvMsg_Arg;
            var entityGuid = arg.entityGuid;
            var stateBean = arg.stateBean;
           
            var entity = BattleEntityManager.Instance.FindEntity(entityGuid);
            if (entity != null)
            {
                EventDispatcher.Broadcast<BattleEntity_Client,AbnormalStateBean>(EventIDs.OnEntityAbnormalEffect,
                    entity,stateBean);
            }
        }
    }

    public class AbnormalEffect_RecvMsg_Arg : BaseClientRecvMsgArg
    {
        public int entityGuid;
        public AbnormalStateBean stateBean;
    }
}