using FixedPointy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalConfig
{
    //是否局域网服务器
    public static bool isLANServer = false;

    //path
    public const string AppName = "Jeko";
    
    public static string AppStreamingAssetPath = Application.streamingAssetsPath; //游戏第一次安装的游戏内部包
    //public static string AssetBundlePath = Application.persistentDataPath + "/" + AppName + "/Resource";
    // public static string AssetBundlePath = Application.persistentDataPath + "/" + AppName + "";///Resources

    public static string AssetBundlePath
    {
        get
        {
            if (!isUseInternalAB)
            {
                return Application.persistentDataPath + "/" + AppName + "";
            }
            else
            {
                return AppStreamingAssetPath;
            }
        }
    }


    ///
    /// 
    //public static string AssetBundlePath = Application.streamingAssetsPath;//方便测试
    public const string ABExtName = ".ab";

    //public static bool isUpdateMode = false;//是否开启更新模式(需要开启资源服务端)
    public static bool isUseAB = false; //资源是否从 AB 中读取 不是的话 从 项目中 Assets 目录读取
    // public static bool isUpdateResFromServer = false; //是否从服务器更新资源(需要开启资源服务端)
    public static bool isUseInternalAB = false; //是否使用内部的 AB  （StreamingAssets）
    public static bool isLocalBattleTest = false;   //纯本地战斗模式

    //本地资源管理器作为上传 ab 的资源服务端地址(热更时资源会从这里下载)
    public static string localUploadABResPath = "D:\\_LocalABPath";
    
    public static string ABPackageStrategyPath = "Assets/Editor/BuildPackage/ABPackageStrategy.asset";

    public static string projectRootPath = "Assets";

    public static string buildPath = "Assets/BuildRes";

    public static string configPath = "Config";

    public static string sceneRootPath = "Scenes";

    public const string WebUrl = "http://127.0.0.1:8080/";
    public const string ip = "196.168.1.100";
    public const int port = 9595;

    public const string ResourceVersion = "1.00";

    public const string combatServerIP = "127.0.0.1";
    public const int combatServerPort = 2500;
    public const int combatClientBindPort = 2349;

    //heartBeat
    public const int heartBeatInterval = 5000;
    public const int sendHeartBeatMsgId = 100;
    public const int receiveHeartBeatMsgId = 101;

    public const int frameTime = 66;
    public const int floatMul = 10000;

    public static string LANServerIP = "";
    
    public static Fix timeDelta = Fix.Ratio(frameTime, 1000);

    public const string Ground = "Ground";
    public const string UnderstudyArea = "UnderstudyArea";
}