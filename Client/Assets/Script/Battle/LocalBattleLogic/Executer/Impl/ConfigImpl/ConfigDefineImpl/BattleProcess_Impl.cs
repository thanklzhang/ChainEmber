/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
using Config;
namespace Battle
{
    
    
 
    public class BattleProcess_Impl : IBattleProcess
    {
        private Config.BattleProcess config;
        
        public void Init(int id)
        {
            config = ConfigManager.Instance.GetById<Config.BattleProcess>(id);
        }
        
        public int Id => config.Id;
        
        /// <summary>
        ///介绍
        /// </summary>
        public string Describe => config.Describe;
        
        /// <summary>
        ///胜利条件
        /// </summary>
        public int WinType => config.WinType;
        
        /// <summary>
        ///失败条件
        /// </summary>
        public int LoseType => config.LoseType;
        
    } 
}