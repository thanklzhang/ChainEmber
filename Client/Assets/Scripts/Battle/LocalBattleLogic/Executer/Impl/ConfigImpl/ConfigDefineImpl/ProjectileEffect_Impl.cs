/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
using Config;
namespace Battle
{
    
    
 
    public class ProjectileEffect_Impl : IProjectileEffect
    {
        private Config.ProjectileEffect config;
        
        public void Init(int id)
        {
            config = ConfigManager.Instance.GetById<Config.ProjectileEffect>(id);
        }
        
        public int Id => config.Id;
        
        /// <summary>
        ///技能名称
        /// </summary>
        public string Name => config.Name;
        
        /// <summary>
        ///技能介绍
        /// </summary>
        public string Describe => config.Describe;
        
        /// <summary>
        ///是否跟随
        /// </summary>
        public int IsFollow => config.IsFollow;
        
        /// <summary>
        ///投掷物类型
        /// </summary>
        public int ProjectileType => config.ProjectileType;
        
        /// <summary>
        ///发射方向
        /// </summary>
        public int DirType => config.DirType;
        
        /// <summary>
        ///持续时间(ms)
        /// </summary>
        public int LastTime => config.LastTime;
        
        /// <summary>
        ///偏转角度(适用非跟随)
        /// </summary>
        public int DeflectionAngle => config.DeflectionAngle;
        
        /// <summary>
        ///速度（* 1000）
        /// </summary>
        public int Speed => config.Speed;
        
        /// <summary>
        ///碰撞半径(*1000)
        /// </summary>
        public int CollisionRadius => config.CollisionRadius;
        
        /// <summary>
        ///是否穿透
        /// </summary>
        public int IsThrough => config.IsThrough;
        
        /// <summary>
        ///是否飞行最大距离(只看飞行时间,不看是否到达技能目标点)
        /// </summary>
        public int IsFlyMaxRange => config.IsFlyMaxRange;
        
        /// <summary>
        ///开始的时候触发的效果列表
        /// </summary>
        public List<int> StartEffectList => config.StartEffectList;
        
        /// <summary>
        ///碰到物体时触发的效果列表
        /// </summary>
        public List<int> CollisionEffectList => config.CollisionEffectList;
        
        /// <summary>
        ///碰到物体后的伤害改变值（千分比，加法）
        /// </summary>
        public int CollisionDamageChange => config.CollisionDamageChange;
        
        /// <summary>
        ///碰到物体后的伤害改变值限制（千分比，加法）
        /// </summary>
        public int CollisionDamageChangeLimit => config.CollisionDamageChangeLimit;
        
        /// <summary>
        ///结束的时候触发的效果列表
        /// </summary>
        public List<int> EndEffectList => config.EndEffectList;
        
        /// <summary>
        ///碰撞实体关系筛选
        /// </summary>
        public int CollisionEntityRelationFilterType => config.CollisionEntityRelationFilterType;
        
        /// <summary>
        ///结束时转向次数
        /// </summary>
        public int EndRedirectCount => config.EndRedirectCount;
        
        /// <summary>
        ///结束时转向类型
        /// </summary>
        public int EndRedirectType => config.EndRedirectType;
        
        /// <summary>
        ///结束时转向参数(intList)
        /// </summary>
        public List<int> EndRedirectIntListParam => config.EndRedirectIntListParam;
        
        /// <summary>
        ///结束时转向之后持续时间
        /// </summary>
        public int EndRedirectLastTime => config.EndRedirectLastTime;
        
        /// <summary>
        ///是否结束时转向的时候保留碰撞实体信息
        /// </summary>
        public int IsEndRedirectReserveCollisionInfo => config.IsEndRedirectReserveCollisionInfo;
        
        /// <summary>
        ///效果资源id
        /// </summary>
        public int EffectResId => config.EffectResId;
        
    } 
}