/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
namespace Battle
{
    
    
 
    public interface IBattleItem : IConfig
    {
        
        /// <summary>
        ///名称
        /// </summary>
        string Name {get;} 
        
        /// <summary>
        ///备注
        /// </summary>
        string Describe {get;} 
        
        /// <summary>
        ///图片资源id
        /// </summary>
        int IconResId {get;} 
        
        /// <summary>
        ///道具类型
        /// </summary>
        int ItemType {get;} 
        
        /// <summary>
        ///道具品质
        /// </summary>
        int ItemQuality {get;} 
        
        /// <summary>
        ///叠加类型
        /// </summary>
        int AddType {get;} 
        
        /// <summary>
        ///使用后是否销毁
        /// </summary>
        int IsDestroyAfterUse {get;} 
        
        /// <summary>
        ///包含的技能id（包含主动效果和被动效果）
        /// </summary>
        int SkillId {get;} 
        
        /// <summary>
        ///增加的属性组配置Id
        /// </summary>
        int AttrGroupConfigId {get;} 
        
    } 
}