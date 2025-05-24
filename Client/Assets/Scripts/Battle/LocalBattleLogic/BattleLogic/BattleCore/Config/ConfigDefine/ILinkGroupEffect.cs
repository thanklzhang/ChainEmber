/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
namespace Battle
{
    
    
 
    public interface ILinkGroupEffect : IConfig
    {
        
        /// <summary>
        ///技能名称
        /// </summary>
        string Name {get;} 
        
        /// <summary>
        ///介绍
        /// </summary>
        string Describe {get;} 
        
        /// <summary>
        ///链接类型
        /// </summary>
        int LinkType {get;} 
        
        /// <summary>
        ///持续时间（*1000）
        /// </summary>
        int LastTime {get;} 
        
        /// <summary>
        ///效果类型
        /// </summary>
        int EffectType {get;} 
        
        /// <summary>
        ///效果参数
        /// </summary>
        List<string> EffectParam {get;} 
        
        /// <summary>
        ///最大链接实体数（0为所有全连）
        /// </summary>
        int MaxLinkEntityCount {get;} 
        
        /// <summary>
        /// 是否链接释放者（自己）
        /// </summary>
        int IsAddReleaser {get;} 
        
        /// <summary>
        ///开始时候的技能效果列表
        /// </summary>
        List<int> StartEffectList {get;} 
        
    } 
}