/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
namespace Battle
{
    
    
 
    public interface IProjectileEffect : IConfig
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
        ///是否跟随
        /// </summary>
        int IsFollow {get;} 
        
        /// <summary>
        ///投掷物类型
        /// </summary>
        int ProjectileType {get;} 
        
        /// <summary>
        ///发射方向
        /// </summary>
        int DirType {get;} 
        
        /// <summary>
        ///持续时间(ms)
        /// </summary>
        int LastTime {get;} 
        
        /// <summary>
        ///偏转角度(适用非跟随)
        /// </summary>
        int DeflectionAngle {get;} 
        
        /// <summary>
        ///速度（* 1000）
        /// </summary>
        int Speed {get;} 
        
        /// <summary>
        ///碰撞半径(*1000)
        /// </summary>
        int CollisionRadius {get;} 
        
        /// <summary>
        ///是否穿透
        /// </summary>
        int IsThrough {get;} 
        
        /// <summary>
        ///是否飞行最大距离(只看飞行时间,不看是否到达技能目标点)
        /// </summary>
        int IsFlyMaxRange {get;} 
        
        /// <summary>
        ///开始的时候触发的效果列表
        /// </summary>
        List<int> StartEffectList {get;} 
        
        /// <summary>
        ///碰到物体时触发的效果列表
        /// </summary>
        List<int> CollisionEffectList {get;} 
        
        /// <summary>
        ///碰到物体后的伤害改变值（千分比，加法）
        /// </summary>
        int CollisionDamageChange {get;} 
        
        /// <summary>
        ///碰到物体后的伤害改变值限制（千分比，加法）
        /// </summary>
        int CollisionDamageChangeLimit {get;} 
        
        /// <summary>
        ///结束的时候触发的效果列表
        /// </summary>
        List<int> EndEffectList {get;} 
        
        /// <summary>
        ///碰撞实体关系筛选
        /// </summary>
        int CollisionEntityRelationFilterType {get;} 
        
        /// <summary>
        ///结束时转向次数
        /// </summary>
        int EndRedirectCount {get;} 
        
        /// <summary>
        ///结束时转向类型
        /// </summary>
        int EndRedirectType {get;} 
        
        /// <summary>
        ///结束时转向参数(intList)
        /// </summary>
        List<int> EndRedirectIntListParam {get;} 
        
        /// <summary>
        ///结束时转向之后持续时间
        /// </summary>
        int EndRedirectLastTime {get;} 
        
        /// <summary>
        ///是否结束时转向的时候保留碰撞实体信息
        /// </summary>
        int IsEndRedirectReserveCollisionInfo {get;} 
        
        /// <summary>
        ///效果资源id
        /// </summary>
        int EffectResId {get;} 
        
    } 
}