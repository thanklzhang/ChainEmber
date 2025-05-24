using System.Collections.Generic;

namespace Battle
{
   
    //技能释放目标类型
    public enum SkillReleaseTargetType
    {
        //无目标
        NoTarget = 0,

        //实体单位
        Entity = 1,

        //点（区域，中点）
        Point = 2,
    }

    public enum ReleaseSkillState
    {
        ReadyRelease = 1,
        SkillBefore = 2,
        Releasing = 3,
        SkillAfter = 4,
        CD = 5
    }

    public enum SkillReleaseType
    {
        //有前摇后摇的正常技能
        NormalRelease = 0,

        //持续技能
        LastRelease = 1,

        //瞬发技能
        ImmediatelyRelease = 2,
    }

    //施加技能目标类型
    public enum SkillEffectTargetType
    {
        //无目标
        No = 0,

        //技能目标单位
        SkillTargetEntity = 1,

        //技能释放者
        SkillReleaserEntity = 2,
        
        //技能目标点
        SkillTargetPos = 11,
    }
    
    //TODO 需要都加上这个筛选
    public enum EntityRelationFilterType
    {
        Enemy = 0,
        FriendAndEnemy = 1,
        Friend = 2,
        MeAndFriend = 3,
        All = 4,
        //自己 这个纯粹就是为了选自己加上的筛选
        Self = 5,
        //自己的召唤兽 ， 自己作为召唤者 与 召唤兽 的 关系
        Mater_SummonEntity = 6,
        //自己的召唤者 ， 自己作为召唤兽 与 召唤者 的 关系
        SummonEntity_Master = 7,
    }


    //技能轨道的开始时机类型
    public enum SkillTrackStartTimeType
    {
        Null = 0,

        //技能前摇开始的时候
        SkillBeforeStart = 1,

        //技能开始了效果的时候
        SkillStartEffect = 2
    }

    //技能轨道的结束时机类型
    public enum SkillTrackEndTimeType
    {
        Null = 0,

        //技能完整流程结束时候(后摇结束时候)
        SkillFinishAllProcess = 1,

        //死亡时候清除(默认)
        OnEntityDead = 100
    }
    
    public partial class Skill
    {

        public static float maxSkillCDRatio = 0.8f;

    }

}