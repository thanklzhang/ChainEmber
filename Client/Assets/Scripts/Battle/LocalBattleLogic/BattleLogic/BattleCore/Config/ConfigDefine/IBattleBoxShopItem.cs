/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
namespace Battle
{
    
    
 
    public interface IBattleBoxShopItem : IConfig
    {
        
        /// <summary>
        ///名称
        /// </summary>
        string Name {get;} 
        
        /// <summary>
        ///介绍
        /// </summary>
        string Describe {get;} 
        
        /// <summary>
        ///图标资源id
        /// </summary>
        int IconResId {get;} 
        
        /// <summary>
        ///宝箱组品质（绿 - 红）
        /// </summary>
        int Quality {get;} 
        
        /// <summary>
        ///花费道具id
        /// </summary>
        int CostItemId {get;} 
        
        /// <summary>
        ///花费数量
        /// </summary>
        int CostCount {get;} 
        
        /// <summary>
        ///刷出 宝箱 id 列表
        /// </summary>
        List<int> BoxIdList {get;} 
        
        /// <summary>
        ///刷出 宝箱 权重 列表
        /// </summary>
        List<int> BoxWeightList {get;} 
        
        /// <summary>
        ///保底刷出数量
        /// </summary>
        int MinCount {get;} 
        
        /// <summary>
        ///刷出数量上限
        /// </summary>
        int MaxCount {get;} 
        
        /// <summary>
        ///刷出概率（不算保底）千分比
        /// </summary>
        int Chance {get;} 
        
    } 
}