/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
using Config;
namespace Battle
{
    
    
 
    public class CalculateEffect_Impl : ICalculateEffect
    {
        private Config.CalculateEffect config;
        
        public void Init(int id)
        {
            config = ConfigManager.Instance.GetById<Config.CalculateEffect>(id);
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
        ///计算目标类型
        /// </summary>
        public int CalculateEffectTargetType => config.CalculateEffectTargetType;
        
        /// <summary>
        ///附加伤害组（, | ）
        /// </summary>
        public List<List<int>> AddedValueGroup => config.AddedValueGroup;
        
        /// <summary>
        ///伤害附加的倍数计算效果列表（适用比值）
        /// </summary>
        public List<int> AddedValueScaleEffectIds => config.AddedValueScaleEffectIds;
        
        /// <summary>
        ///最终的效果类型 1 物理伤害 2 法强伤害 
        /// </summary>
        public int FinalEffectType => config.FinalEffectType;
        
        /// <summary>
        ///伤害时候的效果ids
        /// </summary>
        public List<int> AfterDamageEffectIds => config.AfterDamageEffectIds;
        
        /// <summary>
        ///效果资源id
        /// </summary>
        public int EffectResId => config.EffectResId;
        
        /// <summary>
        ///产生效果点位置类型
        /// </summary>
        public int EffectPosType => config.EffectPosType;
        
        /// <summary>
        ///特效是否跟随目标
        /// </summary>
        public int IsEffectFollowTarget => config.IsEffectFollowTarget;
        
        /// <summary>
        ///效果点节点名称
        /// </summary>
        public string EffectPosName => config.EffectPosName;
        
        /// <summary>
        ///是否为了显示（只同步效果，实际效果等均不触发）
        /// </summary>
        public int IsForShow => config.IsForShow;
        
    } 
}