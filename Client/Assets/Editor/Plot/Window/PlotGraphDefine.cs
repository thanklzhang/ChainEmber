using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;
using System.Text;
using LitJson;

public class PlotGraphDefine
{
    public static int timeSpanCountPerSeconds = 10;//1s的刻度数量
    public static float lenPerTimeSpan = 5.0f;//刻度显示长度
    public static int showSecondsCount = 10;//显示多少秒

    //轨道
    public static float trackHeight = 30.0f;
    public static float trackPlayWidth = 550.0f;
    public static float trackSpaceY = 5.0f;
}
