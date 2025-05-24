/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
namespace Config
{
    
    
    
       
    public class EntityInfo : BaseConfig
    {
        
        /// <summary>
        ///名称
        /// </summary>
        private string name; 
        
        /// <summary>
        ///介绍
        /// </summary>
        private string describe; 
        
        /// <summary>
        ///战斗相关介绍
        /// </summary>
        private string describe2; 
        
        /// <summary>
        ///类型(0 npc单位实例 , 1 英雄模板)
        /// </summary>
        private int type; 
        
        /// <summary>
        ///模型Id(先填资源id 之后换成另一张表格过度)
        /// </summary>
        private int modelId; 
        
        /// <summary>
        ///基础属性模板id
        /// </summary>
        private int baseAttrId; 
        
        /// <summary>
        ///等级属性模板id
        /// </summary>
        private int levelAttrId; 
        
        /// <summary>
        ///星级属性模板id
        /// </summary>
        private int starAttrId; 
        
        /// <summary>
        ///技能id列表
        /// </summary>
        private List<int> skillIds; 
        
        /// <summary>
        ///大招技能配置id
        /// </summary>
        private int ultimateSkillId; 
        
        /// <summary>
        ///等级
        /// </summary>
        private int level_pre; 
        
        /// <summary>
        ///品质
        /// </summary>
        private int quality; 
        
        /// <summary>
        ///星级
        /// </summary>
        private int star_pre; 
        
        /// <summary>
        ///技能等级
        /// </summary>
        private List<int> skillLevels_pre; 
        
        /// <summary>
        ///源实体id，某些情况和这个实体算作一个，如 不同等级的召唤兽都算作一个 id
        /// </summary>
        private int originEntityId; 
        
        /// <summary>
        ///AI脚本
        /// </summary>
        private string aiScript; 
        
        /// <summary>
        ///是否boss
        /// </summary>
        private int isBoss; 
        
        /// <summary>
        ///头像资源id
        /// </summary>
        private int avatarResId; 
        
        /// <summary>
        ///全身像资源id
        /// </summary>
        private int allBodyResId; 
        

        
        public string Name { get => name; }     
        
        public string Describe { get => describe; }     
        
        public string Describe2 { get => describe2; }     
        
        public int Type { get => type; }     
        
        public int ModelId { get => modelId; }     
        
        public int BaseAttrId { get => baseAttrId; }     
        
        public int LevelAttrId { get => levelAttrId; }     
        
        public int StarAttrId { get => starAttrId; }     
        
        public List<int> SkillIds { get => skillIds; }     
        
        public int UltimateSkillId { get => ultimateSkillId; }     
        
        public int Level_pre { get => level_pre; }     
        
        public int Quality { get => quality; }     
        
        public int Star_pre { get => star_pre; }     
        
        public List<int> SkillLevels_pre { get => skillLevels_pre; }     
        
        public int OriginEntityId { get => originEntityId; }     
        
        public string AiScript { get => aiScript; }     
        
        public int IsBoss { get => isBoss; }     
        
        public int AvatarResId { get => avatarResId; }     
        
        public int AllBodyResId { get => allBodyResId; }     
        

    } 
}