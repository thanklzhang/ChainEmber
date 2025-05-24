using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Battle;
using Battle_Client;
using GameData;
using NetProto;
using UnityEngine;

namespace Battle_Client
{
    //客户端战斗消息接收
    public class ClientRecvMsg
    {
        public BaseClientRecvMsgArg msgArg;
        
        public virtual void Handle()
        {
            
        }
    }

    public class BaseClientRecvMsgArg
    {
        
    }
}