/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
using Config;
namespace Battle
{
    
    
 
    public class BattleTrigger_Impl : IBattleTrigger
    {
        private Config.BattleTrigger config;
        
        public void Init(int id)
        {
            config = ConfigManager.Instance.GetById<Config.BattleTrigger>(id);
        }
        
        public int Id => config.Id;
        
        /// <summary>
        ///触发器名称
        /// </summary>
        public string Name => config.Name;
        
        /// <summary>
        ///脚本文件目录(相对于打包根目录 文件夹) (server则是相对于 资源中 Resource 文件夹)
        /// </summary>
        public string ScriptPath => config.ScriptPath;
        
    } 
}