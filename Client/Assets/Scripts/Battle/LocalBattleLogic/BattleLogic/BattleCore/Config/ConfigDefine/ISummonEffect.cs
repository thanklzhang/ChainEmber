/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
namespace Battle
{
    
    
 
    public interface ISummonEffect : IConfig
    {
        
        /// <summary>
        ///技能名称
        /// </summary>
        string Name {get;} 
        
        /// <summary>
        ///介绍
        /// </summary>
        string Describe {get;} 
        
        /// <summary>
        ///召唤物实体配置id
        /// </summary>
        int SummonConfigId {get;} 
        
        /// <summary>
        ///召唤物出生点类型
        /// </summary>
        int SummonBornPosType {get;} 
        
        /// <summary>
        ///召唤数量
        /// </summary>
        int SummonCount {get;} 
        
        /// <summary>
        ///最大召唤数量
        /// </summary>
        int MaxSummonCount {get;} 
        
        /// <summary>
        ///召唤物持续时间(ms)
        /// </summary>
        int LastTime {get;} 
        
        /// <summary>
        ///持续时间附加组(ms)
        /// </summary>
        List<List<int>> LastTimeAddedGroup {get;} 
        
        /// <summary>
        ///开始的时候在召唤物身上触发的效果列表(例如增加和召唤者相关属性的附加)
        /// </summary>
        List<int> StartEffectList {get;} 
        
        /// <summary>
        ///召唤物增加的属性组(,分割)
        /// </summary>
        List<int> AddedAttrGroup {get;} 
        
        /// <summary>
        ///增加属性组数值(,|分割 目前只做一个属性之只受一种属性增加)
        /// </summary>
        List<List<int>> AddedValueGroup {get;} 
        
        /// <summary>
        ///效果资源id
        /// </summary>
        int EffectResId {get;} 
        
    } 
}