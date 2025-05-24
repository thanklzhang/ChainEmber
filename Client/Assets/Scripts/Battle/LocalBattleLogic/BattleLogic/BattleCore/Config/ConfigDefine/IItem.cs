/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
namespace Battle
{
    
    
 
    public interface IItem : IConfig
    {
        
        /// <summary>
        ///道具名称
        /// </summary>
        string Name {get;} 
        
        /// <summary>
        ///道具介绍
        /// </summary>
        string Describe {get;} 
        
        /// <summary>
        ///图标显示资源id
        /// </summary>
        int IconResId {get;} 
        
        /// <summary>
        ///是否可叠加
        /// </summary>
        int IsCanOverlying {get;} 
        
    } 
}