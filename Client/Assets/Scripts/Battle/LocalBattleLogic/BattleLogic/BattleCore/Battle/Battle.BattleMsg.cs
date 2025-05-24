using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using System.IO;


namespace Battle
{
    
    //战斗消息
    public partial class Battle
    {
        public void SendBattleMsg()
        {
            
        }

        public void OnRecvBattleMsg<T>(int playerIndex, BaseBattleRecvMsgArg arg) 
            where T : BaseBattleRecvMsg, new()
        {
            // battleMsg.context = new BattleMsgContext()
            // {
            //     battle = this,
            //     playerIndex = playerIndex
            // };
            battleRecvMsgManager.OnRecvMsg<T>(playerIndex, arg);
        }

    }
}