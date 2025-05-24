/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
namespace Config
{
    
    
    
       
    public class SkillUpgradeParam : BaseConfig
    {
        
        /// <summary>
        ///技能每个等级升级所需要的经验
        /// </summary>
        private List<int> upgradeExpPerLevel; 
        
        /// <summary>
        ///技能每个等级对应的分解经验
        /// </summary>
        private List<int> decomoseExpPerLevel; 
        

        
        public List<int> UpgradeExpPerLevel { get => upgradeExpPerLevel; }     
        
        public List<int> DecomoseExpPerLevel { get => decomoseExpPerLevel; }     
        

    } 
}