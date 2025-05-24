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
    public class CreateEntities_RecvMsg : ClientRecvMsg
    {
        public override void Handle()
        {
            var arg = this.msgArg as CreateEntities_RecvMsg_Arg;

            var entityList = arg.entityList;
            
            for (int i = 0; i < entityList.Count; i++)
            {
                var msgEntity = entityList[i];
                var entity = BattleEntityManager.Instance.CreateEntity(msgEntity);
                // EventDispatcher.Broadcast<BattleEntity_Client>(EventIDs.OnCreateEntity, entity);
            }
        }
    }

    public class CreateEntities_RecvMsg_Arg : BaseClientRecvMsgArg
    {
        public List<BattleClientMsg_Entity> entityList;
    }
}