/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
namespace Config
{
    
    
    
       
    public class BattleAttributeReward : BaseConfig
    {
        
        /// <summary>
        ///技能名称
        /// </summary>
        private string name; 
        
        /// <summary>
        ///buff介绍
        /// </summary>
        private string describe; 
        
        /// <summary>
        ///增加的属性组(,分割)
        /// </summary>
        private List<int> addedAttrGroup; 
        
        /// <summary>
        ///增加属性组数值(,|分割 目前只做一个属性之只受一种属性增加)
        /// </summary>
        private List<List<int>> addedValueGroup; 
        
        /// <summary>
        ///是否是持续性改变类型的属性
        /// </summary>
        private List<int> isAddedAttrGroupContinuous; 
        

        
        public string Name { get => name; }     
        
        public string Describe { get => describe; }     
        
        public List<int> AddedAttrGroup { get => addedAttrGroup; }     
        
        public List<List<int>> AddedValueGroup { get => addedValueGroup; }     
        
        public List<int> IsAddedAttrGroupContinuous { get => isAddedAttrGroupContinuous; }     
        

    } 
}