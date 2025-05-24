using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace Battle
{
    public partial class BattleEntity
    {
        //得到和谁的关系
        public List<EntityRelationType> GetRelationWith(BattleEntity desEntity)
        {
            List<EntityRelationType> relationTypeList = new List<EntityRelationType>();

            if (this.guid == desEntity.guid)
            {
                relationTypeList.Add(EntityRelationType.Self);
            }

            if (this.Team == desEntity.Team)
            {
                if (this.guid != desEntity.guid)
                {
                    relationTypeList.Add(EntityRelationType.Friend);

                    if (this.IsMySummonEntity(desEntity))
                    {
                        relationTypeList.Add(EntityRelationType.Master_SummonEntity);
                    }
                    else if (desEntity.IsMySummonEntity(this))
                    {
                        relationTypeList.Add(EntityRelationType.SummonEntity_Master);
                    }
                }
            }
            else
            {
                relationTypeList.Add(EntityRelationType.Enemy);
            }

            return relationTypeList;
        }

        public BattleEntity GetNearestEntityFromAll(EntityRelationFilterType selectType)
        {
            var entityDic = battle.GetAllEntities();
            return GetNearestEntity(entityDic, selectType);
        }

        // public List<BattleEntity> GetRandAroundEntity(float radius, int count)
        // {
        //     List<BattleEntity> list = new List<BattleEntity>();
        //     foreach (var kv in battle.GetAllEntities())
        //     {
        //         var entity = kv.Value;
        //         var sqrDis = (entity.position - this.position).sqrMagnitude;
        //
        //         if (radius * radius <= sqrDis)
        //         {
        //             list.Add(entity);
        //         }
        //     }
        //
        //     return BattleRandom.GetRandEntityList(list, 0, list.Count, count, this.battle);
        // }


        //获得和自身相匹配关系的单位列表
        public List<BattleEntity> GetRelationEntitiesFromAll(EntityRelationFilterType selectType)
        {
            List<BattleEntity> list = new List<BattleEntity>();
            var dic = battle.GetAllEntities();
            foreach (var kv in dic)
            {
                var entity = kv.Value;
                var relations = this.GetRelationWith(entity);

                var isSuit = BattleEntity.IsSuitEntityRelationType(selectType, relations);

                if (isSuit)
                {
                    list.Add(entity);
                }
            }

            return list;
        }

        public BattleEntity GetNearestEntity(Dictionary<int, BattleEntity> dic, EntityRelationFilterType selectType)
        {
            if (dic.Count > 0)
            {
                BattleEntity nearestEntity = null;
                float nearestSqrDis = 9999999.0f;
                foreach (var kv in dic)
                {
                    var entity = kv.Value;
                    var relations = this.GetRelationWith(entity);

                    var isSuit = BattleEntity.IsSuitEntityRelationType(selectType, relations);

                    if (isSuit)
                    {
                        var sqrDis = (entity.position - this.position).sqrMagnitude;


                        if (sqrDis <= nearestSqrDis)
                        {
                            nearestSqrDis = sqrDis;
                            nearestEntity = entity;
                        }
                    }
                }

                return nearestEntity;
            }

            return null;
        }

        public bool IsSuitSkillSelectType(EntityRelationFilterType selectType, BattleEntity target)
        {
            var relations = this.GetRelationWith(target);

            return IsSuitEntityRelationType(selectType, relations);
        }

        public static bool IsSuitEntityRelationType(EntityRelationFilterType selectType,
            List<EntityRelationType> relations)
        {
            //TODO 这里也可以用 按位与 操作
            if (selectType == EntityRelationFilterType.All)
            {
                return true;
            }

            foreach (var type in relations)
            {
                if (selectType == EntityRelationFilterType.Self)
                {
                    if (type == EntityRelationType.Self)
                    {
                        return true;
                    }
                }
                else if (selectType == EntityRelationFilterType.Enemy)
                {
                    if (type == EntityRelationType.Enemy)
                    {
                        return true;
                    }
                }
                else if (selectType == EntityRelationFilterType.Friend)
                {
                    if (type == EntityRelationType.Friend)
                    {
                        return true;
                    }
                }
                else if (selectType == EntityRelationFilterType.MeAndFriend)
                {
                    if (type == EntityRelationType.Self ||
                        type == EntityRelationType.Friend)
                    {
                        return true;
                    }
                }
                else if (selectType == EntityRelationFilterType.FriendAndEnemy)
                {
                    if (type == EntityRelationType.Friend ||
                        type == EntityRelationType.Enemy)
                    {
                        return true;
                    }
                }
                else if (selectType == EntityRelationFilterType.Mater_SummonEntity)
                {
                    if (type == EntityRelationType.Master_SummonEntity)
                    {
                        return true;
                    }
                }
                else if (selectType == EntityRelationFilterType.SummonEntity_Master)
                {
                    if (type == EntityRelationType.SummonEntity_Master)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}