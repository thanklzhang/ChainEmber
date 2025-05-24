using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using System.IO;


namespace Battle
{
    //战斗地图相关
    public partial class Battle
    {
        public Map GetMap()
        {
            return battleMapMgr.GetMap();
        }

        public bool IsOutOfMap(int x, int y)
        {
            return this.battleMapMgr.IsOutOfMap(x, y);
        }
        public bool IsOutOfMapF(float x, float y)
        {
            return this.battleMapMgr.IsOutOfMapF(x, y);
        }

        public bool IsObstacle(int x, int y)
        {
            return this.battleMapMgr.IsObstacle(x, y);
        }

        public int GetMapSizeX()
        {
            return this.battleMapMgr.MapSizeX;
        }

        public int GetMapSizeZ()
        {
            return this.battleMapMgr.MapSizeX;
        }

        public List<Vector3> GetAroundEmptyPos(BattleEntity entity,int count)
        {
            return this.battleMapMgr.GetAroundEmptyPos(entity,count);
        }

        public Vector3 GetValidMapPos(Vector3 nextPos)
        {
            return this.battleMapMgr.GetValidMapPos(nextPos);
        }
    }
}