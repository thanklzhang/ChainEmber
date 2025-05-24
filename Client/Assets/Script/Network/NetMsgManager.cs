using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

//网络消息管理器 可以在这里收敛所有协议消息
public class NetMsgManager : Singleton<NetMsgManager>
{
    public void Init()
    {
        
    }

    public void OnReceiveMsg(MsgPack msg)
    {
        var cmdId = msg.cmdId;
        //Logx.LogNet("NetMsgManager : OnReceiveMsg : cmdId : " + cmdId);
        EventManager.Broadcast(cmdId, msg);
    }

}
