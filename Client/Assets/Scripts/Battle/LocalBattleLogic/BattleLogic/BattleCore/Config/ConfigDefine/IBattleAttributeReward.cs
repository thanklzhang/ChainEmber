/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
namespace Battle
{
    
    
 
    public interface IBattleAttributeReward : IConfig
    {
        
        /// <summary>
        ///技能名称
        /// </summary>
        string Name {get;} 
        
        /// <summary>
        ///buff介绍
        /// </summary>
        string Describe {get;} 
        
        /// <summary>
        ///增加的属性组(,分割)
        /// </summary>
        List<int> AddedAttrGroup {get;} 
        
        /// <summary>
        ///增加属性组数值(,|分割 目前只做一个属性之只受一种属性增加)
        /// </summary>
        List<List<int>> AddedValueGroup {get;} 
        
        /// <summary>
        ///是否是持续性改变类型的属性
        /// </summary>
        List<int> IsAddedAttrGroupContinuous {get;} 
        
    } 
}