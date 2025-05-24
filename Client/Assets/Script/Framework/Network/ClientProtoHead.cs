using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

//客户端 包头 指的是用户客户端
public class ClientProtoHead
{
    public uint len;
    public ushort cmd;
    public uint seq;
    public uint timeStamp;
    public ulong uid;

}

