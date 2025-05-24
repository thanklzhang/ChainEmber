/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
using Config;
namespace Battle
{
    
    
 
    public class Skill_Impl : ISkill
    {
        private Config.Skill config;
        
        public void Init(int id)
        {
            config = ConfigManager.Instance.GetById<Config.Skill>(id);
        }
        
        public int Id => config.Id;
        
        /// <summary>
        ///技能名称
        /// </summary>
        public string Name => config.Name;
        
        /// <summary>
        ///技能介绍
        /// </summary>
        public string Describe => config.Describe;
        
        /// <summary>
        ///技能等级
        /// </summary>
        public int Level => config.Level;
        
        /// <summary>
        ///技能图标资源id
        /// </summary>
        public int IconResId => config.IconResId;
        
        /// <summary>
        ///技能类别
        /// </summary>
        public int SkillCategory => config.SkillCategory;
        
        /// <summary>
        ///技能释放目标类型
        /// </summary>
        public int SkillReleaseTargeType => config.SkillReleaseTargeType;
        
        /// <summary>
        ///技能释放类型 
        /// </summary>
        public int SkillReleaseType => config.SkillReleaseType;
        
        /// <summary>
        ///是否是被动技能
        /// </summary>
        public int IsPassiveSkill => config.IsPassiveSkill;
        
        /// <summary>
        ///实体按照关系筛选
        /// </summary>
        public int EntityRelationFilterType => config.EntityRelationFilterType;
        
        /// <summary>
        ///技能效果目标类型（选取即将触发效果的单位）
        /// </summary>
        public int SkillEffectTargetType => config.SkillEffectTargetType;
        
        /// <summary>
        ///触发的效果列表（主动释放产生的效果）
        /// </summary>
        public List<int> EffectList => config.EffectList;
        
        /// <summary>
        ///获得时候触发的效果列表（获得技能的时候立即生效）
        /// </summary>
        public List<int> EffectListOnGain => config.EffectListOnGain;
        
        /// <summary>
        ///释放距离(普通攻击不走这个 走属性)
        /// </summary>
        public int ReleaseRange => config.ReleaseRange;
        
        /// <summary>
        ///释放技能时前摇时间(毫秒)
        /// </summary>
        public int BeforeTime => config.BeforeTime;
        
        /// <summary>
        ///释放技能时后摇时间(毫秒)
        /// </summary>
        public int AfterTime => config.AfterTime;
        
        /// <summary>
        ///技能CD（毫秒）(在释放技能结束后,后摇时间开始前,则开始计时)(普通攻击不计算在这里)
        /// </summary>
        public int CdTime => config.CdTime;
        
        /// <summary>
        ///是否'不能被打断' 0: 能被打断 , 1:不能被打断
        /// </summary>
        public int IsNoBreak => config.IsNoBreak;
        
        /// <summary>
        ///动画速度缩放(*1000),1 000为正常 根据前后摇校正动画 
        /// </summary>
        public int AnimationSpeedScale => config.AnimationSpeedScale;
        
        /// <summary>
        ///技能动画触发名称
        /// </summary>
        public string AnimationTriggerName => config.AnimationTriggerName;
        
        /// <summary>
        ///释放者释放技能时候在身上的特效
        /// </summary>
        public int ReleaserEffectResId => config.ReleaserEffectResId;
        
        /// <summary>
        ///技能指示器id
        /// </summary>
        public int SkillDirectionId => config.SkillDirectionId;
        
        /// <summary>
        ///技能轨迹 Id 列表
        /// </summary>
        public List<int> SkillTrackList => config.SkillTrackList;
        
        /// <summary>
        ///技能标签（目前是给 ai 作倾向用的）
        /// </summary>
        public List<int> TagList => config.TagList;
        
    } 
}