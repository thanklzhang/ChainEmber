/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
namespace Config
{
    
    
    
       
    public class SummonEffect : BaseConfig
    {
        
        /// <summary>
        ///技能名称
        /// </summary>
        private string name; 
        
        /// <summary>
        ///介绍
        /// </summary>
        private string describe; 
        
        /// <summary>
        ///召唤物实体配置id
        /// </summary>
        private int summonConfigId; 
        
        /// <summary>
        ///召唤物出生点类型
        /// </summary>
        private int summonBornPosType; 
        
        /// <summary>
        ///召唤数量
        /// </summary>
        private int summonCount; 
        
        /// <summary>
        ///最大召唤数量
        /// </summary>
        private int maxSummonCount; 
        
        /// <summary>
        ///召唤物持续时间(ms)
        /// </summary>
        private int lastTime; 
        
        /// <summary>
        ///持续时间附加组(ms)
        /// </summary>
        private List<List<int>> lastTimeAddedGroup; 
        
        /// <summary>
        ///开始的时候在召唤物身上触发的效果列表(例如增加和召唤者相关属性的附加)
        /// </summary>
        private List<int> startEffectList; 
        
        /// <summary>
        ///召唤物增加的属性组(,分割)
        /// </summary>
        private List<int> addedAttrGroup; 
        
        /// <summary>
        ///增加属性组数值(,|分割 目前只做一个属性之只受一种属性增加)
        /// </summary>
        private List<List<int>> addedValueGroup; 
        
        /// <summary>
        ///效果资源id
        /// </summary>
        private int effectResId; 
        

        
        public string Name { get => name; }     
        
        public string Describe { get => describe; }     
        
        public int SummonConfigId { get => summonConfigId; }     
        
        public int SummonBornPosType { get => summonBornPosType; }     
        
        public int SummonCount { get => summonCount; }     
        
        public int MaxSummonCount { get => maxSummonCount; }     
        
        public int LastTime { get => lastTime; }     
        
        public List<List<int>> LastTimeAddedGroup { get => lastTimeAddedGroup; }     
        
        public List<int> StartEffectList { get => startEffectList; }     
        
        public List<int> AddedAttrGroup { get => addedAttrGroup; }     
        
        public List<List<int>> AddedValueGroup { get => addedValueGroup; }     
        
        public int EffectResId { get => effectResId; }     
        

    } 
}