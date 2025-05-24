/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
using Config;
namespace Battle
{
    
    
 
    public class SummonEffect_Impl : ISummonEffect
    {
        private Config.SummonEffect config;
        
        public void Init(int id)
        {
            config = ConfigManager.Instance.GetById<Config.SummonEffect>(id);
        }
        
        public int Id => config.Id;
        
        /// <summary>
        ///技能名称
        /// </summary>
        public string Name => config.Name;
        
        /// <summary>
        ///介绍
        /// </summary>
        public string Describe => config.Describe;
        
        /// <summary>
        ///召唤物实体配置id
        /// </summary>
        public int SummonConfigId => config.SummonConfigId;
        
        /// <summary>
        ///召唤物出生点类型
        /// </summary>
        public int SummonBornPosType => config.SummonBornPosType;
        
        /// <summary>
        ///召唤数量
        /// </summary>
        public int SummonCount => config.SummonCount;
        
        /// <summary>
        ///最大召唤数量
        /// </summary>
        public int MaxSummonCount => config.MaxSummonCount;
        
        /// <summary>
        ///召唤物持续时间(ms)
        /// </summary>
        public int LastTime => config.LastTime;
        
        /// <summary>
        ///持续时间附加组(ms)
        /// </summary>
        public List<List<int>> LastTimeAddedGroup => config.LastTimeAddedGroup;
        
        /// <summary>
        ///开始的时候在召唤物身上触发的效果列表(例如增加和召唤者相关属性的附加)
        /// </summary>
        public List<int> StartEffectList => config.StartEffectList;
        
        /// <summary>
        ///召唤物增加的属性组(,分割)
        /// </summary>
        public List<int> AddedAttrGroup => config.AddedAttrGroup;
        
        /// <summary>
        ///增加属性组数值(,|分割 目前只做一个属性之只受一种属性增加)
        /// </summary>
        public List<List<int>> AddedValueGroup => config.AddedValueGroup;
        
        /// <summary>
        ///效果资源id
        /// </summary>
        public int EffectResId => config.EffectResId;
        
    } 
}