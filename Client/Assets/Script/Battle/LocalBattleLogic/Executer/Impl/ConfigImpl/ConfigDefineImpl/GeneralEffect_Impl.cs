/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
using Config;
namespace Battle
{
    
    
 
    public class GeneralEffect_Impl : IGeneralEffect
    {
        private Config.GeneralEffect config;
        
        public void Init(int id)
        {
            config = ConfigManager.Instance.GetById<Config.GeneralEffect>(id);
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
        ///触发的事件动作类型
        /// </summary>
        public int TriggerEventType => config.TriggerEventType;
        
        /// <summary>
        ///触发参数列表
        /// </summary>
        public List<string> TriggerParamList => config.TriggerParamList;
        
        /// <summary>
        ///触发目标类型
        /// </summary>
        public int TriggerTargetType => config.TriggerTargetType;
        
    } 
}