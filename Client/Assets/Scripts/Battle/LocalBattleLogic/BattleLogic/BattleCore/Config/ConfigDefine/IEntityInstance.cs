/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
namespace Battle
{
    
    
 
    public interface IEntityInstance : IConfig
    {
        
        /// <summary>
        ///名称
        /// </summary>
        string Name {get;} 
        
        /// <summary>
        ///实体模版Id
        /// </summary>
        int EntityConfigId {get;} 
        
        /// <summary>
        ///等级
        /// </summary>
        int Level {get;} 
        
        /// <summary>
        ///星级
        /// </summary>
        int Star {get;} 
        
        /// <summary>
        ///技能等级
        /// </summary>
        List<int> SkillLevels {get;} 
        
    } 
}