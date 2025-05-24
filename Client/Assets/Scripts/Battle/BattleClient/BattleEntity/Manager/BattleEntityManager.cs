using System.Collections.Generic;
using UnityEngine;

namespace Battle_Client
{
    public partial class BattleEntityManager : Singleton<BattleEntityManager>
    {
        //guid => entity
        Dictionary<int, BattleEntity_Client> entityDic = new Dictionary<int, BattleEntity_Client>();

        public void Init()
        {
            entityDic = new Dictionary<int, BattleEntity_Client>();
            this.RegisterListener();
        }

        public void RegisterListener()
        {
            //EventDispatcher.AddListener<BattleEntityInfo>(EventIDs.OnCreateBattle, CreateEntity);
        }

        public BattleEntity_Client FindEntityByColliderInstanceId(int instanceID)
        {
            foreach (var item in entityDic)
            {
                var entity = item.Value;
                if (entity.collider != null)
                {
                    if (entity.collider.gameObject.GetInstanceID() == instanceID)
                    {
                        return entity;
                    }
                }
            }

            return null;
        }

        public BattleEntity_Client FindNearestEntity(BattleEntity_Client originEntity, float dis)
        {
            var excludeInsId = originEntity.gameObject.GetInstanceID();
            var pos = originEntity.gameObject.transform.position;
            float minDis = 9999999;
            BattleEntity_Client battleEntity = null;

            foreach (var item in entityDic)
            {
                var entity = item.Value;
                if (entity.collider != null)
                {
                    if (entity.CurrHealth <= 0)
                    {
                        continue;
                    }

                    var currGo = entity.collider.gameObject;

                    var vector = currGo.transform.position - pos;
                    vector = new Vector3(vector.x, 0, vector.z);
                    var currDis = vector.sqrMagnitude;

                    if (excludeInsId != entity.gameObject.GetInstanceID())
                    {
                        // TODO: 这里应该根据筛选目标来进行判断 先按照只选择敌对关系来判断
                        if (entity.Team != originEntity.Team)
                        {
                            if (currDis <= dis * dis)
                            {
                                if (currDis <= minDis)
                                {
                                    minDis = currDis;
                                    battleEntity = entity;
                                }
                            }
                        }
                    }
                }
            }

            return battleEntity;
        }

        public BattleEntity_Client FindEntityByInstanceId(int instanceID)
        {
            foreach (var item in entityDic)
            {
                var entity = item.Value;
                if (entity.gameObject.GetInstanceID() == instanceID)
                {
                    return entity;
                }
            }

            return null;
        }

        public BattleEntity_Client FindEntity(int guid)
        {
            if (entityDic.ContainsKey(guid))
            {
                return entityDic[guid];
            }
            else
            {
                //Logx.LogWarning("the guid is not found : " + guid);
            }

            return null;
        }

        public void SetAllEntityShowState(bool isShow)
        {
            foreach (var kv in this.entityDic)
            {
                var entity = kv.Value;
                entity.SetShowState(isShow);
                EventDispatcher.Broadcast(EventIDs.OnEntityChangeShowState, entity, isShow);
            }
        }

        //这里时只是控制显隐
        internal void SetEntitiesShowState(bool isShow, List<int> entityGuids)
        {
            foreach (var guid in entityGuids)
            {
                var entity = FindEntity(guid);
                if (entity != null)
                {
                    entity.SetShowState(isShow);
                    EventDispatcher.Broadcast(EventIDs.OnEntityChangeShowState, entity, isShow);
                }
            }
        }
        
        public void Update(float timeDelta)
        {
            List<BattleEntity_Client> willDeleteList = new List<BattleEntity_Client>();
            foreach (var item in entityDic)
            {
                var entity = item.Value;
                entity.Update(timeDelta);

                if (entity.State == BattleEntityState.Destroy)
                {
                    willDeleteList.Add(entity);
                }
            }
            foreach (var entity in willDeleteList)
            {
                BattleEntityManager.Instance.DestoryEntity(entity.guid);
            }
        }
        
        public void DestoryEntity(int guid)
        {
            if (entityDic.ContainsKey(guid))
            {
                BattleEntity_Client entity = entityDic[guid];
                entity.Destroy();
                entityDic.Remove(guid);
            }
            else
            {
                Logx.LogWarning("the guid is not found : " + guid);
            }
        }

        public void OnBattleEnd()
        {
            foreach (var item in entityDic)
            {
                var entity = item.Value;
                if (entity.State != BattleEntityState.Dead && entity.State != BattleEntityState.WillDead)
                {
                    entity.State = BattleEntityState.Idle;
                    entity.PlayAnimation("idle");
                }
            }
        }

        public void DestoryAllEntities()
        {
            foreach (var item in entityDic)
            {
                var entity = item.Value;
                entity.Destroy();
            }

            entityDic.Clear();
        }

        public void RemoveListener()
        {
            //EventDispatcher.RemoveListener<BattleEntityInfo>(EventIDs.OnCreateBattle, CreateEntity);
        }

        //战斗清理 可以认为 reset
        public void Clear()
        {
            this.RemoveListener();
            DestoryAllEntities();
        }

        //完全释放
        public void Release()
        {
            
        }
    }
}