/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
namespace Config
{
    
    
    
       
    public class Skill : BaseConfig
    {
        
        /// <summary>
        ///技能名称
        /// </summary>
        private string name; 
        
        /// <summary>
        ///技能介绍
        /// </summary>
        private string describe; 
        
        /// <summary>
        ///技能等级
        /// </summary>
        private int level; 
        
        /// <summary>
        ///技能图标资源id
        /// </summary>
        private int iconResId; 
        
        /// <summary>
        ///技能类别
        /// </summary>
        private int skillCategory; 
        
        /// <summary>
        ///技能释放目标类型
        /// </summary>
        private int skillReleaseTargeType; 
        
        /// <summary>
        ///技能释放类型 
        /// </summary>
        private int skillReleaseType; 
        
        /// <summary>
        ///是否是被动技能
        /// </summary>
        private int isPassiveSkill; 
        
        /// <summary>
        ///实体按照关系筛选
        /// </summary>
        private int entityRelationFilterType; 
        
        /// <summary>
        ///技能效果目标类型（选取即将触发效果的单位）
        /// </summary>
        private int skillEffectTargetType; 
        
        /// <summary>
        ///触发的效果列表（主动释放产生的效果）
        /// </summary>
        private List<int> effectList; 
        
        /// <summary>
        ///获得时候触发的效果列表（获得技能的时候立即生效）
        /// </summary>
        private List<int> effectListOnGain; 
        
        /// <summary>
        ///释放距离(普通攻击不走这个 走属性)
        /// </summary>
        private int releaseRange; 
        
        /// <summary>
        ///释放技能时前摇时间(毫秒)
        /// </summary>
        private int beforeTime; 
        
        /// <summary>
        ///释放技能时后摇时间(毫秒)
        /// </summary>
        private int afterTime; 
        
        /// <summary>
        ///技能CD（毫秒）(在释放技能结束后,后摇时间开始前,则开始计时)(普通攻击不计算在这里)
        /// </summary>
        private int cdTime; 
        
        /// <summary>
        ///是否'不能被打断' 0: 能被打断 , 1:不能被打断
        /// </summary>
        private int isNoBreak; 
        
        /// <summary>
        ///动画速度缩放(*1000),1 000为正常 根据前后摇校正动画 
        /// </summary>
        private int animationSpeedScale; 
        
        /// <summary>
        ///技能动画触发名称
        /// </summary>
        private string animationTriggerName; 
        
        /// <summary>
        ///释放者释放技能时候在身上的特效
        /// </summary>
        private int releaserEffectResId; 
        
        /// <summary>
        ///技能指示器id
        /// </summary>
        private int skillDirectionId; 
        
        /// <summary>
        ///技能轨迹 Id 列表
        /// </summary>
        private List<int> skillTrackList; 
        
        /// <summary>
        ///技能标签（目前是给 ai 作倾向用的）
        /// </summary>
        private List<int> tagList; 
        

        
        public string Name { get => name; }     
        
        public string Describe { get => describe; }     
        
        public int Level { get => level; }     
        
        public int IconResId { get => iconResId; }     
        
        public int SkillCategory { get => skillCategory; }     
        
        public int SkillReleaseTargeType { get => skillReleaseTargeType; }     
        
        public int SkillReleaseType { get => skillReleaseType; }     
        
        public int IsPassiveSkill { get => isPassiveSkill; }     
        
        public int EntityRelationFilterType { get => entityRelationFilterType; }     
        
        public int SkillEffectTargetType { get => skillEffectTargetType; }     
        
        public List<int> EffectList { get => effectList; }     
        
        public List<int> EffectListOnGain { get => effectListOnGain; }     
        
        public int ReleaseRange { get => releaseRange; }     
        
        public int BeforeTime { get => beforeTime; }     
        
        public int AfterTime { get => afterTime; }     
        
        public int CdTime { get => cdTime; }     
        
        public int IsNoBreak { get => isNoBreak; }     
        
        public int AnimationSpeedScale { get => animationSpeedScale; }     
        
        public string AnimationTriggerName { get => animationTriggerName; }     
        
        public int ReleaserEffectResId { get => releaserEffectResId; }     
        
        public int SkillDirectionId { get => skillDirectionId; }     
        
        public List<int> SkillTrackList { get => skillTrackList; }     
        
        public List<int> TagList { get => tagList; }     
        

    } 
}