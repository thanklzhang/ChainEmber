using System;

public static class NetUtil
{
    /// <summary>
    /// 判断字符串是否为网络URL
    /// </summary>
    /// <param name="url">要检查的字符串</param>
    /// <returns>是否是网络URL</returns>
    public static bool IsNetworkUrl(string url)
    {
        if (string.IsNullOrEmpty(url))
            return false;
        return url.StartsWith("http://") || url.StartsWith("https://") || url.StartsWith("www.");
    }
} 