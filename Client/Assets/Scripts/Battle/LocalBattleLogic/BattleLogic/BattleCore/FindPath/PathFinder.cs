using System;
using System.Collections;
using System.Collections.Generic;
namespace Battle
{
    public class Pos
    {
        public int x;
        public int y;
        public int type;
        public Pos parent;

        public int G;
        public int H;
        public int F;
    }
    public class PathFinder
    {
        Map map;
        List<List<Pos>> posMap;

        int width;
        int height;

        const int normalPrice = 10;
        const int highPrice = 14;

        public void Init(Map map)
        {
            this.map = map;

            width = this.map.mapNodes.Count;
            height = this.map.mapNodes[0].Count;
        }

        public int Clamp(int x, int min, int max)
        {
            x = Math.Max(min, x);
            x = Math.Min(max, x);
            return x;
        }

        public List<Pos> Find(int startX, int startY, int endX, int endY, List<Pos> dynamicObstaclePos)
        {
            List<Pos> resultPosList = new List<Pos>();

            //clamp
            startX = Clamp(startX, 0, width - 1);
            startY = Clamp(startY, 0, height - 1);
            endX = Clamp(endX, 0, width - 1);
            endY = Clamp(endY, 0, height - 1);

            //生成临时地图
            posMap = new List<List<Pos>>();
            for (int i = 0; i < this.map.mapNodes.Count; i++)
            {
                var list = this.map.mapNodes[i];
                List<Pos> posList = new List<Pos>();
                for (int j = 0; j < list.Count; j++)
                {
                    var node = list[j];

                    Pos pos = new Pos();
                    pos.x = node.x;
                    pos.y = node.y;
                    pos.type = (int)node.nodeType;

                    //动态障碍
                    if (dynamicObstaclePos != null)
                    {
                        if (dynamicObstaclePos.Exists(d => d.x == pos.x && d.y == pos.y))
                        {
                            pos.type = 1;
                        }
                    }

                    posList.Add(pos);
                }
                posMap.Add(posList);
            }



            //找到 startPos 点周围 8 个相邻点(非障碍) 加入到 openList 中
            HashSet<Pos> openList = new HashSet<Pos>();
            HashSet<Pos> closeList = new HashSet<Pos>();
            var startPos = posMap[startX][startY];
            var endPos = posMap[endX][endY];

            openList.Add(startPos);

            int tInt = 0;
            //反复执行以下步骤 ----------------------------
            Pos currPos = null;
            while (true)
            {
                tInt = tInt + 1;
                if (tInt > 100000)
                {
                    BattleLog.LogWarning("the count is too many");
                    break;
                }


                //直到 openList 中包含了终点 , 或者 openList 为空了 , 就跳出步骤 
                var isContainsEndPos = IsExistInList(endPos, openList);
                var isOpenListNull = (0 == openList.Count);

                if (isContainsEndPos)
                {
                    //如果 openList 中包含了目的地 ，那么到终点时按照父节点的指向 即寻路的结果
                    resultPosList = GetPathByReverseParent(endPos);
                    break;
                }
                else if (isOpenListNull)
                {
                    //如果 openList 为空了 ， 那么说明不能到达目的地
                    //对于不能走到的点 那么就选择距离最近的点//对于不能走到的点 那么就选择距离最近的点
                    //Battle_Log.Log("cant move hero ");
                    resultPosList = GetNearestPosByNoMove(currPos, endPos, closeList);
                    break;
                }


                //从 openList 中找到 F 最小的一个节点 作为当前节点 currPos
                var minFNode = GetMinFPos(openList);
                if (null == minFNode)
                {
                    break;
                }
                currPos = minFNode;
                //找到 currPos 点周围 有效相邻点
                var aroundList = GetAroundEnableNodes(currPos, closeList);
                //currPos 加入到 closeList 中
                closeList.Add(currPos);
                openList.Remove(currPos);
                //currPos 点周围相邻点开始筛选(有效相邻点) 

                for (int i = 0; i < aroundList.Count; i++)
                {
                    var tNode = aroundList[i];

                    //判断当前的点是否已经在 openList 中
                    var isExistInOpenList = IsExistInList(tNode, openList);
                    if (isExistInOpenList)
                    {
                        //如果其中的点已经在 openList 中 , 那么判断 (currPos 的 G) + (currPos 到该点 的 G) 是否小于该点的 G
                        var aG = currPos.G + GetGValue(currPos, tNode);
                        var bG = tNode.G;
                        var isLessThan = aG < bG;

                        if (isLessThan)
                        {
                            //如果小于 , 那么该点的父节点设置为 currPos ,并且重新计算 G H F
                            tNode.parent = currPos;
                            tNode.G = aG;
                            tNode.H = GetHValue(tNode, endPos);
                            tNode.F = tNode.G + tNode.H;
                        }
                        else
                        {
                            //否则 什么也不做
                        }
                    }
                    else
                    {
                        //如果其中的点不在 openList 中 , 那么加入到 openList 中 , 并且设置父节点为 currPos , 并计算 G H F
                        openList.Add(tNode);
                        tNode.parent = currPos;
                        tNode.G = currPos.G + GetGValue(currPos, tNode);
                        tNode.H = GetHValue(tNode, endPos);
                        tNode.F = tNode.G + tNode.H;
                    }
                }

            }
            //----------------------------------------------

            //对于有些直接能够到达的点 如果中间没有障碍的话 那么就可以优化路径 直接连接两个点
            resultPosList = FileterNoObstaclePosList(resultPosList);

            return resultPosList;
        }

        //过滤掉三个不同方向的点 如果中带你没有障碍 那么会变成两个点
        public List<Pos> FileterNoObstaclePosList(List<Pos> posList)
        {
            List<Pos> tPosList = new List<Pos>();

            if (posList.Count <= 2)
            {
                return posList;
            }

            int index0 = 0;
            int index1 = 1;
            int index2 = 2;
            tPosList.Add(posList[index0]);

            while (true)
            {
                if (index2 >= posList.Count)
                {
                    tPosList.Add(posList[index1]);
                    break;
                }
                var startPos = posList[index0];
                var middletPos = posList[index1];
                var endPos = posList[index2];

                var isHaveObstable = IsHaveObstacleBetweenTwoPos(startPos, endPos);
                if (isHaveObstable)
                {
                    tPosList.Add(middletPos);
                    index0 = index1;
                    index1 = index2;
                    index2 += 1;
                }
                else
                {
                    index1 = index2;
                    index2 += 1;
                }
            }

            return tPosList;
        }

        //两个点的直线间是否有障碍
        public bool IsHaveObstacleBetweenTwoPos(Pos pos0, Pos pos1)
        {
            var isHave = false;

            float w = 0.5f;
            float h = 0.5f;

            var dir_x = pos1.x - pos0.x;
            var dir_y = pos1.y - pos0.y;

            var minX = pos0.x + w;
            var maxX = pos1.x + w;
            var minY = pos0.y + h;
            var maxY = pos1.y + h;

            if (Math.Abs(dir_x) > Math.Abs(dir_y))
            {
                if (dir_x < 0)
                {
                    //转化
                    var t = minX;
                    minX = maxX;
                    maxX = t;

                    t = minY;
                    minY = maxY;
                    maxY = t;
                }

                //得到一元二次函数的参数
                var k = (maxY - minY) / (float)(maxX - minX);
                var b = minY - (float)(k * minX);

                //寻找障碍
                for (float i = minX; i <= maxX - 1; i = i + 1.0f)
                {
                    var currY = k * i + b;
                    var checkX = (int)i;
                    var checkY = (int)currY;
                    if (IsObstacle(checkX, checkY))
                    {
                        isHave = true;
                        break;
                    }
                    else
                    {
                        //判断对角的另外两个点是否有障碍
                        var nextCheckX = (int)(i + 1);
                        var nextCheckY = (int)(k * (i + 1) + b);

                        if (1 == Math.Abs(checkX - nextCheckX) && 1 == Math.Abs(checkY - nextCheckY))
                        {
                            var sPos = this.posMap[checkX][checkY];
                            var ePos = this.posMap[nextCheckX][nextCheckY];
                            if (!CheckLongNodeMove(sPos, ePos))
                            {
                                isHave = true;
                                break;
                            }
                        }
                    }
                }
            }
            else
            {
                if (dir_y < 0)
                {
                    //转化
                    var t = minX;
                    minX = maxX;
                    maxX = t;

                    t = minY;
                    minY = maxY;
                    maxY = t;
                }

                if (minX == maxX)
                {
                    //斜率为 0 的情况
                    for (float i = minY; i <= maxY; i = i + 1.0f)
                    {

                        var checkX = (int)minX;
                        var checkY = (int)i;
                        if (IsObstacle(checkX, checkY))
                        {
                            isHave = true;
                            break;
                        }
                    }
                }
                else
                {
                    //得到一元二次函数的参数
                    var k = (maxY - minY) / (float)(maxX - minX);
                    var b = minY - (float)(k * minX);

                    //寻找障碍
                    for (float i = minY; i <= maxY - 1; i = i + 1.0f)
                    {
                        var currX = (i - b) / k;
                        var checkX = (int)currX;
                        var checkY = (int)i;

                        if (IsObstacle(checkX, checkY))
                        {
                            isHave = true;
                            break;
                        }
                        else
                        {
                            //判断对角的另外两个点是否有障碍
                            var nextCheckX = (int)((i + 1 - b) / k);
                            var nextCheckY = (int)(i + 1);

                            if (1 == Math.Abs(checkX - nextCheckX) && 1 == Math.Abs(checkY - nextCheckY))
                            {
                                var sPos = this.posMap[checkX][checkY];
                                var ePos = this.posMap[nextCheckX][nextCheckY];
                                if (!CheckLongNodeMove(sPos, ePos))
                                {
                                    isHave = true;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            return isHave;
        }


        //获得有效相邻点
        public List<Pos> GetAroundEnableNodes(Pos pos, HashSet<Pos> closeList)
        {
            List<Pos> posList = new List<Pos>();

            var x = pos.x;
            var y = pos.y;

            int len = 1;
            for (int i = x - len; i <= x + len; i++)
            {
                for (int j = y - len; j <= y + len; j++)
                {
                    if (i != x || j != y)
                    {
                        bool isCanMove = false;
                        var endPos = GetPos(i, j);
                        isCanMove = IsCanMoveToAroundPos(pos, endPos);
                        if (isCanMove)
                        {
                            var node = this.posMap[i][j];
                            if (!closeList.Contains(node))
                            {
                                posList.Add(node);
                            }
                        }
                    }
                }
            }

            return posList;
        }


        public bool IsObstacle(int x, int y)
        {
            var currPos = posMap[x][y];
            if (1 == currPos.type)
            {
                return true;
            }
            return false;
        }


        public Pos GetPos(int x, int y)
        {
            Pos pos = null;
            if (x >= 0 && x < width && y >= 0 && y < height)
            {
                pos = this.posMap[x][y];
                return pos;
            }

            return pos;
        }

        //是否能够移动到周围的某点 (周围 8 格)
        public bool IsCanMoveToAroundPos(Pos startPos, Pos endPos)
        {
            if (null == endPos)
            {
                return false;
            }
            var xDelta = Math.Abs(startPos.x - endPos.x);
            var yDelta = Math.Abs(startPos.y - endPos.y);
            var sub = xDelta + yDelta;
            var isLongSide = sub >= 2;

            if (isLongSide)
            {
                return CheckLongNodeMove(startPos, endPos);
            }
            else
            {
                return CheckNormalNodeMove(startPos, endPos);
            }

        }
        //检查正常的四个相邻格子
        public bool CheckNormalNodeMove(Pos startPos, Pos endPos)
        {
            var isCanMove = endPos.type != 1;
            return isCanMove;
        }


        //判断否能移动到斜边的某一点
        //斜边周围只要有一个障碍 就不能走
        public bool CheckLongNodeMove(Pos startPos, Pos endPos)
        {
            //非起点和终点的两个点
            var posA = this.GetPos(startPos.x, endPos.y);
            var posB = this.GetPos(endPos.x, startPos.y);

            if (1 == endPos.type || 1 == posA.type || 1 == posB.type)
            {
                return false;
            }
            return true;
        }

        //在给定 list 中 获得最小 F 值的点
        public Pos GetMinFPos(HashSet<Pos> posList)
        {
            Pos minPos = null;
            int minF = 99999999;
            var enumerator = posList.GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (enumerator.Current.F <= minF)
                {
                    minPos = enumerator.Current;
                    minF = enumerator.Current.F;
                }

            }
            //for (int i = 0; i < posList.Count; i++)
            //{
            //    var currPos = posList[i];
            //    if (currPos.F <= minF)
            //    {
            //        minPos = currPos;
            //        minF = currPos.F;
            //    }

            //}
            return minPos;
        }

        //某点是否存在于 pos list 中
        bool IsExistInList(Pos pos, HashSet<Pos> posList)
        {
            return posList.Contains(pos);
        }

        //获得从 a 到 b 的 移动代价 G 值(只限周围 1 格)
        int GetGValue(Pos a, Pos b)
        {
            int G = normalPrice;
            int x = Math.Abs(b.x - a.x);
            int y = Math.Abs(b.y - a.y);
            if (x + y >= 2)
            {
                G = highPrice;
            }

            return G;
        }

        //获得 从 a 到 b 的 估算成本 H 值
        int GetHValue(Pos a, Pos b)
        {
            return (Math.Abs(a.x - b.x) + Math.Abs(a.y - b.y)) * normalPrice;
        }

        //根据终点的 parent 反向组成路径
        List<Pos> GetPathByReverseParent(Pos endPos)
        {
            List<Pos> posList = GetPosListByEndPos(endPos);
            posList.Reverse();
            return posList;
        }

        //对于不能走的点 获得距离最近的可移动点作为移动目标点
        List<Pos> GetNearestPosByNoMove(Pos stopPos, Pos endPos, HashSet<Pos> hasUsedList)
        {
            float minDis = 999999;
            Pos minPos = null;
            foreach (var item in hasUsedList)
            {
                var xDelta = (float)item.x - endPos.x;
                var yDelta = (float)item.y - endPos.y;
                var sqrtDis = xDelta * xDelta + yDelta * yDelta;
                if (sqrtDis < minDis)
                {
                    minDis = sqrtDis;
                    minPos = item;
                }
            }
            var resultPosList = GetPosListByEndPos(minPos);
            resultPosList.Reverse();
            return resultPosList;
        }

        //根据终点 pos 获得列表
        List<Pos> GetPosListByEndPos(Pos endPos)
        {
            List<Pos> posList = new List<Pos>();

            var curr = endPos;
            int tInt = 0;
            while (curr != null)
            {

                tInt = tInt + 1;
                if (tInt > 100000)
                {
                    BattleLog.LogWarning("too ......");
                    break;
                }

                posList.Add(curr);
                curr = curr.parent;

            }

            return posList;
        }


    }

}
