/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
namespace Config
{
    
    
    
       
    public class BattleCommonParam : BaseConfig
    {
        
        /// <summary>
        ///初始金币
        /// </summary>
        private int initCoin; 
        
        /// <summary>
        ///初始人口
        /// </summary>
        private int initPopulation; 
        
        /// <summary>
        ///初始复活币
        /// </summary>
        private int initReviveCoin; 
        
        /// <summary>
        ///实体初始解锁实体装备栏数目
        /// </summary>
        private int initEntityItemBarCellUnlockCount; 
        
        /// <summary>
        ///实体的装备栏最大格子数量
        /// </summary>
        private int maxEntityItemBarCellCount; 
        
        /// <summary>
        ///实体装备栏解锁星级
        /// </summary>
        private List<int> entityItemBarCellUnlockStarLevel; 
        
        /// <summary>
        ///玩家仓库初始解锁道具栏数目
        /// </summary>
        private int initPlayerWarhouseCellUnlockCount; 
        
        /// <summary>
        ///实体的仓库道具栏最大格子数量
        /// </summary>
        private int maxPlayerWarhouseCellCount; 
        
        /// <summary>
        ///购买人口消耗的金币（索引代表购买之前的已购买次数）
        /// </summary>
        private List<int> buyPopulationCostCoin; 
        

        
        public int InitCoin { get => initCoin; }     
        
        public int InitPopulation { get => initPopulation; }     
        
        public int InitReviveCoin { get => initReviveCoin; }     
        
        public int InitEntityItemBarCellUnlockCount { get => initEntityItemBarCellUnlockCount; }     
        
        public int MaxEntityItemBarCellCount { get => maxEntityItemBarCellCount; }     
        
        public List<int> EntityItemBarCellUnlockStarLevel { get => entityItemBarCellUnlockStarLevel; }     
        
        public int InitPlayerWarhouseCellUnlockCount { get => initPlayerWarhouseCellUnlockCount; }     
        
        public int MaxPlayerWarhouseCellCount { get => maxPlayerWarhouseCellCount; }     
        
        public List<int> BuyPopulationCostCoin { get => buyPopulationCostCoin; }     
        

    } 
}