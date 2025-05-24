/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
using Config;
namespace Battle
{
    
    
 
    public class BattleAttributeReward_Impl : IBattleAttributeReward
    {
        private Config.BattleAttributeReward config;
        
        public void Init(int id)
        {
            config = ConfigManager.Instance.GetById<Config.BattleAttributeReward>(id);
        }
        
        public int Id => config.Id;
        
        /// <summary>
        ///技能名称
        /// </summary>
        public string Name => config.Name;
        
        /// <summary>
        ///buff介绍
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
        ///是否是持续性改变类型的属性
        /// </summary>
        public List<int> IsAddedAttrGroupContinuous => config.IsAddedAttrGroupContinuous;
        
    } 
}