using System;
using System.Collections.Generic;

namespace Battle
{
    public class BuyBoxFromShop_BattleMsgArg : BaseBattleRecvMsgArg
    {
        public RewardQuality quality;
        public int buyCount;
    }
    public class BuyBoxFromShop_BattleMsg : BaseBattleRecvMsg
    {   
        public override void Handle()
        {
            var arg = this.msgArg as BuyBoxFromShop_BattleMsgArg;   
            var player = GetRecvPlayer();
            
            player.BuyBox(arg.quality,arg.buyCount);
        }
    }

}