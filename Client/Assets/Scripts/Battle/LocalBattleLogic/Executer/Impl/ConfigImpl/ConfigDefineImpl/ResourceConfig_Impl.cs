/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
using Config;
namespace Battle
{
    
    
 
    public class ResourceConfig_Impl : IResourceConfig
    {
        private Config.ResourceConfig config;
        
        public void Init(int id)
        {
            config = ConfigManager.Instance.GetById<Config.ResourceConfig>(id);
        }
        
        public int Id => config.Id;
        
        /// <summary>
        ///资源名称
        /// </summary>
        public string Name => config.Name;
        
        /// <summary>
        ///备注
        /// </summary>
        public string Des => config.Des;
        
        /// <summary>
        ///资源路径
        /// </summary>
        public string Path => config.Path;
        
        /// <summary>
        ///后缀
        /// </summary>
        public string Ext => config.Ext;
        
        /// <summary>
        ///资源类型
        /// </summary>
        public int Type => config.Type;
        
        /// <summary>
        ///资源标签
        /// </summary>
        public string Tag => config.Tag;
        
    } 
}