using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class HeroCardUIData
{
    public int guid;
    public int configId;
    public int level;
    public bool isUnlock;
}

public class HeroCardUIConvert
{
    public static HeroCardUIData GetUIData(GameData.HeroData heroData)
    {
        HeroCardUIData heroUIData = new HeroCardUIData()
        {
            configId = heroData.configId,
            guid = heroData.guid,
            level = heroData.level,
            isUnlock = true
        };

        return heroUIData;
    }
}