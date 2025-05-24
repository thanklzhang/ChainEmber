/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
namespace Config
{
    
    
    
       
    public class BattleRewardPool : BaseConfig
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
        ///奖励id总列表
        /// </summary>
        private List<int> rewardIdList; 
        
        /// <summary>
        ///奖励权重
        /// </summary>
        private List<int> rewardWeightList; 
        
        /// <summary>
        ///固定奖励（一定产出）
        /// </summary>
        private List<int> fixedRewardList; 
        

        
        public string Name { get => name; }     
        
        public string Describe { get => describe; }     
        
        public List<int> RewardIdList { get => rewardIdList; }     
        
        public List<int> RewardWeightList { get => rewardWeightList; }     
        
        public List<int> FixedRewardList { get => fixedRewardList; }     
        

    } 
}