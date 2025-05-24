/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
namespace Config
{
    
    
    
       
    public class BattleBoxShopItem : BaseConfig
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
        ///图标资源id
        /// </summary>
        private int iconResId; 
        
        /// <summary>
        ///宝箱组品质（绿 - 红）
        /// </summary>
        private int quality; 
        
        /// <summary>
        ///花费道具id
        /// </summary>
        private int costItemId; 
        
        /// <summary>
        ///花费数量
        /// </summary>
        private int costCount; 
        
        /// <summary>
        ///刷出 宝箱 id 列表
        /// </summary>
        private List<int> boxIdList; 
        
        /// <summary>
        ///刷出 宝箱 权重 列表
        /// </summary>
        private List<int> boxWeightList; 
        
        /// <summary>
        ///保底刷出数量
        /// </summary>
        private int minCount; 
        
        /// <summary>
        ///刷出数量上限
        /// </summary>
        private int maxCount; 
        
        /// <summary>
        ///刷出概率（不算保底）千分比
        /// </summary>
        private int chance; 
        

        
        public string Name { get => name; }     
        
        public string Describe { get => describe; }     
        
        public int IconResId { get => iconResId; }     
        
        public int Quality { get => quality; }     
        
        public int CostItemId { get => costItemId; }     
        
        public int CostCount { get => costCount; }     
        
        public List<int> BoxIdList { get => boxIdList; }     
        
        public List<int> BoxWeightList { get => boxWeightList; }     
        
        public int MinCount { get => minCount; }     
        
        public int MaxCount { get => maxCount; }     
        
        public int Chance { get => chance; }     
        

    } 
}