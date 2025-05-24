/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
using Config;
namespace Battle
{
    
    
 
    public class SkillDirection_Impl : ISkillDirection
    {
        private Config.SkillDirection config;
        
        public void Init(int id)
        {
            config = ConfigManager.Instance.GetById<Config.SkillDirection>(id);
        }
        
        public int Id => config.Id;
        
        /// <summary>
        ///技能指向器名称
        /// </summary>
        public string Name => config.Name;
        
        /// <summary>
        ///技能指向器介绍
        /// </summary>
        public string Describe => config.Describe;
        
        /// <summary>
        ///技能释放 释放者 指示类型
        /// </summary>
        public int SkillReleaserDirectType => config.SkillReleaserDirectType;
        
        /// <summary>
        ///释放者指示参数
        /// </summary>
        public List<int> SkillReleaserDirectParam => config.SkillReleaserDirectParam;
        
        /// <summary>
        ///技能释放 投掷物 指示器类型
        /// </summary>
        public int SkillDirectorProjectileType => config.SkillDirectorProjectileType;
        
        /// <summary>
        ///技能释放 投掷物 指示器参数
        /// </summary>
        public List<int> SkillDirectorProjectileParam => config.SkillDirectorProjectileParam;
        
        /// <summary>
        ///技能释放 目标 指示类型
        /// </summary>
        public int SkillTargetDirectType => config.SkillTargetDirectType;
        
        /// <summary>
        ///目标指示参数
        /// </summary>
        public List<int> SkillTargetDirectParam => config.SkillTargetDirectParam;
        
    } 
}