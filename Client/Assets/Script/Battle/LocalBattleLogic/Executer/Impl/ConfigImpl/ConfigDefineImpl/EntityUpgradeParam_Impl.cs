/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
using Config;
namespace Battle
{
    
    
 
    public class EntityUpgradeParam_Impl : IEntityUpgradeParam
    {
        private Config.EntityUpgradeParam config;
        
        public void Init(int id)
        {
            config = ConfigManager.Instance.GetById<Config.EntityUpgradeParam>(id);
        }
        
        public int Id => config.Id;
        
        /// <summary>
        ///英雄每次升星所需要的升星经验
        /// </summary>
        public List<int> UpgradeExpPerStarLevel => config.UpgradeExpPerStarLevel;
        
        /// <summary>
        ///英雄每个星级对应的分解星级经验
        /// </summary>
        public List<int> DecomposeExpPerStarLevel => config.DecomposeExpPerStarLevel;
        
    } 
}