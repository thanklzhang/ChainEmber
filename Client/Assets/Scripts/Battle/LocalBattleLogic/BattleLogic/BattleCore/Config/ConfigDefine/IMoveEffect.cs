/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
namespace Battle
{
    
    
 
    public interface IMoveEffect : IConfig
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
        ///移动速度(*1000)
        /// </summary>
        int MoveSpeed {get;} 
        
        /// <summary>
        ///移动目标类型
        /// </summary>
        int MoveTargetPosType {get;} 
        
        /// <summary>
        ///移动过程类型
        /// </summary>
        int MoveProcessType {get;} 
        
        /// <summary>
        ///移动终点类型
        /// </summary>
        int EndPosType {get;} 
        
        /// <summary>
        ///持续时间(无目标点类型适用)(*1000)
        /// </summary>
        int LastTime {get;} 
        
        /// <summary>
        ///开始的时候触发的效果列表
        /// </summary>
        List<int> StartEffectList {get;} 
        
        /// <summary>
        ///到达的时候触发的效果列表（如果被打断则不会触发）
        /// </summary>
        List<int> ReachEffectList {get;} 
        
        /// <summary>
        ///结束的时候移除的效果列表(无论被打断还是不打断都会触发)
        /// </summary>
        List<int> EndRemoveEffectList {get;} 
        
        /// <summary>
        ///此技能效果结束时是否判定为释放者技能结束
        /// </summary>
        int IsThisEndForSkillEnd {get;} 
        
        /// <summary>
        ///效果资源id
        /// </summary>
        int EffectResId {get;} 
        
    } 
}