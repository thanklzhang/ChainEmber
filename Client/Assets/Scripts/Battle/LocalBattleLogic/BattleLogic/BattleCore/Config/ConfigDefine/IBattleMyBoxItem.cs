/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
namespace Battle
{
    
    
 
    public interface IBattleMyBoxItem : IConfig
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
        
    } 
}