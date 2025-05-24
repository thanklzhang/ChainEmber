using NetProto;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public enum ServerType
{
    Login = 1,
    Gate = 2,
}

public class NetworkManager : Singleton<NetworkManager>
{
    TcpNetClient tcpNetClient;
    Action<bool> connectCallback;

    public void Init()
    {
        tcpNetClient = new TcpNetClient();
    }

    public void ConnectToLoginServer(string ip, int port, Action<bool> connectCallback)
    {
        Logx.Log(LogxType.Net, "start to connect login server : " + ip + ":" + port);

        this.connectCallback = connectCallback;
        //tcpNetClient = new TcpNetClient();
        //var ip = "192.168.3.13";
        //var ip = "120.245.26.19";
        tcpNetClient.connectAction += this.OnConnectToLoginServerFinish;
        tcpNetClient.Connect(ip, port);
        tcpNetClient.servetType = ServerType.Login;
        tcpNetClient.ReceiveMsgAction += this.OnReceveMsg;
    }


    public void OnConnectToLoginServerFinish(bool isSuccess)
    {
        Logx.Log(LogxType.Net, "connect login server success");

        //注意这里 要保证 loom 在 gameScene 中有实例 因为 loom 中的自动实例创建是需要在主线程的 这里不是
        Loom.QueueOnMainThread(() =>
        {
            // Logx.Log("curr OnConnectToLoginServerFinish : " + System.Threading.Thread.CurrentThread.ManagedThreadId);
            this.connectCallback?.Invoke(isSuccess);
        });
    }

    public void ConnectToGateServer(string ip, int port, Action<bool> connectCallback)
    {
        Logx.Log(LogxType.Net, "start to connect gate server : " + ip + ":" + port);

        tcpNetClient?.Close();

        this.connectCallback = connectCallback;
        tcpNetClient = new TcpNetClient();
        tcpNetClient.connectAction += this.OnConnectToGateServerFinish;

        tcpNetClient.servetType = ServerType.Gate;

        tcpNetClient.Connect(ip, port);
        tcpNetClient.ReceiveMsgAction += this.OnReceveMsg;
    }

    public void OnConnectToGateServerFinish(bool isSuccess)
    {
        Logx.Log(LogxType.Net, "connect gate server success");

        Loom.QueueOnMainThread(() =>
        {
            //Logx.Log("net", "NetworkManager : OnConnectToGateServerFinish : " + isSuccess);
            this.connectCallback?.Invoke(isSuccess);
        });
    }

    private void OnReceveMsg(TcpNetClient netClient, MsgPack msg)
    {
        var now = CommonFunction.GetTimeStamp();
        var delayTime = now - lastSendTimeStamp;
        //Logx.Log("receive timeStamp : " + now);
        //Logx.Log("net delay time : " + delayTime);

        if (msg != null)
        {
            //Logx.Log("Network Mgr : ReceiveMsg : cmd : " + (ProtoIDs)msg.cmdId);
            //Debug.Log("zxy : Network Mgr : ReceiveMsg : cmd : " + (ProtoIDs)msg.cmdId);

            var logStr = string.Format("receive net msg : {0} ({1}) , from : {2}",(ProtoIDs)msg.cmdId,msg.cmdId,netClient.servetType);
            
            Logx.Log(LogxType.Net, logStr);

            NetMsgManager.Instance.OnReceiveMsg(msg);
        }
        else
        {
            Logx.LogError(LogxType.Net,"NetworkManager : OnReceveMsg : the msg is null");
        }
    }

    public long lastSendTimeStamp = 0;

    public void SendMsg(ProtoIDs cmd, byte[] data)
    {
        // Logx.Log("Network : SendMsg : cmd : " + cmd + ":" + (int)cmd);
        lastSendTimeStamp = CommonFunction.GetTimeStamp();
        //Logx.Log("send timeStamp : " + lastSendTimeStamp);

        var logStr = string.Format("send net msg : {0} ({1}) , to : {2}",cmd,(int)cmd,tcpNetClient.servetType);
        
        Logx.Log(LogxType.Net, logStr);

        tcpNetClient.Send((int)cmd, data);
        //Logx.Log("Network Mgr : send time stamp : " + lastSendTimeStamp);
    }

    public void Update()
    {
        this.tcpNetClient.Update();
    }


}