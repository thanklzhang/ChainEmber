using System.Collections.Generic;


namespace Battle
{
    public class PassiveEffect : SkillEffect
    {
        //public Config.BuffEffect tableConfig;
        public IPassiveEffect tableConfig;

        //BuffType buffType;
        Battle battle;

        private float maxTriggerCDTime;
        private float triggerCDTimer;

        private int canTriggerCount;
        private int maxTriggerCount;

        public override void OnInit()
        {
            battle = this.context.battle;
            tableConfig = BattleConfigManager.Instance.GetById<IPassiveEffect>(this.configId);

            var skillTargetType = context.fromSkill.infoConfig.SkillEffectTargetType;
            if ((SkillEffectTargetType)skillTargetType == SkillEffectTargetType.SkillReleaserEntity)
            {
                context.selectEntities = new List<BattleEntity>();
                context.selectEntities.Add(context.fromSkill.releaser);
            }

            maxTriggerCDTime = tableConfig.TriggerCD / 1000.0f;
            triggerCDTimer = 0;

            maxTriggerCount = tableConfig.MaxTriggerCount;
            canTriggerCount = maxTriggerCount;
        }

        public void RegisterEntityEvent(BattleEntity entity)
        {
            entity.normalAttackToOtherSuccessAction += OnEntityTriggerEvent;
            entity.beNormalAttackFromOtherSuccessAction += OnEntityTriggerEvent;
            entity.changeHealthAction += OnEntityTriggerEvent;
            entity.attackStartEffectAction += OnEntityTriggerEvent;
            entity.beforeDeadAction += OnEntityTriggerEvent;
            entity.avoidProjectileTriggerAction += OnEntityTriggerEvent;
            entity.avoidDamageTriggerAction += OnEntityTriggerEvent;
            entity.beHurtAction += OnEntityTriggerEvent;
            entity.hurtToOtherAction += OnEntityTriggerEvent;
            entity.summonEntityHurtToOtherAction += OnEntityTriggerEvent;
            entity.summonEntityBeHurtAction += OnEntityTriggerEvent;
            entity.collisionEntityActin += OnEntityTriggerEvent;
            entity.afterDeadAction += OnEntityTriggerEvent;
            entity.summonEntityAction += OnEntityTriggerEvent;
        }

        public override void OnStart()
        {
            // if (this.configId == 18004201)
            // {
            //     Battle_Log.LogZxy("");
            // }

            var entities = context.selectEntities;
            foreach (var _entity in entities)
            {
                RegisterEntityEvent(_entity);

                _entity.AddPassiveEffect(this);

                if ((EffectTriggerTimeType)tableConfig.TriggerTimeType == EffectTriggerTimeType.OnCollisionEntity)
                {
                    //如果已经碰撞了 那么触发一次 enter 碰撞
                    var guids = battle.collisionMgr.GetCollisionEntityGuids(_entity.guid);
                    if (guids != null)
                    {
                        foreach (var guid in guids)
                        {
                            var entity = this.battle.FindEntity(guid);
                            if (entity != null && !entity.IsDead())
                            {
                                // this.OnCollisionEntity(entity);
                                this.OnEntityTriggerEvent(new EntityTriggerEventArg()
                                {
                                    oppositeTriggerEntity = entity,
                                    triggerType = EffectTriggerTimeType.OnCollisionEntity
                                });
                            }
                        }
                    }
                }
            }
        }


        public override void OnUpdate(float timeDelta)
        {
            triggerCDTimer -= timeDelta;
            if (triggerCDTimer <= 0)
            {
                triggerCDTimer = 0;
            }
        }

        private bool IsInTriggerCD()
        {
            return this.triggerCDTimer > 0;
        }

        private bool IsHasTriggerCount()
        {
            if (this.maxTriggerCount <= 0)
            {
                return true;
            }

            return this.canTriggerCount > 0;
        }

        //检查触发条件
        public bool CheckTriggerCondition()
        {
            var chance = tableConfig.TriggerChance;
            var randInt = BattleRandom.GetRandInt(1, 1001, battle);

            var isOccurred = randInt <= chance;
            var isInCD = IsInTriggerCD();
            var isHasTriggerCount = IsHasTriggerCount();

            var result = isOccurred && !isInCD && isHasTriggerCount;

            return result;
        }

        #region trigger

        public void OnEntityTriggerEvent(EntityTriggerEventArg triggerArg)
        {
            // if (this.configId == 18004201)
            // {
            //     Battle_Log.LogZxy("");
            // }

            var isAfterDeadEvent = triggerArg.triggerType == EffectTriggerTimeType.OnAfterDead;
            if (!isAfterDeadEvent)
            {
                if (this.context.fromSkill.releaser.IsDead())
                {
                    return;
                }

                if (triggerArg.oppositeTriggerEntity != null &&
                    triggerArg.oppositeTriggerEntity.IsDead())
                {
                    return;
                }
            }

            var currTriggerTimeType = (EffectTriggerTimeType)tableConfig.TriggerTimeType;
            if (currTriggerTimeType == triggerArg.triggerType)
            {
                if (!CheckTriggerCondition())
                {
                    return;
                }

                AddSkillEffect(triggerArg);

                AfterTrigger();
            }
        }

        private void AddSkillEffect(EntityTriggerEventArg triggerArg)
        {
            if (triggerArg.triggerType == EffectTriggerTimeType.OnHurtToOther)
            {
                //伤害参数：技能id需要对上才能触发  没有填写则表示什么都能通过
                if (tableConfig.TriggerIntListParam.Count > 0)
                {
                    if (triggerArg.damageSrcSkill != null)
                    {
                        var index = tableConfig.TriggerIntListParam.IndexOf(triggerArg.damageSrcSkill.configId);
                        if (index < 0)
                        {
                            return;
                        }
                    }
                }
            }

            var selectEntity = this.context.selectEntities[0];

            SkillEffectContext context = new SkillEffectContext();
            context.battle = this.battle;
            context.fromSkill = this.context.fromSkill;
            context.selectEntities = new List<BattleEntity>();
            context.damage = triggerArg.damage;

            if ((EffectTriggerTargetType)this.tableConfig.TriggerTargetType ==
                EffectTriggerTargetType.TriggerEntity)
            {
                if (this.context.selectEntities is { Count: > 0 })
                {
                    var e = this.context.selectEntities[0];
                    if (e != null)
                    {
                        context.selectEntities.Add(e);
                    }

                   
                }
            }
            else if ((EffectTriggerTargetType)this.tableConfig.TriggerTargetType ==
                     EffectTriggerTargetType.OppositeEntity)
            {
                if (triggerArg.oppositeTriggerEntity != null)
                {
                    context.selectEntities.Add(triggerArg.oppositeTriggerEntity);
                }
            }
            else if ((EffectTriggerTargetType)this.tableConfig.TriggerTargetType ==
                     EffectTriggerTargetType.SkillReleaser)
            {
                if (this.context.fromSkill.releaser != null)
                {
                    context.selectEntities.Add(this.context.fromSkill.releaser);
                }
               
            }

            if (context.selectEntities.Count > 0)
            {
                battle.AddSkillEffectGroup(tableConfig.TriggerEffectList, context);
            }

        }

        #endregion


        public void AfterTrigger()
        {
            if (canTriggerCount > 0)
            {
                canTriggerCount -= 1;
            }

            triggerCDTimer = maxTriggerCDTime;

            foreach (var item in tableConfig.AfterTriggerRemoveEffectList)
            {
                var configId = item;
                //默认为技能释放者 之后拓展

                //检查 buff(这个可以直接用 entity 进行删除 buff)

                var releaser = this.context.fromSkill.releaser;
                battle.DeleteBuffFromEntity(releaser.guid, configId);

                //检查 被动
                releaser.ForceRemovePassiveEffect(configId);
            }

            if (maxTriggerCount > 0)
            {
                if (canTriggerCount <= 0)
                {
                    this.SetWillEndState();
                }
            }
        }

        void CheckTriggerEvent()
        {
        }

        public void UnregisterEntityEvent(BattleEntity entity)
        {
            entity.attackStartEffectAction -= OnEntityTriggerEvent;
            entity.beNormalAttackFromOtherSuccessAction -= OnEntityTriggerEvent;
            entity.changeHealthAction -= OnEntityTriggerEvent;
            entity.beforeDeadAction -= OnEntityTriggerEvent;
            entity.avoidProjectileTriggerAction -= OnEntityTriggerEvent;
            entity.avoidDamageTriggerAction -= OnEntityTriggerEvent;
            entity.beHurtAction -= OnEntityTriggerEvent;
            entity.hurtToOtherAction -= OnEntityTriggerEvent;
            entity.summonEntityHurtToOtherAction -= OnEntityTriggerEvent;
            entity.summonEntityBeHurtAction -= OnEntityTriggerEvent;
            entity.normalAttackToOtherSuccessAction -= OnEntityTriggerEvent;
            entity.collisionEntityActin -= OnEntityTriggerEvent;
            entity.afterDeadAction -= OnEntityTriggerEvent;
            entity.summonEntityAction -= OnEntityTriggerEvent;
        }

        public override void OnEnd()
        {
            var entities = context.selectEntities;
            foreach (var _entity in entities)
            {
                UnregisterEntityEvent(_entity);

                _entity.RemovePassiveEffect(this);
            }
        }
    }


    //触发的时机类型
    public enum EffectTriggerTimeType
    {
        OnNormalAttack = 1,
        OnSkillRelease = 2,
        OnNormalAttackToOtherSuccess = 3,
        OnBeNormalAttackByOtherSuccess = 4,
        OnHurtToOther = 5,
        OnBeHurt = 6,
        OnCollisionEntity = 7,
        OnChangeHealth = 8,
        OnBeforeDead = 9,
        OnAvoidAttack = 10,
        OnAvoidProjectile = 11,
        OnAvoidDamage = 12,
        OnAfterDead = 13,
        OnSummonEntity = 14,
        OnSummonEntityHurtToOther = 30,
        OnSummonEntityBeHurt = 31,
    }
    //
    // //触发目标类型
    // public enum EffectTriggerTargetType
    // {
    //     ContextEntity = 0,
    //     NormalAttackEntity = 1,
    //     SkillReleaseEntity = 2,
    //
    //     //被普通攻击命中的单位 / 将要被普通攻击的单位
    //     BeNormalAttackEntity = 3,
    //     BeSkillEffectEntity = 4,
    //     CollisionEntity = 5,
    //     BeHurtEntity = 6
    // }


    //触发目标类型
    public enum EffectTriggerTargetType
    {
        TriggerEntity = 0,
        OppositeEntity = 1,
        SkillReleaser = 2
    }
}