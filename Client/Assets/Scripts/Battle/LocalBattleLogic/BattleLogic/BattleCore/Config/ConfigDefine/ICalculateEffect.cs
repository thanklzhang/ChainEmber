/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
namespace Battle
{
    
    
 
    public interface ICalculateEffect : IConfig
    {
        
        /// <summary>
        ///技能名称
        /// </summary>
        string Name {get;} 
        
        /// <summary>
        ///技能介绍
        /// </summary>
        string Describe {get;} 
        
        /// <summary>
        ///计算目标类型
        /// </summary>
        int CalculateEffectTargetType {get;} 
        
        /// <summary>
        ///附加伤害组（, | ）
        /// </summary>
        List<List<int>> AddedValueGroup {get;} 
        
        /// <summary>
        ///伤害附加的倍数计算效果列表（适用比值）
        /// </summary>
        List<int> AddedValueScaleEffectIds {get;} 
        
        /// <summary>
        ///最终的效果类型 1 物理伤害 2 法强伤害 
        /// </summary>
        int FinalEffectType {get;} 
        
        /// <summary>
        ///伤害时候的效果ids
        /// </summary>
        List<int> AfterDamageEffectIds {get;} 
        
        /// <summary>
        ///效果资源id
        /// </summary>
        int EffectResId {get;} 
        
        /// <summary>
        ///产生效果点位置类型
        /// </summary>
        int EffectPosType {get;} 
        
        /// <summary>
        ///特效是否跟随目标
        /// </summary>
        int IsEffectFollowTarget {get;} 
        
        /// <summary>
        ///效果点节点名称
        /// </summary>
        string EffectPosName {get;} 
        
        /// <summary>
        ///是否为了显示（只同步效果，实际效果等均不触发）
        /// </summary>
        int IsForShow {get;} 
        
    } 
}