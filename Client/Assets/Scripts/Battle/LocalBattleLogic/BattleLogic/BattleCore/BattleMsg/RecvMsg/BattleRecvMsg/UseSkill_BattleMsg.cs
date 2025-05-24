using System;
using System.Collections.Generic;

namespace Battle
{
    public class UseSkill_BattleMsgArg :
        BaseBattleRecvMsgArg
    {
        public int releaserGuid;
        public int skillId;
        public int targetGuid;
        public Vector3 targetPos;
        public Vector3 mousePos;
    }

    public class UseSkill_BattleMsg : BaseBattleRecvMsg
    {
      

        public override void Handle()
        {
            var battle = context.battle;

            var arg = this.msgArg as UseSkill_BattleMsgArg;
            // UseSkillAction action = new UseSkillAction()
            // {
            //     releaserGuid = releaserGuid,
            //     skillId = skillId,
            //     targetGuid = targetGuid,
            //     targetPos = targetPos
            // };
            // battle.AddPlayerAction(action);


            // var entityAI = battle.FindAI(releaserGuid);
            // if (entityAI != null)
            // {
            //     entityAI.AskReleaseSkill(skillId, targetGuid, targetPos);
            // }
            // else
            // {
            //     Logx.LogWarning("the entityAI is not found : releaserGuid : " + releaserGuid);
            // }

           
            var playerIndex = context.playerIndex;
            var player = battle.FindPlayerByPlayerIndex(playerIndex);

            // if(player.ctrlHeroGuid == arg.moveEntityGuid){}

            var entity = battle.FindEntity(player.ctrlHeroGuid);
            if (entity != null)
            {
                entity.AskReleaseSkill(arg.skillId, arg.targetGuid, arg.targetPos,arg.mousePos);
            }
            
            // player.EntityCtrl.AskReleaseSkill(arg.skillId, arg.targetGuid, arg.targetPos);
        }
    }
}