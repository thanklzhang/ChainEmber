using Battle;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathProxy
{
    PathFinder finder;
    internal void Init(Map map)
    {
        finder = new PathFinder();
        finder.Init(map);
    }

    public List<Pos> Find(int startX, int startY, int endX, int endY, List<Pos> dynamicList)
    {
        return finder.Find(startX, startY, endX, endY, dynamicList);
    }

}
