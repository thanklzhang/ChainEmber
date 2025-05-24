using System.Collections;
using System.IO;
using System.Text;
using System;
using NetProto;

public class ProtoMsgUtil
{
    //服务端消息

    public static byte[] MakeServerMsgBytes(byte[] body, ServerProtoHead head)
    {
        uint headLength = 22;
        var dataLen = (uint)body.Length;

        uint totalLen = headLength + dataLen;
        ushort cmd = head.cmd;
        uint seq = head.seq;
        uint timeStamp = head.timeStamp;
        ulong uid = head.uid;
        //uint cid = head.cid;

        ByteBuffer buffer = new ByteBuffer();
        buffer.WriteUInt(totalLen);
        buffer.WriteUShort(cmd);
        buffer.WriteUInt(seq);
        buffer.WriteUInt(timeStamp);
        buffer.WriteULong(uid);
        //buffer.WriteUInt(cid);

        buffer.WriteBytes(body);

        var resultBytes = buffer.ToBytes();

        return resultBytes;
    }


    public static ServerMsg GetServerMsg(byte[] bytes)
    {
        ByteBuffer buffer = new ByteBuffer(bytes);
        uint len = buffer.ReadUInt();
        ushort cmd = buffer.ReadUShort();
        uint seq = buffer.ReadUInt();
        uint timeStamp = buffer.ReadUInt();
        ulong uid = buffer.ReadULong();
        //uint cid = buffer.ReadUInt();

        var headLength = 22;
        var dataLen = bytes.Length - headLength;
        byte[] data = buffer.ReadBytes(dataLen);

        ServerMsg serverMsg = new ServerMsg();
        serverMsg.head = new ServerProtoHead();
        serverMsg.head.len = len;
        serverMsg.head.cmd = cmd;
        serverMsg.head.seq = seq;
        serverMsg.head.timeStamp = timeStamp;
        serverMsg.head.uid = uid;
        //serverMsg.head.cid = cid;

        serverMsg.data = data;

        buffer.Close();

        return serverMsg;
    }

    //客户端消息

    public static byte[] MakeClientMsgBytes(byte[] body, ClientProtoHead head)
    {
        uint headLength = 22;
        var dataLen = (uint)body.Length;

        uint totalLen = headLength + dataLen;
        ushort cmd = head.cmd;
        uint seq = head.seq;
        uint timeStamp = head.timeStamp;
        ulong uid = head.uid;

        ByteBuffer buffer = new ByteBuffer();
        buffer.WriteUInt(totalLen);
        buffer.WriteUShort(cmd);
        buffer.WriteUInt(seq);
        buffer.WriteUInt(timeStamp);
        buffer.WriteULong(uid);
        buffer.WriteBytes(body);

        var resultBytes = buffer.ToBytes();

        return resultBytes;
    }

    public static ClientMsg GetClientMsg(byte[] bytes)
    {
        ByteBuffer buffer = new ByteBuffer(bytes);
        uint len = buffer.ReadUInt();
        ushort cmd = buffer.ReadUShort();
        uint seq = buffer.ReadUInt();
        uint timeStamp = buffer.ReadUInt();
        ulong uid = buffer.ReadULong();

        var headLength = 22;

        var dataLen = bytes.Length - headLength;
        byte[] data = buffer.ReadBytes(dataLen);

        ClientMsg clientMsg = new ClientMsg();
        clientMsg.head = new ClientProtoHead();
        clientMsg.head.len = len;
        clientMsg.head.cmd = cmd;
        clientMsg.head.seq = seq;
        clientMsg.head.timeStamp = timeStamp;
        clientMsg.head.uid = uid;
        //clientMsg.head.uid = uid;
        clientMsg.data = data;

        buffer.Close();

        return clientMsg;
    }


}
