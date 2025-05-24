using System;
using System.Collections.Generic;
using System.Runtime.Remoting;

namespace Battle
{
    public class MoveItem_BattleMsg_Arg : BaseBattleRecvMsgArg
    {
        public MoveItemOpLocation srcOpLoc;
        public MoveItemOpLocation desOpLoc;
    }

    public class MoveItem_BattleMsg : BaseBattleRecvMsg
    {
        public override void Handle()
        {
            var msgArg = this.msgArg as MoveItem_BattleMsg_Arg;
            var player = GetRecvPlayer();

            player.MoveItemTo(msgArg.srcOpLoc, msgArg.desOpLoc);
        }
    }
}