using System;
using System.Collections.Generic;
using System.Runtime.Remoting;

namespace Battle
{
    // public enum OperateHeroType
    // {
    //     Replace = 0,
    //     MoveToBattle = 1,
    //     MoveToUnderstudy = 2
    // }

    public class OperateHeroArg
    {
        // public OperateHeroType opType;
        public int opHeroGuid;

        public Vector3 targetPos;

        //如果 > 0 ，就是去替补位 ， 否则就是去战场
        public int toUnderstudyIndex;
    }

    public class HeroOperationByArraying_BattleMsgArg : BaseBattleRecvMsgArg
    {
        public int opHeroGuid;
        public Vector3 targetPos;
        public int toUnderstudyIndex;
    }

    public class HeroOperationByArraying_BattleMsg : BaseBattleRecvMsg
    {
        public override void Handle()
        {
            var msgArg = this.msgArg as HeroOperationByArraying_BattleMsgArg;
            var player = GetRecvPlayer();

            var opArg = new OperateHeroArg()
            {
                opHeroGuid = msgArg.opHeroGuid,
                targetPos = msgArg.targetPos,
                toUnderstudyIndex = msgArg.toUnderstudyIndex
            };

            player.MoveMemberByArraying(opArg);
        }
    }
}