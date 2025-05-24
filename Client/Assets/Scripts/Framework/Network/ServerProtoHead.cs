using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
//服务端 包头
public class ServerProtoHead
{
    public uint len;
    public ushort cmd;
    public uint seq;
    public uint timeStamp;
    public ulong uid;
    public uint cid;//connect id
}

