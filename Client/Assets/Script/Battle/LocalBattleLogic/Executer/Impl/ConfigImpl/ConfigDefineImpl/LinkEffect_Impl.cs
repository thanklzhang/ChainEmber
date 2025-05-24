/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
using Config;
namespace Battle
{
    
    
 
    public class LinkEffect_Impl : ILinkEffect
    {
        private Config.LinkEffect config;
        
        public void Init(int id)
        {
            config = ConfigManager.Instance.GetById<Config.LinkEffect>(id);
        }
        
        public int Id => config.Id;
        
        /// <summary>
        ///技能名称
        /// </summary>
        public string Name => config.Name;
        
        /// <summary>
        ///介绍
        /// </summary>
        public string Describe => config.Describe;
        
        /// <summary>
        ///链接资源id
        /// </summary>
        public int LinkResId => config.LinkResId;
        
    } 
}