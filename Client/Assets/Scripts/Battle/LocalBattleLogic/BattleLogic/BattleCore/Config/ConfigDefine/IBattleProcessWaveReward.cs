/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
namespace Battle
{
    
    
 
    public interface IBattleProcessWaveReward : IConfig
    {
        
        /// <summary>
        ///介绍
        /// </summary>
        string Describe {get;} 
        
        /// <summary>
        ///道具奖励
        /// </summary>
        List<int> ItemIdList {get;} 
        
        /// <summary>
        ///道具数目
        /// </summary>
        List<int> ItemCountList {get;} 
        
        /// <summary>
        ///固定箱子奖励
        /// </summary>
        List<int> FixedBoxIdList {get;} 
        
        /// <summary>
        ///固定箱子奖励数目
        /// </summary>
        List<int> FixedBoxCountList {get;} 
        
        /// <summary>
        ///概率箱子奖励
        /// </summary>
        List<int> RandBoxIdList {get;} 
        
        /// <summary>
        ///概率箱子权重
        /// </summary>
        List<int> RandBoxWeightList {get;} 
        
        /// <summary>
        ///概率箱子数目
        /// </summary>
        int RandBoxCount {get;} 
        
    } 
}