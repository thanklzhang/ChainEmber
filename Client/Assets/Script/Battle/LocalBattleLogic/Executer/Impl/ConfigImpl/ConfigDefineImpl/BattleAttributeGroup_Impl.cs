/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
using Config;
namespace Battle
{
    
    
 
    public class BattleAttributeGroup_Impl : IBattleAttributeGroup
    {
        private Config.BattleAttributeGroup config;
        
        public void Init(int id)
        {
            config = ConfigManager.Instance.GetById<Config.BattleAttributeGroup>(id);
        }
        
        public int Id => config.Id;
        
        /// <summary>
        ///属性加成名称
        /// </summary>
        public string Name => config.Name;
        
        /// <summary>
        ///介绍
        /// </summary>
        public string Describe => config.Describe;
        
        /// <summary>
        ///增加的属性组(,分割)
        /// </summary>
        public List<int> AddedAttrGroup => config.AddedAttrGroup;
        
        /// <summary>
        ///增加属性组数值(,|分割 目前只做一个属性之只受一种属性增加)
        /// </summary>
        public List<List<int>> AddedValueGroup => config.AddedValueGroup;
        
        /// <summary>
        ///随机值（,|分割 ,如果配了值，那么就代表是随机， 格式：最小值,最大值）
        /// </summary>
        public List<List<int>> AddedValueRand => config.AddedValueRand;
        
        /// <summary>
        ///是否是持续性改变类型的属性
        /// </summary>
        public List<int> IsAddedAttrGroupContinuous => config.IsAddedAttrGroupContinuous;
        
    } 
}