/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
namespace Battle
{
    
    
 
    public interface IBattleReward : IConfig
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
        ///奖励图标
        /// </summary>
        int IconResId {get;} 
        
        /// <summary>
        ///奖励效果id列表
        /// </summary>
        List<int> RewardEffectOptionIds {get;} 
        
        /// <summary>
        ///获得数目
        /// </summary>
        int Count {get;} 
        
        /// <summary>
        ///确定实际奖励时机
        /// </summary>
        int MakeSureRewardOccasion {get;} 
        
        /// <summary>
        ///奖励产出上限(0表示无限制)
        /// </summary>
        int MaxAcquireCount {get;} 
        
    } 
}