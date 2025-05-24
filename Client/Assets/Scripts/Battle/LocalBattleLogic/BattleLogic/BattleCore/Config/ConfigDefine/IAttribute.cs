/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
namespace Battle
{
    
    
 
    public interface IAttribute : IConfig
    {
        
        /// <summary>
        ///属性类型
        /// </summary>
        int Type {get;} 
        
        /// <summary>
        ///名称
        /// </summary>
        string Name {get;} 
        
        /// <summary>
        ///描述
        /// </summary>
        string Describe {get;} 
        
        /// <summary>
        ///图标资源id
        /// </summary>
        int IconResId {get;} 
        
    } 
}