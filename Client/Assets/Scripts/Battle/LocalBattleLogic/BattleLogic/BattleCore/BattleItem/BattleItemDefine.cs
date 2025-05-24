using System.Collections.Generic;


namespace Battle
{
    //叠层类型
    public enum ItemAddLayerType
    {
        //不可叠加
        No = 0,
        //可叠加 被动效果叠加(目前没有)
        AddLayerAndEffect = 1,
        //可叠加 被动效果不叠加
        AddLayerWithoutEffect = 2,
    }
}