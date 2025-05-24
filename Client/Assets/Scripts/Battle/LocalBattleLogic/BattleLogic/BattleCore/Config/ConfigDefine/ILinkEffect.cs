/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
namespace Battle
{
    
    
 
    public interface ILinkEffect : IConfig
    {
        
        /// <summary>
        ///技能名称
        /// </summary>
        string Name {get;} 
        
        /// <summary>
        ///介绍
        /// </summary>
        string Describe {get;} 
        
        /// <summary>
        ///链接资源id
        /// </summary>
        int LinkResId {get;} 
        
    } 
}