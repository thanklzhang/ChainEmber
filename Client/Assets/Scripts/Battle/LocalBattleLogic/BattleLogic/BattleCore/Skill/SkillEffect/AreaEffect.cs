using System.Collections.Generic;
using System.Linq;

namespace Battle
{
    public enum CenterType
    {
        SkillReleaser = 0,
        SkillTarget = 1
    }

    public enum StartPosType
    {
        Context = 0,
    }

    public enum AreaType
    {
        Circle = 0,
        Rectangle = 1,
        Sector = 2
    }

    public enum StartPosShiftDirType
    {
        Null = 0,

        //释放技能者 到 技能目标(或技能目标点) 的方向
        FromReleaserToTarget = 1,

        //初始技能释放者的坐标点（点不会因为移动着移动而改变） 到 初始技能目标（或技能目标点）的方向（同理也不会改变
        FromInitReleaserToInitTarget = 20,
    }

    public enum ExcludeEntityType
    {
        Null = 0,
        ContextEntity = 1,
    }

    public class AreaEffect : SkillEffect
    {
        //public Config.AreaEffect tableConfig;
        public IAreaEffect tableConfig;
        Vector3 centerPos;
        Battle battle;

        public override void OnInit()
        {
            battle = this.context.battle;
            tableConfig = BattleConfigManager.Instance.GetById<IAreaEffect>(this.configId);
        }

        public override CreateEffectInfo ToCreateEffectInfo()
        {
            var config = BattleConfigManager.Instance.GetById<IAreaEffect>(this.configId);

            var resId = config.EffectResId;
            if (resId > 0)
            {
                isAutoDestroy = true;
            }

            //TODO : 增加上下文的参数：选择点
            var centerPos = context.fromSkill.releaser.position;
            if (context.selectPositions is { Count: > 0 })
            {
                centerPos = context.selectPositions[0];
            }
            else if (context.selectEntities is { Count: > 0 })
            {
                if (context.selectEntities[0] != null)
                {
                    centerPos = context.selectEntities[0].position;
                }
                else
                {
                    BattleLog.LogWarningZxy($"the selectEntity is null : this.configId ： {this.configId}");
                }
            }

            var position = centerPos;

            CreateEffectInfo createInfo = new CreateEffectInfo();
            createInfo.guid = guid;
            createInfo.resId = resId;
            createInfo.effectPosType = EffectPosType.Custom_Pos;
            createInfo.createPos = position;

            createInfo.followEntityGuid = -1;
            createInfo.isAutoDestroy = isAutoDestroy;

            return createInfo;
        }

        public override void OnStart()
        {
            base.OnStart();

            this.centerPos = context.fromSkill.releaser.position;
            var startPosType = (StartPosType)tableConfig.StartPosType;
            if (startPosType == StartPosType.Context)
            {
                if (context.selectPositions is { Count: > 0 })
                {
                    centerPos = context.selectPositions[0];
                }
                else if (context.selectEntities is { Count: > 0 })
                {
                    if (context.selectEntities[0] != null)
                    {
                        centerPos = context.selectEntities[0].position;
                    }
                    else
                    {
                        BattleLog.LogWarningZxy($"the selectEntity is null : this.configId ： {this.configId}");
                    }
                }
            }

            var battle = this.context.battle;
            // var allEntities = battle.GetAllEntities();

            var selectType = (EntityRelationFilterType)tableConfig.EntityRelationFilterType;
            var suitEntities = this.context.fromSkill.releaser.GetRelationEntitiesFromAll(selectType);
            var areaType = tableConfig.AreaType;
            // var startPosType = (StartPosType)tableConfig.StartPosType;
            var startPosShiftType = (StartPosShiftDirType)tableConfig.StartPosShiftDirType;

            List<BattleEntity> selectEntities = new List<BattleEntity>();


            foreach (var entity in suitEntities)
            {
                Vector3 dir = Vector3.right; // 使用Vector3.right代替Vector3.UnitX
                if (startPosShiftType == StartPosShiftDirType.FromReleaserToTarget)
                {
                    var targetPos = context.fromSkill.targetPos;
                    if (context.fromSkill.targetGuid > 0)
                    {
                        var targetEntity = battle.FindEntity(context.fromSkill.targetGuid);
                        if (targetEntity != null)
                        {
                            targetPos = targetEntity.position;
                        }
                    }

                    dir = (targetPos - context.fromSkill.releaser.position);
                    dir = new Vector3(dir.x, 0, dir.z).normalized;
                }
                else if (startPosShiftType == StartPosShiftDirType.FromInitReleaserToInitTarget)
                {
                    var targetPos = context.initTargetPos;
                    dir = (targetPos - context.initReleaserPos);
                    dir = new Vector3(dir.x, 0, dir.z).normalized;
                }


                if ((AreaType)areaType == AreaType.Circle)
                {
                    var center = centerPos + dir * (tableConfig.StartPosShiftDistance / 1000.0f);
                    center.y = 0;
                

                    var sqrtDis = Vector3.SqrtDistance(center, entity.position);

                    var range = tableConfig.RangeParam[0] / 1000.0f;
                    var calDis = range * range;
                    //_G.Log("AreaEffect OnStart : " + sqrtDis + " ? " + calDis);
                    if (sqrtDis <= calDis)
                    {
                        selectEntities.Add(entity);
                    }
                }
                else if ((AreaType)areaType == AreaType.Rectangle)
                {
                    Rect rect = new Rect();


                    var width = tableConfig.RangeParam[0] / 1000.0f;
                    var height = tableConfig.RangeParam[1] / 1000.0f;

                    rect.center = centerPos.ToVector2ByXZ() +
                                  dir.ToVector2ByXZ() *
                                  (width / 2.0f + tableConfig.StartPosShiftDistance / 1000.0f);
                    rect.width = width;
                    rect.height = height;

                    rect.widthDir = dir.ToVector2ByXZ().normalized;
                    rect.heightDir = (Vector3.Cross(rect.widthDir.ToVector3ByXZ(), Vector3.up) // 使用Vector3.up替代Vector3.UnitY
                            .normalized)
                        .ToVector2ByXZ();

                    Circle circle = new Circle();
                    //缩小下判断半径 防止擦个边都生效这种情况
                    circle.radius = entity.collisionCircle / 3.0f;
                    circle.center = new Vector2(entity.position.x, entity.position.z);

                    var isCollision = CollisionTool.CheckBoxAndCircle(rect, circle);
                    if (isCollision)
                    {
                        selectEntities.Add(entity);
                    }
                }
                else if ((AreaType)areaType == AreaType.Sector)
                {
                    // 扇形区域判断
                    // 获取中心点位置（可能需要偏移）
                    var center = centerPos + dir * (tableConfig.StartPosShiftDistance / 1000.0f);
                    center.y = 0;
                    
                    // 从配置获取扇形半径和角度
                    var radius = tableConfig.RangeParam[0] / 1000.0f;
                    var angle = tableConfig.RangeParam[1];
                    var halfAngle = angle * 0.5f;
                    
                    // 尝试可视化扇形区域（只在客户端运行）
                    #if !PURE_LOGIC_SERVER
                    // try
                    {
                        // 通过反射调用可视化工具，避免直接依赖
                        var bridgeType = System.Type.GetType("BattleClient.Debug.SectorAreaVisualizerBridge");
                       // if (bridgeType != null)
                        {
                            var visualizeBattleSectorMethod = bridgeType.GetMethod("VisualizeBattleSector");
                            //if (visualizeBattleSectorMethod != null)
                            {
                                visualizeBattleSectorMethod.Invoke(null, new object[] { center, radius, angle, dir });
                            }
                        }
                    }
                    // catch (System.Exception e)
                    {
                        // 在纯逻辑环境下忽略可视化异常
                        // Battle_Log.LogError("Failed to visualize sector area: " + e.Message);
                    }
                    #endif
                    
                    // 判断实体是否在扇形范围内
                    if (IsEntityInSector(center, radius, halfAngle, dir, entity))
                    {
                        selectEntities.Add(entity);
                    }
                }
            }

            var excludeType = (ExcludeEntityType)tableConfig.ExcludeEntityType;
            if (excludeType == ExcludeEntityType.ContextEntity)
            {
                if (context.selectEntities.Count > 0)
                {
                    var entity = context.selectEntities[0];
                    selectEntities.RemoveAll(e => e.guid == entity.guid);
                }
            }

            //处理随机选取
            var randCount = tableConfig.RandSelectCount;
            if (randCount > 0)
            {
                List<int> randIndexes = BattleRandom.GetRandIndexes(0, selectEntities.Count, randCount, this.battle);
                if (randIndexes.Count > 0)
                {
                    selectEntities = randIndexes.Select(index => selectEntities[index]).ToList();
                }
            }

            //所有选中单位都施加这个单个效果
            SkillEffectContext newContext = context.Copy();
            foreach (var entity in selectEntities)
            {
                newContext.selectEntities = new List<BattleEntity>() { entity };

                //触发下一步 effect
                TriggerEffect(newContext);
            }

            //整体组施加效果，如 linkGroup 
            newContext.selectEntities = selectEntities;

            TriggerGroupEffect(newContext);
        }

        /// <summary>
        /// 判断实体是否在扇形区域内
        /// </summary>
        private bool IsEntityInSector(Vector3 center, float radius, float halfAngle, Vector3 forwardDir, BattleEntity entity)
        {
            // 计算目标和中心点之间的距离
            var distanceSquared = Vector3.SqrtDistance(center, entity.position);
            var radiusSquared = radius * radius;
            
            // 判断距离是否在半径范围内
            if (distanceSquared <= radiusSquared)
            {
                // 计算从中心点到目标的向量
                Vector3 toTargetDir = (entity.position - center);
                toTargetDir = new Vector3(toTargetDir.x, 0, toTargetDir.z);
                
                // 将两个向量归一化
                Vector3 normalizedForward = new Vector3(forwardDir.x, 0, forwardDir.z).normalized;
                Vector3 normalizedToTarget = toTargetDir.normalized;
                
                // 计算两个向量之间的点积
                float dotProduct = Vector3.Dot(normalizedForward, normalizedToTarget);
                
                // 根据点积计算夹角的余弦值，dotProduct = cos(angle)
                // 将角度转换为弧度
                float cosHalfAngle = (float)System.Math.Cos(halfAngle * System.Math.PI / 180.0f);
                
                // 如果点积 >= 余弦值，则点在扇形内
                // (点积越大，角度越小；cos(0)=1，cos(90)=0)
                return dotProduct >= cosHalfAngle;
            }
            
            return false;
        }

        //触发单个效果
        public void TriggerEffect(SkillEffectContext context)
        {
            battle.AddSkillEffectGroup(tableConfig.EffectList, context);
        }

        //触发整体组效果
        public void TriggerGroupEffect(SkillEffectContext context)
        {
            battle.AddSkillEffectGroup(tableConfig.GroupEffectList, context);
        }


        public override void OnUpdate(float timeDelta)
        {
        }
    }
}