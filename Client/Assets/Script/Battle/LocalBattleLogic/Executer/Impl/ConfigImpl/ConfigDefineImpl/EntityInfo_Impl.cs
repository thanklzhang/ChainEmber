/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
using Config;
namespace Battle
{
    
    
 
    public class EntityInfo_Impl : IEntityInfo
    {
        private Config.EntityInfo config;
        
        public void Init(int id)
        {
            config = ConfigManager.Instance.GetById<Config.EntityInfo>(id);
        }
        
        public int Id => config.Id;
        
        /// <summary>
        ///名称
        /// </summary>
        public string Name => config.Name;
        
        /// <summary>
        ///介绍
        /// </summary>
        public string Describe => config.Describe;
        
        /// <summary>
        ///战斗相关介绍
        /// </summary>
        public string Describe2 => config.Describe2;
        
        /// <summary>
        ///类型(0 npc单位实例 , 1 英雄模板)
        /// </summary>
        public int Type => config.Type;
        
        /// <summary>
        ///模型Id(先填资源id 之后换成另一张表格过度)
        /// </summary>
        public int ModelId => config.ModelId;
        
        /// <summary>
        ///基础属性模板id
        /// </summary>
        public int BaseAttrId => config.BaseAttrId;
        
        /// <summary>
        ///等级属性模板id
        /// </summary>
        public int LevelAttrId => config.LevelAttrId;
        
        /// <summary>
        ///星级属性模板id
        /// </summary>
        public int StarAttrId => config.StarAttrId;
        
        /// <summary>
        ///技能id列表
        /// </summary>
        public List<int> SkillIds => config.SkillIds;
        
        /// <summary>
        ///大招技能配置id
        /// </summary>
        public int UltimateSkillId => config.UltimateSkillId;
        
        /// <summary>
        ///等级
        /// </summary>
        public int Level_pre => config.Level_pre;
        
        /// <summary>
        ///品质
        /// </summary>
        public int Quality => config.Quality;
        
        /// <summary>
        ///星级
        /// </summary>
        public int Star_pre => config.Star_pre;
        
        /// <summary>
        ///技能等级
        /// </summary>
        public List<int> SkillLevels_pre => config.SkillLevels_pre;
        
        /// <summary>
        ///源实体id，某些情况和这个实体算作一个，如 不同等级的召唤兽都算作一个 id
        /// </summary>
        public int OriginEntityId => config.OriginEntityId;
        
        /// <summary>
        ///AI脚本
        /// </summary>
        public string AiScript => config.AiScript;
        
        /// <summary>
        ///是否boss
        /// </summary>
        public int IsBoss => config.IsBoss;
        
        /// <summary>
        ///头像资源id
        /// </summary>
        public int AvatarResId => config.AvatarResId;
        
        /// <summary>
        ///全身像资源id
        /// </summary>
        public int AllBodyResId => config.AllBodyResId;
        
    } 
}