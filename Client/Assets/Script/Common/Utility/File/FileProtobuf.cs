using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
//protobuf 的文件操作类
public class FileProtobuf
{
    //c# , unity Resource 目录在这里不生效

    /// <summary>
    /// 读文件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="filePath"></param>
    /// <returns></returns>
    public static byte[] Read(string filePath)
    {
        using (FileStream fs = new FileStream(filePath, FileMode.Open))
        {
            byte[] byteArray = new byte[fs.Length];
            fs.Read(byteArray, 0, byteArray.Length);
            
            return byteArray;
        }
    }

    /// <summary>
    /// 写文件
    /// </summary>
    /// <param name="filePath">文件路径</param>
    /// <param name="content">文件内容</param>
    /// <returns></returns>
    public static void Write(string filePath, IMessage msg)
    {
        //将文件信息读入流中
        using (FileStream fs = new FileStream(filePath, FileMode.Truncate))
        {
            lock (fs)//锁住流
            {
                if (!fs.CanWrite)
                {
                    throw new System.Security.SecurityException("filePath = " + filePath + " is only to read !");
                }

                msg.WriteTo(fs);
                Debug.Log(msg.ToByteArray().Length);
            }
        }
    }

    //------ Unity 文件(Resource)

    public static void Write()
    {


    }


}

