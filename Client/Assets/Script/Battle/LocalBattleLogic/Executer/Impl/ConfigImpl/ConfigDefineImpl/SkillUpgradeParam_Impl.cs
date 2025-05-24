/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
using Config;
namespace Battle
{
    
    
 
    public class SkillUpgradeParam_Impl : ISkillUpgradeParam
    {
        private Config.SkillUpgradeParam config;
        
        public void Init(int id)
        {
            config = ConfigManager.Instance.GetById<Config.SkillUpgradeParam>(id);
        }
        
        public int Id => config.Id;
        
        /// <summary>
        ///技能每个等级升级所需要的经验
        /// </summary>
        public List<int> UpgradeExpPerLevel => config.UpgradeExpPerLevel;
        
        /// <summary>
        ///技能每个等级对应的分解经验
        /// </summary>
        public List<int> DecomoseExpPerLevel => config.DecomoseExpPerLevel;
        
    } 
}