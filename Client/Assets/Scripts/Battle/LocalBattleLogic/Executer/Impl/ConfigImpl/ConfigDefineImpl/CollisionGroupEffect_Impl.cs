/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
using Config;
namespace Battle
{
    
    
 
    public class CollisionGroupEffect_Impl : ICollisionGroupEffect
    {
        private Config.CollisionGroupEffect config;
        
        public void Init(int id)
        {
            config = ConfigManager.Instance.GetById<Config.CollisionGroupEffect>(id);
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
        ///包含的技能效果id列表(表示这些效果碰撞到的实体只有第一次是正常效果)
        /// </summary>
        public List<int> SkillEffectIds => config.SkillEffectIds;
        
        /// <summary>
        ///影响效果类型
        /// </summary>
        public int AffectType => config.AffectType;
        
        /// <summary>
        ///影响效果参数
        /// </summary>
        public int AffectParam => config.AffectParam;
        
        /// <summary>
        ///效果资源id
        /// </summary>
        public int EffectResId => config.EffectResId;
        
    } 
}