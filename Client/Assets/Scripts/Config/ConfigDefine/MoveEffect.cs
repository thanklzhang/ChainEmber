/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
namespace Config
{
    
    
    
       
    public class MoveEffect : BaseConfig
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
        ///移动速度(*1000)
        /// </summary>
        private int moveSpeed; 
        
        /// <summary>
        ///移动目标类型
        /// </summary>
        private int moveTargetPosType; 
        
        /// <summary>
        ///移动过程类型
        /// </summary>
        private int moveProcessType; 
        
        /// <summary>
        ///移动终点类型
        /// </summary>
        private int endPosType; 
        
        /// <summary>
        ///持续时间(无目标点类型适用)(*1000)
        /// </summary>
        private int lastTime; 
        
        /// <summary>
        ///开始的时候触发的效果列表
        /// </summary>
        private List<int> startEffectList; 
        
        /// <summary>
        ///到达的时候触发的效果列表（如果被打断则不会触发）
        /// </summary>
        private List<int> reachEffectList; 
        
        /// <summary>
        ///结束的时候移除的效果列表(无论被打断还是不打断都会触发)
        /// </summary>
        private List<int> endRemoveEffectList; 
        
        /// <summary>
        ///此技能效果结束时是否判定为释放者技能结束
        /// </summary>
        private int isThisEndForSkillEnd; 
        
        /// <summary>
        ///效果资源id
        /// </summary>
        private int effectResId; 
        

        
        public string Name { get => name; }     
        
        public string Describe { get => describe; }     
        
        public int MoveSpeed { get => moveSpeed; }     
        
        public int MoveTargetPosType { get => moveTargetPosType; }     
        
        public int MoveProcessType { get => moveProcessType; }     
        
        public int EndPosType { get => endPosType; }     
        
        public int LastTime { get => lastTime; }     
        
        public List<int> StartEffectList { get => startEffectList; }     
        
        public List<int> ReachEffectList { get => reachEffectList; }     
        
        public List<int> EndRemoveEffectList { get => endRemoveEffectList; }     
        
        public int IsThisEndForSkillEnd { get => isThisEndForSkillEnd; }     
        
        public int EffectResId { get => effectResId; }     
        

    } 
}