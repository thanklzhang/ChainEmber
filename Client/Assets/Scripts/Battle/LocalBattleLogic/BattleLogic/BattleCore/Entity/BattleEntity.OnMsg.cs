using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;


namespace Battle
{
    public enum DamageFromType
    {
        //正常来源，包括技能，普通攻击等
        Normal = 0,

        //召唤物持续时间到了
        SummonTimeToDead = 10,

        //召唤物超出了上限
        SummonOverLimitCount = 11,

        //队友数量超出上限
        TeamMemberOverLimitCount = 20
    }

    public partial class BattleEntity
    {
        //受到伤害(或者治疗)
        internal void OnBeHurt(int damageValue, Skill damageSrcSkill, DamageFromType from = DamageFromType.Normal)
        {
            if (this.IsDead())
            {
                return;
            }

            if (from == DamageFromType.SummonTimeToDead ||
                from == DamageFromType.SummonOverLimitCount ||
                from == DamageFromType.TeamMemberOverLimitCount)
            {
                this.SetCurrHp(0);
                this.SyncStateValue();
                this.OnDead(true);
                return;
            }

            var preHp = CurrHealth;
            int resultDamage = 0;
            if (damageValue > 0)
            {
                //伤害
                if (this.abnormalStateMgr.IsCanBeHurt())
                {
                    //计算护甲
                    // resultDamage = (int)(damageValue - Defence);
                    resultDamage = CalculateDamageAndDefence(damageValue, (int)Defence);

                    //计算 承受伤害和输出伤害
                    var damageRate = this.GetEntityAttrFinalValue(EntityAttrType.InputDamageRate); /// 1000.0f
                    resultDamage = (int)(resultDamage * damageRate);
                    if (damageSrcSkill != null)
                    {
                        var outputEntity = damageSrcSkill.releaser;
                        if (outputEntity != null)
                        {
                            var outDamageRate = outputEntity.GetEntityAttrFinalValue(EntityAttrType.OutputDamageRate);

                            resultDamage = (int)(resultDamage * outDamageRate);
                        }
                    }
                }
            }
            else
            {
                //治疗
                resultDamage = damageValue;
            }

            //检测躲避伤害
            if (resultDamage > 0)
            {
                if (this.IsExistAbnormalState(EntityAbnormalStateType.AvoidDamage))
                {
                    //躲避投掷物
                    this.OnTriggerAvoidDamage();

                    battle.PlayerMsgSender.Notify_EntityAbnormalEffect(this.guid,
                        new AbnormalStateBean()
                        {
                            stateType = EntityAbnormalStateType.AvoidDamage,
                            triggerType = AbnormalStateTriggerType.Trigger
                        });
                    return;
                }
            }


            AddCurrHp(-resultDamage, false);


            var attackerGuid = 0;
            var attackerName = "NotFound_Error";
            var attacker = damageSrcSkill.releaser;
            if (attacker != null)
            {
                attackerName = attacker.infoConfig.Name;
                attackerGuid = attacker.guid;
            }

            var beHurtName = this.infoConfig.Name;

            var damageStr = string.Format(
                "{0}({1}) be hurt by {2}({3}) ,damage : {4}, after defence calculate , result damage : {5}" +
                " hp : {6} -> {7}", beHurtName, this.guid, attackerName, attackerGuid, damageValue,
                resultDamage, preHp, CurrHealth);
            BattleLog.Log(damageStr);


            //受到伤害了
            if (resultDamage > 0)
            {
                this.ai?.OnBeHurt(resultDamage, damageSrcSkill);

                this.beHurtAction?.Invoke(new EntityTriggerEventArg()
                {
                    oppositeTriggerEntity = this, damage = resultDamage, damageSrcSkill = damageSrcSkill,
                    triggerType = EffectTriggerTimeType.OnBeHurt
                });

                //作为召唤兽的时候 被攻击了 触发召唤者的事件
                if (this.IsSummonEntity() && !this.beSummonMaster.IsDead())
                {
                    this.beSummonMaster.OnSummonEntityBeHurt(new EntityTriggerEventArg()
                    {
                        damage = resultDamage,
                        damageSrcSkill = damageSrcSkill,
                        oppositeTriggerEntity = damageSrcSkill.releaser,
                        triggerType = EffectTriggerTimeType.OnSummonEntityBeHurt,
                    });
                }

                //造成伤害者作为召唤兽的时候 触发召唤者的事件
                if (damageSrcSkill.releaser.IsSummonEntity())
                {
                    var beSummonMaster = damageSrcSkill.releaser.beSummonMaster;
                    beSummonMaster.OnMySummonEntityHurtToOther(new EntityTriggerEventArg()
                    {
                        damage = resultDamage,
                        damageSrcSkill = damageSrcSkill,
                        oppositeTriggerEntity = this,
                        triggerType = EffectTriggerTimeType.OnSummonEntityHurtToOther,
                    });
                }
            }

            this.battle.OnEntityBeHurt(this, resultDamage, damageSrcSkill);
            if (this.IsDead()) return;

            if (CurrHealth <= 0)
            {
                beforeDeadAction?.Invoke(new EntityTriggerEventArg()
                {
                    oppositeTriggerEntity = this, triggerType = EffectTriggerTimeType.OnBeforeDead
                });
                if (this.IsDead()) return;

                //这里是死亡之前的最后判断
                var isCanDead = this.abnormalStateMgr.IsCanDead();
                if (isCanDead)
                {
                    this.OnDead(false);
                }
                else
                {
                    SetCurrHp(1);
                    ;
                    // this.SyncCurrHealth();
                }
            }
        }

        public float GetCurrHp()
        {
            return this.CurrHealth;
        }

        public void AddCurrHp(float addedHp, bool isSync = true)
        {
            SetCurrHp(CurrHealth + addedHp, isSync);
        }

        public void SetCurrHp(float hp, bool isSync = true)
        {
            CurrHealth = hp;

            CurrHealth = Math.Max(0, CurrHealth);
            CurrHealth = Math.Min((int)MaxHealth, CurrHealth);

            AfterChangeCurrHp();

            if (isSync)
            {
                this.SyncCurrHealth();
            }
        }

        void AfterChangeCurrHp()
        {
            this.changeHealthAction?.Invoke(new EntityTriggerEventArg()
            {
                oppositeTriggerEntity = this, triggerType = EffectTriggerTimeType.OnChangeHealth
            });
        }

        int CalculateDamageAndDefence(int damageValue, int defence)
        {

            //公式 1
            // var resultDamage = damageValue - defence;
            // return resultDamage;

            //公式 2
            var factor = 100;
            var resultDamage = damageValue * (1 - defence / (float)(factor + defence));
            return (int)resultDamage;

            //公式 3 
            // //公式： (攻击力 * 攻击力) / （攻击力 + 防御力）
            // //负数不考虑
            // float temp = damageValue + defence;
            // if (temp <= 0)
            // {
            //     temp = 1;
            // }
            //
            // var resultDamage = (int)((damageValue * damageValue) / (temp));
            // return resultDamage;
        }

        //当普通攻击释放出来的时候(前摇过了的那个时刻)
        public void OnNormalAttackStartEffect(BattleEntity other)
        {
            attackStartEffectAction?.Invoke(new EntityTriggerEventArg()
            {
                oppositeTriggerEntity = other, triggerType = EffectTriggerTimeType.OnNormalAttack
            });

            // //检测被动技能
            // foreach (var item in this.passiveEffectDic)
            // {
            //     item.Value.OnEntityTriggerEvent(new EntityTriggerEventArg()
            //     {
            //         oppositeTriggerEntity = other, triggerType = EffectTriggerTimeType.OnNormalAttack
            //     });
            // }
        }

        //当普通攻击别人命中时
        public void OnNormalAttackToOtherSuccess(BattleEntity other, int resultDamage, Skill damageSrcSkill)
        {
            normalAttackToOtherSuccessAction?.Invoke(new EntityTriggerEventArg()
            {
                oppositeTriggerEntity = other, damage = resultDamage, damageSrcSkill = damageSrcSkill,
                triggerType = EffectTriggerTimeType.OnNormalAttackToOtherSuccess
            });

            // //检测被动技能
            // foreach (var item in this.passiveEffectDic)
            // {
            //     item.Value.OnEntityTriggerEvent(new EntityTriggerEventArg()
            //     {
            //         oppositeTriggerEntity = other, damage = resultDamage, damageSrcSkill = damageSrcSkill,
            //         triggerType = EffectTriggerTimeType.OnNormalAttackToOtherSuccess
            //     });
            //     // item.Value.OnNormalAttackToOtherSuccess(other, resultDamage, damageSrcSkill);
            // }
        }

        //当被别的单位普通攻击命中时
        public void OnBeNormalAttackFromOtherSuccess(BattleEntity attacker, int resultDamage, Skill damageSrcSkill)
        {
            beNormalAttackFromOtherSuccessAction?.Invoke(new EntityTriggerEventArg()
            {
                oppositeTriggerEntity = attacker, damage = resultDamage, damageSrcSkill = damageSrcSkill,
                triggerType = EffectTriggerTimeType.OnBeNormalAttackByOtherSuccess
            });

            // //检测被动技能
            // foreach (var item in this.passiveEffectDic)
            // {
            //     item.Value.OnEntityTriggerEvent(new EntityTriggerEventArg()
            //     {
            //         oppositeTriggerEntity = other, damage = resultDamage, damageSrcSkill = damageSrcSkill,
            //         triggerType = EffectTriggerTimeType.OnNormalAttackToOtherSuccess
            //     });
            //     // item.Value.OnNormalAttackToOtherSuccess(other, resultDamage, damageSrcSkill);
            // }
        }

        
        public void OnHurtToOther(EntityTriggerEventArg triggerArg)
        {
            hurtToOtherAction?.Invoke(triggerArg);
        }

        //当技能命中别人时
        public void OnSkillToOtehrSuccess()
        {
        }

        //当召唤兽受到伤害的时候
        public void OnSummonEntityBeHurt(EntityTriggerEventArg triggerArg)
        {
            summonEntityBeHurtAction?.Invoke(triggerArg);
        }

        //当自己的召唤兽造成伤害的时候
        public void OnMySummonEntityHurtToOther(EntityTriggerEventArg triggerArg)
        {
            summonEntityHurtToOtherAction?.Invoke(triggerArg);
        }
        
        //当召唤一个召唤兽的时候
        public void OnSummonEntity(EntityTriggerEventArg triggerArg)
        {
            summonEntityAction?.Invoke(triggerArg);
        }

        //当成功躲避投掷物
        public void OnTriggerAvoidProjectile()
        {
            avoidProjectileTriggerAction?.Invoke(new EntityTriggerEventArg()
            {
                oppositeTriggerEntity = this, triggerType = EffectTriggerTimeType.OnAvoidProjectile
            });
        }

        //当成功躲避伤害
        public void OnTriggerAvoidDamage()
        {
            avoidDamageTriggerAction?.Invoke(new EntityTriggerEventArg()
            {
                oppositeTriggerEntity = this, triggerType = EffectTriggerTimeType.OnAvoidDamage
            });
        }

        public void OnSkillReleaseEnd(Skill skill)
        {
            if (this.IsDead())
            {
                return;
            }

            //_Battle_Log.Log(string.Format("{0} OnSkillReleaseEnd : {1}", this.infoConfig.Name, skill.infoConfig.Name));
            this.ChangeToIdle();

            this.operateModule.OnNodeExecuteFinish(skill.configId);

            //普通攻击的话会自动继续攻击
            if (skill.isNormalAttack)
            {
                this.AskReleaseSkill(skill.configId, skill.targetGuid, skill.targetPos, skill.mousePos, true);
            }
        }


        public void OnEnterCollision(BattleEntity other)
        {
            this.collisionEntityActin?.Invoke(new EntityTriggerEventArg()
            {
                oppositeTriggerEntity = other, triggerType = EffectTriggerTimeType.OnCollisionEntity
            });

            // foreach (var item in this.passiveEffectDic)
            // {
            //     item.Value.OnEntityTriggerEvent(new EntityTriggerEventArg()
            //     {
            //         oppositeTriggerEntity = other, triggerType = EffectTriggerTimeType.OnCollisionEntity
            //     });
            //     // item.Value.OnCollisionEntity(other);
            // }
        }

        //TODO : entity 死后是要触发 碰撞退出的
        public void OnExitCollision(BattleEntity other)
        {
            //foreach (var item in this.passiveEffectDic)
            //{
            //    item.Value.OnCollisionEntity(other);
            //}
            // Logx.Log("OnExitCollision : " + this.infoConfig.Name + " -> " + other.infoConfig.Name);
        }


        //死亡 但不是真的死了
        public void OnDead(bool forceTrueDead)
        {
            BattleLog.Log("entity dead : guid : " + guid + " , name : " + this.infoConfig.Name);
            if (this.IsDead()) return;

            // this.EntityState = EntityState.Dead;

            if (forceTrueDead)
            {
                this.EntityState = EntityState.Dead;
            }
            else
            {
                if (this.isPlayerCtrl)
                {
                    var player = this.battle.FindPlayerByPlayerIndex(this.playerIndex);
                    var revive = player.GetCurrency(BattleCurrency.reviveCoinId);
                    if (revive.count > 0)
                    {
                        this.EntityState = EntityState.WillDead;
                        //send msg
                        this.battle.OnSyncIsUseReviveCoin(player.playerIndex);
                    }
                    else
                    {
                        this.EntityState = EntityState.Dead;
                    }
                }
                else
                {
                    if (this.teamLeader != null && this.teamLeader.isPlayerCtrl)
                    {
                        this.EntityState = EntityState.WillDead;
                    }
                    else
                    {
                        this.EntityState = EntityState.Dead;
                    }
                }
            }

            var isTrueDead = this.EntityState == EntityState.Dead;

            if (isTrueDead)
            {
                afterDeadAction?.Invoke(new EntityTriggerEventArg()
                {
                    oppositeTriggerEntity = this, triggerType = EffectTriggerTimeType.OnAfterDead
                });
            }

            this.battle.OnEntityDead(this, isTrueDead);
        }

        //强迫移除该实体的队友
        public void ForceRemoveTeamMember(int memberConfigId, DamageFromType type)
        {
            //强制移除同伙
            var memberEntity = this.GetMemberEntityByConfigId(memberConfigId);
            if (memberEntity != null)
            {
                memberEntity.OnBeHurt(0, null, type);
                //删除实体和队友的联系
                RemoveTeamMemberEntity(memberEntity.guid);
            }
        }
    }
}