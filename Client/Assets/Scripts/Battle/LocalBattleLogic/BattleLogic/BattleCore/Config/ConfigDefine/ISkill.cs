/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
namespace Battle
{
    
    
 
    public interface ISkill : IConfig
    {
        
        /// <summary>
        ///技能名称
        /// </summary>
        string Name {get;} 
        
        /// <summary>
        ///技能介绍
        /// </summary>
        string Describe {get;} 
        
        /// <summary>
        ///技能等级
        /// </summary>
        int Level {get;} 
        
        /// <summary>
        ///技能图标资源id
        /// </summary>
        int IconResId {get;} 
        
        /// <summary>
        ///技能类别
        /// </summary>
        int SkillCategory {get;} 
        
        /// <summary>
        ///技能释放目标类型
        /// </summary>
        int SkillReleaseTargeType {get;} 
        
        /// <summary>
        ///技能释放类型 
        /// </summary>
        int SkillReleaseType {get;} 
        
        /// <summary>
        ///是否是被动技能
        /// </summary>
        int IsPassiveSkill {get;} 
        
        /// <summary>
        ///实体按照关系筛选
        /// </summary>
        int EntityRelationFilterType {get;} 
        
        /// <summary>
        ///技能效果目标类型（选取即将触发效果的单位）
        /// </summary>
        int SkillEffectTargetType {get;} 
        
        /// <summary>
        ///触发的效果列表（主动释放产生的效果）
        /// </summary>
        List<int> EffectList {get;} 
        
        /// <summary>
        ///获得时候触发的效果列表（获得技能的时候立即生效）
        /// </summary>
        List<int> EffectListOnGain {get;} 
        
        /// <summary>
        ///释放距离(普通攻击不走这个 走属性)
        /// </summary>
        int ReleaseRange {get;} 
        
        /// <summary>
        ///释放技能时前摇时间(毫秒)
        /// </summary>
        int BeforeTime {get;} 
        
        /// <summary>
        ///释放技能时后摇时间(毫秒)
        /// </summary>
        int AfterTime {get;} 
        
        /// <summary>
        ///技能CD（毫秒）(在释放技能结束后,后摇时间开始前,则开始计时)(普通攻击不计算在这里)
        /// </summary>
        int CdTime {get;} 
        
        /// <summary>
        ///是否'不能被打断' 0: 能被打断 , 1:不能被打断
        /// </summary>
        int IsNoBreak {get;} 
        
        /// <summary>
        ///动画速度缩放(*1000),1 000为正常 根据前后摇校正动画 
        /// </summary>
        int AnimationSpeedScale {get;} 
        
        /// <summary>
        ///技能动画触发名称
        /// </summary>
        string AnimationTriggerName {get;} 
        
        /// <summary>
        ///释放者释放技能时候在身上的特效
        /// </summary>
        int ReleaserEffectResId {get;} 
        
        /// <summary>
        ///技能指示器id
        /// </summary>
        int SkillDirectionId {get;} 
        
        /// <summary>
        ///技能轨迹 Id 列表
        /// </summary>
        List<int> SkillTrackList {get;} 
        
        /// <summary>
        ///技能标签（目前是给 ai 作倾向用的）
        /// </summary>
        List<int> TagList {get;} 
        
    } 
}