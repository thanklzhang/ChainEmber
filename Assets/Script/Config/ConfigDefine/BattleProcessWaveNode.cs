/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
namespace Config
{
    
    
    
       
    public class BattleProcessWaveNode : BaseConfig
    {
        
        /// <summary>
        ///介绍
        /// </summary>
        private string describe; 
        
        /// <summary>
        ///介绍2
        /// </summary>
        private string describe2; 
        
        /// <summary>
        ///波次索引
        /// </summary>
        private int index; 
        
        /// <summary>
        ///是否是本波结束节点（如果是怪物死亡胜利的条件，那么这波怪物只有一个，怪物死亡就胜利）
        /// </summary>
        private int isEndNode; 
        
        /// <summary>
        ///初始延迟（*1000）
        /// </summary>
        private int delayTime; 
        
        /// <summary>
        ///间隔时间（*1000）
        /// </summary>
        private int intervalTime; 
        
        /// <summary>
        ///触发次数
        /// </summary>
        private int triggerCount; 
        
        /// <summary>
        ///实体实例id
        /// </summary>
        private int entityInstanceId; 
        
        /// <summary>
        ///实体个数
        /// </summary>
        private int entityCount; 
        
        /// <summary>
        ///生成位置类型
        /// </summary>
        private int posType; 
        

        
        public string Describe { get => describe; }     
        
        public string Describe2 { get => describe2; }     
        
        public int Index { get => index; }     
        
        public int IsEndNode { get => isEndNode; }     
        
        public int DelayTime { get => delayTime; }     
        
        public int IntervalTime { get => intervalTime; }     
        
        public int TriggerCount { get => triggerCount; }     
        
        public int EntityInstanceId { get => entityInstanceId; }     
        
        public int EntityCount { get => entityCount; }     
        
        public int PosType { get => posType; }     
        

    } 
}