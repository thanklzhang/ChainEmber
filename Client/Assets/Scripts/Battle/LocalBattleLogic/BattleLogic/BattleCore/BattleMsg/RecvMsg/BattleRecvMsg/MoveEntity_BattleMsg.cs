using System;
using System.Collections.Generic;

namespace Battle
{
    public class MoveEntity_MsgArg : BaseBattleRecvMsgArg
    {
        public int moveEntityGuid;
        public Vector3 targetPos;
    }

    public class MoveEntity_BattleMsg : BaseBattleRecvMsg
    {
       
        public override void Handle()
        {
            var battle = context.battle;
            var arg = this.msgArg as MoveEntity_MsgArg;
            // MoveAction action = new MoveAction()
            // {
            //     moveEntityGuid = moveEntityGuid,
            //     targetPos = targetPos
            // };
            // battle.AddPlayerAction(action);
            
            var playerIndex = context.playerIndex;
            var player = battle.FindPlayerByPlayerIndex(playerIndex);

            // if(player.ctrlHeroGuid == arg.moveEntityGuid){}

            var entity = battle.FindEntity(player.ctrlHeroGuid);
            if (entity != null)
            {
                entity.AskMoveToPos(arg.targetPos);
            }

            // player.EntityCtrl.AskMoveToPos(arg.targetPos);
            
            
            
        }
    }

}