/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
namespace Battle
{
    
    
 
    public interface IBattle : IConfig
    {
        
        /// <summary>
        ///战斗名称
        /// </summary>
        string Name {get;} 
        
        /// <summary>
        ///战斗介绍
        /// </summary>
        string Describe {get;} 
        
        /// <summary>
        ///活动 id
        /// </summary>
        int ActivityId {get;} 
        
        /// <summary>
        ///地图 id
        /// </summary>
        int MapId {get;} 
        
        /// <summary>
        ///战斗触发器id
        /// </summary>
        int TriggerId {get;} 
        
        /// <summary>
        ///强制玩家控制某一个实体(|分割玩家 ,分割索引和实体id)
        /// </summary>
        List<List<int>> ForceUseHeroList {get;} 
        
        /// <summary>
        ///玩家控制实体初始位置(|分割玩家 ,分割坐标轴)(长度作为玩家数最大数目)
        /// </summary>
        List<List<int>> InitPos_pre {get;} 
        
        /// <summary>
        ///boss限时击杀时间(*1000 微妙)
        /// </summary>
        int BossLimitTime {get;} 
        
        /// <summary>
        ///关卡流程id
        /// </summary>
        int ProcessId {get;} 
        
        /// <summary>
        ///通关奖励id
        /// </summary>
        int PassRewardId {get;} 
        
        /// <summary>
        ///战斗配置id
        /// </summary>
        int BattleConfigId {get;} 
        
        /// <summary>
        ///队伍信息(| 分割队伍  ,分割玩家索引)
        /// </summary>
        List<List<int>> TeamInfo {get;} 
        
    } 
}