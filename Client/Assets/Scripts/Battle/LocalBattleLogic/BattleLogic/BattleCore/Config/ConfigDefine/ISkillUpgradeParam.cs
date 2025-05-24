/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
namespace Battle
{
    
    
 
    public interface ISkillUpgradeParam : IConfig
    {
        
        /// <summary>
        ///技能每个等级升级所需要的经验
        /// </summary>
        List<int> UpgradeExpPerLevel {get;} 
        
        /// <summary>
        ///技能每个等级对应的分解经验
        /// </summary>
        List<int> DecomoseExpPerLevel {get;} 
        
    } 
}