/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
using Config;
namespace Battle
{
    
    
 
    public class Reward_Impl : IReward
    {
        private Config.Reward config;
        
        public void Init(int id)
        {
            config = ConfigManager.Instance.GetById<Config.Reward>(id);
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
        ///奖励道具id列表
        /// </summary>
        public List<int> RewardItemIdList => config.RewardItemIdList;
        
        /// <summary>
        ///获得数目列表
        /// </summary>
        public List<int> RewardItemCountList => config.RewardItemCountList;
        
    } 
}