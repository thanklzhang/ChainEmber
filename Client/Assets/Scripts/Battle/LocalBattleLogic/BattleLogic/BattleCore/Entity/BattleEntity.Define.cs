using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace Battle
{
    public enum EntityState
    {
        Null = 0,
        Idle = 1,
        Move = 2,

        //使用技能中 包括 前摇 释放中 后摇
        UseSkill = 3,
        //死了，但是不是真死，倒下了，等待复活
        WillDead = 4,
        //真死了
        Dead = 5,
    }

    //实体当前数据值类型
    public enum EntityCurrValueType
    {
        CurrHealth = 1,
        CurrMagic = 2,
    }

    public enum BattleEntityRoleType
    {
        //正常角色
        Normal = 0,

        //召唤兽
        Summon = 1,

        //队友（佣兵）
        TeamMember = 2,
    }

    public partial class BattleEntity
    {
    }

    public class EntityStateDataBean
    {
        public int starLv;
        public int starExp;
    }

    public class EntityTriggerEventArg
    {
        public EffectTriggerTimeType triggerType;
        //相对的触发实体 例如：造成伤害时候，这个就代表受到伤害的实体
        public BattleEntity oppositeTriggerEntity;
        public int damage;
        public Skill damageSrcSkill;
    }
}