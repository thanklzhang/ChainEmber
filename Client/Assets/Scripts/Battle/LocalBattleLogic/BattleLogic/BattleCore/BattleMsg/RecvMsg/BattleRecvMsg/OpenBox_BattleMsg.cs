using System;
using System.Collections.Generic;

namespace Battle
{
    public class Battle_OpenBoxArg : BaseBattleRecvMsgArg
    {
        // public int releaserGuid;
        public RewardQuality quality;
    }
    
    public class OpenBox_BattleMsg : BaseBattleRecvMsg
    {
        // public Battle_OpenBoxArg arg;
        public override void Handle()
        {
            var battle = context.battle;
            var arg = this.msgArg as Battle_OpenBoxArg;
            // // OpenBoxAction useItemAction = new OpenBoxAction()
            // // {
            // //     arg = arg
            // // };
            // // battle.AddPlayerAction(useItemAction);
            //
            // var entityAI = battle.FindAI(arg.releaserGuid);
            // if (entityAI != null)
            // {
            //     entityAI.AskOpenBox(arg);
            // }
            // else
            // {
            //     Logx.LogWarning("the entityAI is not found : releaserGuid : " + arg.releaserGuid);
            // }
            
            var playerIndex = context.playerIndex;
            var player = battle.FindPlayerByPlayerIndex(playerIndex);
            player.OpenBox(arg.quality);
            
            
            // var entity = battle.FindEntity(player.ctrlHeroGuid);
            // if (entity != null)
            // {
            //     entity.AskOpenBox(arg);
            // }
            
        }
    }

}