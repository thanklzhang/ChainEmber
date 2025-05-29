using System;
using GameData;
using UnityEngine;

public class UserUtil
{
    /// <summary>
    /// 获取设备唯一标识符
    /// </summary>
    public static string GetDeviceIdentifier()
    {
        // 使用SystemInfo.deviceUniqueIdentifier获取设备唯一标识符
        // 实际项目中可能需要更复杂的处理来确保跨设备唯一性
        string deviceId = SystemInfo.deviceUniqueIdentifier;
        
        // 为防止ID过长，可以截取或Hash处理
        if (deviceId.Length > 20)
        {
            deviceId = deviceId.Substring(0, 20);
        }
        
        // 添加前缀区分设备类型
        string prefix = Application.platform == RuntimePlatform.Android ? "AD_" : 
            Application.platform == RuntimePlatform.IPhonePlayer ? "IOS_" : "PC_";
                         
        return prefix + deviceId;
    }
}