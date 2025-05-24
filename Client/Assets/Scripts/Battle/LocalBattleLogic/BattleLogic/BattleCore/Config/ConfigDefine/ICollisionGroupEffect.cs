/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
namespace Battle
{
    
    
 
    public interface ICollisionGroupEffect : IConfig
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
        ///包含的技能效果id列表(表示这些效果碰撞到的实体只有第一次是正常效果)
        /// </summary>
        List<int> SkillEffectIds {get;} 
        
        /// <summary>
        ///影响效果类型
        /// </summary>
        int AffectType {get;} 
        
        /// <summary>
        ///影响效果参数
        /// </summary>
        int AffectParam {get;} 
        
        /// <summary>
        ///效果资源id
        /// </summary>
        int EffectResId {get;} 
        
    } 
}