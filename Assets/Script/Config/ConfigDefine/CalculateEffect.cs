/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
namespace Config
{
    
    
    
       
    public class CalculateEffect : BaseConfig
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
        ///计算目标类型
        /// </summary>
        private int calculateEffectTargetType; 
        
        /// <summary>
        ///附加伤害组（, | ）
        /// </summary>
        private List<List<int>> addedValueGroup; 
        
        /// <summary>
        ///伤害附加的倍数计算效果列表（适用比值）
        /// </summary>
        private List<int> addedValueScaleEffectIds; 
        
        /// <summary>
        ///最终的效果类型 1 物理伤害 2 法强伤害 
        /// </summary>
        private int finalEffectType; 
        
        /// <summary>
        ///伤害时候的效果ids
        /// </summary>
        private List<int> afterDamageEffectIds; 
        
        /// <summary>
        ///效果资源id
        /// </summary>
        private int effectResId; 
        
        /// <summary>
        ///产生效果点位置类型
        /// </summary>
        private int effectPosType; 
        
        /// <summary>
        ///特效是否跟随目标
        /// </summary>
        private int isEffectFollowTarget; 
        
        /// <summary>
        ///效果点节点名称
        /// </summary>
        private string effectPosName; 
        
        /// <summary>
        ///是否为了显示（只同步效果，实际效果等均不触发）
        /// </summary>
        private int isForShow; 
        

        
        public string Name { get => name; }     
        
        public string Describe { get => describe; }     
        
        public int CalculateEffectTargetType { get => calculateEffectTargetType; }     
        
        public List<List<int>> AddedValueGroup { get => addedValueGroup; }     
        
        public List<int> AddedValueScaleEffectIds { get => addedValueScaleEffectIds; }     
        
        public int FinalEffectType { get => finalEffectType; }     
        
        public List<int> AfterDamageEffectIds { get => afterDamageEffectIds; }     
        
        public int EffectResId { get => effectResId; }     
        
        public int EffectPosType { get => effectPosType; }     
        
        public int IsEffectFollowTarget { get => isEffectFollowTarget; }     
        
        public string EffectPosName { get => effectPosName; }     
        
        public int IsForShow { get => isForShow; }     
        

    } 
}