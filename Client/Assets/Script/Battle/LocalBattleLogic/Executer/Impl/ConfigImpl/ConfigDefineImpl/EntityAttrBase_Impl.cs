/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
using Config;
namespace Battle
{
    
    
 
    public class EntityAttrBase_Impl : IEntityAttrBase
    {
        private Config.EntityAttrBase config;
        
        public void Init(int id)
        {
            config = ConfigManager.Instance.GetById<Config.EntityAttrBase>(id);
        }
        
        public int Id => config.Id;
        
        /// <summary>
        ///介绍
        /// </summary>
        public string Describe => config.Describe;
        
        /// <summary>
        ///攻击力
        /// </summary>
        public int Attack => config.Attack;
        
        /// <summary>
        ///防御值
        /// </summary>
        public int Defence => config.Defence;
        
        /// <summary>
        ///生命值
        /// </summary>
        public int Health => config.Health;
        
        /// <summary>
        ///魔法值
        /// </summary>
        public int Magic => config.Magic;
        
        /// <summary>
        ///攻击速度(*1000) 1 秒中攻击次数
        /// </summary>
        public int AttackSpeed => config.AttackSpeed;
        
        /// <summary>
        ///移动速度(*1000)
        /// </summary>
        public int MoveSpeed => config.MoveSpeed;
        
        /// <summary>
        ///攻击距离(*1000)
        /// </summary>
        public int AttackRange => config.AttackRange;
        
        /// <summary>
        ///承受伤害千分比
        /// </summary>
        public int InputDamageRate => config.InputDamageRate;
        
        /// <summary>
        ///输出伤害千分比
        /// </summary>
        public int OutputDamageRate => config.OutputDamageRate;
        
        /// <summary>
        ///暴击几率（千分比）
        /// </summary>
        public int CritRate => config.CritRate;
        
        /// <summary>
        ///暴击伤害（千分比）
        /// </summary>
        public int CritDamage => config.CritDamage;
        
        /// <summary>
        ///技能冷却
        /// </summary>
        public int SkillCD => config.SkillCD;
        
        /// <summary>
        ///治疗比率
        /// </summary>
        public int TreatmentRate => config.TreatmentRate;
        
        /// <summary>
        ///生命恢复速度（*1000）每秒
        /// </summary>
        public int HealthRecoverSpeed => config.HealthRecoverSpeed;
        
    } 
}