/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
using Config;
namespace Battle
{
    
    
 
    public class SkillTrack_Impl : ISkillTrack
    {
        private Config.SkillTrack config;
        
        public void Init(int id)
        {
            config = ConfigManager.Instance.GetById<Config.SkillTrack>(id);
        }
        
        public int Id => config.Id;
        
        /// <summary>
        ///名称
        /// </summary>
        public string Name => config.Name;
        
        /// <summary>
        ///介绍
        /// </summary>
        public string Describe => config.Describe;
        
        /// <summary>
        ///开始时机的类型
        /// </summary>
        public int StartTimeType => config.StartTimeType;
        
        /// <summary>
        ///轨迹类型
        /// </summary>
        public int Type => config.Type;
        
        /// <summary>
        ///方向类型
        /// </summary>
        public int DirectType => config.DirectType;
        
        /// <summary>
        ///角度
        /// </summary>
        public int Angle => config.Angle;
        
        /// <summary>
        ///开始点类型
        /// </summary>
        public int StartPosType => config.StartPosType;
        
        /// <summary>
        ///长度(*1000)
        /// </summary>
        public int Length => config.Length;
        
        /// <summary>
        ///宽度(*1000)
        /// </summary>
        public int Width => config.Width;
        
        /// <summary>
        ///是否跟随施法者
        /// </summary>
        public int IsFollow => config.IsFollow;
        
        /// <summary>
        ///结束时机类型
        /// </summary>
        public int EndTimeType => config.EndTimeType;
        
        /// <summary>
        ///延迟结束时间(*1000)
        /// </summary>
        public int DelayEndTime => config.DelayEndTime;
        
        /// <summary>
        ///效果资源Id
        /// </summary>
        public int EffectResId => config.EffectResId;
        
        /// <summary>
        ///显示的颜色类型
        /// </summary>
        public int ShowColorType => config.ShowColorType;
        
        /// <summary>
        ///进度完成时间(*1000)
        /// </summary>
        public int ProgressFinishTime => config.ProgressFinishTime;
        
    } 
}