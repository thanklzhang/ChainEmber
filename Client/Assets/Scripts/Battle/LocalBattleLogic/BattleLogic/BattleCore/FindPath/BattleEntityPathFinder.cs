using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace Battle
{
  
    public class BattleEntityPathFinder
    {
        BattleEntity entity;

        //TODO 这里寻路应该只有一份
        PathFinder pathFinder;
        List<Pos> currPosList;
        int currPosIndex;

        public List<Pos> GetCurrPosList()
        {
            return currPosList;
        }

        public void Init(BattleEntity entity)
        {
            currPosList = new List<Pos>();
            pathFinder = new PathFinder();
            
            currPosIndex = 0;
            
            this.entity = entity;
        }

        public void InitMap(Map map)
        {
            // var map = entity.GetBattle().GetMap();
            pathFinder.Init(map);
        }

        public bool IsFindingPath()
        {
            return currPosList.Count > 0;
        }

        public List<Pos> FindPath(Vector3 targetPos)
        {
            var battle = this.entity.GetBattle();
            var entityList = battle.GetAllEntities();
            var currPos = this.entity.position;
            var intCurPos = GetIntPos(currPos);

            var aIntPos = intCurPos;

            //周围单位算作动态障碍
            List<Pos> dynamicList = new List<Pos>();
            foreach (var kv in entityList)
            {
                var currEntity = kv.Value;
                if (currEntity == this.entity)
                {
                    continue;
                }

                var currEntityPos = currEntity.position;
                var bIntPos = new Pos()
                {
                    x = (int)currEntityPos.x,
                    y = (int)currEntityPos.z
                };

                var xLen = Math.Abs(aIntPos.x - bIntPos.x);
                var yLen = Math.Abs(aIntPos.y - bIntPos.y);

                if (xLen + yLen <= 4)
                {
                    dynamicList.Add(bIntPos);
                }
            }

            //var endPos = this.currPosList[this.currPosList.Count - 1];
            ////Logx.Log("collision , change next path : start:" + GetPosStr(aIntPos) + " end:" +
            ////    GetPosStr(endPos) + " dynamicList.Count : " + dynamicList.Count);
            //FindPathTest.instance.FindPath(this, aIntPos, endPos, dynamicList);


            Pos aPos = GetIntPos(entity.position);
            Pos bPos = GetIntPos(targetPos);
            var currFindNodes = pathFinder.Find(aPos.x, aPos.y, bPos.x, bPos.y, dynamicList);
            if (currFindNodes.Count >= 2)
            {
                //移除本身 保留终点
                currFindNodes.RemoveAt(0);
            }
            currPosList = currFindNodes;

            string str = "find path test : path : ";

            for (int i = 0; i < currPosList.Count; i++)
            {
                var p = currPosList[i];
                str += string.Format("({0},{1}) -> ", p.x, p.y);
            }
            //_Battle_Log.Log(str);

            currPosIndex = 0;

            return currPosList;

        }

        //internal bool FindPath(Vector3 targetPos, out Vector3 findPos)
        //{
        //    var battle = this.entity.GetBattle();
        //    var entityList = battle.GetAllEntities();
        //    var currPos = this.entity.position;
        //    var intCurPos = GetIntPos(currPos);

        //    var aIntPos = intCurPos;

        //    //周围单位算作动态障碍
        //    List<Pos> dynamicList = new List<Pos>();
        //    foreach (var kv in entityList)
        //    {
        //        var currEntity = kv.Value;
        //        if (currEntity == this.entity)
        //        {
        //            continue;
        //        }

        //        var currEntityPos = currEntity.position;
        //        var bIntPos = new Pos()
        //        {
        //            x = (int)currEntityPos.x,
        //            y = (int)currEntityPos.z
        //        };

        //        var xLen = Math.Abs(aIntPos.x - bIntPos.x);
        //        var yLen = Math.Abs(aIntPos.y - bIntPos.y);

        //        if (xLen + yLen <= 2)
        //        {
        //            dynamicList.Add(bIntPos);
        //        }
        //    }

        //    //var endPos = this.currPosList[this.currPosList.Count - 1];
        //    ////Logx.Log("collision , change next path : start:" + GetPosStr(aIntPos) + " end:" +
        //    ////    GetPosStr(endPos) + " dynamicList.Count : " + dynamicList.Count);
        //    //FindPathTest.instance.FindPath(this, aIntPos, endPos, dynamicList);


        //    Pos aPos = GetIntPos(entity.position);
        //    Pos bPos = GetIntPos(targetPos);
        //    var currFindNodes = pathFinder.Find(aPos.x, aPos.y, bPos.x, bPos.y, dynamicList);
        //    if (currFindNodes.Count >= 2)
        //    {
        //        //移除本身 保留终点
        //        currFindNodes.RemoveAt(0);
        //    }
        //    currPosList = currFindNodes;

        //    string str = "find path test : path : ";

        //    for (int i = 0; i < currPosList.Count; i++)
        //    {
        //        var p = currPosList[i];
        //        str += string.Format("({0},{1}) -> ", p.x, p.y);
        //    }
        //    _Battle_Log.Log(str);


        //    //设置第一个移动点目标
        //    currPosIndex = 0;

        //    var isHavePos = currPosList.Count > 0;

        //    findPos = Vector3.zero;
        //    if (isHavePos)
        //    {
        //        findPos = GetCenterPos(currPosList[currPosIndex]);
        //    }
        //    return isHavePos;
        //}

        public void ClearPath()
        {
            currPosIndex = 0;
            currPosList.Clear();
        }


        public bool TryToGetNextStepPos(out Vector3 nextStepPos)
        {
            var isFinishAllStepPos = false;
            nextStepPos = Vector3.zero;
            currPosIndex = currPosIndex + 1;
            if (currPosIndex < currPosList.Count)
            {
                isFinishAllStepPos = true;
                nextStepPos = GetCenterPos(currPosList[currPosIndex]);
            }
            return isFinishAllStepPos;
        }


        public void Update()
        {

        }

       public  Pos GetIntPos(Vector3 pos)
        {
            var x = (int)pos.x;
            var y = (int)pos.z;
            Pos p = new Pos()
            {
                x = x,
                y = y
            };
            return p;
        }

        public Vector3 GetCenterPos(Pos pos)
        {
            return new Vector3()
            {
                x = pos.x + Map.CellWidth / 2,
                y = 0,
                z = pos.y + Map.CellHeight / 2,
            };
        }

    }
}
