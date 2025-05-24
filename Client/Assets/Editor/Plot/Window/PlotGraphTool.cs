using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;
using System.Text;
using LitJson;

public class PlotGraphTool
{
    //根据当前对应时间 获取 相对 剧情播放区域 坐标 
    public static float GetGraphXByTime(float time)
    {
        float timeSpanCount = time / (1.0f / PlotGraphDefine.timeSpanCountPerSeconds);
        var x = timeSpanCount * PlotGraphDefine.lenPerTimeSpan;
        return x;
    }

    //根据 相对 剧情播放区域 坐标  获取 当前对应时间
    public static float GetTimeByGraphX(float x)
    {
        //多少个刻度单位
        float timeSpanCount = x / PlotGraphDefine.lenPerTimeSpan;
        //时间
        float time = timeSpanCount * (1.0f / PlotGraphDefine.timeSpanCountPerSeconds);
        return time;
    }

}
