using System;
using System.Collections.Generic;

namespace Battle
{
    public class UseSkillItem_BattleMsg : BaseBattleRecvMsg
    {
        
        public override void Handle()
        {
            var battle = context.battle;
            var arg = this.msgArg as Battle_ItemUseArg;
            // UseSkillItemAction action = new UseSkillItemAction()
            // {
            //     arg = itemUseArg
            // };
            // battle.AddPlayerAction(action);
            
            // var entityAI = battle.FindAI(itemUseArg.releaserGuid);
            // if (entityAI != null)
            // {
            //     entityAI.AskUseSkillItem(itemUseArg);
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
                entity.AskUseSkillItem(arg);
            }
            
            // player.EntityCtrl.AskUseSkillItem(arg);
        }
    }

}