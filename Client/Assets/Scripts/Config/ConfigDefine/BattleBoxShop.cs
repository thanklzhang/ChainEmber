/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
namespace Config
{
    
    
    
       
    public class BattleBoxShop : BaseConfig
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
        ///商店宝箱购买项列表
        /// </summary>
        private List<int> boxShopItemList; 
        

        
        public string Name { get => name; }     
        
        public string Describe { get => describe; }     
        
        public List<int> BoxShopItemList { get => boxShopItemList; }     
        

    } 
}