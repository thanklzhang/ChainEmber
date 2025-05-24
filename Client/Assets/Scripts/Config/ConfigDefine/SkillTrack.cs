/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
namespace Config
{
    
    
    
       
    public class SkillTrack : BaseConfig
    {
        
        /// <summary>
        ///名称
        /// </summary>
        private string name; 
        
        /// <summary>
        ///介绍
        /// </summary>
        private string describe; 
        
        /// <summary>
        ///开始时机的类型
        /// </summary>
        private int startTimeType; 
        
        /// <summary>
        ///轨迹类型
        /// </summary>
        private int type; 
        
        /// <summary>
        ///方向类型
        /// </summary>
        private int directType; 
        
        /// <summary>
        ///角度
        /// </summary>
        private int angle; 
        
        /// <summary>
        ///开始点类型
        /// </summary>
        private int startPosType; 
        
        /// <summary>
        ///长度(*1000)
        /// </summary>
        private int length; 
        
        /// <summary>
        ///宽度(*1000)
        /// </summary>
        private int width; 
        
        /// <summary>
        ///是否跟随施法者
        /// </summary>
        private int isFollow; 
        
        /// <summary>
        ///结束时机类型
        /// </summary>
        private int endTimeType; 
        
        /// <summary>
        ///延迟结束时间(*1000)
        /// </summary>
        private int delayEndTime; 
        
        /// <summary>
        ///效果资源Id
        /// </summary>
        private int effectResId; 
        
        /// <summary>
        ///显示的颜色类型
        /// </summary>
        private int showColorType; 
        
        /// <summary>
        ///进度完成时间(*1000)
        /// </summary>
        private int progressFinishTime; 
        

        
        public string Name { get => name; }     
        
        public string Describe { get => describe; }     
        
        public int StartTimeType { get => startTimeType; }     
        
        public int Type { get => type; }     
        
        public int DirectType { get => directType; }     
        
        public int Angle { get => angle; }     
        
        public int StartPosType { get => startPosType; }     
        
        public int Length { get => length; }     
        
        public int Width { get => width; }     
        
        public int IsFollow { get => isFollow; }     
        
        public int EndTimeType { get => endTimeType; }     
        
        public int DelayEndTime { get => delayEndTime; }     
        
        public int EffectResId { get => effectResId; }     
        
        public int ShowColorType { get => showColorType; }     
        
        public int ProgressFinishTime { get => progressFinishTime; }     
        

    } 
}