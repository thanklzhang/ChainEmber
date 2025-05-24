/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
namespace Battle
{
    
    
 
    public interface ITimeSequenceEffect : IConfig
    {
        
        /// <summary>
        ///名称
        /// </summary>
        string Name {get;} 
        
        /// <summary>
        ///间隔时间列表（*1000）
        /// </summary>
        List<int> IntervalTimeList {get;} 
        
        /// <summary>
        ///每个间隔时间到了的效果列表
        /// </summary>
        List<List<int>> IntervalEffectList {get;} 
        
    } 
}