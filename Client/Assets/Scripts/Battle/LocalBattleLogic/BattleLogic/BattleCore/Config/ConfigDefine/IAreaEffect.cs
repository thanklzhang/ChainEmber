/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
namespace Battle
{
    
    
 
    public interface IAreaEffect : IConfig
    {
        
        /// <summary>
        ///技能名称
        /// </summary>
        string Name {get;} 
        
        /// <summary>
        ///技能介绍
        /// </summary>
        string Describe {get;} 
        
        /// <summary>
        ///区域类型
        /// </summary>
        int AreaType {get;} 
        
        /// <summary>
        ///开始点类型
        /// </summary>
        int StartPosType {get;} 
        
        /// <summary>
        ///开始点位移方向类型
        /// </summary>
        int StartPosShiftDirType {get;} 
        
        /// <summary>
        ///开始点位移方向的距离(*1000)
        /// </summary>
        int StartPosShiftDistance {get;} 
        
        /// <summary>
        ///影响范围的参数 半径 或者 长宽等  (*1000)
        /// </summary>
        List<int> RangeParam {get;} 
        
        /// <summary>
        ///选择实体类型（1级筛选类型）(实体间关系)
        /// </summary>
        int EntityRelationFilterType {get;} 
        
        /// <summary>
        ///2级筛选类型（在1级筛选类型选取之后）
        /// </summary>
        int FilterEntityType {get;} 
        
        /// <summary>
        ///排除实体类型
        /// </summary>
        int ExcludeEntityType {get;} 
        
        /// <summary>
        ///随机选择数量，大于 0 表示最近进行随机选取
        /// </summary>
        int RandSelectCount {get;} 
        
        /// <summary>
        ///触发的效果列表（对每个选取单位）
        /// </summary>
        List<int> EffectList {get;} 
        
        /// <summary>
        ///触发的效果列表（对所有选择的单位，一般来说是对于之后的组效果，如 linkGroup）
        /// </summary>
        List<int> GroupEffectList {get;} 
        
        /// <summary>
        ///效果资源id
        /// </summary>
        int EffectResId {get;} 
        
    } 
}