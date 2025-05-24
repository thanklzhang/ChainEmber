using System.Collections.Generic;
using System.Linq;

namespace Battle
{
    public enum RedirectType
    {
        //跟随施法单位
        FollowReleaserEntity = 1,

        //转向点到施法单位的方向
        RedirectPointToReleaserEntityDir = 2,

        //固定偏转角度
        FixedDeflectionAngle = 3,

        //随机找一个周围单位进行转向跟随
        RandFollowToAroundEntity = 4
    }

    public enum ProjectileType
    {
        PhysicalProjectile = 0,
        InvisibleProjectile = 1,
    }

    public enum ProjectileDirType
    {
        FromReleaserToContextPos = 0,
        ReleaserToward = 1
        
    }

    public class ProjectileEffect : SkillEffect
    {
        public Vector3 position;
        public float triggerTimer; //每一段时间触发一次(默认会直接执行一次)
        public float lastTimer;
        public float speed;
        public Vector3 targetPos;
        public int targetGuid;
        public IProjectileEffect tableConfig;
        Battle battle;
        Vector3 initDir;
        int currSuplusDirectCount;
        public bool isFollow;
        HashSet<int> colliderEntityGuidSet;
        private float currCollisionChangeDamageRate = 0.0f;

        public override void OnInit()
        {
            battle = this.context.battle;
            tableConfig = BattleConfigManager.Instance.GetById<IProjectileEffect>(this.configId);

            if (context.selectEntities.Count > 0)
            {
                targetGuid = context.selectEntities[0].guid;
            }
            else
            {
                targetGuid = context.fromSkill.targetGuid;
            }

            targetPos = context.fromSkill.targetPos;
            speed = tableConfig.Speed / 1000.0f;
            isFollow = 1 == tableConfig.IsFollow;
            lastTimer = this.tableConfig.LastTime / 1000.0f;

            currSuplusDirectCount = this.tableConfig.EndRedirectCount;

            colliderEntityGuidSet = new HashSet<int>();
        }

        public override CreateEffectInfo ToCreateEffectInfo()
        {
            position = context.fromSkill.releaser.position;

            CreateEffectInfo createInfo = new CreateEffectInfo();
            createInfo.guid = guid;
            createInfo.resId = tableConfig.EffectResId;
            createInfo.createPos = position;
            // battle?.OnCreateSkillEffect(createInfo);

            return createInfo;
        }

        public override void OnStart()
        {
            base.OnStart();

            var battle = this.context.battle;

            this.position = this.context.fromSkill.releaser.position;

            var startPos = position;

            // lastTimer = this.tableConfig.LastTime / 1000.0f;
            //
            // //tableConfig = ConfigManager.Instance.GetById<Config.ProjectileEffect>(this.configId);
            // isFollow = 1 == tableConfig.IsFollow;
            //
            // currSuplusDirectCount = this.tableConfig.EndRedirectCount;
            //
            // colliderEntityGuidSet = new HashSet<int>();

            EffectMoveArg effectrMoveArg = new EffectMoveArg()
            {
                effectGuid = this.guid,
                startPos = startPos,
                //targetPos = targetPos,
                targetGuid = targetGuid,
                isFollow = isFollow,
                moveSpeed = speed
            };

            if (!isFollow)
            {
                if (targetGuid > 0)
                {
                    var targetEntity = battle.FindEntity(targetGuid);
                    if (targetEntity != null)
                    {
                        initDir = (targetEntity.position - this.position).normalized;
                    }
                }
                else
                {
                    initDir = (targetPos - this.position).normalized;
                }

                var dirType = (ProjectileDirType)this.tableConfig.DirType;
                if (dirType == ProjectileDirType.FromReleaserToContextPos)
                {
                    
                }
                else if (dirType == ProjectileDirType.ReleaserToward)
                {
                    initDir = context.fromSkill.releaser.dir;
                }


                //计算偏转角度
                float angle = this.tableConfig.DeflectionAngle;

                Quaternion q = new Quaternion(0, angle * MathTool.Deg2Rad, 0);
                this.initDir = (q * this.initDir).normalized;
                this.initDir = new Vector3(this.initDir.x, 0, this.initDir.z);

                effectrMoveArg.targetPos = this.position + this.initDir * 1;

                effectrMoveArg.isFlyMaxRange = 1 == this.tableConfig.IsFlyMaxRange;
                effectrMoveArg.dirByFlyMaxRange = initDir;
            }
            else
            {
                effectrMoveArg.targetPos = targetPos;
            }


            battle.OnSkillEffectStartMove(effectrMoveArg);
        }

        public override void OnUpdate(float timeDetla)
        {
            var battle = this.context.battle;
            lastTimer = lastTimer - battle.TimeDelta;

            if (lastTimer > 0)
            {
                //存活

                // if (1 == this.tableConfig.IsFlyMaxRange)
                // {
                //     Battle_Log.Log("");
                // }

                //判断碰撞--
                if (CheckCollision())
                {
                    //销毁了
                    return;
                }

                //处理移动--

                if (3003201 == this.configId)
                {
                    BattleLog.Log("");
                }

                //是否跟随实体
                if (isFollow)
                {
                    //跟随某一个实体
                    var target = battle.FindEntity(targetGuid);
                    if (target != null)
                    {
                        var moveTargetPos = target.position;
                        var isReach = this.MoveToPos(moveTargetPos);
                        if (isReach)
                        {
                            //追到了目标实体
                            this.colliderEntityGuidSet.Add(target.guid);
                            HandleEndRedirect();
                        }
                    }
                    else
                    {
                        // _Battle_Log.LogError("the target is not found : " + targetGuid);
                        this.SetWillEndState();
                    }
                }
                else
                {
                    //判断是否按照最大飞行距离飞行
                    var isFlyMaxLength = this.tableConfig.IsFlyMaxRange;
                    if (1 == isFlyMaxLength)
                    {
                        //按照最大飞行距离飞行 而不是到目标点就消失
                        var dir = initDir;
                        dir.y = 0;
                        MoveByDir(dir);
                    }
                    else
                    {
                        //到目标点就消失
                        var isReach = this.MoveToPos(targetPos);
                        if (isReach)
                        {
                            //到了目标点
                            HandleEndRedirect();
                        }
                    }
                }
            }
            else
            {
                HandleEndRedirect();
            }
        }


        //碰撞检测
        //return 投掷物是否销毁
        public bool CheckCollision()
        {
            //有待优化
            var allEntities = battle.GetAllEntities();
            foreach (var item in allEntities)
            {
                var entity = item.Value;
                
                var filterTypen = (EntityRelationFilterType)this.tableConfig.CollisionEntityRelationFilterType;
                var isSuit = entity.IsSuitSkillSelectType(filterTypen, this.context.fromSkill.releaser);
                
              
                if(isSuit)
                {
                    var sqrtDis = Vector3.SqrtDistance(this.position, entity.position);
                    var dis = tableConfig.CollisionRadius / 1000.0f;
                    var calDis = dis * dis;
                    if (sqrtDis <= calDis)
                    {
                        if (colliderEntityGuidSet.Contains(entity.guid))
                        {
                            //已经存在 不触发
                        }
                        else
                        {
                            //触发碰撞技能效果

                            //判断碰撞组(多个技能效果击中一个目标只生效一个)
                            bool isCanEffect = false;
                            var group = this.context.collisonGroupEffect;
                            if (group != null)
                            {
                                if (group.IsHasCollsion(entity.guid))
                                {
                                    isCanEffect = false;
                                }
                                else
                                {
                                    group.OnCollisionEntity(entity.guid);
                                    isCanEffect = true;
                                }
                            }
                            else
                            {
                                isCanEffect = true;
                            }

                            if (isCanEffect)
                            {
                                //填充此时的上下文 目前不考虑群招 
                                SkillEffectContext context = new SkillEffectContext()
                                {
                                    selectEntities = new List<BattleEntity>() { entity },
                                    battle = this.context.battle,
                                    fromSkill = this.context.fromSkill
                                };

                                var isAvoid = this.TriggerCollisionEffect(entity, context);
                                if (isAvoid)
                                {
                                    return true;
                                }

                                if (!isFollow)
                                {
                                    colliderEntityGuidSet.Add(entity.guid);
                                }

                                var isThrough = 1 == this.tableConfig.IsThrough;
                                if (isThrough)
                                {
                                    //穿透 继续飞行
                                }
                                else
                                {
                                    //飞行结束 判断转向 
                                    var isRedirect = HandleEndRedirect();
                                    if (!isRedirect)
                                    {
                                        //没有转向则已经销毁了
                                        return true;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return false;
        }

        //移动
        //return 是否到达目的地
        public bool MoveToPos(Vector3 targetPos)
        {
            var dir = (targetPos - this.position).normalized;
            var currFramePos = this.position;
            var nextFramePos = this.position + dir * speed * battle.TimeDelta;
            var dotValue = Vector3.Dot(nextFramePos - targetPos, targetPos - currFramePos);

            MoveByDir(dir);

            var isReach = dotValue >= 0;
            if (isReach)
            {
                this.position = targetPos;
            }

            return isReach;
        }

        public void MoveByDir(Vector3 dir)
        {
            this.position = this.position + dir * speed * battle.TimeDelta;
        }


        public bool CheckAvoid(BattleEntity target)
        {
            if (target.IsExistAbnormalState(EntityAbnormalStateType.AvoidNormalAttack))
            {
                //躲避普通攻击
                battle.PlayerMsgSender.Notify_EntityAbnormalEffect(target.guid,
                    new AbnormalStateBean()
                    {
                        stateType = EntityAbnormalStateType.AvoidNormalAttack,
                        triggerType = AbnormalStateTriggerType.Trigger
                    });
                return true;
            }
            else if (target.IsExistAbnormalState(EntityAbnormalStateType.AvoidProjectile))
            {
                var type = (ProjectileType)this.tableConfig.ProjectileType;

                if (type == ProjectileType.PhysicalProjectile)
                {
                    //躲避投掷物
                    target.OnTriggerAvoidProjectile();

                    battle.PlayerMsgSender.Notify_EntityAbnormalEffect(target.guid,
                        new AbnormalStateBean()
                        {
                            stateType = EntityAbnormalStateType.AvoidProjectile,
                            triggerType = AbnormalStateTriggerType.Trigger
                        });
                    return true;
                }
            }

            return false;
        }

        //碰到敌人触发 effect
        public bool TriggerCollisionEffect(BattleEntity target, SkillEffectContext context)
        {
            //_Battle_Log.Log(string.Format("ProjectileEffect effect of guid : {0} TriggerCollisionEffect", this.guid));

            var isAvoid = CheckAvoid(target);
            if (isAvoid)
            {
                this.SetWillEndState();
                return true;
            }

            context.damageChangeRate = this.currCollisionChangeDamageRate;

            battle.AddSkillEffectGroup(tableConfig.CollisionEffectList, context);


            //碰撞后减少造成的伤害
            this.currCollisionChangeDamageRate += this.tableConfig.CollisionDamageChange / 1000.0f;
            var limit = this.tableConfig.CollisionDamageChangeLimit / 1000.0f;
            if (limit != 0)
            {
                if (limit > 0)
                {
                    if (this.currCollisionChangeDamageRate > limit)
                    {
                        this.currCollisionChangeDamageRate = limit;
                    }
                }
                else
                {
                    if (this.currCollisionChangeDamageRate < limit)
                    {
                        this.currCollisionChangeDamageRate = limit;
                    }
                }
            }

            return false;
        }

        //处理结束转向
        //return 是否转向 如果不转向 那么直接销毁
        public bool HandleEndRedirect()
        {
            var isRedirect = CheckEndRedirect();
            if (isRedirect)
            {
                if (this.context.fromSkill.isNormalAttack)
                {
                    var target = battle.FindEntity(targetGuid);
                    if (target != null)
                    {
                        var isAvoid = CheckAvoid(target);
                        if (isAvoid)
                        {
                            this.SetWillEndState();
                            return false;
                        }
                    }
                }

                battle.AddSkillEffectGroup(tableConfig.EndEffectList, context);
                DoRedirect();
            }
            else
            {
                battle.AddSkillEffectGroup(tableConfig.EndEffectList, context);
                this.SetWillEndState();
            }

            return isRedirect;
        }

        //判断结束的时候是否会转向
        public bool CheckEndRedirect()
        {
            return currSuplusDirectCount > 0;
        }

        //转向
        public void DoRedirect()
        {
            this.lastTimer = this.tableConfig.EndRedirectLastTime / 1000.0f;

            RedirectType directType = (RedirectType)this.tableConfig.EndRedirectType;
            if (directType == RedirectType.FollowReleaserEntity)
            {
                //转向为跟随施法单位
                this.isFollow = true;
                this.targetGuid = this.context.fromSkill.releaser.guid;

                var targetEntity = battle.FindEntity(targetGuid);
                var targetPos = Vector3.one;
                if (targetEntity != null)
                {
                    targetPos = targetEntity.position;
                    ChangeEndEffectTarget(targetEntity);
                    
                }
                else
                {   
                    this.SetWillEndState();
                    return;
                }

                EffectMoveArg effectMoveArg = new EffectMoveArg()
                {
                    effectGuid = this.guid,
                    startPos = this.position,
                    targetPos = targetPos,
                    targetGuid = targetGuid,
                    isFollow = isFollow,
                    moveSpeed = speed
                };

                battle.OnSkillEffectStartMove(effectMoveArg);
            }
            else if (directType == RedirectType.RandFollowToAroundEntity)
            {
                this.isFollow = true;
                //Battle_Log.LogZxy("redirect : start ---");
                //默认过滤掉碰撞过的单位
                List<BattleEntity> excludeList = null;
                if (colliderEntityGuidSet is { Count: > 0 })
                {
                    excludeList = colliderEntityGuidSet.Select(guid => this.battle.FindEntity(guid))
                        .Where(e => e != null).ToList();
                }

                var radius = tableConfig.EndRedirectIntListParam[0] / 1000.0f;
                var relationFilterType = (EntityRelationFilterType)tableConfig.EndRedirectIntListParam[1];
                List<BattleEntity> aroundEntities = battle.GetRandAroundEntityByPos(this.position,
                    radius, 1, excludeList, context.fromSkill.releaser, relationFilterType);

                if (aroundEntities.Count >= 1)
                {
                    var nextTarget = aroundEntities[0];
                    this.targetGuid = nextTarget.guid;
                    
                    //Battle_Log.LogZxy("redirect : " + nextTarget.guid);
                    EffectMoveArg effectMoveArg = new EffectMoveArg()
                    {
                        effectGuid = this.guid,
                        startPos = this.position,
                        targetPos = Vector3.one,
                        targetGuid = targetGuid,
                        isFollow = isFollow,
                        moveSpeed = speed
                    };

                        
                    ChangeEndEffectTarget(nextTarget);
                    battle.OnSkillEffectStartMove(effectMoveArg);
                }
                else
                {
                    this.SetWillEndState();
                    return;
                }
                //Battle_Log.LogZxy("redirect : end ---");
            }
            else
            {
                this.SetWillEndState();
                return;
            }

            currSuplusDirectCount = currSuplusDirectCount - 1;

            this.currCollisionChangeDamageRate = 0.0f;
            if (0 == tableConfig.IsEndRedirectReserveCollisionInfo)
            {
                colliderEntityGuidSet?.Clear();
                this.context.collisonGroupEffect?.ClearCollisionCache(this.tableConfig.Id);
            }
        }

        void ChangeEndEffectTarget(BattleEntity nextEntity)
        {
            this.context.selectEntities.Clear();
            if (nextEntity != null)
            {
                this.context.selectEntities.Add(nextEntity);
            }
        }

        public override void OnEnd()
        {
            // if (this.context.fromSkill.isNormalAttack)
            // {
            //     
            //     var target = battle.FindEntity(targetGuid);
            //     if (target != null)
            //     {
            //         var isAvoid = CheckAvoid(target);
            //         if (isAvoid)
            //         {
            //             return;
            //         }
            //     }
            // }
            // battle.AddSkillEffectGroup(tableConfig.EndEffectList,context);
        }
    }
}