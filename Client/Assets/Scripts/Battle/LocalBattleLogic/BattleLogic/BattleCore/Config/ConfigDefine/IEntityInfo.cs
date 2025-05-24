/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
namespace Battle
{
    
    
 
    public interface IEntityInfo : IConfig
    {
        
        /// <summary>
        ///名称
        /// </summary>
        string Name {get;} 
        
        /// <summary>
        ///介绍
        /// </summary>
        string Describe {get;} 
        
        /// <summary>
        ///战斗相关介绍
        /// </summary>
        string Describe2 {get;} 
        
        /// <summary>
        ///类型(0 npc单位实例 , 1 英雄模板)
        /// </summary>
        int Type {get;} 
        
        /// <summary>
        ///模型Id(先填资源id 之后换成另一张表格过度)
        /// </summary>
        int ModelId {get;} 
        
        /// <summary>
        ///基础属性模板id
        /// </summary>
        int BaseAttrId {get;} 
        
        /// <summary>
        ///等级属性模板id
        /// </summary>
        int LevelAttrId {get;} 
        
        /// <summary>
        ///星级属性模板id
        /// </summary>
        int StarAttrId {get;} 
        
        /// <summary>
        ///技能id列表
        /// </summary>
        List<int> SkillIds {get;} 
        
        /// <summary>
        ///大招技能配置id
        /// </summary>
        int UltimateSkillId {get;} 
        
        /// <summary>
        ///等级
        /// </summary>
        int Level_pre {get;} 
        
        /// <summary>
        ///品质
        /// </summary>
        int Quality {get;} 
        
        /// <summary>
        ///星级
        /// </summary>
        int Star_pre {get;} 
        
        /// <summary>
        ///技能等级
        /// </summary>
        List<int> SkillLevels_pre {get;} 
        
        /// <summary>
        ///源实体id，某些情况和这个实体算作一个，如 不同等级的召唤兽都算作一个 id
        /// </summary>
        int OriginEntityId {get;} 
        
        /// <summary>
        ///AI脚本
        /// </summary>
        string AiScript {get;} 
        
        /// <summary>
        ///是否boss
        /// </summary>
        int IsBoss {get;} 
        
        /// <summary>
        ///头像资源id
        /// </summary>
        int AvatarResId {get;} 
        
        /// <summary>
        ///全身像资源id
        /// </summary>
        int AllBodyResId {get;} 
        
    } 
}