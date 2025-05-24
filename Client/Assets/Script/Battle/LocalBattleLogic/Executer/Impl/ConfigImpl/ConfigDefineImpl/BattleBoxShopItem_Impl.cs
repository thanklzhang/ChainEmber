/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
using Config;
namespace Battle
{
    
    
 
    public class BattleBoxShopItem_Impl : IBattleBoxShopItem
    {
        private Config.BattleBoxShopItem config;
        
        public void Init(int id)
        {
            config = ConfigManager.Instance.GetById<Config.BattleBoxShopItem>(id);
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
        ///图标资源id
        /// </summary>
        public int IconResId => config.IconResId;
        
        /// <summary>
        ///宝箱组品质（绿 - 红）
        /// </summary>
        public int Quality => config.Quality;
        
        /// <summary>
        ///花费道具id
        /// </summary>
        public int CostItemId => config.CostItemId;
        
        /// <summary>
        ///花费数量
        /// </summary>
        public int CostCount => config.CostCount;
        
        /// <summary>
        ///刷出 宝箱 id 列表
        /// </summary>
        public List<int> BoxIdList => config.BoxIdList;
        
        /// <summary>
        ///刷出 宝箱 权重 列表
        /// </summary>
        public List<int> BoxWeightList => config.BoxWeightList;
        
        /// <summary>
        ///保底刷出数量
        /// </summary>
        public int MinCount => config.MinCount;
        
        /// <summary>
        ///刷出数量上限
        /// </summary>
        public int MaxCount => config.MaxCount;
        
        /// <summary>
        ///刷出概率（不算保底）千分比
        /// </summary>
        public int Chance => config.Chance;
        
    } 
}