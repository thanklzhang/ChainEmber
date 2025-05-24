/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
using Config;
namespace Battle
{
    
    
 
    public class Attribute_Impl : IAttribute
    {
        private Config.Attribute config;
        
        public void Init(int id)
        {
            config = ConfigManager.Instance.GetById<Config.Attribute>(id);
        }
        
        public int Id => config.Id;
        
        /// <summary>
        ///属性类型
        /// </summary>
        public int Type => config.Type;
        
        /// <summary>
        ///名称
        /// </summary>
        public string Name => config.Name;
        
        /// <summary>
        ///描述
        /// </summary>
        public string Describe => config.Describe;
        
        /// <summary>
        ///图标资源id
        /// </summary>
        public int IconResId => config.IconResId;
        
    } 
}