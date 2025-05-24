/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
namespace Config
{
    
    
    
       
    public class BattleReward : BaseConfig
    {
        
        /// <summary>
        ///名称
        /// </summary>
        private string name; 
        
        /// <summary>
        ///介绍
        /// </summary>
        private string describe; 
        
        /// <summary>
        ///奖励图标
        /// </summary>
        private int iconResId; 
        
        /// <summary>
        ///奖励效果id列表
        /// </summary>
        private List<int> rewardEffectOptionIds; 
        
        /// <summary>
        ///获得数目
        /// </summary>
        private int count; 
        
        /// <summary>
        ///确定实际奖励时机
        /// </summary>
        private int makeSureRewardOccasion; 
        
        /// <summary>
        ///奖励产出上限(0表示无限制)
        /// </summary>
        private int maxAcquireCount; 
        

        
        public string Name { get => name; }     
        
        public string Describe { get => describe; }     
        
        public int IconResId { get => iconResId; }     
        
        public List<int> RewardEffectOptionIds { get => rewardEffectOptionIds; }     
        
        public int Count { get => count; }     
        
        public int MakeSureRewardOccasion { get => makeSureRewardOccasion; }     
        
        public int MaxAcquireCount { get => maxAcquireCount; }     
        

    } 
}