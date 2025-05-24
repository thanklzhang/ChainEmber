/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
using Config;
namespace Battle
{
    
    
 
    public class MoveEffect_Impl : IMoveEffect
    {
        private Config.MoveEffect config;
        
        public void Init(int id)
        {
            config = ConfigManager.Instance.GetById<Config.MoveEffect>(id);
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
        ///移动速度(*1000)
        /// </summary>
        public int MoveSpeed => config.MoveSpeed;
        
        /// <summary>
        ///移动目标类型
        /// </summary>
        public int MoveTargetPosType => config.MoveTargetPosType;
        
        /// <summary>
        ///移动过程类型
        /// </summary>
        public int MoveProcessType => config.MoveProcessType;
        
        /// <summary>
        ///移动终点类型
        /// </summary>
        public int EndPosType => config.EndPosType;
        
        /// <summary>
        ///持续时间(无目标点类型适用)(*1000)
        /// </summary>
        public int LastTime => config.LastTime;
        
        /// <summary>
        ///开始的时候触发的效果列表
        /// </summary>
        public List<int> StartEffectList => config.StartEffectList;
        
        /// <summary>
        ///到达的时候触发的效果列表（如果被打断则不会触发）
        /// </summary>
        public List<int> ReachEffectList => config.ReachEffectList;
        
        /// <summary>
        ///结束的时候移除的效果列表(无论被打断还是不打断都会触发)
        /// </summary>
        public List<int> EndRemoveEffectList => config.EndRemoveEffectList;
        
        /// <summary>
        ///此技能效果结束时是否判定为释放者技能结束
        /// </summary>
        public int IsThisEndForSkillEnd => config.IsThisEndForSkillEnd;
        
        /// <summary>
        ///效果资源id
        /// </summary>
        public int EffectResId => config.EffectResId;
        
    } 
}