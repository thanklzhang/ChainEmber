/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
using Config;
namespace Battle
{
    
    
 
    public class BattleCommonParam_Impl : IBattleCommonParam
    {
        private Config.BattleCommonParam config;
        
        public void Init(int id)
        {
            config = ConfigManager.Instance.GetById<Config.BattleCommonParam>(id);
        }
        
        public int Id => config.Id;
        
        /// <summary>
        ///初始金币
        /// </summary>
        public int InitCoin => config.InitCoin;
        
        /// <summary>
        ///初始人口
        /// </summary>
        public int InitPopulation => config.InitPopulation;
        
        /// <summary>
        ///初始复活币
        /// </summary>
        public int InitReviveCoin => config.InitReviveCoin;
        
        /// <summary>
        ///实体初始解锁实体装备栏数目
        /// </summary>
        public int InitEntityItemBarCellUnlockCount => config.InitEntityItemBarCellUnlockCount;
        
        /// <summary>
        ///实体的装备栏最大格子数量
        /// </summary>
        public int MaxEntityItemBarCellCount => config.MaxEntityItemBarCellCount;
        
        /// <summary>
        ///实体装备栏解锁星级
        /// </summary>
        public List<int> EntityItemBarCellUnlockStarLevel => config.EntityItemBarCellUnlockStarLevel;
        
        /// <summary>
        ///玩家仓库初始解锁道具栏数目
        /// </summary>
        public int InitPlayerWarhouseCellUnlockCount => config.InitPlayerWarhouseCellUnlockCount;
        
        /// <summary>
        ///实体的仓库道具栏最大格子数量
        /// </summary>
        public int MaxPlayerWarhouseCellCount => config.MaxPlayerWarhouseCellCount;
        
        /// <summary>
        ///购买人口消耗的金币（索引代表购买之前的已购买次数）
        /// </summary>
        public List<int> BuyPopulationCostCoin => config.BuyPopulationCostCoin;
        
    } 
}