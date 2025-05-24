/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
namespace Config
{
    
    
    
       
    public class BattleSkillItemDraw : BaseConfig
    {
        
        /// <summary>
        ///名称
        /// </summary>
        private string name; 
        
        /// <summary>
        ///介绍
        /// </summary>
        private string describe; 
        
        /// <summary>
        ///图片资源id
        /// </summary>
        private int iconResId; 
        
        /// <summary>
        ///消耗道具id（，分割）
        /// </summary>
        private List<int> costItemIds; 
        
        /// <summary>
        ///消耗道具数目（，分割）
        /// </summary>
        private List<int> costItemCounts; 
        
        /// <summary>
        ///抽取权重（，分割 蓝紫橙红）
        /// </summary>
        private List<int> drawWeights; 
        
        /// <summary>
        ///抽取值
        /// </summary>
        private List<int> drawValues; 
        
        /// <summary>
        ///蓝色卡池ids（不填则是所有蓝色卡池）
        /// </summary>
        private List<int> pool1ItemIds; 
        
        /// <summary>
        ///蓝色卡池权重（不填的话就是都相同）
        /// </summary>
        private List<int> pool1ItemWeights; 
        
        /// <summary>
        ///紫色卡池ids
        /// </summary>
        private List<int> pool2ItemIds; 
        
        /// <summary>
        ///紫色卡池权重
        /// </summary>
        private List<int> pool2ItemWeights; 
        
        /// <summary>
        ///橙色卡池ids
        /// </summary>
        private List<int> pool3ItemIds; 
        
        /// <summary>
        ///橙色卡池权重
        /// </summary>
        private List<int> pool3ItemWeights; 
        
        /// <summary>
        ///红色卡池ids
        /// </summary>
        private List<int> pool4ItemIds; 
        
        /// <summary>
        ///红色卡池权重
        /// </summary>
        private List<int> pool4ItemWeights; 
        

        
        public string Name { get => name; }     
        
        public string Describe { get => describe; }     
        
        public int IconResId { get => iconResId; }     
        
        public List<int> CostItemIds { get => costItemIds; }     
        
        public List<int> CostItemCounts { get => costItemCounts; }     
        
        public List<int> DrawWeights { get => drawWeights; }     
        
        public List<int> DrawValues { get => drawValues; }     
        
        public List<int> Pool1ItemIds { get => pool1ItemIds; }     
        
        public List<int> Pool1ItemWeights { get => pool1ItemWeights; }     
        
        public List<int> Pool2ItemIds { get => pool2ItemIds; }     
        
        public List<int> Pool2ItemWeights { get => pool2ItemWeights; }     
        
        public List<int> Pool3ItemIds { get => pool3ItemIds; }     
        
        public List<int> Pool3ItemWeights { get => pool3ItemWeights; }     
        
        public List<int> Pool4ItemIds { get => pool4ItemIds; }     
        
        public List<int> Pool4ItemWeights { get => pool4ItemWeights; }     
        

    } 
}