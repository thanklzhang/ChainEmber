using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NetProto;

public class ServerMsg
{
    public ServerProtoHead head;
    public byte[] data;
}

public class ClientMsg
{
    public ClientProtoHead head;
    public byte[] data;
}
