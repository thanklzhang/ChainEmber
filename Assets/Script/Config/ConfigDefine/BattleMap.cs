/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
namespace Config
{
    
    
    
       
    public class BattleMap : BaseConfig
    {
        
        /// <summary>
        ///地图名称
        /// </summary>
        private string name; 
        
        /// <summary>
        ///资源 id
        /// </summary>
        private int resId; 
        
        /// <summary>
        ///地图文件路径(相对于打包根目录 文件夹 (server则是相对于 资源中 Resource 文件夹))
        /// </summary>
        private string mapDataPath; 
        

        
        public string Name { get => name; }     
        
        public int ResId { get => resId; }     
        
        public string MapDataPath { get => mapDataPath; }     
        

    } 
}