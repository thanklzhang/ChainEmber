/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
using Config;
namespace Battle
{
    
    
 
    public class PassiveEffect_Impl : IPassiveEffect
    {
        private Config.PassiveEffect config;
        
        public void Init(int id)
        {
            config = ConfigManager.Instance.GetById<Config.PassiveEffect>(id);
        }
        
        public int Id => config.Id;
        
        /// <summary>
        ///名称
        /// </summary>
        public string Name => config.Name;
        
        /// <summary>
        ///介绍
        /// </summary>
        public string Describe => config.Describe;
        
        /// <summary>
        ///触发的时机类型
        /// </summary>
        public int TriggerTimeType => config.TriggerTimeType;
        
        /// <summary>
        ///触发参数(intList)
        /// </summary>
        public List<int> TriggerIntListParam => config.TriggerIntListParam;
        
        /// <summary>
        ///触发参数
        /// </summary>
        public string TriggerParam => config.TriggerParam;
        
        /// <summary>
        ///触发几率(千分比)
        /// </summary>
        public int TriggerChance => config.TriggerChance;
        
        /// <summary>
        ///触发目标类型
        /// </summary>
        public int TriggerTargetType => config.TriggerTargetType;
        
        /// <summary>
        ///触发效果列表
        /// </summary>
        public List<int> TriggerEffectList => config.TriggerEffectList;
        
        /// <summary>
        ///触发后移除的效果列表
        /// </summary>
        public List<int> AfterTriggerRemoveEffectList => config.AfterTriggerRemoveEffectList;
        
        /// <summary>
        ///最大触发次数
        /// </summary>
        public int MaxTriggerCount => config.MaxTriggerCount;
        
        /// <summary>
        ///触发CD(毫秒)
        /// </summary>
        public int TriggerCD => config.TriggerCD;
        
        /// <summary>
        ///触发时候的效果资源id（自身）
        /// </summary>
        public int TriggerEffectResId => config.TriggerEffectResId;
        
    } 
}