using System;
using System.Collections.Generic;
using System.Runtime.Remoting;

namespace Battle
{
    public class AskEnterBattleProcess_BattleMsg_Arg : BaseBattleRecvMsgArg
    {
       
    }

    public class AskEnterBattleProcess_BattleMsg : BaseBattleRecvMsg
    {
        public override void Handle()
        {
            var msgArg = this.msgArg as MoveItem_BattleMsg_Arg;
            var player = GetRecvPlayer();
            var battle = player.battle;

            player.AskStartBattleProcess();
        }
    }
}