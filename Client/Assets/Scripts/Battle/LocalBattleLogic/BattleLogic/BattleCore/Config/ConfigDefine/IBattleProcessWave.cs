/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
namespace Battle
{
    
    
 
    public interface IBattleProcessWave : IConfig
    {
        
        /// <summary>
        ///介绍
        /// </summary>
        string Describe {get;} 
        
        /// <summary>
        ///流程id
        /// </summary>
        int ProcessId {get;} 
        
        /// <summary>
        ///波次索引
        /// </summary>
        int WaveIndex {get;} 
        
        /// <summary>
        ///波次节点
        /// </summary>
        List<int> WaveNodeIdList {get;} 
        
        /// <summary>
        ///波次类型
        /// </summary>
        int WaveType {get;} 
        
        /// <summary>
        ///准备时间（*1000）
        /// </summary>
        int ReadyTime {get;} 
        
        /// <summary>
        ///限制时间（*1000）（战斗时间）
        /// </summary>
        int LimitTime {get;} 
        
        /// <summary>
        ///准备阶段获得的人口数
        /// </summary>
        int ReadyAddPopulationCount {get;} 
        
        /// <summary>
        ///准备阶段获得的奖励
        /// </summary>
        int ReadyRewardId {get;} 
        
        /// <summary>
        ///通过类型
        /// </summary>
        int PassType {get;} 
        
        /// <summary>
        ///通过奖励
        /// </summary>
        int PassRewardId {get;} 
        
    } 
}