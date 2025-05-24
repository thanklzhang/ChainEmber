/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
using Config;
namespace Battle
{
    
    
 
    public class Item_Impl : IItem
    {
        private Config.Item config;
        
        public void Init(int id)
        {
            config = ConfigManager.Instance.GetById<Config.Item>(id);
        }
        
        public int Id => config.Id;
        
        /// <summary>
        ///道具名称
        /// </summary>
        public string Name => config.Name;
        
        /// <summary>
        ///道具介绍
        /// </summary>
        public string Describe => config.Describe;
        
        /// <summary>
        ///图标显示资源id
        /// </summary>
        public int IconResId => config.IconResId;
        
        /// <summary>
        ///是否可叠加
        /// </summary>
        public int IsCanOverlying => config.IsCanOverlying;
        
    } 
}