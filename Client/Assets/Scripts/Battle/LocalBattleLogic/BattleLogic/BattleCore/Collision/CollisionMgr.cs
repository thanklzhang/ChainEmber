using System;
using System.Collections.Generic;

namespace Battle
{
    //碰撞管理器
    //--暂定地图格子长度和宽度都为 1
    public class CollisionMgr
    {
        Battle battle;

        public void Init(Battle battle)
        {
            this.battle = battle;
        }

        Dictionary<int, HashSet<int>> collisionDic = new Dictionary<int, HashSet<int>>();
        //正常的单位之间的碰撞检测
        public void CheckCollisionForAllEntities()
        {
            var entities = battle.GetAllEntities();
            foreach (var item in entities)
            {
                var aEntity = item.Value;
                if (!aEntity.IsDead())
                {
                    foreach (var item2 in entities)
                    {
                        var bEntity = item2.Value;
                        if (!bEntity.IsDead())
                        {
                            if (aEntity.guid != bEntity.guid)
                            {
                                var preIsCollision = false;
                                if (collisionDic.ContainsKey(aEntity.guid))
                                {
                                    var preOtherGuidHash = collisionDic[aEntity.guid];
                                    preIsCollision = preOtherGuidHash.Contains(bEntity.guid);
                                }

                                if (collisionDic.ContainsKey(bEntity.guid))
                                {
                                    var preOtherGuidHash = collisionDic[bEntity.guid];
                                    preIsCollision = preOtherGuidHash.Contains(aEntity.guid);
                                }

                                var nowIsCollision = CheckCollisionBetweenEntity(aEntity, bEntity);
                                if (!preIsCollision && nowIsCollision)
                                {
                                    aEntity.OnEnterCollision(bEntity);
                                    bEntity.OnEnterCollision(aEntity);

                                    if (!collisionDic.ContainsKey(aEntity.guid))
                                    {
                                        collisionDic.Add(aEntity.guid, new HashSet<int>() { bEntity.guid });
                                    }
                                    else
                                    {
                                        var h = collisionDic[aEntity.guid];
                                        if (!h.Contains(bEntity.guid))
                                        {
                                            h.Add(bEntity.guid);
                                        }
                                    }
                                    if (!collisionDic.ContainsKey(bEntity.guid))
                                    {
                                        collisionDic.Add(bEntity.guid, new HashSet<int>() { aEntity.guid });
                                    }
                                    else
                                    {
                                        var h = collisionDic[bEntity.guid];
                                        if (!h.Contains(aEntity.guid))
                                        {
                                            h.Add(aEntity.guid);
                                        }
                                    }
                                }
                                else if (preIsCollision && !nowIsCollision)
                                {

                                    aEntity.OnExitCollision(bEntity);
                                    bEntity.OnExitCollision(aEntity);
                                    if (collisionDic.ContainsKey(aEntity.guid))
                                    {
                                        if (collisionDic[aEntity.guid].Contains(bEntity.guid))
                                        {
                                            collisionDic[aEntity.guid].Remove(bEntity.guid);
                                        }
                                    }
                                    if (collisionDic.ContainsKey(bEntity.guid))
                                    {
                                        if (collisionDic[bEntity.guid].Contains(aEntity.guid))
                                        {
                                            collisionDic[bEntity.guid].Remove(aEntity.guid);
                                        }
                                    }

                                }
                            }
                        }
                    }
                }
            }
        }

        //检查两个单位之间是否碰撞
        public bool CheckCollisionBetweenEntity(BattleEntity aEntity, BattleEntity bEntity)
        {
            var aCollisionCircle = aEntity.collisionCircle;
            var bCollisionCircle = bEntity.collisionCircle;

            var sqrDis = (aEntity.position - bEntity.position).SqrDistanceOnXZ();

            return sqrDis < (aCollisionCircle * aCollisionCircle + bCollisionCircle * bCollisionCircle);
        }

        //根据传进来的实体 guid 来获得和他正在碰撞的实体
        public HashSet<int> GetCollisionEntityGuids(int entityGuid)
        {
            if (collisionDic.ContainsKey(entityGuid))
            {
                return collisionDic[entityGuid];
            }
            return null;
        }

        const float RayLength = 0.75f;
        const float ExploreEndR = 0.65f;
        // //移动碰撞检测 带有方向
        // //TODO : 单位体积可能不同
        // public bool IsMoveCollision(BattleEntity checkEntity, out BattleEntity collisionEntity)//, out bool isNeedWait
        // {
        //     //碰撞检测
        //     var dir = checkEntity.dir.normalized;
        //     var allEntityList = battle.GetAllEntities();
        //     var currPos = checkEntity.position;
        //     collisionEntity = null;
        //     foreach (var kv in allEntityList)
        //     {
        //         var currEntity = kv.Value;
        //         if (currEntity == checkEntity)
        //         {
        //             continue;
        //         }
        //
        //         //判断前方射线
        //         var explorePos = currPos + dir * RayLength;
        //
        //         var isRayCollision = IsCollisionCircle(explorePos, 0, currEntity.position, ExploreEndR);
        //         if (isRayCollision)
        //         {
        //             collisionEntity = currEntity;
        //             break;
        //         }
        //     }
        //
        //     var isCollision = collisionEntity != null;
        //     
        //     //isNeedWait = false;
        //   
        //     if (isCollision)
        //     {
        //         if (!collisionEntity.IsMoveCollisionWaiting)
        //         {
        //             // //如果碰撞到的实体 B 不在碰撞等待中
        //             // //那么本实体 A 就设置成碰撞等待中
        //             // checkEntity.IsMoveCollisionWaiting = true;
        //         
        //             //isNeedWait = true;
        //         }
        //         else
        //         {
        //             //
        //             //FindPath();
        //         }
        //     }
        //
        //     return isCollision;
        // }
        
        
        
        //移动碰撞检测 带有方向
        //TODO : 单位体积可能不同
        public bool IsMoveCollision(BattleEntity checkEntity, out BattleEntity collisionEntity)//, out bool isNeedWait
        {
            //碰撞检测
            var dir = checkEntity.dir.normalized;
            var allEntityList = battle.GetAllEntities();
            var currPos = checkEntity.position;
            collisionEntity = null;
            foreach (var kv in allEntityList)
            {
                var currEntity = kv.Value;
                if (currEntity == checkEntity)
                {
                    continue;
                }

                //判断前方射线
                var explorePos = currPos + dir * RayLength;

                var isRayCollision = IsCollisionCircle(explorePos, 0, currEntity.position, ExploreEndR);
                if (isRayCollision)
                {
                    collisionEntity = currEntity;
                    break;
                }
            }

            var isCollision = collisionEntity != null;
            
            //isNeedWait = false;
          
            if (isCollision)
            {
                if (!collisionEntity.IsMoveCollisionWaiting)
                {
                    // //如果碰撞到的实体 B 不在碰撞等待中
                    // //那么本实体 A 就设置成碰撞等待中
                    // checkEntity.IsMoveCollisionWaiting = true;
                
                    //isNeedWait = true;
                }
                else
                {
                    //
                    //FindPath();
                }
            }

            return isCollision;
        }
        

        bool IsCollisionCircle(Vector3 pos0, float r0, Vector3 pos1, float r1)
        {
            var centerLength = (pos0 - pos1).sqrMagnitude;
            var rLengh = (r0 + r1) * (r0 + r1);
            return centerLength < rLengh;
        }

        public void Update(float timeDelta)
        {
            CheckCollisionForAllEntities();
        }

    }

}


