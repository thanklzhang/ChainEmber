/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
namespace Config
{
    
    
    
       
    public class PassiveEffect : BaseConfig
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
        ///触发的时机类型
        /// </summary>
        private int triggerTimeType; 
        
        /// <summary>
        ///触发参数(intList)
        /// </summary>
        private List<int> triggerIntListParam; 
        
        /// <summary>
        ///触发参数
        /// </summary>
        private string triggerParam; 
        
        /// <summary>
        ///触发几率(千分比)
        /// </summary>
        private int triggerChance; 
        
        /// <summary>
        ///触发目标类型
        /// </summary>
        private int triggerTargetType; 
        
        /// <summary>
        ///触发效果列表
        /// </summary>
        private List<int> triggerEffectList; 
        
        /// <summary>
        ///触发后移除的效果列表
        /// </summary>
        private List<int> afterTriggerRemoveEffectList; 
        
        /// <summary>
        ///最大触发次数
        /// </summary>
        private int maxTriggerCount; 
        
        /// <summary>
        ///触发CD(毫秒)
        /// </summary>
        private int triggerCD; 
        
        /// <summary>
        ///触发时候的效果资源id（自身）
        /// </summary>
        private int triggerEffectResId; 
        

        
        public string Name { get => name; }     
        
        public string Describe { get => describe; }     
        
        public int TriggerTimeType { get => triggerTimeType; }     
        
        public List<int> TriggerIntListParam { get => triggerIntListParam; }     
        
        public string TriggerParam { get => triggerParam; }     
        
        public int TriggerChance { get => triggerChance; }     
        
        public int TriggerTargetType { get => triggerTargetType; }     
        
        public List<int> TriggerEffectList { get => triggerEffectList; }     
        
        public List<int> AfterTriggerRemoveEffectList { get => afterTriggerRemoveEffectList; }     
        
        public int MaxTriggerCount { get => maxTriggerCount; }     
        
        public int TriggerCD { get => triggerCD; }     
        
        public int TriggerEffectResId { get => triggerEffectResId; }     
        

    } 
}