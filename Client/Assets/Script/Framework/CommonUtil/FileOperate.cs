using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
//简单的文件操作类
public class FileOperate
{
    /// <summary>
    /// 简单的获取文本 之后要统一到文件加载或者资源加载中
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    public static string GetTextFromFile(string filePath)
    {
        // 从文件中读取并显示每行
        string line = "";
        //Logx.Log("zxy : file : " + file);
        using (StreamReader sr = new StreamReader(filePath))
        {
            var str = "";
            while ((str = sr.ReadLine()) != null)
            {
                line = line + str;
            }
        }

        return line;
    }


    public static void SaveToFile(string path, string str)
    {
        FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write);
        using (StreamWriter sw = new StreamWriter(fs))
        {
            sw.Write(str);
        }

    }


    public static string[] GetAllFilesFromFolder(string folderPath,string pattern = "", SearchOption option = SearchOption.TopDirectoryOnly)
    {
        string[] files = System.IO.Directory.GetFiles(folderPath, pattern, option);
        return files;
    }
}

