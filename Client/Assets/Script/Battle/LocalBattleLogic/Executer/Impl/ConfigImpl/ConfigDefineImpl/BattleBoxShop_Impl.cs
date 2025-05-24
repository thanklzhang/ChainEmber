/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
using Config;
namespace Battle
{
    
    
 
    public class BattleBoxShop_Impl : IBattleBoxShop
    {
        private Config.BattleBoxShop config;
        
        public void Init(int id)
        {
            config = ConfigManager.Instance.GetById<Config.BattleBoxShop>(id);
        }
        
        public int Id => config.Id;
        
        /// <summary>
        ///名称
        /// </summary>
        public string Name => config.Name;
        
        /// <summary>
        ///介绍
        /// </summary>
        public string Describe => config.Describe;
        
        /// <summary>
        ///商店宝箱购买项列表
        /// </summary>
        public List<int> BoxShopItemList => config.BoxShopItemList;
        
    } 
}