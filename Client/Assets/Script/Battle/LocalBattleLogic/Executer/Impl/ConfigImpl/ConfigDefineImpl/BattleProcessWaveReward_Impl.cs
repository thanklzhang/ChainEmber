/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
using Config;
namespace Battle
{
    
    
 
    public class BattleProcessWaveReward_Impl : IBattleProcessWaveReward
    {
        private Config.BattleProcessWaveReward config;
        
        public void Init(int id)
        {
            config = ConfigManager.Instance.GetById<Config.BattleProcessWaveReward>(id);
        }
        
        public int Id => config.Id;
        
        /// <summary>
        ///介绍
        /// </summary>
        public string Describe => config.Describe;
        
        /// <summary>
        ///道具奖励
        /// </summary>
        public List<int> ItemIdList => config.ItemIdList;
        
        /// <summary>
        ///道具数目
        /// </summary>
        public List<int> ItemCountList => config.ItemCountList;
        
        /// <summary>
        ///固定箱子奖励
        /// </summary>
        public List<int> FixedBoxIdList => config.FixedBoxIdList;
        
        /// <summary>
        ///固定箱子奖励数目
        /// </summary>
        public List<int> FixedBoxCountList => config.FixedBoxCountList;
        
        /// <summary>
        ///概率箱子奖励
        /// </summary>
        public List<int> RandBoxIdList => config.RandBoxIdList;
        
        /// <summary>
        ///概率箱子权重
        /// </summary>
        public List<int> RandBoxWeightList => config.RandBoxWeightList;
        
        /// <summary>
        ///概率箱子数目
        /// </summary>
        public int RandBoxCount => config.RandBoxCount;
        
    } 
}