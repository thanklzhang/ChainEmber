using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Battle_Client;
using GameData;
using NetProto;
using UnityEngine;
using UnityEngine.UI;

namespace Battle_Client
{
    //同步实体的道具栏
    public class EntityRevive_RecvMsg : ClientRecvMsg
    {
        public override void Handle()
        {
            var arg = this.msgArg as EntityRevive_RecvMsg_Arg;
            var entity = BattleEntityManager.Instance.FindEntity(arg.entityGuid);
            if (entity != null)
            {
                entity.OnRevive();

                var localPlayerEntityGuid = BattleManager.Instance.GetLocalCtrlHeroGuid();
                if (localPlayerEntityGuid == arg.entityGuid)
                {
                    UIManager.Instance.Close<BattleReviveUI>();
                }
            }
        }
    }

    public class EntityRevive_RecvMsg_Arg : BaseClientRecvMsgArg
    {
        public int entityGuid;
    }
}