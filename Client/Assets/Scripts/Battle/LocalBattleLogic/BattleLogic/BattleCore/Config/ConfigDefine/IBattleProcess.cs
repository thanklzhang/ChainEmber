/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
namespace Battle
{
    
    
 
    public interface IBattleProcess : IConfig
    {
        
        /// <summary>
        ///介绍
        /// </summary>
        string Describe {get;} 
        
        /// <summary>
        ///胜利条件
        /// </summary>
        int WinType {get;} 
        
        /// <summary>
        ///失败条件
        /// </summary>
        int LoseType {get;} 
        
    } 
}