/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
namespace Battle
{
    
    
 
    public interface IBattleProcessWaveNode : IConfig
    {
        
        /// <summary>
        ///介绍
        /// </summary>
        string Describe {get;} 
        
        /// <summary>
        ///介绍2
        /// </summary>
        string Describe2 {get;} 
        
        /// <summary>
        ///波次索引
        /// </summary>
        int Index {get;} 
        
        /// <summary>
        ///是否是本波结束节点（如果是怪物死亡胜利的条件，那么这波怪物只有一个，怪物死亡就胜利）
        /// </summary>
        int IsEndNode {get;} 
        
        /// <summary>
        ///初始延迟（*1000）
        /// </summary>
        int DelayTime {get;} 
        
        /// <summary>
        ///间隔时间（*1000）
        /// </summary>
        int IntervalTime {get;} 
        
        /// <summary>
        ///触发次数
        /// </summary>
        int TriggerCount {get;} 
        
        /// <summary>
        ///实体实例id
        /// </summary>
        int EntityInstanceId {get;} 
        
        /// <summary>
        ///实体个数
        /// </summary>
        int EntityCount {get;} 
        
        /// <summary>
        ///生成位置类型
        /// </summary>
        int PosType {get;} 
        
    } 
}