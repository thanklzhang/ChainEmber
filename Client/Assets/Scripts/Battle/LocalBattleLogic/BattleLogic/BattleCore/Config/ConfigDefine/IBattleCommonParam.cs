/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
namespace Battle
{
    
    
 
    public interface IBattleCommonParam : IConfig
    {
        
        /// <summary>
        ///初始金币
        /// </summary>
        int InitCoin {get;} 
        
        /// <summary>
        ///初始人口
        /// </summary>
        int InitPopulation {get;} 
        
        /// <summary>
        ///初始复活币
        /// </summary>
        int InitReviveCoin {get;} 
        
        /// <summary>
        ///实体初始解锁实体装备栏数目
        /// </summary>
        int InitEntityItemBarCellUnlockCount {get;} 
        
        /// <summary>
        ///实体的装备栏最大格子数量
        /// </summary>
        int MaxEntityItemBarCellCount {get;} 
        
        /// <summary>
        ///实体装备栏解锁星级
        /// </summary>
        List<int> EntityItemBarCellUnlockStarLevel {get;} 
        
        /// <summary>
        ///玩家仓库初始解锁道具栏数目
        /// </summary>
        int InitPlayerWarhouseCellUnlockCount {get;} 
        
        /// <summary>
        ///实体的仓库道具栏最大格子数量
        /// </summary>
        int MaxPlayerWarhouseCellCount {get;} 
        
        /// <summary>
        ///购买人口消耗的金币（索引代表购买之前的已购买次数）
        /// </summary>
        List<int> BuyPopulationCostCoin {get;} 
        
    } 
}