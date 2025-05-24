/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
namespace Config
{
    
    
    
       
    public class GeneralEffect : BaseConfig
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
        ///触发的事件动作类型
        /// </summary>
        private int triggerEventType; 
        
        /// <summary>
        ///触发参数列表
        /// </summary>
        private List<string> triggerParamList; 
        
        /// <summary>
        ///触发目标类型
        /// </summary>
        private int triggerTargetType; 
        

        
        public string Name { get => name; }     
        
        public string Describe { get => describe; }     
        
        public int TriggerEventType { get => triggerEventType; }     
        
        public List<string> TriggerParamList { get => triggerParamList; }     
        
        public int TriggerTargetType { get => triggerTargetType; }     
        

    } 
}