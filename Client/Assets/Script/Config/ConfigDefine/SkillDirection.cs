/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
namespace Config
{
    
    
    
       
    public class SkillDirection : BaseConfig
    {
        
        /// <summary>
        ///技能指向器名称
        /// </summary>
        private string name; 
        
        /// <summary>
        ///技能指向器介绍
        /// </summary>
        private string describe; 
        
        /// <summary>
        ///技能释放 释放者 指示类型
        /// </summary>
        private int skillReleaserDirectType; 
        
        /// <summary>
        ///释放者指示参数
        /// </summary>
        private List<int> skillReleaserDirectParam; 
        
        /// <summary>
        ///技能释放 投掷物 指示器类型
        /// </summary>
        private int skillDirectorProjectileType; 
        
        /// <summary>
        ///技能释放 投掷物 指示器参数
        /// </summary>
        private List<int> skillDirectorProjectileParam; 
        
        /// <summary>
        ///技能释放 目标 指示类型
        /// </summary>
        private int skillTargetDirectType; 
        
        /// <summary>
        ///目标指示参数
        /// </summary>
        private List<int> skillTargetDirectParam; 
        

        
        public string Name { get => name; }     
        
        public string Describe { get => describe; }     
        
        public int SkillReleaserDirectType { get => skillReleaserDirectType; }     
        
        public List<int> SkillReleaserDirectParam { get => skillReleaserDirectParam; }     
        
        public int SkillDirectorProjectileType { get => skillDirectorProjectileType; }     
        
        public List<int> SkillDirectorProjectileParam { get => skillDirectorProjectileParam; }     
        
        public int SkillTargetDirectType { get => skillTargetDirectType; }     
        
        public List<int> SkillTargetDirectParam { get => skillTargetDirectParam; }     
        

    } 
}