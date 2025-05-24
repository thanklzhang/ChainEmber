/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
using Config;
namespace Battle
{
    
    
 
    public class PlayerAvatar_Impl : IPlayerAvatar
    {
        private Config.PlayerAvatar config;
        
        public void Init(int id)
        {
            config = ConfigManager.Instance.GetById<Config.PlayerAvatar>(id);
        }
        
        public int Id => config.Id;
        
        /// <summary>
        ///名称
        /// </summary>
        public string Name => config.Name;
        
        /// <summary>
        ///介绍
        /// </summary>
        public string Describe => config.Describe;
        
        /// <summary>
        ///资源id
        /// </summary>
        public int ResId => config.ResId;
        
    } 
}