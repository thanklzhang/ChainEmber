/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
namespace Config
{
    
    
    
       
    public class TimeSequenceEffect : BaseConfig
    {
        
        /// <summary>
        ///名称
        /// </summary>
        private string name; 
        
        /// <summary>
        ///间隔时间列表（*1000）
        /// </summary>
        private List<int> intervalTimeList; 
        
        /// <summary>
        ///每个间隔时间到了的效果列表
        /// </summary>
        private List<List<int>> intervalEffectList; 
        

        
        public string Name { get => name; }     
        
        public List<int> IntervalTimeList { get => intervalTimeList; }     
        
        public List<List<int>> IntervalEffectList { get => intervalEffectList; }     
        

    } 
}