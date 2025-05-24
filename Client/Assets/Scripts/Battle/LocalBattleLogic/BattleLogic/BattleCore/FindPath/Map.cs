using System.Collections;
using System.Collections.Generic;
using Battle;

public enum PathNodeType
{
    Normal = 0,
    Obstacle = 1
}
public class PathNode
{
    public int id;

    public int x;
    public int y;

    public PathNodeType nodeType;
}


public class Map
{
    public const float CellWidth = 1;
    public const float CellHeight = 1;

    public List<List<PathNode>> mapNodes;

    List<List<int>> mapInfo;

    public List<Vector3> enemyInitPosList;
    public List<Vector3> playerInitPosList;

    public void Init(List<List<int>> map,List<Vector3> enemyInitPosList,List<Vector3> playerInitPosList)
    {
        mapInfo = map;
        
        this.enemyInitPosList = enemyInitPosList;
        this.playerInitPosList = playerInitPosList;

        mapNodes = new List<List<PathNode>>();

        //int width = mapInfo[0].Count;
        //int height = mapInfo.Count;
        //for (int i = 0; i < width; i++)
        //{
        //    List<PathNode> nodeList = new List<PathNode>();
        //    for (int j = 0; j < height; j++)
        //    {
        //        PathNode node = new PathNode();
        //        node.x = i;
        //        node.y = j;

        //        node.nodeType = (PathNodeType)(mapInfo[mapInfo.Count - j - 1][i]);
        //        nodeList.Add(node);
        //    }
        //    mapNodes.Add(nodeList);
        //}

        int width = mapInfo[0].Count;
        int height = mapInfo.Count;
        for (int i = 0; i < mapInfo.Count; i++)
        {
            List<PathNode> nodeList = new List<PathNode>();
            for (int j = 0; j < mapInfo[i].Count; j++)
            {
                PathNode node = new PathNode();
                node.x = i;
                node.y = j;

                node.nodeType = (PathNodeType)(mapInfo[i][j]);
                nodeList.Add(node);
            }
            mapNodes.Add(nodeList);
        }
    }

    public int GetXMaxLimit()
    {
        return mapNodes.Count - 1;
    }
    
    public int GetYMaxLimit()
    {
        return mapNodes[0].Count - 1;
    }

    public bool IsOutOfMap(int x, int y)
    {
        if (x < 0 || x > mapNodes.Count - 1)
        {
            return true;
        }

        if (y < 0 || y > mapNodes[0].Count - 1)
        {
            return true;
        }

        return false;

    }
    
    public bool IsOutOfMapF(float x, float y)
    {
        if (x < 0 || x > mapNodes.Count - 1)
        {
            return true;
        }

        if (y < 0 || y > mapNodes[0].Count - 1)
        {
            return true;
        }

        return false;

    }

    public bool IsObstacle(int x, int y)
    {
        if (IsOutOfMap(x, y))
        {
            return false;
        }

        return mapNodes[x][y].nodeType == PathNodeType.Obstacle;
    }
}
