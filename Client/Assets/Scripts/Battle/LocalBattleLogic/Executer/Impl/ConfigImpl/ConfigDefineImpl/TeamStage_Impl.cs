/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
using Config;
namespace Battle
{
    
    
 
    public class TeamStage_Impl : ITeamStage
    {
        private Config.TeamStage config;
        
        public void Init(int id)
        {
            config = ConfigManager.Instance.GetById<Config.TeamStage>(id);
        }
        
        public int Id => config.Id;
        
        /// <summary>
        ///名称
        /// </summary>
        public string Name => config.Name;
        
        /// <summary>
        ///描述
        /// </summary>
        public string Describe => config.Describe;
        
        /// <summary>
        ///战斗id
        /// </summary>
        public int BattleId => config.BattleId;
        
        /// <summary>
        ///最多玩家数
        /// </summary>
        public int MaxPlayerCount => config.MaxPlayerCount;
        
        /// <summary>
        ///关卡图片资源id
        /// </summary>
        public int IconResId => config.IconResId;
        
    } 
}