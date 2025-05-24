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
    //单位实体相关
    public partial class Battle
    {
        public Dictionary<int, BattleEntity> GetAllEntities(bool isIncludeDeath = false)
        {
            return this.battleEntityMgr.GetAllEntity(isIncludeDeath);
        }

        public BattleEntity FindEntity(int guid, bool isIncludeDeath = false)
        {
            return battleEntityMgr.FindEntity(guid, isIncludeDeath);
        }

        public List<BattleEntity> CreateEntities(List<EntityInit> entityInitList)
        {
            return this.battleEntityMgr.CreateEntities(entityInitList);
        }

        public BattleEntity FindNearestEnemyEntity(BattleEntity entity)
        {
            BattleEntity minDisEntity = null;
            float minDis = 9999999;
            var allEntities = GetAllEntities();
            foreach (var item in allEntities)
            {
                var currEntity = item.Value;
                var sqrtDis = Vector3.SqrtDistance(currEntity.position, entity.position);

                if (sqrtDis <= minDis && currEntity.Team != entity.Team)
                {
                    minDis = sqrtDis;
                    minDisEntity = currEntity;
                }
            }

            return minDisEntity;
        }

        public void SetEntitiesShowState(List<int> entityGuids, bool isShow)
        {
            this.battleEntityMgr.SetEntitiesShowState(entityGuids, isShow);
        }

        public bool CheckMoveCollision(BattleEntity battleEntity, out BattleEntity collisionEntity)
        {
            return this.collisionMgr.IsMoveCollision(battleEntity, out collisionEntity);
        }

        //当一个实体正常移动时发生碰撞的时候
        public void OnEntityMoveCollision(int checkEntityGuid, int collisionEntityGuid, bool isNeedCollisionWait)
        {
            var entity = this.FindEntity(checkEntityGuid);
            if (entity != null)
            {
                if (!isNeedCollisionWait)
                {
                    //如果不需要碰撞等待的话 就直接寻路
                    // var ai = this.aiMgr.FindAI(entity.guid);
                    // ai.FindPathByCurrPath();
                    entity.FindPathByCurrPath();
                }
            }
        }

        public bool IsHaveEntityOnPos(int x,int y)
        {
            return this.battleEntityMgr.IsHaveEntityOnPos(x,y);
        }
        
        // public void EntityContinueFindPath(BattleEntity battleEntity)
        // {
        //     // var ai = this.aiMgr.FindAI(battleEntity.guid);
        //     // ai.FindPathByCurrPath();
        // }


        // public BaseAI FindAI(int guid)
        // {
        //     return aiMgr.FindAI(guid);
        // }

        // public void RegisterEntityAI(BattleEntity entity)
        // {
        //     //var isPlayerCtrl = entity.playerIndex >= 0;
        //     var isPlayerCtrl = entity.isPlayerCtrl;
        //     BaseAI ai = null;
        //     if (isPlayerCtrl)
        //     {
        //         //ai = new PlayerAI();
        //     }
        //     else
        //     {
        //         ai = new CpuAI();
        //     }
        //
        //     if (ai != null)
        //     {
        //         this.aiMgr.UseAIToEntity(ai, entity);
        //     }
        // }
    }
}