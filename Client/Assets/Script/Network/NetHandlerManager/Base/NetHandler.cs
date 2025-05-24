using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class NetHandler
{
    public virtual void Init()
    {

    }

    //protected void AddListener(int eventType, Callback<byte[]> action)
    //{
    //    EventManager.AddListener<byte[]>(eventType, action);
    //}

    protected void AddListener(int eventType, Callback<MsgPack> action)
    {
        EventManager.AddListener<MsgPack>(eventType, action);
    }

    //protected void AddListener(ProtoMsgIds eventType, Callback<byte[]> action)
    //{
    //    EventManager.AddListener<byte[]>((int)eventType, action);
    //}

    //public void SendMsgToLS(ProtoMsgIds eventType, IMessage data)
    //{
    //    Debug.Log("send msg to LS server: " + eventType.ToString() + " (" + (int)eventType + ")");
    //    NetworkManager.Instance.SendMsgToLS((int)eventType, data.ToByteArray());
    //}

    //public void SendMsgToCS(ProtoMsgIds eventType, IMessage data)//, Callback<byte[]> callback = null
    //{
    //    Debug.Log("send msg to CS server: " + eventType.ToString() + " (" + (int)eventType + ")");
    //    NetworkManager.Instance.SendMsgToCS((int)eventType, data.ToByteArray());
    //}

    //public void SendMsgToSS(ProtoMsgIds eventType, IMessage data)//, Callback<byte[]> callback = null
    //{
    //    Debug.Log("send msg to SS server: " + eventType.ToString() + " (" + (int)eventType + ")");
    //    NetworkManager.Instance.SendMsgToSS((int)eventType, data.ToByteArray());
    //}


    //log str
    public void LogNetErrStr(int cmd, int err)
    {
        var str = "receive msg err : " + (NetProto.ProtoIDs)cmd + "(" + (int)cmd + ") , errCode : " + err;
        Logx.LogError(str);
    
        //CtrlManager.Instance.GlobalCtrlPre.ShowTips(str);
    
    }
}

