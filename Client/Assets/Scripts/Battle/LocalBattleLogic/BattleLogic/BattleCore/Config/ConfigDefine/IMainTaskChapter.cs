/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
namespace Battle
{
    
    
 
    public interface IMainTaskChapter : IConfig
    {
        
        /// <summary>
        ///章节名称
        /// </summary>
        string Name {get;} 
        
        /// <summary>
        ///描述
        /// </summary>
        string Describe {get;} 
        
        /// <summary>
        ///关卡
        /// </summary>
        List<int> StageList {get;} 
        
    } 
}