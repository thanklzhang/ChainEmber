/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
namespace Battle
{
    
    
 
    public interface ITeamStage : IConfig
    {
        
        /// <summary>
        ///名称
        /// </summary>
        string Name {get;} 
        
        /// <summary>
        ///描述
        /// </summary>
        string Describe {get;} 
        
        /// <summary>
        ///战斗id
        /// </summary>
        int BattleId {get;} 
        
        /// <summary>
        ///最多玩家数
        /// </summary>
        int MaxPlayerCount {get;} 
        
        /// <summary>
        ///关卡图片资源id
        /// </summary>
        int IconResId {get;} 
        
    } 
}