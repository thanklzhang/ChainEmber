/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
namespace Config
{
    
    
    
       
    public class AreaEffect : BaseConfig
    {
        
        /// <summary>
        ///技能名称
        /// </summary>
        private string name; 
        
        /// <summary>
        ///技能介绍
        /// </summary>
        private string describe; 
        
        /// <summary>
        ///区域类型
        /// </summary>
        private int areaType; 
        
        /// <summary>
        ///开始点类型
        /// </summary>
        private int startPosType; 
        
        /// <summary>
        ///开始点位移方向类型
        /// </summary>
        private int startPosShiftDirType; 
        
        /// <summary>
        ///开始点位移方向的距离(*1000)
        /// </summary>
        private int startPosShiftDistance; 
        
        /// <summary>
        ///影响范围的参数 半径 或者 长宽等  (*1000)
        /// </summary>
        private List<int> rangeParam; 
        
        /// <summary>
        ///选择实体类型（1级筛选类型）(实体间关系)
        /// </summary>
        private int entityRelationFilterType; 
        
        /// <summary>
        ///2级筛选类型（在1级筛选类型选取之后）
        /// </summary>
        private int filterEntityType; 
        
        /// <summary>
        ///排除实体类型
        /// </summary>
        private int excludeEntityType; 
        
        /// <summary>
        ///随机选择数量，大于 0 表示最近进行随机选取
        /// </summary>
        private int randSelectCount; 
        
        /// <summary>
        ///触发的效果列表（对每个选取单位）
        /// </summary>
        private List<int> effectList; 
        
        /// <summary>
        ///触发的效果列表（对所有选择的单位，一般来说是对于之后的组效果，如 linkGroup）
        /// </summary>
        private List<int> groupEffectList; 
        
        /// <summary>
        ///效果资源id
        /// </summary>
        private int effectResId; 
        

        
        public string Name { get => name; }     
        
        public string Describe { get => describe; }     
        
        public int AreaType { get => areaType; }     
        
        public int StartPosType { get => startPosType; }     
        
        public int StartPosShiftDirType { get => startPosShiftDirType; }     
        
        public int StartPosShiftDistance { get => startPosShiftDistance; }     
        
        public List<int> RangeParam { get => rangeParam; }     
        
        public int EntityRelationFilterType { get => entityRelationFilterType; }     
        
        public int FilterEntityType { get => filterEntityType; }     
        
        public int ExcludeEntityType { get => excludeEntityType; }     
        
        public int RandSelectCount { get => randSelectCount; }     
        
        public List<int> EffectList { get => effectList; }     
        
        public List<int> GroupEffectList { get => groupEffectList; }     
        
        public int EffectResId { get => effectResId; }     
        

    } 
}