/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
using Config;
namespace Battle
{
    
    
 
    public class Battle_Impl : IBattle
    {
        private Config.Battle config;
        
        public void Init(int id)
        {
            config = ConfigManager.Instance.GetById<Config.Battle>(id);
        }
        
        public int Id => config.Id;
        
        /// <summary>
        ///战斗名称
        /// </summary>
        public string Name => config.Name;
        
        /// <summary>
        ///战斗介绍
        /// </summary>
        public string Describe => config.Describe;
        
        /// <summary>
        ///活动 id
        /// </summary>
        public int ActivityId => config.ActivityId;
        
        /// <summary>
        ///地图 id
        /// </summary>
        public int MapId => config.MapId;
        
        /// <summary>
        ///战斗触发器id
        /// </summary>
        public int TriggerId => config.TriggerId;
        
        /// <summary>
        ///强制玩家控制某一个实体(|分割玩家 ,分割索引和实体id)
        /// </summary>
        public List<List<int>> ForceUseHeroList => config.ForceUseHeroList;
        
        /// <summary>
        ///玩家控制实体初始位置(|分割玩家 ,分割坐标轴)(长度作为玩家数最大数目)
        /// </summary>
        public List<List<int>> InitPos_pre => config.InitPos_pre;
        
        /// <summary>
        ///boss限时击杀时间(*1000 微妙)
        /// </summary>
        public int BossLimitTime => config.BossLimitTime;
        
        /// <summary>
        ///关卡流程id
        /// </summary>
        public int ProcessId => config.ProcessId;
        
        /// <summary>
        ///战斗配置id
        /// </summary>
        public int BattleConfigId => config.BattleConfigId;
        
        /// <summary>
        ///队伍信息(| 分割队伍  ,分割玩家索引)
        /// </summary>
        public List<List<int>> TeamInfo => config.TeamInfo;
        
    } 
}