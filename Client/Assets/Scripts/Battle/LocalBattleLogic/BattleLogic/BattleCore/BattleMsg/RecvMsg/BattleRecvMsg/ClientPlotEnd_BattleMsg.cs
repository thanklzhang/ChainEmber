using System;
using System.Collections.Generic;

namespace Battle
{
    public class ClientPlotEnd_BattleMsg : BaseBattleRecvMsg
    {
        public override void Handle()
        {
            var battle = context.battle;
            var playerIndex = context.playerIndex;

            battle.PlayerPlotEnd(playerIndex);
        }
    }
}