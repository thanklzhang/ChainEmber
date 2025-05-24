/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
namespace Battle
{
    
    
 
    public interface IBattleTrigger : IConfig
    {
        
        /// <summary>
        ///触发器名称
        /// </summary>
        string Name {get;} 
        
        /// <summary>
        ///脚本文件目录(相对于打包根目录 文件夹) (server则是相对于 资源中 Resource 文件夹)
        /// </summary>
        string ScriptPath {get;} 
        
    } 
}