using System;
using System.Collections.Generic;
using System.Numerics;
using UnityEditorInternal;

namespace Battle
{
    public class MapSaveData
    {
        public List<List<int>> mapList;
        public List<float[]> enemyInitPosList;
        public List<float[]> playerInitPosList;

        public string ToJson()
        {
            return "";
        }

        public void FromJson(string json)
        {
        }
    }

    public class MapInitArg
    {
        //public int[][] map;
        //public int mapSizeX;
        //public int mapSizeZ;

        public List<List<int>> mapList;
        public List<Vector3> enemyInitPosList;
        public List<Vector3> playerInitPosList;
    }


    public enum MapNodeType
    {
        Null = 0,

        //正常节点 可移动
        Normal = 1,

        //障碍 不能移动
        Obstacle = 2
    }

    //public class MapNode
    //{
    //    public int x;
    //    public int y;

    //    public MapNodeType type;
    //}

    public class BattleMapMgr
    {
        int mapSizeX;
        int mapSizeZ;

        public int MapSizeX
        {
            get => mapSizeX;
        }

        public int MapSizeZ
        {
            get => mapSizeZ;
        }

        Map map;

        //MapData mapData;
        public void Init(MapInitArg arg)
        {
            //var mapInfo = arg.map;

            //mapData = new MapData();
            //mapData.Load(mapInfo);

            //this.mapSizeX = arg.mapSizeX;
            //this.mapSizeZ = arg.mapSizeZ;

            map = new Map();

            //test
            //var mapInfo = new int[][]
            //{
            //    //new int[]{ 1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,},
            //    //new int[]{ 1,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,1,},
            //    //new int[]{ 1,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,1,},
            //    //new int[]{ 1,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,1,0,1,},
            //    //new int[]{ 1,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,1,0,1,},
            //    //new int[]{ 1,0,0,1,1,1,0,0,0,0,0,0,0,0,0,0,0,1,0,1,},
            //    //new int[]{ 1,0,0,0,0,0,0,0,0,0,1,1,1,1,0,0,0,1,0,1,},
            //    //new int[]{ 1,0,0,1,0,0,0,0,0,0,1,0,0,0,0,0,0,1,0,1,},
            //    //new int[]{ 1,0,1,0,0,0,0,0,0,0,1,1,0,0,0,0,0,1,0,1,},
            //    //new int[]{ 1,1,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,1,0,1,},
            //    //new int[]{ 1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,1,},
            //    //new int[]{ 1,0,0,1,1,1,1,0,0,0,0,0,0,0,0,0,0,1,0,1,},
            //    //new int[]{ 1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,1,},
            //    //new int[]{ 1,0,0,0,0,1,1,1,0,0,0,0,0,0,0,0,0,1,0,1,},
            //    //new int[]{ 1,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,1,},
            //    //new int[]{ 1,0,0,0,0,0,0,0,0,0,0,1,0,0,1,0,0,0,0,1,},
            //    //new int[]{ 1,0,0,0,1,0,0,0,1,0,0,0,0,0,1,0,0,0,0,1,},
            //    //new int[]{ 1,0,0,0,0,0,0,0,1,0,0,0,0,0,1,0,0,0,0,1,},
            //    //new int[]{ 1,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,1,},
            //    //new int[]{ 1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,},

            //    new int[]{ 1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,},
            //    new int[]{ 1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,},
            //    new int[]{ 1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,},
            //    new int[]{ 1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,},
            //    new int[]{ 1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,},
            //    new int[]{ 1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,},
            //    new int[]{ 1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,},
            //    new int[]{ 1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,},
            //    new int[]{ 1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,},
            //    new int[]{ 1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,},
            //    new int[]{ 1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,},
            //    new int[]{ 1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,},
            //    new int[]{ 1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,},
            //    new int[]{ 1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,},
            //    new int[]{ 1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,},
            //    new int[]{ 1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,},
            //    new int[]{ 1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,},
            //    new int[]{ 1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,},
            //    new int[]{ 1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,},
            //    new int[]{ 1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,},


            //};

            //显示数组转换成数据数组
            //var logicArray = arg.mapList;
            var list = arg.mapList;
            //List<List<int>> logicArray = new List<List<int>>();

            //for (int i = 0; i < list[i].Count; i++)
            //{
            //    logicArray.Add(new List<int>());
            //    for (int j = 0; j < list.Count; j++)
            //    {
            //        var state = list[j][list[i].Count - i - 1];
            //        logicArray[i].Add(state);
            //    }
            //}


            //map.Init(logicArray);

            map.Init(list, arg.enemyInitPosList, arg.playerInitPosList);
        }

        internal void Update()
        {
        }

        internal Map GetMap()
        {
            return map;
        }

        public bool IsOutOfMap(int x, int y)
        {
            return map.IsOutOfMap(x, y);
        }
        
        public bool IsOutOfMapF(float x, float y)
        {
            return map.IsOutOfMapF(x, y);
        }

        public bool IsObstacle(int x, int y)
        {
            return map.IsObstacle(x, y);
        }

        // 获取 nextPos 的 x, z 在地图内的合理坐标
        public Vector3 GetValidMapPos(Vector3 nextPos)
        {
            float x = nextPos.x;
            float z = nextPos.z;
            
            // 将坐标限制在地图边界内
            x = MathTool.ClampF(x, 0, map.GetXMaxLimit());
            z = MathTool.ClampF(z, 0, map.GetYMaxLimit());
            
            return new Vector3(x, nextPos.y, z);
        }

        //获得实体周围几个空点
        //优先获取近距离，相同距离的话优先选择在单位的朝向上的点
         public List<Vector3> GetAroundEmptyPos(BattleEntity entity, int count)
         {
             var entityDir = entity.dir;
            List<Vector3> resultList = new List<Vector3>();
            var entityX = (int)entity.position.x;
            var entityZ = (int)entity.position.z;

            if (IsOutOfMap(entityX, entityZ))
            {
                return resultList;
            }

            List<PathNode> hasCheckList = new List<PathNode>();
            var xMaxLimit = map.GetXMaxLimit();
            var YMaxLimit = map.GetYMaxLimit();
            
            //逐渐向周围扩散检测
            var maxDis = xMaxLimit > YMaxLimit ? xMaxLimit : YMaxLimit;
            for (int checkDis = 1; checkDis <= maxDis; checkDis++)
            {
                var startX = entityX - checkDis;
                startX = MathTool.Clamp(startX, 0, xMaxLimit);

                var endX = entityX + checkDis;
                endX = MathTool.Clamp(endX, 0, xMaxLimit);

                var startY = entityZ - checkDis;
                startY = MathTool.Clamp(startY, 0, YMaxLimit);

                var endY = entityZ + checkDis;
                endY = MathTool.Clamp(endY, 0, YMaxLimit);

                var nodes = map.mapNodes;

                var currAroundList = new List<Vector3>();
                bool isFinish = false;
                for (int i = startX; i <= endX; i++)
                {
                    for (int j = startY; j <= endY; j++)
                    {
                        var currNode = nodes[i][j];

                        //只检测当前的一圈
                        bool isCheck = false;
                        if (i == startX || i == endX)
                        {
                            isCheck = true;
                        }
                        else
                        {
                            if (j == startY || j == endY)
                            {
                                isCheck = true;
                            }
                        }

                        if (!isCheck)
                        {
                            continue;
                        }

                        //判断当前点有障碍和实体 ，有的话不能创建
                        var battle = entity.GetBattle();
                        var isHaveEntity = battle.IsHaveEntityOnPos(currNode.x, currNode.y);
                        var isObstacle = map.IsObstacle(currNode.x, currNode.y);
                        if (!isHaveEntity && !isObstacle)
                        {
                            currAroundList.Add(new Vector3(currNode.x, 0, currNode.y));
                        }
                    }
                }
                
                //按照'格子到单位点的方向' 和 '单位朝向' 的点乘排序（值越大 越靠近单位方向）
                currAroundList.Sort((a, b) =>
                {
                    var aDotValue = Vector3.Dot(a, entityDir);
                    var bDotValue = Vector3.Dot(b, entityDir);
                    return bDotValue.CompareTo(aDotValue);

                });
                
                resultList.AddRange(currAroundList);
                if (resultList.Count >= count)
                {
                    break;
                }
            }
            return resultList;
        }
         
         
        
        // public List<Vector3> GetAroundEmptyPos(BattleEntity entity, int count)
        // {
        //     List<Vector3> list = new List<Vector3>();
        //
        //     var entityX = (int)entity.position.x;
        //     var entityZ = (int)entity.position.z;
        //
        //     if (IsOutOfMap(entityX, entityZ))
        //     {
        //         return list;
        //     }
        //
        //     List<PathNode> hasCheckList = new List<PathNode>();
        //     var xMaxLimit = map.GetXMaxLimit();
        //     var YMaxLimit = map.GetYMaxLimit();
        //     
        //     var maxDis = xMaxLimit > YMaxLimit ? xMaxLimit : YMaxLimit;
        //     for (int checkDis = 1; checkDis <= maxDis; checkDis++)
        //     {
        //         var startX = entityX - checkDis;
        //         startX = MathTool.Clamp(startX, 0, xMaxLimit);
        //
        //         var endX = entityX + checkDis;
        //         endX = MathTool.Clamp(endX, 0, xMaxLimit);
        //
        //         var startY = entityZ - checkDis;
        //         startY = MathTool.Clamp(startY, 0, YMaxLimit);
        //
        //         var endY = entityZ + checkDis;
        //         endY = MathTool.Clamp(endY, 0, YMaxLimit);
        //
        //         var nodes = map.mapNodes;
        //
        //         for (int i = startX; i <= endX; i++)
        //         {
        //             for (int j = startY; j <= endY; j++)
        //             {
        //                 var currNode = nodes[i][j];
        //                 if (!hasCheckList.Contains(currNode))
        //                 {
        //                     var battle = entity.GetBattle();
        //                     var isHaveEntity = battle.IsHaveEntityOnPos(currNode.x, currNode.y);
        //                     var isObstacle = map.IsObstacle(currNode.x, currNode.y);
        //                     if (!isHaveEntity && !isObstacle)
        //                     {
        //                         list.Add(new Vector3(currNode.x, 0, currNode.y));
        //                         if (list.Count >= count)
        //                         {
        //                             return list;
        //                         }
        //                     }
        //
        //                     hasCheckList.Add(currNode);
        //                 }
        //             }
        //         }
        //     }
        //
        //
        //     return list;
        // }
    }
}