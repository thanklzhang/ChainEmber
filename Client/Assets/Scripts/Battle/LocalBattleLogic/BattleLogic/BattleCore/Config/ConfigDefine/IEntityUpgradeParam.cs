/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
namespace Battle
{
    
    
 
    public interface IEntityUpgradeParam : IConfig
    {
        
        /// <summary>
        ///英雄每次升星所需要的升星经验
        /// </summary>
        List<int> UpgradeExpPerStarLevel {get;} 
        
        /// <summary>
        ///英雄每个星级对应的分解星级经验
        /// </summary>
        List<int> DecomposeExpPerStarLevel {get;} 
        
    } 
}