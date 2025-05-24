using System;
using System.Collections.Generic;

namespace Battle
{
    public class Battle_ItemUseArg : BaseBattleRecvMsgArg
    {
        public int itemIndex;
        public int releaserGuid;
        public int targetGuid;
        public Vector3 targetPos;
        public Vector3 mousePos;
    }

    
    public class UseItem_BattleMsg : BaseBattleRecvMsg
    {
        // public Battle_ItemUseArg itemUseArg;
        public override void Handle()
        {
            var battle = context.battle;
            var arg = this.msgArg as Battle_ItemUseArg;
            // UseItemAction action = new UseItemAction()
            // {
            //     arg = itemUseArg
            // };
            // battle.AddPlayerAction(action);
            //
            //
            // var entityAI = battle.FindAI(itemUseArg.releaserGuid);
            // if (entityAI != null)
            // {
            //     entityAI.AskUseItem(itemUseArg);
            // }
            // else
            // {
            //     Logx.LogWarning("the entityAI is not found : releaserGuid : " + itemUseArg.releaserGuid);
            // }
            //
            var playerIndex = context.playerIndex;
            var player = battle.FindPlayerByPlayerIndex(playerIndex);

            // if(player.ctrlHeroGuid == arg.moveEntityGuid){}

            var entity = battle.FindEntity(player.ctrlHeroGuid);
            if (entity != null)
            {
                entity.AskUseItem(arg);
            }

            // player.EntityCtrl.AskUseItem(arg);
        }
    }

}