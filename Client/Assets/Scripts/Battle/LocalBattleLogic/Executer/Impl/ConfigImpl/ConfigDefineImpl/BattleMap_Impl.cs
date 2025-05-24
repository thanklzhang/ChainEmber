/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
using Config;
namespace Battle
{
    
    
 
    public class BattleMap_Impl : IBattleMap
    {
        private Config.BattleMap config;
        
        public void Init(int id)
        {
            config = ConfigManager.Instance.GetById<Config.BattleMap>(id);
        }
        
        public int Id => config.Id;
        
        /// <summary>
        ///地图名称
        /// </summary>
        public string Name => config.Name;
        
        /// <summary>
        ///资源 id
        /// </summary>
        public int ResId => config.ResId;
        
        /// <summary>
        ///地图文件路径(相对于打包根目录 文件夹 (server则是相对于 资源中 Resource 文件夹))
        /// </summary>
        public string MapDataPath => config.MapDataPath;
        
    } 
}