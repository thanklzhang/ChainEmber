using System.Collections.Generic;
using Battle;
using UnityEngine;

public class ColorDefine
{
    public static Dictionary<RewardQuality, Color> colorDic
        = new Dictionary<RewardQuality, Color>()
        {
            { RewardQuality.Green,Color.green},
            { RewardQuality.Blue,Color.blue},
            { RewardQuality.Purple,Color.magenta},
            { RewardQuality.Orange,new Color(255 / 255/0f
                ,152 / 255.0f,0)},
            { RewardQuality.Red,Color.red},
        };

    public static Color GetColorByQuality(RewardQuality quality)
    {
        var color = colorDic[RewardQuality.Green];
        if (colorDic.ContainsKey(quality))
        {
            return colorDic[quality];
        }

        return color;
    }

}