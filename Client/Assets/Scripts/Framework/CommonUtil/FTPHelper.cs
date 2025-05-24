using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;
using System.Text;
using LitJson;
using System;
using System.Net;

public class FTPHelper
{
    public string ftpURI;
    public string ftpUserID;
    public string ftpPassword;

    public void Init(string ftpURI, string ftpUserID, string ftpPassword)
    {
        this.ftpURI = ftpURI;
        this.ftpUserID = ftpUserID;
        this.ftpPassword = ftpPassword;
    }

    #region 上传文件
    /// <summary>
    /// 上传文件
    /// </summary>
    /// <param name="localFile">要上传到FTP服务器的文件</param>
    /// <param name="ftpPath"></param>
    public  void  UpLoadFile(string localFile, string ftpPath)
    {
        var ftpUser = this.ftpUserID;
        var ftpPassword = this.ftpPassword;
        //ftpPath = this.ftpURI + "/" + ftpPath;


        //检测本地文件
        if (!File.Exists(localFile))
        {
            Logx.LogWarning("文件：“" + localFile + "” 不存在！");
            return;
        }

        //检测远端目录
        var dirPath = Path.GetDirectoryName(ftpPath);
        CheckRemoteDir(dirPath);


        FtpWebRequest ftpWebRequest = null;
        FileStream localFileStream = null;
        Stream requestStream = null;
        try
        {
            ftpWebRequest = (FtpWebRequest)FtpWebRequest.Create(new Uri(this.ftpURI + "/" + ftpPath));
            ftpWebRequest.Credentials = new NetworkCredential(ftpUser, ftpPassword);
            ftpWebRequest.UseBinary = true;
            ftpWebRequest.KeepAlive = false;
            ftpWebRequest.Method = WebRequestMethods.Ftp.UploadFile;
            ftpWebRequest.ContentLength = localFile.Length;
            int buffLength = 4096;
            byte[] buff = new byte[buffLength];
            int contentLen;
            localFileStream = new FileInfo(localFile).OpenRead();
            requestStream = ftpWebRequest.GetRequestStream();
            contentLen = localFileStream.Read(buff, 0, buffLength);
            while (contentLen != 0)
            {
                requestStream.Write(buff, 0, contentLen);
                contentLen = localFileStream.Read(buff, 0, buffLength);
            }
        }
        catch (Exception ex)
        {
            Logx.LogError(ex.Message);
        }
        finally
        {
            if (requestStream != null)
            {
                requestStream.Close();
            }
            if (localFileStream != null)
            {
                localFileStream.Close();
            }
        }
    }
    #endregion


    /// <summary>
    /// 判断文件的目录是否存,不存则创建
    /// </summary>
    /// <param name="destFilePath">本地文件目录</param>
    public void CheckRemoteDir(string destFilePath)
    {
        string fullDir = destFilePath.IndexOf(':') > 0 ? destFilePath.Substring(destFilePath.IndexOf(':') + 1) : destFilePath;
        fullDir = fullDir.Replace('\\', '/');
        string[] dirs = fullDir.Split('/');//解析出路径上所有的文件名
        string curDir = "/";
        for (int i = 0; i < dirs.Length; i++)//循环查询每一个文件夹
        {
            if (dirs[i] == "") continue;
            string dir = dirs[i];
            //如果是以/开始的路径,第一个为空 
            if (dir != null && dir.Length > 0)
            {
                try
                {
                    CheckRemoteDirAndMake(curDir, dir);
                    curDir += dir + "/";
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                }
            }
        }
    }

    public void CheckRemoteDirAndMake(string rootDir, string remoteDirName)
    {
        if (!CheckRemoteDirectoryExist(rootDir, remoteDirName))//判断当前目录下子目录是否存在
        {
            var resultPath = "";

            if ("/".Equals(rootDir))
            {
                resultPath = remoteDirName;
            }
            else
            {
                resultPath = rootDir + "/" + remoteDirName;
            }

            MakeDirByRemote(resultPath);

        }
        else
        {
            
        }

    }

    public bool CheckRemoteDirectoryExist(string rootDir, string RemoteDirectoryName)
    {
        string[] dirList = GetRemoteDirectoryList(rootDir);//获取子目录
        if (dirList.Length > 0)
        {
            foreach (string str in dirList)
            {
                if (str.Trim() == RemoteDirectoryName.Trim())
                {
                    return true;
                }
            }
        }
        return false;
    }

    public string[] GetRemoteDirectoryList(string dirName)
    {
        string[] drectory = GetRemoteFilesDetailList(dirName);
        List<string> strList = new List<string>();
        if (drectory.Length > 0)
        {
            foreach (string str in drectory)
            {
                if (str.Trim().Length == 0)
                    continue;
                //会有两种格式的详细信息返回
                //一种包含<DIR>
                //一种第一个字符串是drwxerwxx这样的权限操作符号
                //现在写代码包容两种格式的字符串
                if (str.Trim().Contains("<DIR>"))
                {
                    strList.Add(str.Substring(39).Trim());
                }
                else
                {
                    if (str.Trim().Substring(0, 1).ToUpper() == "D")
                    {
                        strList.Add(str.Substring(55).Trim());
                    }
                }
            }
        }
        return strList.ToArray();
    }

    /// <summary>
    /// 获得文件明晰
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public string[] GetRemoteFilesDetailList(string path)
    {
        var resultPath = this.ftpURI + "/";
        if (!"/".Equals(path))
        {
            resultPath += path;
        }
        return GetRemoteFileList(resultPath, WebRequestMethods.Ftp.ListDirectoryDetails);
    }
    //都调用这个
    //上面的代码示例了如何从ftp服务器上获得文件列表
    private string[] GetRemoteFileList(string path, string WRMethods)
    {
        StringBuilder result = new StringBuilder();
        try
        {
            var ftpWebRequest = (FtpWebRequest)FtpWebRequest.Create(new Uri(path));
            ftpWebRequest.Credentials = new NetworkCredential(this.ftpUserID, this.ftpPassword);
            ftpWebRequest.UseBinary = true;
            ftpWebRequest.KeepAlive = false;
            ftpWebRequest.Method = WRMethods;
            WebResponse response = ftpWebRequest.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream(), System.Text.Encoding.Default);//中文文件名
            string line = reader.ReadLine();

            while (line != null)
            {
                result.Append(line);
                result.Append("\n");
                line = reader.ReadLine();
            }


            // to remove the trailing '' '' 
            if (result.ToString() != "")
            {
                result.Remove(result.ToString().LastIndexOf("\n"), 1);
            }
            reader.Close();
            response.Close();
            return result.ToString().Split('\n');
        }

        catch (Exception ex)
        {
            throw new Exception("获取文件列表失败。原因： " + ex.Message);
        }
    }

    /// <summary>
    /// 创建目录
    /// </summary>
    /// <param name="dirName"></param>
    public void MakeDirByRemote(string dirName)
    {
        try
        {
            string uri = this.ftpURI + "/" + dirName;
            //Debug.Log("MakeDirByRemote uri : " + uri);
            var ftpWebRequest = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));
            ftpWebRequest.Credentials = new NetworkCredential(this.ftpUserID, this.ftpPassword);
            ftpWebRequest.UseBinary = true;
            ftpWebRequest.KeepAlive = false;
            ftpWebRequest.Method = WebRequestMethods.Ftp.MakeDirectory;

            FtpWebResponse response = (FtpWebResponse)ftpWebRequest.GetResponse();
            response.Close();
        }
        catch (Exception ex)
        {
            throw new Exception("创建文件失败，原因: " + ex.Message);
        }
    }


}