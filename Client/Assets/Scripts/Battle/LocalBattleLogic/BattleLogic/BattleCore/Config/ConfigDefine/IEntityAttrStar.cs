/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
namespace Battle
{
    
    
 
    public interface IEntityAttrStar : IConfig
    {
        
        /// <summary>
        ///作为模板 id
        /// </summary>
        int TemplateId {get;} 
        
        /// <summary>
        ///星级
        /// </summary>
        int StarLevel {get;} 
        
        /// <summary>
        ///介绍
        /// </summary>
        string Describe {get;} 
        
        /// <summary>
        ///攻击力
        /// </summary>
        int Attack {get;} 
        
        /// <summary>
        ///防御值
        /// </summary>
        int Defence {get;} 
        
        /// <summary>
        ///生命值
        /// </summary>
        int Health {get;} 
        
        /// <summary>
        ///攻击速度(*1000) 1 秒中攻击次数
        /// </summary>
        int AttackSpeed {get;} 
        
        /// <summary>
        ///移动速度(*1000)
        /// </summary>
        int MoveSpeed {get;} 
        
        /// <summary>
        ///攻击距离(*1000)
        /// </summary>
        int AttackRange {get;} 
        
        /// <summary>
        ///承受伤害千分比
        /// </summary>
        int InputDamageRate {get;} 
        
        /// <summary>
        ///输出伤害千分比
        /// </summary>
        int OutputDamageRate {get;} 
        
        /// <summary>
        ///暴击几率（千分比）
        /// </summary>
        int CritRate {get;} 
        
        /// <summary>
        ///暴击伤害（千分比）
        /// </summary>
        int CritDamage {get;} 
        
        /// <summary>
        ///技能冷却
        /// </summary>
        int SkillCD {get;} 
        
        /// <summary>
        ///治疗比率
        /// </summary>
        int TreatmentRate {get;} 
        
        /// <summary>
        ///生命恢复速度（*1000）每秒
        /// </summary>
        int HealthRecoverSpeed {get;} 
        
        /// <summary>
        ///魔法值
        /// </summary>
        int Magic {get;} 
        
    } 
}