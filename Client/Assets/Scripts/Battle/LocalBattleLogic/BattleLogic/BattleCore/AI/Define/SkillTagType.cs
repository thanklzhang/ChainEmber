using System;
using System.Collections.Generic;

namespace Battle
{
    //技能标签类型
    public enum SkillTagType
    {
        //伤害
        Hurt = 0,

        //治疗
        Healing = 1,

        //控制
        Control = 2,

        //防御
        Defence = 3,

        //位移
        Displacement = 4,

        //保护
        Protect = 5,

        //召唤
        Summon = 6,

        //增益--------------------
        //增益 攻击向 buff(这个跟攻击有关都算  如 攻击力 攻速  暴击 反伤 等)
        Buff_Attack = 101,

        //增益 防御向 buff （同理 跟放于相关都算 如 防御力 减伤）
        Buff_Defence = 102,

        //增益 移动向 buff
        Buff_Move = 103,

        //减益--------------------
        //减益 攻击向 buff
        DeBuff_Attack = 201,

        //减益 防御向 buff
        DeBuff_Defence = 202,

        //减益 移动向 buff
        DeBuff_Move = 203,
    }

    //SelectEntityType SkillReleaseTargetType
}