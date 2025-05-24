using System;
using System.Collections.Generic;

namespace Battle
{
    
    public class BattleReadyFinish_MsgArg : BaseBattleRecvMsgArg
    {
            
    }
    public class ReadyFinish_BattleMsg : BaseBattleRecvMsg
    {
        public override void Handle()
        {
            var battle = context.battle;
            battle.PlayerReadyFinish(context.playerIndex);
            
        }
    }

}