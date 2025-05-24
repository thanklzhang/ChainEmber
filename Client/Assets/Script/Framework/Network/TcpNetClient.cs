using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using UnityEngine;

public class MsgPack
{
    public int cmdId;

    //public int clientId;//转发消息的时候需要
    public byte[] data;

    public static MsgPack Create(int cmdId, byte[] data)
    {
        var p = new MsgPack();
        p.cmdId = cmdId;
        //p.clientId = clientId;
        p.data = data;
        return p;
    }
}

/// <summary>
/// 网络状态
/// </summary>
public enum NetState
{
    Null,
    Connect,
    Close,
}

public class TcpNetClient
{
    public Socket netSocket;

    //接收消息缓冲
    private byte[] buffer = null;

    //消息数据缓冲区 目前用于解决粘包问题
    public byte[] dataBuffer;

    public NetState netState;

    int MSG_HEAD_LEN = 22; //len 4 , msgId 2 , seq 4 , timeStamp 4 , uid 8
    int MAX_MSG_SIZE = 1024 * 256;

    //接受消息封装包的缓存队列
    public List<MsgPack> receiveMsgQueue;

    public ServerType servetType;

    //event
    public Action<bool> connectAction;
    public Action<int> closeAction;
    public Action<TcpNetClient, MsgPack> ReceiveMsgAction;

    public TcpNetClient()
    {
        netSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        Init();
    }
    //public TcpNetClient(Socket s)
    //{

    //    this.netSocket = s;
    //    Init();
    //}

    void Init()
    {
        buffer = new byte[MAX_MSG_SIZE];
        receiveMsgQueue = new List<MsgPack>();
    }

    public void Connect(string ip, int port)
    {
        try
        {
            //Logx.Log("net", "start to connect ...");
            IPAddress mIp = IPAddress.Parse(ip);
            IPEndPoint ip_end_point = new IPEndPoint(mIp, port);
            netSocket.BeginConnect(ip_end_point, new AsyncCallback(OnConnectCallback), netSocket);
        }
        catch (Exception e)
        {
            Logx.LogError(e);
            //ChangeToCloseState();
        }
    }

    void OnConnectCallback(IAsyncResult ar)
    {
        //Loom.QueueOnMainThread(()=>
        //{


        //});

        var s = (Socket)ar.AsyncState;
        //Logx.Log("net", "OnConnectCallback : on connect : " + s.Connected);
        if (s.Connected)
        {
            buffer = new byte[MAX_MSG_SIZE];

            netState = NetState.Connect;
            s.EndConnect(ar);

            s.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(HandleReceiveCallback), s);
        }
        else
        {
            Logx.LogWarning("OnConnectCallback : the socket connect fail");
        }

        //heartBeatService.Start();
        bool isSuccessConnect = netSocket.Connected;
        connectAction?.Invoke(isSuccessConnect);
    }

    void HandleReceiveCallback(IAsyncResult ar)
    {
        try
        {
            this.OnReceiveCallback(ar);
        }
        catch (Exception e)
        {
            Logx.LogError(e);
        }
    }

    void OnReceiveCallback(IAsyncResult ar)
    {
        var socket = (Socket)ar.AsyncState;
        //Debug.Log(socket.Connected);

        if (!socket.Connected)
        {
            Logx.LogWarning("OnReceiveCallback : connect : false");
            return;
        }

        int length = socket.EndReceive(ar);


        if (length == 0)
        {
            Logx.Log(LogxType.Net, socket.RemoteEndPoint + " : disconnect");
            return;
        }


        //Logx.Log("floor net : receive msg : " + length);
        //dataBuffer 加上这段数据
        if (dataBuffer == null)
        {
            dataBuffer = new byte[length];
            Array.Copy(buffer, dataBuffer, length);
        }
        else
        {
            byte[] finalB = new byte[length];
            Array.Copy(buffer, 0, finalB, 0, length);
            dataBuffer = dataBuffer.Concat(finalB).ToArray();
        }

        //Logx.LogWarning("length : " + length);

        while (dataBuffer.Length >= MSG_HEAD_LEN)
        {
            //第一位是 total len 读出来(head + body)
            int totalPackLen = BitConverter.ToInt32(dataBuffer, 0);

            if (totalPackLen <= dataBuffer.Length)
            {
                //达到了消息整包大小
                byte[] currData = new byte[totalPackLen];

                Array.Copy(dataBuffer, 0, currData, 0, totalPackLen);

                //解析消息
                var msgPack = ParseFromMsg(currData);
                //Logx.Log("floor net : add msg to queue ");
                //加入队列
                AddMsgToReceiveQueue(msgPack);

                byte[] nextData = new byte[dataBuffer.Length - totalPackLen];
                Array.Copy(dataBuffer, totalPackLen, nextData, 0, dataBuffer.Length - totalPackLen);
                dataBuffer = nextData;
            }
            else
            {
                //没到达一个数据包 所以接着接收 直到到了一个数据包的长度
                break;
            }
        }

        if (socket.Connected)
        {
            socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(HandleReceiveCallback),
                socket);
        }
    }

    public void Send(MsgPack msg)
    {
        Send(msg.cmdId, msg.data);
    }

    public void Send(int msgId, byte[] data)
    {
        try
        {
            byte[] resultData = BuildData(msgId, data);
            netSocket.BeginSend(resultData, 0, resultData.Length, SocketFlags.None,
                new AsyncCallback(OnSendMsgCallback), netSocket);
        }
        catch (Exception e)
        {
            Logx.LogError(e);
            //ChangeToCloseState();
        }
    }

    private void OnSendMsgCallback(IAsyncResult ar)
    {
        var socket = (ar.AsyncState as Socket);
        socket.EndSend(ar);

        //Console.WriteLine("finish!");
    }


    /// <summary>
    /// 组装消息数据
    /// </summary>
    /// <param name="msg"></param>
    public virtual byte[] BuildData(int msgId, byte[] dataContent)
    {
        ClientProtoHead head = new ClientProtoHead()
        {
            cmd = (ushort)msgId,
            seq = 1,
            timeStamp = 1,
            //uid = (ulong)myUid
        };
        var bytes = ProtoMsgUtil.MakeClientMsgBytes(dataContent, head);


        //byte[] data = null;
        //MemoryStream ms = null;
        //using (ms = new MemoryStream())
        //{
        //    BinaryWriter writer = new BinaryWriter(ms);

        //    writer.Write(dataContent.Length);
        //    writer.Write(msgId);
        //    writer.Write(dataContent);
        //    //TODO:
        //    //writer.Write(timeStemp);//时间戳
        //    //writer.Write(clientId);//用户唯一 id  登录之后即可拥有

        //    writer.Flush();
        //    data = ms.ToArray();
        //    writer.Close();
        //}

        return bytes;
    }


    /// <summary>
    /// 解析消息
    /// </summary>
    /// <param name="msg"></param>
    public MsgPack ParseFromMsg(byte[] msg)
    {
        //Logx.Log("receive ParseFromMsg ");

        var clientMsg = ProtoMsgUtil.GetClientMsg(msg);

        var msgPack = MsgPack.Create(clientMsg.head.cmd, clientMsg.data);


        //MemoryStream ms = null;
        //using (ms = new MemoryStream(msg))
        //{
        //    BinaryReader reader = new BinaryReader(ms);
        //    int len = reader.ReadInt32();
        //    int msgId = reader.ReadInt32();
        //    //int clientId = reader.ReadInt32();
        //    byte[] data = reader.ReadBytes(len);
        //    var msgPack = MsgPack.Create(msgId, data);
        //    reader.Close();

        //    return msgPack;
        //}

        return msgPack;
    }

    public void AddMsgToReceiveQueue(MsgPack pack)
    {
        lock (recvObj)
        {
            receiveMsgQueue.Add(pack);
        }
    }


    public void Update()
    {
        this.UpdateReceiveQueue();
    }

    object recvObj = new object();

    public void UpdateReceiveQueue()
    {
        lock (recvObj)
        {
            for (int i = 0; i < receiveMsgQueue.Count; i++)
            {
                try
                {
                    var msgPack = receiveMsgQueue[i];
                    this.ReceiveMsgAction?.Invoke(this, msgPack);
                }
                catch (Exception e)
                {
                    Logx.LogException(e);
                    continue;
                }
            }

            receiveMsgQueue.Clear();
        }
    }


    public bool IsConnect()
    {
        return netState == NetState.Connect;
    }

    public void Close()
    {
        receiveMsgQueue.Clear();
        connectAction = null;
        closeAction = null;
        ReceiveMsgAction = null;
        //netSocket?.Disconnect(false);
        netSocket.Dispose();
        buffer = null;
        dataBuffer = null;
    }
}