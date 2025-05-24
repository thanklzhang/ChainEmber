using System;
using System.Collections.Generic;
using System.Linq;

namespace Battle
{
    public class EntityAbnormalStateMgr
    {
        Battle battle;
        BattleEntity battleEntity;

        public Dictionary<EntityAbnormalStateType, EntityAbnormalState> abnormalStateDic;

        public void Init(BattleEntity battleEntity)
        {
            this.battleEntity = battleEntity;
            this.battle = this.battleEntity.GetBattle();

            abnormalStateDic = new Dictionary<EntityAbnormalStateType, EntityAbnormalState>();
        }

        public void Add(EntityAbnormalStateType stateType)
        {
            if (abnormalStateDic.ContainsKey(stateType))
            {
                abnormalStateDic[stateType].Add();
            }
            else
            {
                EntityAbnormalState newState = new EntityAbnormalState();
                newState.Init(stateType);
                abnormalStateDic.Add(stateType, newState);
                newState.Add();
            }
        }

        public void Remove(EntityAbnormalStateType stateType)
        {
            if (abnormalStateDic.ContainsKey(stateType))
            {
                abnormalStateDic[stateType].Remove();
            }
        }

        public bool IsExist(EntityAbnormalStateType stateType)
        {
            if (abnormalStateDic.ContainsKey(stateType))
            {
                return abnormalStateDic[stateType].IsActive();
            }

            return false;
        }

        //是否能够被伤害
        public bool IsCanBeHurt()
        {
            var isInvincible = IsExist(EntityAbnormalStateType.Invincible);
            var result = !isInvincible;

            return result;
        }

        //是否能够死去
        public bool IsCanDead()
        {
            var isInvincible = IsExist(EntityAbnormalStateType.Invincible);
            var result = !isInvincible;

            return result;
        }

        //现在带有异常状态下 是否能主动移动
        public bool IsCanMove()
        {
            var checkStun = this.IsExist(EntityAbnormalStateType.Stun);
            var checkFreeze = this.IsExist(EntityAbnormalStateType.Freeze);
            return !checkStun && !checkFreeze;
        }

        //现在带有异常状态下 是否能释放技能(包括普通攻击)
        public bool IsCanReleaseSkill()
        {
            var checkStun = this.IsExist(EntityAbnormalStateType.Stun);
            var checkFreeze = this.IsExist(EntityAbnormalStateType.Freeze);
            return !checkStun && !checkFreeze;
        }

        public bool IsAbnormalForSkill()
        {
            var checkStun = this.IsExist(EntityAbnormalStateType.Stun);
            var checkFreeze = this.IsExist(EntityAbnormalStateType.Freeze);
            return checkStun || checkFreeze;
        }

        //该类型异常状态是否在添加的时候直接打断技能
        public bool IsBreakSkill(EntityAbnormalStateType type)
        {
            bool isBreak = false;
            if (type == EntityAbnormalStateType.Stun)
            {
                isBreak = true;
            }
            else if (type == EntityAbnormalStateType.Freeze)
            {
                isBreak = true;
            }

            return isBreak;
        }
    }
    
    public enum EntityAbnormalStateType
    {
        Null = 0,

        Attack_Add = 1,
        Defence_Add = 2,
        MaxHealth_Add = 3,
        AttackSpeed_Add = 4,
        MoveSpeed_Add = 5,
        AttackRange_Add = 6,
        CritRate_Add = 7,
        CritDamage_Add = 8,

        //增伤状态 : 提升输出伤害
        OutputDamageRate_Add = 9,

        //易伤状态 : 增加受到的伤害
        InputDamageRate_Add = 10,
        
        SkillCD_Add = 11,
        TreatmentRate_Add = 12,
        HealthRecoverSpeed_Add = 15,

        Attack_Sub = 1001,
        Defence_Sub = 1002,
        MaxHealth_Sub = 1003,
        AttackSpeed_Sub = 1004,
        MoveSpeed_Sub = 1005,
        AttackRange_Sub = 1006,
        CritRate_Sub = 1007,
        CritDamage_Sub = 1008,

        //降伤状态 : 降低输出伤害
        OutputDamageRate_Sub = 1009,

        //减伤状态 : 减少受到的伤害
        InputDamageRate_Sub = 1010,
        
        TreatmentRate_Sub = 1012,
        
        HealthRecoverSpeed_Sub = 1015,

        //击晕
        Stun = 2000,

        //冰冻
        Freeze = 2001,

        //沉默
        Silence = 2002,

        //嘲讽
        Taunt = 2003,

        //无敌
        Invincible = 2050,

        //躲避（摧毁）投掷物
        AvoidProjectile = 2051,

        //躲避普通攻击(包括普通攻击的投掷物)
        AvoidNormalAttack = 2052,
        
        //躲避伤害
        AvoidDamage = 2053,
        
        //当前生命值增加 （为了显示增加的）
        CurrHp_Add = 3000,
        
        //当前生命值减少 （为了显示增加的）
        CurrHp_Sub = 3001, 
    }

    public enum AbnormalStateTriggerType
    {
        Start = 0,
        Trigger = 1,
        End = 2
    }

    public class AbnormalStateBean
    {
        public EntityAbnormalStateType stateType;
        public AbnormalStateTriggerType triggerType;
    }

}


