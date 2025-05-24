/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
using Config;
namespace Battle
{
    
    
 
    public class BattleReward_Impl : IBattleReward
    {
        private Config.BattleReward config;
        
        public void Init(int id)
        {
            config = ConfigManager.Instance.GetById<Config.BattleReward>(id);
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
        ///奖励图标
        /// </summary>
        public int IconResId => config.IconResId;
        
        /// <summary>
        ///奖励效果id列表
        /// </summary>
        public List<int> RewardEffectOptionIds => config.RewardEffectOptionIds;
        
        /// <summary>
        ///获得数目
        /// </summary>
        public int Count => config.Count;
        
        /// <summary>
        ///确定实际奖励时机
        /// </summary>
        public int MakeSureRewardOccasion => config.MakeSureRewardOccasion;
        
        /// <summary>
        ///奖励产出上限(0表示无限制)
        /// </summary>
        public int MaxAcquireCount => config.MaxAcquireCount;
        
    } 
}