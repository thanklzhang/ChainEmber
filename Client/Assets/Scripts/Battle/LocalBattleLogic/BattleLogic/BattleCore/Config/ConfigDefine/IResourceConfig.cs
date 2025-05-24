/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
namespace Battle
{
    
    
 
    public interface IResourceConfig : IConfig
    {
        
        /// <summary>
        ///资源名称
        /// </summary>
        string Name {get;} 
        
        /// <summary>
        ///备注
        /// </summary>
        string Des {get;} 
        
        /// <summary>
        ///资源路径
        /// </summary>
        string Path {get;} 
        
        /// <summary>
        ///后缀
        /// </summary>
        string Ext {get;} 
        
        /// <summary>
        ///资源类型
        /// </summary>
        int Type {get;} 
        
        /// <summary>
        ///资源标签
        /// </summary>
        string Tag {get;} 
        
    } 
}