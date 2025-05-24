using System;
using System.Collections.Generic;

namespace Battle
{
    public class Battle_SelectBoxRewardArg : BaseBattleRecvMsgArg
    {
        public RewardQuality quality;
        // public int releaserGuid;
        public int index;
    }
    public class SelectBoxReward_BattleMsg : BaseBattleRecvMsg
    {   
        public override void Handle()
        {
            var battle = context.battle;
            var arg = this.msgArg as Battle_SelectBoxRewardArg;
            
            // SelectBoxRewardAction action = new SelectBoxRewardAction()
            // {
            //     arg = arg
            // };
            // battle.AddPlayerAction(action);
            //
            //
            //
            // var entityAI = battle.FindAI(arg.releaserGuid);
            // if (entityAI != null)
            // {
            //     entityAI.AskSelectBoxReward(arg);
            // }
            // else
            // {
            //     Logx.LogWarning("the entityAI is not found : releaserGuid : " + arg.releaserGuid);
            // }
            
            var playerIndex = context.playerIndex;
            var player = battle.FindPlayerByPlayerIndex(playerIndex);

            player.SelectBoxReward(arg.quality,arg.index);
            // if(player.ctrlHeroGuid == arg.moveEntityGuid){}

            // var entity = battle.FindEntity(player.ctrlHeroGuid);
            // if (entity != null)
            // {
            //     entity.AskSelectBoxReward(arg);
            // }

            
         
            // player.EntityCtrl.AskSelectBoxReward(arg);
        }
    }

}