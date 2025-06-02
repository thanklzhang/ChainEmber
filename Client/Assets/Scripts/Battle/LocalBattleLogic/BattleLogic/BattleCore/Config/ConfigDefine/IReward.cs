/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
namespace Battle
{
    
    
 
    public interface IReward : IConfig
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
        ///奖励道具id列表
        /// </summary>
        List<int> RewardItemIdList {get;} 
        
        /// <summary>
        ///获得数目列表
        /// </summary>
        List<int> RewardItemCountList {get;} 
        
    } 
}