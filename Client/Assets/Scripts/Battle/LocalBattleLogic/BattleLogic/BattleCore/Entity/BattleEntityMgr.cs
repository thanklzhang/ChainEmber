using System;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    public class BattleEntityMgr
    {
        Dictionary<int, BattleEntity> entityDic = new Dictionary<int, BattleEntity>();

        Battle battle;

        private BattleEntityInitArg arg;
            
        public void Init(BattleEntityInitArg arg, Battle battle)
        {
            this.battle = battle;
            this.arg = arg;
        }

        //相关组件都已经初始化完毕 可以组件间互相引用调用了
        public void SetRelativeInfo()
        {
          
            //填充 entityDic
            //目前作为一开始就有实体的处理
            for (int i = 0; i < arg.entityInitList.Count; i++)
            {
                //注意 这里是初始化实体 不会发送创建实体事件
                var entityInit = arg.entityInitList[i];
                BattleEntity entity = AddEntity(entityInit);

                // //设置是否是受玩家控制的英雄(目前一个玩家只能操控一个英雄实体)
                // if (entityInit.isPlayerCtrl)
                // {
                //     battle.SetPlayerCtrlEntity(entity.playerIndex, entity);
                // }
            }
            
        }

        //激活 entity ， 可以认为是所有战斗组件初始化后的生命周期函数 
        void ActiveEntity(BattleEntity entity)
        {
            // var entity = kv.Value;
            // //注意 这里是初始化实体 不会发送创建实体事件
            // var entityInit = arg.entityInitList[i];
            // BattleEntity entity = AddEntity(entityInit);

            //设置是否是受玩家控制的英雄(目前一个玩家只能操控一个英雄实体)
            if (entity.isPlayerCtrl)
            {
                battle.InitPlayerCtrlEntity(entity.playerIndex, entity);
            }

            entity.SetSkillInitinfo();
            entity.SetAISkillInfo();
            //星级在技能设置之后
            entity.AddInitStarExp();
        }

        //添加实体
        private BattleEntity AddEntity(EntityInit entityInit)
        {
            // _Battle_Log.Log((int)LogxType.BattleItem,"add a entity");

            BattleEntity entity = new BattleEntity();
           
            entity.guid = this.GenGuid();
            
            entity.configId = entityInit.configId;
            entity.playerIndex = entityInit.playerIndex;
            entity.position = entityInit.position;
            
            //如果产生点旁边有人，那么就在旁边找一个空的位置生成
            if (IsHaveEntityOnPos((int)entity.position.x, (int)entity.position.z))
            {
                entity.position = FindEmptyPosAround(entity.position);
                entityInit.position = entity.position;
            }
            
            //面向玩家
            if (!entity.isPlayerCtrl) // 如果不是玩家控制的单位（即敌人）
            {
                var playerList = battle.GetAllPlayers();
                foreach (var player in playerList)
                {
                    if (player.IsPlayerCtrl && player.entity != null) // 寻找玩家控制的实体
                    {
                        var playerEntity = player.entity;
                        
                        // 检查实体是否已经死亡
                        if (!playerEntity.IsDead())
                        {
                            // 计算从敌人到玩家的方向向量
                            Vector3 dirToPlayer = (playerEntity.position - entity.position).normalized;
                            // 设置敌人的朝向
                            entity.SetDir(dirToPlayer,false);
                            break; // 找到活着的玩家实体后跳出循环
                        }
                        // 如果该玩家实体已死亡，继续查找下一个
                    }
                }
            }

            entityDic.Add(entity.guid, entity);

            //初始化 entity 自身
            entity.Init(entityInit, battle);
            
          
            ActiveEntity(entity);

            // battle.RegisterEntityAI(entity);
            
            //test
            entity.TestInit();
            //

            return entity;
        }

        public List<BattleEntity> CreateEntities(List<EntityInit> entityInitList)
        {
            List<BattleEntity> list = new List<BattleEntity>();
            foreach (var entityInit in entityInitList)
            {
                var entity = this.AddEntity(entityInit);
                list.Add(entity);
            }

            this.battle.OnCreateEntities(list);

            //同步战斗数据
            foreach (var item in list)
            {
                var entity = item;
                entity.SyncBattleData();
            }

            return list;
        }

        int maxGuid = 1;

        private int GenGuid()
        {
            return maxGuid++;
        }

        internal BattleEntity FindEntity(int guid, bool isIncludeDeath = false)
        {
            if (entityDic.ContainsKey(guid))
            {
                var entity = entityDic[guid];
                if (!isIncludeDeath)
                {
                    if (entity.IsDead())
                    {
                        return null;
                    }
                }

                return entityDic[guid];
            }

            return null;
        }

        public void Update(float timeDelta)
        {
            //移除 entity
            List<BattleEntity> delList = new List<BattleEntity>();
            foreach (var item in entityDic)
            {
                var entity = item.Value;
                var state = entity.EntityState;

                //死亡目前先移除 之后如果有对尸体做操作在进行更改
                if (entity.IsTrueDead())
                {
                    if (!delList.Contains(entity))
                    {
                        delList.Add(entity);
                    }
                }
            }

            //update
            foreach (var item in entityDic)
            {
                var entity = item.Value;
                entity.Update(timeDelta);
            }
        }

        internal Dictionary<int, BattleEntity> GetAllEntity(bool isIncludeDeath = false)
        {
            Dictionary<int, BattleEntity> dic = new Dictionary<int, BattleEntity>();
            foreach (var kv in entityDic)
            {
                if (!isIncludeDeath)
                {
                    if (!kv.Value.IsDead())
                    {
                        dic.Add(kv.Key, kv.Value);
                    }
                }
                else
                {
                    dic.Add(kv.Key, kv.Value);
                }
            }

            return dic;
        }

        public void RemoveEntity(int guid)
        {
            if (entityDic.ContainsKey(guid))
            {
                var entity = entityDic[guid];
                entity.Clear();
                entityDic.Remove(guid);
            }
            else
            {
                //_G.LogWarning("BattleEntityMgr RemoveEntity : the guid is not found : " + guid);
            }
        }

        internal void SetEntitiesShowState(List<int> entityGuids, bool isShow)
        {
            //List<BattleEntity> list = new List<BattleEntity>();
            foreach (var guid in entityGuids)
            {
                var entity = this.FindEntity(guid);
                if (entity != null)
                {
                    entity.SetShowState(isShow);
                    //list.Add(entity);
                }
                else
                {
                    //_G.LogWarning("BattleEntityMgr SetEntitiesShowState : the guid is not found : " + guid);
                }
            }

            this.battle.OnSetEntitiesShowState(entityGuids, isShow);
        }

        //该点是否有单位（TODO：效率问题，应该是用表实时存着，而不是每次都算）
        public bool IsHaveEntityOnPos(int targetX,int targetZ)
        {
            
            foreach (var kv in entityDic)
            {
                var entity = kv.Value;
                var x = (int)entity.position.x;
                var z = (int)entity.position.z;

                if (x == targetX && z == targetZ)
                {
                    return true;
                }
            }

            return false;
        }

        //在指定位置周围寻找一个空位置
        public Vector3 FindEmptyPosAround(Vector3 originPos)
        {
            // 定义搜索的方向（上、右、下、左、右上、右下、左下、左上）
            int[] dx = { 0, 1, 0, -1, 1, 1, -1, -1 };
            int[] dz = { 1, 0, -1, 0, 1, -1, -1, 1 };
            
            int originX = (int)originPos.x;
            int originZ = (int)originPos.z;
            
            // 先检查周围8个方向
            for (int i = 0; i < 8; i++)
            {
                int newX = originX + dx[i];
                int newZ = originZ + dz[i];
                
                // 检查位置是否合法且没有实体
                if (!battle.IsOutOfMap(newX, newZ) && 
                    !battle.IsObstacle(newX, newZ) && 
                    !IsHaveEntityOnPos(newX, newZ))
                {
                    return new Vector3(newX, originPos.y, newZ);
                }
            }
            
            // 如果周围8个方向都没有空位，向外扩展搜索范围
            int searchRadius = 2;
            int maxSearchRadius = 5; // 最大搜索半径
            
            while (searchRadius <= maxSearchRadius)
            {
                // 搜索当前半径的圆周上的点
                for (int x = originX - searchRadius; x <= originX + searchRadius; x++)
                {
                    for (int z = originZ - searchRadius; z <= originZ + searchRadius; z++)
                    {
                        // 只检查圆周上的点
                        if (Math.Abs(x - originX) == searchRadius || Math.Abs(z - originZ) == searchRadius)
                        {
                            // 检查位置是否合法且没有实体
                            if (!battle.IsOutOfMap(x, z) && 
                                !battle.IsObstacle(x, z) && 
                                !IsHaveEntityOnPos(x, z))
                            {
                                return new Vector3(x, originPos.y, z);
                            }
                        }
                    }
                }
                
                searchRadius++;
            }
            
            // 如果实在找不到空位，返回原始位置
            return originPos;
        }
    }
}