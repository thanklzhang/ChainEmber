/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
namespace Battle
{
    
    
 
    public interface IBattleMap : IConfig
    {
        
        /// <summary>
        ///地图名称
        /// </summary>
        string Name {get;} 
        
        /// <summary>
        ///资源 id
        /// </summary>
        int ResId {get;} 
        
        /// <summary>
        ///地图文件路径(相对于打包根目录 文件夹 (server则是相对于 资源中 Resource 文件夹))
        /// </summary>
        string MapDataPath {get;} 
        
    } 
}