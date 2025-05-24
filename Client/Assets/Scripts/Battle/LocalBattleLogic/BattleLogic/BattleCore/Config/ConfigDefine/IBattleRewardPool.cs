/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
namespace Battle
{
    
    
 
    public interface IBattleRewardPool : IConfig
    {
        
        /// <summary>
        ///名称
        /// </summary>
        string Name {get;} 
        
        /// <summary>
        ///介绍
        /// </summary>
        string Describe {get;} 
        
        /// <summary>
        ///奖励id总列表
        /// </summary>
        List<int> RewardIdList {get;} 
        
        /// <summary>
        ///奖励权重
        /// </summary>
        List<int> RewardWeightList {get;} 
        
        /// <summary>
        ///固定奖励（一定产出）
        /// </summary>
        List<int> FixedRewardList {get;} 
        
    } 
}