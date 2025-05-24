/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
namespace Battle
{
    
    
 
    public interface IPassiveEffect : IConfig
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
        ///触发的时机类型
        /// </summary>
        int TriggerTimeType {get;} 
        
        /// <summary>
        ///触发参数(intList)
        /// </summary>
        List<int> TriggerIntListParam {get;} 
        
        /// <summary>
        ///触发参数
        /// </summary>
        string TriggerParam {get;} 
        
        /// <summary>
        ///触发几率(千分比)
        /// </summary>
        int TriggerChance {get;} 
        
        /// <summary>
        ///触发目标类型
        /// </summary>
        int TriggerTargetType {get;} 
        
        /// <summary>
        ///触发效果列表
        /// </summary>
        List<int> TriggerEffectList {get;} 
        
        /// <summary>
        ///触发后移除的效果列表
        /// </summary>
        List<int> AfterTriggerRemoveEffectList {get;} 
        
        /// <summary>
        ///最大触发次数
        /// </summary>
        int MaxTriggerCount {get;} 
        
        /// <summary>
        ///触发CD(毫秒)
        /// </summary>
        int TriggerCD {get;} 
        
        /// <summary>
        ///触发时候的效果资源id（自身）
        /// </summary>
        int TriggerEffectResId {get;} 
        
    } 
}