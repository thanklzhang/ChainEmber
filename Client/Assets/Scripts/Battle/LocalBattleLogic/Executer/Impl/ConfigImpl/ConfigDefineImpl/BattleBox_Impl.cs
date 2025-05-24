/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
using Config;
namespace Battle
{
    
    
 
    public class BattleBox_Impl : IBattleBox
    {
        private Config.BattleBox config;
        
        public void Init(int id)
        {
            config = ConfigManager.Instance.GetById<Config.BattleBox>(id);
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
        ///图标显示资源id
        /// </summary>
        public int IconResId => config.IconResId;
        
        /// <summary>
        ///宝箱品质（0-4对应绿色-红色）
        /// </summary>
        public int Quality => config.Quality;
        
        /// <summary>
        ///奖励选项数
        /// </summary>
        public int SelectionCount => config.SelectionCount;
        
        /// <summary>
        ///奖励池
        /// </summary>
        public int PoolId => config.PoolId;
        
    } 
}