using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;
using System.Text;
using LitJson;
using System;

public class FileTool
{
    /// <summary>
    /// 删除指定文件目录下的所有文件
    /// </summary>
    /// <param name="fullPath">文件路径</param>
    public static void DeleteAllFile(string fullPath)
    {

        foreach (string d in Directory.GetFileSystemEntries(fullPath))
        {

            if (File.Exists(d))
            {
                FileInfo fi = new FileInfo(d);
                if (fi.Attributes.ToString().IndexOf("ReadOnly") != -1)
                    fi.Attributes = FileAttributes.Normal;
                File.Delete(d);//直接删除其中的文件  
            }
            else
            {
                DirectoryInfo d1 = new DirectoryInfo(d);
                if (d1.GetFiles().Length != 0)
                {
                    DeleteAllFile(d1.FullName); //递归删除子文件夹
                }
                Directory.Delete(d);
            }
        }

    }

    public static void CopyFile(string srcFilePath, string desFilePath)
    {
        var tempDir = Path.GetDirectoryName(desFilePath);
        if (!Directory.Exists(tempDir))
        {
            Directory.CreateDirectory(tempDir);
        }

        if (File.Exists(desFilePath))
        {
            File.Delete(desFilePath);
        }

        System.IO.File.Copy(srcFilePath, desFilePath);//复制文件
    }

    /// <summary>
    /// 复制文件夹及文件
    /// </summary>
    /// <param name="sourceFolder">原文件路径</param>
    /// <param name="destFolder">目标文件路径</param>
    /// <returns></returns>
    public static int CopyFolder(string sourceFolder, string destFolder)
    {
        try
        {
            //如果目标路径不存在,则创建目标路径
            if (!System.IO.Directory.Exists(destFolder))
            {
                System.IO.Directory.CreateDirectory(destFolder);
            }
            //得到原文件根目录下的所有文件
            string[] files = System.IO.Directory.GetFiles(sourceFolder);
            foreach (string file in files)
            {
                string name = System.IO.Path.GetFileName(file);
                string dest = System.IO.Path.Combine(destFolder, name);
                System.IO.File.Copy(file, dest);//复制文件
            }
            //得到原文件根目录下的所有文件夹
            string[] folders = System.IO.Directory.GetDirectories(sourceFolder);
            foreach (string folder in folders)
            {
                string name = System.IO.Path.GetFileName(folder);
                string dest = System.IO.Path.Combine(destFolder, name);
                CopyFolder(folder, dest);//构建目标路径,递归复制文件
            }
            return 1;
        }
        catch (Exception e)
        {
            Debug.Log(e);
            return 0;
        }

    }


    public static string ReadAllText(string filePath)
    {
        var str = GetTextFromFile(filePath, true);
        return str;
    }

    /// <summary>
    /// 简单的获取文本
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    public static string GetTextFromFile(string filePath, bool isSaveEnter = false)
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
                if (isSaveEnter)
                {
                    line = line + "\n";
                }
            }

            if (isSaveEnter)
            {
                if (line.Length > 0)
                {
                    line = line.Substring(0, line.Length - 1);
                }
               
            }
        }

        return line;
    }

    /// <summary>
    /// 读文件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="filePath"></param>
    /// <returns></returns>
    public static byte[] ReadAllBytes(string filePath)
    {
        using (FileStream fs = new FileStream(filePath, FileMode.Open))
        {
            byte[] byteArray = new byte[fs.Length];
            fs.Read(byteArray, 0, byteArray.Length);

            return byteArray;
        }
    }


    public static void SaveBytesToFile(string path, byte[] bytes)
    {
        using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
        {
            fs.Write(bytes, 0, bytes.Length);
        }
    }

    public static void SaveToFile(string path, string str)
    {
        FileStream fs = new FileStream(path, FileMode.OpenOrCreate);
        using (StreamWriter sw = new StreamWriter(fs))
        {
            sw.Write(str);
        }

    }



}
