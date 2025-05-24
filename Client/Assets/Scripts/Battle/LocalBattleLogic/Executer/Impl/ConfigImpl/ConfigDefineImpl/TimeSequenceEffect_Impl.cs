/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
using Config;
namespace Battle
{
    
    
 
    public class TimeSequenceEffect_Impl : ITimeSequenceEffect
    {
        private Config.TimeSequenceEffect config;
        
        public void Init(int id)
        {
            config = ConfigManager.Instance.GetById<Config.TimeSequenceEffect>(id);
        }
        
        public int Id => config.Id;
        
        /// <summary>
        ///名称
        /// </summary>
        public string Name => config.Name;
        
        /// <summary>
        ///间隔时间列表（*1000）
        /// </summary>
        public List<int> IntervalTimeList => config.IntervalTimeList;
        
        /// <summary>
        ///每个间隔时间到了的效果列表
        /// </summary>
        public List<List<int>> IntervalEffectList => config.IntervalEffectList;
        
    } 
}