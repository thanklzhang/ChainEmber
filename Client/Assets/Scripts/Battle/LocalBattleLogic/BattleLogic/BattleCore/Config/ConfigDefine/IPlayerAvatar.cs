/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
namespace Battle
{
    
    
 
    public interface IPlayerAvatar : IConfig
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
        ///资源id
        /// </summary>
        int ResId {get;} 
        
    } 
}