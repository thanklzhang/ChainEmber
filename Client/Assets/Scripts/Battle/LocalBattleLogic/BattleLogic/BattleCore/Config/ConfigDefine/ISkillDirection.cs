/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
namespace Battle
{
    
    
 
    public interface ISkillDirection : IConfig
    {
        
        /// <summary>
        ///技能指向器名称
        /// </summary>
        string Name {get;} 
        
        /// <summary>
        ///技能指向器介绍
        /// </summary>
        string Describe {get;} 
        
        /// <summary>
        ///技能释放 释放者 指示类型
        /// </summary>
        int SkillReleaserDirectType {get;} 
        
        /// <summary>
        ///释放者指示参数
        /// </summary>
        List<int> SkillReleaserDirectParam {get;} 
        
        /// <summary>
        ///技能释放 投掷物 指示器类型
        /// </summary>
        int SkillDirectorProjectileType {get;} 
        
        /// <summary>
        ///技能释放 投掷物 指示器参数
        /// </summary>
        List<int> SkillDirectorProjectileParam {get;} 
        
        /// <summary>
        ///技能释放 目标 指示类型
        /// </summary>
        int SkillTargetDirectType {get;} 
        
        /// <summary>
        ///目标指示参数
        /// </summary>
        List<int> SkillTargetDirectParam {get;} 
        
    } 
}