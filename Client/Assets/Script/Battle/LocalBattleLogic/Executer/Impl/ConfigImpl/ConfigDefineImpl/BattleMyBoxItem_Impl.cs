/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
using Config;
namespace Battle
{
    
    
 
    public class BattleMyBoxItem_Impl : IBattleMyBoxItem
    {
        private Config.BattleMyBoxItem config;
        
        public void Init(int id)
        {
            config = ConfigManager.Instance.GetById<Config.BattleMyBoxItem>(id);
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
        ///图标资源id
        /// </summary>
        public int IconResId => config.IconResId;
        
        /// <summary>
        ///宝箱组品质（绿 - 红）
        /// </summary>
        public int Quality => config.Quality;
        
    } 
}