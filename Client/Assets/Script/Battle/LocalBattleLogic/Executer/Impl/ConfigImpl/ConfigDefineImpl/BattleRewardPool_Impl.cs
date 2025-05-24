/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
using Config;
namespace Battle
{
    
    
 
    public class BattleRewardPool_Impl : IBattleRewardPool
    {
        private Config.BattleRewardPool config;
        
        public void Init(int id)
        {
            config = ConfigManager.Instance.GetById<Config.BattleRewardPool>(id);
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
        ///奖励id总列表
        /// </summary>
        public List<int> RewardIdList => config.RewardIdList;
        
        /// <summary>
        ///奖励权重
        /// </summary>
        public List<int> RewardWeightList => config.RewardWeightList;
        
        /// <summary>
        ///固定奖励（一定产出）
        /// </summary>
        public List<int> FixedRewardList => config.FixedRewardList;
        
    } 
}