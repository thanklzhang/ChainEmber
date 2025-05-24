using System.Collections;
using System.Collections.Generic;
using System.Security.Permissions;
using UnityEngine;


public enum ABStrategyType
{
    Null = 0,
    //目录下所有的文件 包括所有层子文件夹 每个文件一个包
    OneByOneFile = 1,

    //该目录所有文件打成一个包
    AllInOneByFolder = 2,

    //该目录所有一级文件夹打成一个包
    AllInOneBySubFolder = 3,
}


[System.Serializable]
public class ABStrategyOption
{
    public Object path;
    public ABStrategyType type;
}

public class ABPackageStrategySO : ScriptableObject
{
     public List<ABStrategyOption> strategyList;
}