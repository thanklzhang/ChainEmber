using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

public class BuildArg
{
    public bool isBuildPackage;
    public bool isBuildAB;
    public UploadABType uploadABType;
    public string productName;
    public string version;
    public BuildTarget buildTarget;
    public string channel;
    public string resVersion = "";
    public string preCompileDefines = "";
}

public enum UploadABType
{
    //不上传
    No = 0,
    //上传到本地
    Local = 1,
    //通过 ftp 上传
    FTP = 2
}

public class BuildPackageTool
{
    static BuildArg buildArgs;

    // public static List<string> commandArgs = new List<string>()
    // {
    //     "is_build_package",
    //     "is_build_ab",
    //     "product_name",
    //     "version",
    //     "platform",
    //     "channel",
    //     "res_version"
    // };

    [MenuItem("Build/Build Package")]
    public static void BuildPackage()
    {
        // 解析命令行参数
        buildArgs = new BuildArg();

        buildArgs.isBuildPackage = false;
        buildArgs.isBuildAB = false;
        buildArgs.buildTarget = BuildTarget.StandaloneWindows64;

        string[] args = System.Environment.GetCommandLineArgs();
        foreach (var s in args)
        {
            Debug.Log("argsargs : " + s);
            if (s.Contains("--") && s.Contains(":"))
            {
                var ss = s.Split(':');
                string value = ss[1];
                var currCmd = ss[0].Split("--")[1].Trim();

                Debug.Log("arg : cmd : " + currCmd + " , value : " + value);
                HandleCommand(currCmd, value);
            }
            //foreach (var currCmd in commandArgs)
            //{
            //    if (s.Contains("--" + currCmd + ":"))
            //    {
            //        string value = s.Split(':')[1];
            //        Debug.Log("arg : cmd : " + currCmd + " , value : " + value);
            //        HandleCommand(currCmd, value);
            //        break;
            //    }
            //}
        }

        //buldArgs 已经填充好
        StartBuild();
    }

    static void StartBuild()
    {
        Logx.Log(LogxType.Build,"Build 包版本 : " + buildArgs.version);
        Logx.Log(LogxType.Build,"Build 平台 : " + buildArgs.buildTarget);

        BuildPlayerOptions opt = new BuildPlayerOptions();
        //场景 这里只有启动项 其他场景会从 ab 加载
        opt.scenes = new string[] { "Assets/Scenes/Startup.unity" };
        //opt.locationPathName = Application.dataPath + "/../Bin/test.apk";

        if (buildArgs.buildTarget == BuildTarget.StandaloneWindows64)
        {
            opt.locationPathName = "D:\\UnityPackageOutPath\\OrderOfChain\\Windows\\OrderOfChain_" + buildArgs.version +
                                   ".exe";
            opt.targetGroup = BuildTargetGroup.Standalone;
        }
        else if (buildArgs.buildTarget == BuildTarget.Android)
        {
            opt.locationPathName = "D:\\UnityPackageOutPath\\OrderOfChain\\Windows\\OrderOfChain_" + buildArgs.version +
                                   ".apk";
            opt.targetGroup = BuildTargetGroup.Android;
        }
        else if (buildArgs.buildTarget == BuildTarget.iOS)
        {
            //opt.locationPathName = "E:\\E_Place_For_Use\\Apk\\test001.apk";
        }


        opt.target = buildArgs.buildTarget;
        opt.options = BuildOptions.None;
        


        PlayerSettings.productName = buildArgs.productName;
        PlayerSettings.bundleVersion = buildArgs.version;
        
        PlayerSettings.defaultScreenWidth = 1920;
        PlayerSettings.defaultScreenHeight = 1080;

        opt.options |= BuildOptions.Development;
        opt.options |= BuildOptions.AllowDebugging;

        string[] preCompileDefines = buildArgs.preCompileDefines.Split(';');

     

        BuildTargetGroup buildTargetGroup = opt.targetGroup;
        Debug.Log("buildTargetGroup" + buildTargetGroup);

        var symbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(opt.targetGroup);
        //添加想要的宏定义
        var symbolsStrs = symbols.Split(';').ToList();
        
        //清空之前的自定义预编译定义
        string[] fixedDefines = { "IS_LOCAL_BATTLE", "IS_USE_AB", "IS_USE_INTERNAL_AB" };
        foreach (var VARIABLE in fixedDefines)
        {
            symbolsStrs.Remove(VARIABLE);
        }

        // Debug.Log("原先的预编译宏 : start ");
        // foreach (var VARIABLE in symbolsStrs)
        // {
        //     Debug.Log("" + VARIABLE);
        // }
        //
        // Debug.Log("原先的预编译宏 : end");

        // if (ss.Contains("UNITY_TEST"))
        // {
        //     return;
        // }
        // ss.Add("UNITY_TEST");

        foreach (var currCompileStr in preCompileDefines)
        {
            // Debug.Log("symbolsStrs : start ");
            // foreach (var VARIABLE in symbolsStrs)
            // {
            //     Debug.Log("symbolsStrs : " + VARIABLE);
            // }
            //
            // Debug.Log("symbolsStrs : end ");
            //
            // Debug.Log("currCompileStr : " + currCompileStr);

            if (!symbolsStrs.Contains(currCompileStr))
            {
                symbolsStrs.Add(currCompileStr);
            }
        }

        symbols = string.Join(";", symbolsStrs);
        PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTargetGroup, symbols);
        //Debug.Log("添加后的宏：" + PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTargetGroup));


        if (buildArgs.isBuildAB)
        {
            Logx.Log(LogxType.Build,"开始 build AB");
            ABPackageTool.BuildAssetResource(buildArgs.buildTarget, buildArgs.resVersion,buildArgs.uploadABType);
        }

        if (buildArgs.isBuildPackage)
        {
            Logx.Log(LogxType.Build,"开始 build 包");
            BuildPipeline.BuildPlayer(opt);
        }

        Logx.Log(LogxType.Build,"build 完成");
    }

    public static void HandleCommand(string cmd, string value)
    {
        // UnityEditor.WindowsStandalone.WinPlayerPostProcessor.PrepareForBuild
        if (cmd == "is_build_package")
        {
            buildArgs.isBuildPackage = bool.Parse(value);
        }
        else if (cmd == "is_build_ab")
        {
            buildArgs.isBuildAB = bool.Parse(value);
        }
        else if (cmd == "product_name")
        {
            buildArgs.productName = value;
        }
        else if (cmd == "version")
        {
            //PlayerSettings.bundleVersion = value;
            buildArgs.version = value;
        }
        else if (cmd == "platform")
        {
            if (value == "Windows")
            {
                buildArgs.buildTarget = BuildTarget.StandaloneWindows64;
            }
            else if (value == "Android")
            {
                buildArgs.buildTarget = BuildTarget.Android;
            }
            else if (value == "IOS")
            {
                buildArgs.buildTarget = BuildTarget.iOS;
            }
        }
        else if (cmd == "channel")
        {
            buildArgs.channel = value;
        }
        else if (cmd == "res_version")
        {
            buildArgs.resVersion = value;
        }
        else if (cmd == "pre_compile_defines")
        {
            buildArgs.preCompileDefines = value;
        }
        else if (cmd == "upload_ab_Type")
        {
            buildArgs.uploadABType = (UploadABType)int.Parse(value);
        }
    }
}