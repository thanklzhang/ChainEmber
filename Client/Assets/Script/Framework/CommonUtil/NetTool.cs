using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

public class NetTool
{
    public static string GetHostIp()
    {
        var hostName = Dns.GetHostName();
        var hostEntry = Dns.GetHostEntry(hostName);
        var addrList = hostEntry.AddressList;
        var localIp = "";
        foreach (var item in addrList)
        {
            if (item.AddressFamily == AddressFamily.InterNetwork)
            {
                localIp = item.ToString().Trim();
                break;
            }
        }

        return localIp;
    }

    public static bool IsIpFormat(string IP)
    {
        string regexStrIPV4 =
            (@"^(([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])\.){3}([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])$");

        if (Regex.IsMatch(IP, regexStrIPV4) && IP != "0.0.0.0")

        {
            return true;
        }

        return false;
    }
}