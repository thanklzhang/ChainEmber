/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
namespace Battle
{
    
    
 
    public interface IMainTaskStage : IConfig
    {
        
        /// <summary>
        ///所属章节
        /// </summary>
        int ChapterId {get;} 
        
        /// <summary>
        ///描述(仅供预览)
        /// </summary>
        string Des_ {get;} 
        
        /// <summary>
        ///章节名称
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
        
    } 
}