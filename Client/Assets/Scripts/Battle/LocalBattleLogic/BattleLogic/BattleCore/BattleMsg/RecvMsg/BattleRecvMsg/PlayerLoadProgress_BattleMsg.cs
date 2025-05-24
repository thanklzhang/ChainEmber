using System;
using System.Collections.Generic;

namespace Battle
{
    public class PlayerLoadProgress_MsgArg : BaseBattleRecvMsgArg
    {
        public int progress;
    }

    public class PlayerLoadProgress_BattleMsg : BaseBattleRecvMsg
    {
       
        
        public override void Handle()
        {
            var arg = this.msgArg as PlayerLoadProgress_MsgArg;
            var battle = context.battle;
            battle.SetPlayerProgress(context.playerIndex, arg.progress);
        }
    } 

}