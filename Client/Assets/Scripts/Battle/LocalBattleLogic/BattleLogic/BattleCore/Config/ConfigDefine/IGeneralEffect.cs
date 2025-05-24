/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
namespace Battle
{
    
    
 
    public interface IGeneralEffect : IConfig
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
        ///触发的事件动作类型
        /// </summary>
        int TriggerEventType {get;} 
        
        /// <summary>
        ///触发参数列表
        /// </summary>
        List<string> TriggerParamList {get;} 
        
        /// <summary>
        ///触发目标类型
        /// </summary>
        int TriggerTargetType {get;} 
        
    } 
}