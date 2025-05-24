/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
namespace Config
{
    
    
    
       
    public class CollisionGroupEffect : BaseConfig
    {
        
        /// <summary>
        ///技能名称
        /// </summary>
        private string name; 
        
        /// <summary>
        ///技能介绍
        /// </summary>
        private string describe; 
        
        /// <summary>
        ///包含的技能效果id列表(表示这些效果碰撞到的实体只有第一次是正常效果)
        /// </summary>
        private List<int> skillEffectIds; 
        
        /// <summary>
        ///影响效果类型
        /// </summary>
        private int affectType; 
        
        /// <summary>
        ///影响效果参数
        /// </summary>
        private int affectParam; 
        
        /// <summary>
        ///效果资源id
        /// </summary>
        private int effectResId; 
        

        
        public string Name { get => name; }     
        
        public string Describe { get => describe; }     
        
        public List<int> SkillEffectIds { get => skillEffectIds; }     
        
        public int AffectType { get => affectType; }     
        
        public int AffectParam { get => affectParam; }     
        
        public int EffectResId { get => effectResId; }     
        

    } 
}