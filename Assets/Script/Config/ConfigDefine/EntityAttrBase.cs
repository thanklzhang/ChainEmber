/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
namespace Config
{
    
    
    
       
    public class EntityAttrBase : BaseConfig
    {
        
        /// <summary>
        ///介绍
        /// </summary>
        private string describe; 
        
        /// <summary>
        ///攻击力
        /// </summary>
        private int attack; 
        
        /// <summary>
        ///防御值
        /// </summary>
        private int defence; 
        
        /// <summary>
        ///生命值
        /// </summary>
        private int health; 
        
        /// <summary>
        ///魔法值
        /// </summary>
        private int magic; 
        
        /// <summary>
        ///攻击速度(*1000) 1 秒中攻击次数
        /// </summary>
        private int attackSpeed; 
        
        /// <summary>
        ///移动速度(*1000)
        /// </summary>
        private int moveSpeed; 
        
        /// <summary>
        ///攻击距离(*1000)
        /// </summary>
        private int attackRange; 
        
        /// <summary>
        ///承受伤害千分比
        /// </summary>
        private int inputDamageRate; 
        
        /// <summary>
        ///输出伤害千分比
        /// </summary>
        private int outputDamageRate; 
        
        /// <summary>
        ///暴击几率（千分比）
        /// </summary>
        private int critRate; 
        
        /// <summary>
        ///暴击伤害（千分比）
        /// </summary>
        private int critDamage; 
        
        /// <summary>
        ///技能冷却
        /// </summary>
        private int skillCD; 
        
        /// <summary>
        ///治疗比率
        /// </summary>
        private int treatmentRate; 
        
        /// <summary>
        ///生命恢复速度（*1000）每秒
        /// </summary>
        private int healthRecoverSpeed; 
        

        
        public string Describe { get => describe; }     
        
        public int Attack { get => attack; }     
        
        public int Defence { get => defence; }     
        
        public int Health { get => health; }     
        
        public int Magic { get => magic; }     
        
        public int AttackSpeed { get => attackSpeed; }     
        
        public int MoveSpeed { get => moveSpeed; }     
        
        public int AttackRange { get => attackRange; }     
        
        public int InputDamageRate { get => inputDamageRate; }     
        
        public int OutputDamageRate { get => outputDamageRate; }     
        
        public int CritRate { get => critRate; }     
        
        public int CritDamage { get => critDamage; }     
        
        public int SkillCD { get => skillCD; }     
        
        public int TreatmentRate { get => treatmentRate; }     
        
        public int HealthRecoverSpeed { get => healthRecoverSpeed; }     
        

    } 
}