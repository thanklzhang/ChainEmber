using System;
using System.Collections.Generic;
using System.Runtime.Remoting;

namespace Battle
{
    public class SelectToRevive_BattleMsg_Arg : BaseBattleRecvMsgArg
    {
        public int entityGuid;
        public bool isRevive;
    }

    public class SelectToRevive_BattleMsg : BaseBattleRecvMsg
    {
        public override void Handle()
        {
            var msgArg = this.msgArg as SelectToRevive_BattleMsg_Arg;
            var player = GetRecvPlayer();
            player.Revive(msgArg.entityGuid,msgArg.isRevive);
        }
    }
}