/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
namespace Battle
{
    
    
 
    public interface ISkillTrack : IConfig
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
        ///开始时机的类型
        /// </summary>
        int StartTimeType {get;} 
        
        /// <summary>
        ///轨迹类型
        /// </summary>
        int Type {get;} 
        
        /// <summary>
        ///方向类型
        /// </summary>
        int DirectType {get;} 
        
        /// <summary>
        ///角度
        /// </summary>
        int Angle {get;} 
        
        /// <summary>
        ///开始点类型
        /// </summary>
        int StartPosType {get;} 
        
        /// <summary>
        ///长度(*1000)
        /// </summary>
        int Length {get;} 
        
        /// <summary>
        ///宽度(*1000)
        /// </summary>
        int Width {get;} 
        
        /// <summary>
        ///是否跟随施法者
        /// </summary>
        int IsFollow {get;} 
        
        /// <summary>
        ///结束时机类型
        /// </summary>
        int EndTimeType {get;} 
        
        /// <summary>
        ///延迟结束时间(*1000)
        /// </summary>
        int DelayEndTime {get;} 
        
        /// <summary>
        ///效果资源Id
        /// </summary>
        int EffectResId {get;} 
        
        /// <summary>
        ///显示的颜色类型
        /// </summary>
        int ShowColorType {get;} 
        
        /// <summary>
        ///进度完成时间(*1000)
        /// </summary>
        int ProgressFinishTime {get;} 
        
    } 
}