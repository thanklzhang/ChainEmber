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
    //获取信息的部分
    public partial class Battle
    {
        public List<BattleEntity> GetEntitiesInCircle(Vector3 center, float radius)
        {
            var selectEntities = new List<BattleEntity>();
            foreach (var kv in this.GetAllEntities())
            {
                var entity = kv.Value;
                var sqrtDis = Vector3.SqrtDistance(center, entity.position);

                var range = radius;
                var calDis = range * range;
                if (sqrtDis <= calDis)
                {
                    selectEntities.Add(entity);
                }
            }

            return selectEntities;
        }

        //根据某个实体的角度来筛选（srcEntity 为关系起点者）
        public List<BattleEntity> GetEntitiesInCircleAtEntity(BattleEntity srcEntity,
            Vector3 center, float radius, EntityRelationFilterType selectType)
        {
            var selectEntities = new List<BattleEntity>();
            foreach (var kv in this.GetAllEntities())
            {
                var entity = kv.Value;

                var relations = srcEntity.GetRelationWith(entity);

                var isSuit = BattleEntity.IsSuitEntityRelationType(selectType, relations);
                if (isSuit)
                {
                    var sqrtDis = Vector3.SqrtDistance(center, entity.position);

                    var range = radius;
                    var calDis = range * range;
                    if (sqrtDis <= calDis)
                    {
                        selectEntities.Add(entity);
                    }
                }
            }

            return selectEntities;
        }

        public List<BattleEntity> GetRandAroundEntityByPos(Vector3 center,float radius, int count,
            List<BattleEntity> excludeEntities,BattleEntity referEntity,EntityRelationFilterType relationType)
        {
            List<BattleEntity> list = new List<BattleEntity>();
            foreach (var kv in this.GetAllEntities())
            {
                var entity = kv.Value;

                // Battle_Log.LogZxy($"redirect : get index 1");
                
                //Battle_Log.LogZxy($"redirect : check entity :{entity.guid}");
                
                if (excludeEntities != null &&
                    excludeEntities.Contains(entity))
                {
                    //Battle_Log.LogZxy($"redirect : has in excludeEntities : {entity.guid}");
                    continue;
                }
                
                // Battle_Log.LogZxy($"redirect : get index 2");
                
                var relations = referEntity.GetRelationWith(entity);
                //Battle_Log.LogZxy($"redirect : relationType：{relationType}   relations：{relations[0]}");
                var isSuit = BattleEntity.IsSuitEntityRelationType(relationType, relations);

                if (!isSuit)
                {
                    //Battle_Log.LogZxy($"redirect :not suit");
                    continue;
                }

                // Battle_Log.LogZxy($"redirect : suit! get index 3");

                var sqrDis = (entity.position - center).sqrMagnitude;

                // Battle_Log.LogZxy($"redirect : sqrDis:{sqrDis} radius * radius : {radius * radius}");
                
                if (sqrDis <= radius * radius)
                {
                    list.Add(entity);
                    //Battle_Log.LogZxy($"redirect : add to list : {entity.guid}");
                }
            }

            var resultList = BattleRandom.GetRandEntityList(list, 0, list.Count, count, this);
            
            //Battle_Log.LogZxy($"redirect : resultList.count: {resultList.Count}");

            
            return resultList;
        }
        
        
    }
}