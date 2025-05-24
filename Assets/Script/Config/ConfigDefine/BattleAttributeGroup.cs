/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
namespace Config
{
    
    
    
       
    public class BattleAttributeGroup : BaseConfig
    {
        
        /// <summary>
        ///属性加成名称
        /// </summary>
        private string name; 
        
        /// <summary>
        ///介绍
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
        ///随机值（,|分割 ,如果配了值，那么就代表是随机， 格式：最小值,最大值）
        /// </summary>
        private List<List<int>> addedValueRand; 
        
        /// <summary>
        ///是否是持续性改变类型的属性
        /// </summary>
        private List<int> isAddedAttrGroupContinuous; 
        

        
        public string Name { get => name; }     
        
        public string Describe { get => describe; }     
        
        public List<int> AddedAttrGroup { get => addedAttrGroup; }     
        
        public List<List<int>> AddedValueGroup { get => addedValueGroup; }     
        
        public List<List<int>> AddedValueRand { get => addedValueRand; }     
        
        public List<int> IsAddedAttrGroupContinuous { get => isAddedAttrGroupContinuous; }     
        

    } 
}