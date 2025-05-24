using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Threading;
using System.Linq;


public enum LogxType
{
    Null = 0,

    //基本类型(100 -> 200)------------------------
    //游戏进程节点
    Game = 100,

    //网络
    Net = 101,

    //界面
    UI = 102,

    //资源
    Resource = 103,

    //战斗
    Battle = 104,
    
    //构建(Package AB 等)
    Build = 105,
    
    //检查和更新资源
    CheckAndUpdateResource = 106,
    
    //asset 资源
    Asset = 107,
    
    //AssetBundle
    AB = 108,
    
    //Ctrl(模块控制)
    Ctrl = 109,
    
    //SceneCtrl(场景控制)
    SceneCtrl = 110,
    
    //战斗流程
    BattleProcess = 111,
    
    //战斗技能
    BattleSkill = 112,
    
    //战斗道具
    BattleItem = 113,
    
    //战斗宝箱
    BattleBox = 114,
    
    //英雄升星
    HeroStar = 115,
    
    //---------------------------------

    //自定义类型(200 以上)-----------------------------
    Zxy = 200,
    //---------------------------------
}

public class Logx
{
    public class LogxConfigInfo
    {
        public bool enable = false;
    }

    public static bool enable = true;

    public static Dictionary<LogxType, LogxConfigInfo> logConfigDic = new Dictionary<LogxType, LogxConfigInfo>()
    {
        { LogxType.Null, new LogxConfigInfo() { enable = true } },
        { LogxType.Game, new LogxConfigInfo() { enable = true } },
        { LogxType.UI, new LogxConfigInfo() { enable = true } },
        { LogxType.Battle, new LogxConfigInfo() { enable = false } },
        { LogxType.Net, new LogxConfigInfo() { enable = true } },

        { LogxType.Resource, new LogxConfigInfo() { enable = false } },
        
        { LogxType.Build, new LogxConfigInfo() { enable = true } },
        
        {LogxType.CheckAndUpdateResource,new LogxConfigInfo() { enable = true }},
        {LogxType.Asset,new LogxConfigInfo() { enable = false }},
        {LogxType.AB,new LogxConfigInfo() { enable = false }},
        // {LogxType.Ctrl,new LogxConfigInfo() { enable = true }},
        {LogxType.SceneCtrl,new LogxConfigInfo() { enable = true }},
        {LogxType.BattleProcess,new LogxConfigInfo() { enable = false }},
        {LogxType.BattleSkill,new LogxConfigInfo() { enable = true }},
        {LogxType.BattleItem,new LogxConfigInfo() { enable = true }},
        {LogxType.BattleBox,new LogxConfigInfo() { enable = true }},
        {LogxType.HeroStar,new LogxConfigInfo() { enable = true }},
        
        {LogxType.Zxy,new LogxConfigInfo() { enable = true }}
        
    };

    public static bool IsLogTypeEnable(LogxType type)
    {
        if (logConfigDic.ContainsKey(type))
        {
            var info = logConfigDic[type];
            return info.enable;
        }

        return false;
    }

    #region Log-------------------

    public static void Log(object obj = null)
    {
        if (!enable)
        {
            return;
        }

        Debug.Log(obj);
    }

    public static void Log(LogxType type, object obj)
    {
        if (!IsLogTypeEnable(type))
        {
            return;
        }

        Log(type.ToString(), obj);
    }

    public static void Log(string flag, object obj)
    {
        Log(flag + " : " + obj);
    }

    #endregion

    #region Warning-------------------

    public static void LogWarning(object obj)
    {
        // if (!enable)
        // {
        //     return;
        // }
        
        Debug.LogWarning(obj);
    }

    public static void LogWarning(LogxType type, object obj)
    {
        // if (!IsLogTypeEnable(type))
        // {
        //     return;
        // }
        
        LogWarning(type.ToString(), obj);
    }

    public static void LogWarning(string flag, object obj)
    {
        LogWarning(flag + " : " + obj);
    }

    #endregion

    #region Error-------------------

    public static void LogError(object obj)
    {
        // if (!enable)
        // {
        //     return;
        // }
        
        Debug.LogError(obj);
    }


    public static void LogError(LogxType type, object obj)
    {
        // if (!IsLogTypeEnable(type))
        // {
        //     return;
        // }
        
        LogError(type.ToString(), obj);
    }

    public static void LogError(string flag, object obj)
    {
        LogError(flag + " : " + obj);
    }

    #endregion

    public static void LogException(Exception e)
    {
        if (!enable)
        {
            return;
        }
        
        Logx.LogError(e.Message + "\n" + e.StackTrace);
    }
}