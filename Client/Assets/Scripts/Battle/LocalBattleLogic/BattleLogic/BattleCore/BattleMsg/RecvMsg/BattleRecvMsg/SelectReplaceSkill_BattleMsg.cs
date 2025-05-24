using System;
using System.Collections.Generic;

namespace Battle
{
    public class SelectReplaceSkill_BattleMsgArg :
        BaseBattleRecvMsgArg
    {
        //选择的技能 id ， 如果 < 0 ，就是放弃
        public int selectSkillId;
    }

    public class SelectReplaceSkill_BattleMsg : BaseBattleRecvMsg
    {
      

        public override void Handle()
        {
            var battle = context.battle;

            var arg = this.msgArg as SelectReplaceSkill_BattleMsgArg;
           
            // if(player.ctrlHeroGuid == arg.moveEntityGuid){}

            var entity = GetRecvPlayerEntity();
            if (entity != null)
            {
                entity.SelectReplaceSkill(arg.selectSkillId);
            }
            
            // player.EntityCtrl.AskReleaseSkill(arg.skillId, arg.targetGuid, arg.targetPos);
        }
    }
}