/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
namespace Battle
{
    
    
 
    public interface IBattleRewardEffectOption : IConfig
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
        ///获得时机类型
        /// </summary>
        int GainTimingType {get;} 
        
        /// <summary>
        ///类型
        /// </summary>
        int Type {get;} 
        
        /// <summary>
        ///效果值列表
        /// </summary>
        List<int> ValueList {get;} 
        
        /// <summary>
        ///奖励中数值的权重
        /// </summary>
        List<int> WeightList {get;} 
        
        /// <summary>
        ///参数，类型不同意义也不同
        /// </summary>
        List<int> ParamIntList {get;} 
        
        /// <summary>
        ///最大获得次数
        /// </summary>
        int MaxGainTimesType {get;} 
        
        /// <summary>
        ///奖励效果是否对新生成的队友也生效（目前适用于全员加属性和buff）
        /// </summary>
        int ApplyToNewTeamMembers {get;} 
        
    } 
}