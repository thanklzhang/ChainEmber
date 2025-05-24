using System;
using System.Collections.Generic;

namespace Battle
{
    public class SelectReplaceHero_BattleMsgArg :
        BaseBattleRecvMsgArg
    {
        //选择的实体 id ， 如果 < 0 ，就是放弃
        public int entityConfigId;
    }

    public class SelectReplaceHero_BattleMsg : BaseBattleRecvMsg
    {
        public override void Handle()
        {
            var arg = this.msgArg as SelectReplaceHero_BattleMsgArg;
            var player = GetRecvPlayer();
            if (player != null)
            {
                player.SelectReplaceHero(arg.entityConfigId);
            }
        }
    }
}