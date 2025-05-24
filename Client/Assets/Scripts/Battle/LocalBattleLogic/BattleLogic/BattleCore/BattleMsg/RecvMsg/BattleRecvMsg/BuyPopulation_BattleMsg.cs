using System;
using System.Collections.Generic;

namespace Battle
{
    public class BuyPopulation_BattleMsgArg : BaseBattleRecvMsgArg
    {
        public int buyCount = 1;
    }
    public class BuyPopulation_BattleMsg : BaseBattleRecvMsg
    {   
        public override void Handle()
        {
            var arg = this.msgArg as BuyPopulation_BattleMsgArg;   
            var player = GetRecvPlayer();
            
            player.BuyPopulation();
        }
    }

}