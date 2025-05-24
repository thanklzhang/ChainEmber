using System;
using System.Collections.Generic;
using System.Linq;

namespace Battle
{
    //用于组装属性的效果（配置 + 收集的数据） ， 供属性模块使用
    public class AttrOption
    {
        public int guid;
        public EntityAttrType entityAttrType;
        public float initValue;
        
        //根据 addedValueType 来计算最后的值
        public AddedValueType addedValueType;
        public int addedParamValue;
        public BattleEntity calculateTarget;

        public bool isContinuous = false;

        public bool isRandValue;
        public int minRandValue;
        public int maxRandValue;
    }

    //单位的属性
    public enum EntityAttrType
    {
        Null = 0,

        //固定值--------------------
        Attack = 1,
        Defence = 2,
        MaxHealth = 3,
        
        AttackSpeed = 4,
        MoveSpeed = 5,
        
        AttackRange = 6,
        
        CritRate = 7,
        CritDamage = 8,

        //输出伤害的比率(千分比)
        OutputDamageRate = 9,
        //承受伤害的比率(千分比)
        InputDamageRate = 10,
        
        //技能冷却
        SkillCD = 11,
        
        //治疗比率（千分比）
        TreatmentRate = 12,
        
        //生命恢复
        HealthRecoverSpeed  = 15,
        
        //--------------------------


        //千分比加成--------------------

        Attack_Permillage = 1001,
        Defence_Permillage = 1002,
        MaxHealth_Permillage = 1003,
        AttackSpeed_Permillage = 1004,
        MoveSpeed_Permillage = 1005,
        AttackRange_Permillage = 1006,
        CritRate_Permillage = 1007,
        CritDamage_Permillage = 1008,
        OutputDamageRate_Permillage = 1009,
        InputDamageRate_Permillage = 1010,
        
        TreatmentRate_Permillage = 1012,
        
        HealthRecoverSpeed_Permillage = 1015,
         

        //---------------------------
    }


    public enum EntityAttrGroupType
    {
        Null = 0,

        Base = 1,
        Level = 2,
        Star = 3,
        //道具
        Item = 4,
        Buff = 5,
        //战斗奖励 包括宝箱开出的奖励等
        BattleReward = 6
    }
    
    public enum AttrCalculateTargetType
    {
        //释放技能者
        Releaser = 0,
        //效果目标单位
        EffectTarget = 1,
        //技能释放者 的 召唤者（作为召唤物）
        BeSummonMaster = 5,
        //队长
        TeamLeader = 10,
    }
}