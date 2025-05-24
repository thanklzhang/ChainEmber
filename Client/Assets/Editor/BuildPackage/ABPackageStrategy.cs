using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;
using System.Text;
using LitJson;

public class StrategyInfo
{
    public string path;
    public ABStrategyType type;
}

public class ABPackageStrategy
{
    List<StrategyInfo> strategyList = new List<StrategyInfo>();

    public List<AssetBundleBuild> StartParse()
    {
        var resPath = GlobalConfig.ABPackageStrategyPath;
        var strategySO = AssetDatabase.LoadAssetAtPath<ABPackageStrategySO>(resPath);
        foreach (var option in strategySO.strategyList)
        {
            var path = AssetDatabase.GetAssetPath(option.path);
            
            var newObj = new StrategyInfo()
            {
                path = path,
                type = option.type
            };
            strategyList.Add(newObj);
        }

        //
        //
        // strategyList.Add(new StrategyInfo()
        // {
        //     path = Const.projectRootPath + "/" + "BuildRes/Prefabs/Models",
        //     type = ABStrategyType.OneByOneFile
        // });
        //
        // strategyList.Add(new StrategyInfo()
        // {
        //     path = Const.projectRootPath + "/" + "BuildRes/Prefabs/UI",
        //     type = ABStrategyType.OneByOneFile
        // });
        //
        // strategyList.Add(new StrategyInfo()
        // {
        //     path = Const.projectRootPath + "/" + "BuildRes/Prefabs/Scene",
        //     type = ABStrategyType.OneByOneFile
        // });
        //
        // strategyList.Add(new StrategyInfo()
        // {
        //     path = Const.projectRootPath + "/" + "SourceRes/Models/Role",
        //     type = ABStrategyType.AllInOneBySubFolder
        // });
        //
        // strategyList.Add(new StrategyInfo()
        // {
        //     path = Const.projectRootPath + "/" + "BuildRes/Prefabs/Effects",
        //     type = ABStrategyType.OneByOneFile
        // });
        //
        // strategyList.Add(new StrategyInfo()
        // {
        //     path = Const.projectRootPath + "/" + "BuildRes/Textures/BG",
        //     type = ABStrategyType.OneByOneFile
        // });
        //
        // strategyList.Add(new StrategyInfo()
        // {
        //     path = Const.projectRootPath + "/" + "BuildRes/Textures/Item",
        //     type = ABStrategyType.OneByOneFile
        // });
        //
        // strategyList.Add(new StrategyInfo()
        // {
        //     path = Const.projectRootPath + "/" + "SourceRes/Models/Build/Model",
        //     type = ABStrategyType.AllInOneByFolder
        // });
        //
        // strategyList.Add(new StrategyInfo()
        // {
        //     path = Const.projectRootPath + "/" + "BuildRes/Table",
        //     type = ABStrategyType.OneByOneFile
        // });
        //
        // strategyList.Add(new StrategyInfo()
        // {
        //     path = Const.projectRootPath + "/" + "BuildRes/BattleTrigger",
        //     type = ABStrategyType.OneByOneFile
        // });


        List<AssetBundleBuild> buildList = new List<AssetBundleBuild>();
        for (int i = 0; i < strategyList.Count; i++)
        {
            var strategyInfo = strategyList[i];
            var currBuildList = this.Parse(strategyInfo.path, strategyInfo.type);
            buildList.AddRange(currBuildList);
        }

        return buildList;
    }

    public List<AssetBundleBuild> Parse(string path, ABStrategyType type)
    {
        var buildList = new List<AssetBundleBuild>();
        if (type == ABStrategyType.OneByOneFile)
        {
            buildList = HandleOneByOneFile(path, type);
        }

        if (type == ABStrategyType.AllInOneByFolder)
        {
            buildList = HandleAllInOneByFolder(path, type);
        }

        if (type == ABStrategyType.AllInOneBySubFolder)
        {
            buildList = AllInOneBySubFolder(path, type);
        }

        return buildList;
    }

    //目录下所有的文件 包括所有层子文件夹 每个文件一个包
    public List<AssetBundleBuild> HandleOneByOneFile(string path, ABStrategyType type)
    {
        List<AssetBundleBuild> buildList = new List<AssetBundleBuild>();

        var files = Directory.GetFiles(path, "*", SearchOption.AllDirectories);

        for (int i = 0; i < files.Length; i++)
        {
            var file = files[i];
            var ext = Path.GetExtension(file);
            if (!IgnorePackageExt.IsCanPackageAsset(ext))
            {
                continue;
            }

            AssetBundleBuild build = new AssetBundleBuild();

            var abName = file.Replace(ext, GlobalConfig.ABExtName).Replace("\\", "/");

            build.assetBundleName = abName;
            var assetName = file.Replace("\\", "/");
            build.assetNames = new string[] { assetName };
            buildList.Add(build);
        }

        return buildList;
    }

    //该目录所有文件打成一个包
    public List<AssetBundleBuild> HandleAllInOneByFolder(string path, ABStrategyType type)
    {
        List<AssetBundleBuild> buildList = new List<AssetBundleBuild>();

        var files = Directory.GetFiles(path, "*", SearchOption.AllDirectories);

        //var abName = path.Replace(Const.projectRootPath + "/", "") + Const.ExtName;
        AssetBundleBuild build = new AssetBundleBuild();
        var bundleName = path.Replace("\\", "/");
        List<string> assetNameList = new List<string>();
        build.assetBundleName = bundleName + GlobalConfig.ABExtName;

        for (int i = 0; i < files.Length; i++)
        {
            var file = files[i];
            var ext = Path.GetExtension(file);

            if (!IgnorePackageExt.IsCanPackageAsset(ext))
            {
                continue;
            }

            var assetName = file.Replace("\\", "/");
            assetNameList.Add(assetName);
            //build.assetNames = new string[] { assetName };
            //buildList.Add(build);
        }

        build.assetNames = assetNameList.ToArray();
        buildList.Add(build);

        return buildList;
    }

    //该目录所有一级文件夹 每个文件夹打成一个包
    public List<AssetBundleBuild> AllInOneBySubFolder(string path, ABStrategyType type)
    {
        List<AssetBundleBuild> buildList = new List<AssetBundleBuild>();

        var dirs = Directory.GetDirectories(path, "*", SearchOption.TopDirectoryOnly);
        for (int i = 0; i < dirs.Length; i++)
        {
            var dir = dirs[i];
            var buildRange = HandleAllInOneByFolder(dir, ABStrategyType.AllInOneByFolder);
            buildList.AddRange(buildRange);
        }

        return buildList;
    }
}