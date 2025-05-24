/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
namespace Battle
{
    
    
 
    public interface IBattleBox : IConfig
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
        ///图标显示资源id
        /// </summary>
        int IconResId {get;} 
        
        /// <summary>
        ///宝箱品质（0-4对应绿色-红色）
        /// </summary>
        int Quality {get;} 
        
        /// <summary>
        ///奖励选项数
        /// </summary>
        int SelectionCount {get;} 
        
        /// <summary>
        ///奖励池
        /// </summary>
        int PoolId {get;} 
        
    } 
}