/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
using Config;
namespace Battle
{
    
    
 
    public class AreaEffect_Impl : IAreaEffect
    {
        private Config.AreaEffect config;
        
        public void Init(int id)
        {
            config = ConfigManager.Instance.GetById<Config.AreaEffect>(id);
        }
        
        public int Id => config.Id;
        
        /// <summary>
        ///技能名称
        /// </summary>
        public string Name => config.Name;
        
        /// <summary>
        ///技能介绍
        /// </summary>
        public string Describe => config.Describe;
        
        /// <summary>
        ///区域类型
        /// </summary>
        public int AreaType => config.AreaType;
        
        /// <summary>
        ///开始点类型
        /// </summary>
        public int StartPosType => config.StartPosType;
        
        /// <summary>
        ///开始点位移方向类型
        /// </summary>
        public int StartPosShiftDirType => config.StartPosShiftDirType;
        
        /// <summary>
        ///开始点位移方向的距离(*1000)
        /// </summary>
        public int StartPosShiftDistance => config.StartPosShiftDistance;
        
        /// <summary>
        ///影响范围的参数 半径 或者 长宽等  (*1000)
        /// </summary>
        public List<int> RangeParam => config.RangeParam;
        
        /// <summary>
        ///选择实体类型（1级筛选类型）(实体间关系)
        /// </summary>
        public int EntityRelationFilterType => config.EntityRelationFilterType;
        
        /// <summary>
        ///2级筛选类型（在1级筛选类型选取之后）
        /// </summary>
        public int FilterEntityType => config.FilterEntityType;
        
        /// <summary>
        ///排除实体类型
        /// </summary>
        public int ExcludeEntityType => config.ExcludeEntityType;
        
        /// <summary>
        ///随机选择数量，大于 0 表示最近进行随机选取
        /// </summary>
        public int RandSelectCount => config.RandSelectCount;
        
        /// <summary>
        ///触发的效果列表（对每个选取单位）
        /// </summary>
        public List<int> EffectList => config.EffectList;
        
        /// <summary>
        ///触发的效果列表（对所有选择的单位，一般来说是对于之后的组效果，如 linkGroup）
        /// </summary>
        public List<int> GroupEffectList => config.GroupEffectList;
        
        /// <summary>
        ///效果资源id
        /// </summary>
        public int EffectResId => config.EffectResId;
        
    } 
}