using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

//资源更新模块
public class UpdateResourceModule
{
    int serBigVer;
    int serSmallVer;
    Dictionary<string, string> localFileDic;
    Dictionary<string, string> serFileDic;
    List<ResInfo> needUpdateResList;

    public Action<UpdateResStateType, string> event_updateResState;
    public Action<int, int> event_updateResInfo;
    public Action<ulong> event_updateDownloadBytes;

    //检查游戏资源
    public IEnumerator CheckResource(UpdateResError error)
    {
        Logx.Log(LogxType.Game,"开始检查游戏资源");
        
        if (!GlobalConfig.isUseAB)
        {
            Logx.Log(LogxType.Game,"非 AB 包模式 ， 完成检查游戏资源");
            yield break;
        }

        if (GlobalConfig.isLocalBattleTest)
        {
            Logx.Log(LogxType.Game,"纯本地战斗 ， 完成检查游戏资源");
            yield break;
        }

        var persistentPath = GlobalConfig.AssetBundlePath;
        var localVersionPath = GlobalConfig.AssetBundlePath + "/" + "version.txt";
        var isExist = Directory.Exists(persistentPath) && File.Exists(localVersionPath);
        if (!isExist)
        {
            //第一次进入游戏 复制游戏包体的资源到游戏资源路径
            yield return CopyGameResource(error);
        }
        else
        {
            //有本地资源文件 开始检测资源版本

             if (!GlobalConfig.isUseInternalAB)
            {
                //从服务端更新资源

                //step 1 : 检查当前版本资源是否正确(待定)

                //step 2 : 检查版本
                yield return CheckResourceVersion(error);
                if (error.err == UpdateResErrorType.ResHasNewest) yield break;
                if (error.IsError()) yield break;

                //step 3 : 获得服务端最新资源列表
                yield return GetResourceList(error);
                if (error.IsError()) yield break;

                //step 4 ：对比资源
                CompareResouce(error);
                if (error.IsError()) yield break;

                //step 5 : 下载需要更新的资源
                yield return DownloadResource(error);
            }
             else
             {
                 //使用本地资源
                 yield return null;
             }


             Logx.Log(LogxType.Game,"完成检查游戏资源");
             
            this.TriggerStateEvent(UpdateResStateType.Finish);
        }
    }

    public IEnumerator CopyGameResource(UpdateResError error)
    {
        Logx.Log(LogxType.CheckAndUpdateResource,"第一次进入游戏 开始复制包内中的现有资源复制到本地");
        var persistentPath = GlobalConfig.AssetBundlePath;
        Directory.CreateDirectory(persistentPath);
        var streamingPath = GlobalConfig.AppStreamingAssetPath;
        var allFiles = System.IO.Directory.GetFiles(streamingPath, "*.*", SearchOption.AllDirectories);
        //过滤文件
        var files = allFiles.Where(f =>
        {
            var isMetaFile = f.EndsWith(".meta");
            return !isMetaFile;
        }).ToList();

        this.TriggerStateEvent(UpdateResStateType.CopyRes);
        for (int i = 0; i < files.Count; i++)
        {
            var filePath = files[i];
            var index = filePath.IndexOf(streamingPath);
            var partFilePath = filePath.Substring(streamingPath.Length + 1);
            var persistentFullPath = persistentPath + "\\" + partFilePath;

            UnityWebRequest request = UnityWebRequest.Get(filePath);
            yield return request.SendWebRequest();

            if (request.isHttpError || request.isNetworkError)
            {
                error.err = UpdateResErrorType.Error;
                error.errInfo = "文件复制失败 ： " + filePath;
                yield break;
            }
            else
            {
                var directory = Path.GetDirectoryName(persistentFullPath);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                if (!File.Exists(persistentFullPath))
                {
                    File.Create(persistentFullPath).Dispose();
                }

                var downBytes = request.downloadHandler.data;

                File.WriteAllBytes(persistentFullPath, downBytes);

                var currFinishCount = i + 1;
                this.event_updateResInfo(currFinishCount, files.Count);
            }
        }


        var verPath = streamingPath + "/" + "version.txt";
        if (!File.Exists(verPath))
        {
            error.err = UpdateResErrorType.Error;
            error.errInfo = "复制游戏失败 StreamingAssets 目录中没有找到 version.txt";
            yield break;
        }

        Logx.Log(LogxType.CheckAndUpdateResource,"复制游戏资源完成");
        yield return CheckResource(error);
    }

    public string GetServerIp()
    {
        if (GlobalConfig.isLANServer)
        {
            //局域网服务器地址
            return GlobalConfig.LANServerIP;
        }

        return "";
    }

    public IEnumerator CheckResourceVersion(UpdateResError error)
    {
        //得到本地资源版本信息
        var localVersionPath = GlobalConfig.AssetBundlePath + "/" + "version.txt";
        int localBigVer;
        int localSmallVer;
        this.TriggerStateEvent(UpdateResStateType.CheckVersion);
        if (File.Exists(localVersionPath))
        {
            var verTxtStr = FileTool.GetTextFromFile(localVersionPath);
            var ver = verTxtStr.Split('v')[1];
            var verParamStr = ver.Split('.');
            localBigVer = int.Parse(verParamStr[0]);
            localSmallVer = int.Parse(verParamStr[1]);
            Logx.Log(LogxType.CheckAndUpdateResource,"本地当前资源版本 : v" + localBigVer + "." + localSmallVer);

            //Logx.Log("localBigVer : " + localBigVer);
            //Logx.Log("localSmallVer : " + localSmallVer);
        }
        else
        {
            error.err = UpdateResErrorType.Error;
            error.errInfo = "缺少 version.txt 文件";
            yield break;
        }

        var localIP = GetServerIp();
        var url = string.Format("http://{0}:{1}/get_res_version", localIP, 8080);
        Logx.Log(LogxType.CheckAndUpdateResource,"开始请求服务器 最新资源版本信息 : " + url);

        UnityWebRequest request = UnityWebRequest.Get(url);
        DownloadHandlerBuffer Download = new DownloadHandlerBuffer();
        request.downloadHandler = Download;
        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
        {
            error.err = UpdateResErrorType.Error;
            error.errInfo = "get_res_version 请求失败 原因 : " + request.error;
            yield break;
        }

        var json = request.downloadHandler.text;
        LitJson.JsonData jd = LitJson.JsonMapper.ToObject(json);
        //Logx.Log("服务端回包 资源版本 json 信息 : " + json);
        serBigVer = int.Parse(jd["big_version"].ToString());
        serSmallVer = int.Parse(jd["small_version"].ToString());
        Logx.Log(LogxType.CheckAndUpdateResource,"服务端回包 服务端最新资源版本 : v" + serBigVer + "." + serSmallVer);


        if (localBigVer < serBigVer)
        {
            error.err = UpdateResErrorType.Error;
            error.errInfo = "版本过低 请下载最新客户端";
            yield break;
        }

        if (localBigVer > serBigVer || localSmallVer > serSmallVer)
        {
            error.err = UpdateResErrorType.Error;
            error.errInfo = "版本错误 高出服务器资源版本 请重新下载客户端包";
            yield break;
        }

        if (localSmallVer == serSmallVer)
        {
            error.err = UpdateResErrorType.ResHasNewest;
            error.errInfo = "资源已经是最新版本 无需再更新";
            yield break;
        }

        var localVerStr = "v" + localBigVer + "." + localSmallVer;
        var serVerStr = "v" + serBigVer + "." + serSmallVer;
        Logx.Log(LogxType.CheckAndUpdateResource,"当前版本 : " + localVerStr + " , 服务器版本 : " + serVerStr + " , 需要更新资源");
    }

    public IEnumerator GetResourceList(UpdateResError error)
    {
        var localIP = GetServerIp();
        var url = string.Format("http://{0}:{1}/get_res_file", localIP, 8080);
        Logx.Log(LogxType.CheckAndUpdateResource,"开始请求服务端最新资源列表 : " + url);
        var request = UnityWebRequest.Get(url);
        var Download = new DownloadHandlerBuffer();
        request.downloadHandler = Download;

        this.TriggerStateEvent(UpdateResStateType.GetNewestResFileList);

        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
        {
            error.err = UpdateResErrorType.Error;
            error.errInfo = "get_res_file 请求失败 原因 : " + request.error;
            yield break;
        }

        serFileDic = new Dictionary<string, string>();
        var json = request.downloadHandler.text;
        Logx.Log(LogxType.CheckAndUpdateResource,"服务端回包 : get_res_file : " + json);
        var jd = LitJson.JsonMapper.ToObject(json);
        string pathListStr = "";
        foreach (JsonData jsonD in jd)
        {
            var path = jsonD["path"].ToString();
            var md5 = jsonD["md5"].ToString();
            serFileDic.Add(path, md5);

            pathListStr += path + "\n";
        }

        Logx.Log(LogxType.CheckAndUpdateResource,"服务端最新资源列表为 : " + pathListStr);
    }

    public void CompareResouce(UpdateResError error)
    {
        //得到本地当前资源列表信息
        localFileDic = new Dictionary<string, string>();
        var localResFileListPath = GlobalConfig.AssetBundlePath + "/" + "file_list.txt";
        if (File.Exists(localResFileListPath))
        {
            var fileTxtStr = FileTool.GetTextFromFile(localResFileListPath, true);
            var filesStr = fileTxtStr.Split('\n');
            foreach (var option in filesStr)
            {
                var paramsStr = option.Split('|');
                var path = paramsStr[0];
                var md5 = paramsStr[1];
                localFileDic.Add(path, md5);
            }
        }
        else
        {
            Logx.LogWarning(LogxType.CheckAndUpdateResource,"缺少 file_list.txt 文件");
            error.err = UpdateResErrorType.Error;
            return;
        }

        var pathListStr = "";
        needUpdateResList = new List<ResInfo>();
        foreach (var ser in serFileDic)
        {
            string serPath = ser.Key;
            string serMd5 = ser.Value;
            bool isNeedDownload = true;

            foreach (var local in localFileDic)
            {
                string localPath = local.Key;
                string localMd5 = local.Value;

                if (serPath == localPath)
                {
                    if (serMd5 != localMd5)
                    {
                        isNeedDownload = true;
                    }
                    else
                    {
                        //找到了 但是 md5 一样 不用下载
                        isNeedDownload = false;
                    }

                    break;
                }
            }

            //如果在本地没找到 那么就是没有该资源 需要下载
            if (isNeedDownload)
            {
                var resInfo = new ResInfo()
                {
                    path = serPath,
                    md5 = serMd5
                };
                needUpdateResList.Add(resInfo);
                pathListStr += resInfo.path + "\n";
            }
        }

        if (needUpdateResList.Count > 0)
        {
            Logx.Log(LogxType.CheckAndUpdateResource,"需要更新的资源 文件数量为 : " + needUpdateResList.Count + " , 文件列表为 : \n" + pathListStr);
        }
        else
        {
            Logx.Log(LogxType.CheckAndUpdateResource,"没有需要更新的资源");
        }
    }

    public IEnumerator DownloadResource(UpdateResError error)
    {
        this.TriggerStateEvent(UpdateResStateType.DownloadRes);
        for (int i = 0; i < needUpdateResList.Count; i++)
        {
            var needUpdateRes = needUpdateResList[i];
            //Logx.Log("开始下载资源 : " + needUpdateRes.path);
            yield return DownloadSingleFile(needUpdateRes, error);
            if (error.IsError()) yield break;
            //Logx.Log("完成下载资源 : " + needUpdateRes.path);

            var finishCount = i + 1;
            this.event_updateResInfo(finishCount, needUpdateResList.Count);
        }

        //全部文件都一致了就更新 version.txt 
        Logx.Log(LogxType.CheckAndUpdateResource,"开始更新 version.txt 文件");
        var localVersionFileListPath = GlobalConfig.AssetBundlePath + "/" + "version.txt";
        var str = "v" + serBigVer + "." + serSmallVer;
        FileTool.SaveToFile(localVersionFileListPath, str);
        Logx.Log(LogxType.CheckAndUpdateResource,"完成更新 version.txt 文件");
    }

    ulong preDownloadBytes = 0;

    public IEnumerator DownloadSingleFile(ResInfo serResInfo, UpdateResError error)
    {
        //得到服务端资源版本信息
        var localIP = GetServerIp();

        var url = string.Format("http://{0}:{1}/download_file/{2}", localIP, 8080, serResInfo.path);
        Logx.Log(LogxType.CheckAndUpdateResource,"请求服务端下载文件 : " + url);

        UnityWebRequest request = UnityWebRequest.Get(url);

        DownloadHandlerBuffer Download = new DownloadHandlerBuffer();
        request.downloadHandler = Download;

        var op = request.SendWebRequest();

        preDownloadBytes = 0;
        while (!op.isDone)
        {
            yield return null;

            if (request.isNetworkError || request.isHttpError)
            {
                error.err = UpdateResErrorType.Error;
                error.errInfo = "download_file " + serResInfo.path + " 请求失败 : 原因 : " + request.error;
                yield break;
            }

            var hasDownloadByets = request.downloadedBytes;
            var speedByBytes = hasDownloadByets - preDownloadBytes;
            //Debug.Log("download bytes : hasDownloadByets " + hasDownloadByets + " " );
            //Debug.Log("download bytes : speedByBytes " + speedByBytes + " ");
            this.event_updateDownloadBytes(speedByBytes);
            preDownloadBytes = hasDownloadByets;
        }

        var bytes = request.downloadHandler.data;

        Logx.Log(LogxType.CheckAndUpdateResource,"服务端回包 : bytes 长度 : " + bytes.Length);
        var serFileData = bytes;
        var savePath = GlobalConfig.AssetBundlePath + "/" + serResInfo.path;
        var tempDir = Path.GetDirectoryName(savePath);
        if (!Directory.Exists(tempDir))
        {
            Directory.CreateDirectory(tempDir);
        }

        FileTool.SaveBytesToFile(savePath, serFileData);

        //判断是否保存到 file_list.txt 中
        if (localFileDic.ContainsKey(serResInfo.path))
        {
            localFileDic[serResInfo.path] = serResInfo.md5;
        }
        else
        {
            localFileDic.Add(serResInfo.path, serResInfo.md5);
        }

        //下载完一个 更新 file_list.txt
        var localResFileListPath = GlobalConfig.AssetBundlePath + "/" + "file_list.txt";
        if (File.Exists(localResFileListPath))
        {
            var str = FileDicToStr(localFileDic);
            FileTool.SaveToFile(localResFileListPath, str);
        }
        else
        {
            error.err = UpdateResErrorType.Error;
            error.errInfo = "更新 file_list.txt 失败 : 缺少 file_list.txt 文件";
        }
    }

    public void TriggerStateEvent(UpdateResStateType state, string errStr = "")
    {
        this.event_updateResState(state, errStr);
    }

    public string FileDicToStr(Dictionary<string, string> localFileDic)
    {
        string str = "";
        foreach (var item in localFileDic)
        {
            var path = item.Key;
            var md5 = item.Value;
            str += path + "|" + md5 + "\n";
        }

        str = str.Substring(0, str.Length - 1);

        return str;
    }

    public class ResInfo
    {
        public string path;
        public string md5;
    }
}

public enum UpdateResErrorType
{
    Success = 0,
    Error = 1,

    //资源已经是最新的了
    ResHasNewest = 10
}

public class UpdateResError
{
    public UpdateResErrorType err;
    public string errInfo;

    public bool IsError()
    {
        return this.err == UpdateResErrorType.Error;
    }
}

public enum UpdateResStateType
{
    //复制游戏资源中
    CopyRes = 1,

    //检查资源版本中
    CheckVersion = 2,

    //获取最新资源文件列表中
    GetNewestResFileList = 3,

    //下载资源中
    DownloadRes = 4,

    //完成整个更新资源过程
    Finish = 5,

    //发生中断性错误
    Error = 10
}

//public class UpdateResState
//{
//    public UpdateResStateType stateType;
//}