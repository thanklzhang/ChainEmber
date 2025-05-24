/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
namespace Config
{
    
    
    
       
    public class BattleProcessWave : BaseConfig
    {
        
        /// <summary>
        ///介绍
        /// </summary>
        private string describe; 
        
        /// <summary>
        ///流程id
        /// </summary>
        private int processId; 
        
        /// <summary>
        ///波次索引
        /// </summary>
        private int waveIndex; 
        
        /// <summary>
        ///波次节点
        /// </summary>
        private List<int> waveNodeIdList; 
        
        /// <summary>
        ///波次类型
        /// </summary>
        private int waveType; 
        
        /// <summary>
        ///准备时间（*1000）
        /// </summary>
        private int readyTime; 
        
        /// <summary>
        ///限制时间（*1000）（战斗时间）
        /// </summary>
        private int limitTime; 
        
        /// <summary>
        ///准备阶段获得的人口数
        /// </summary>
        private int readyAddPopulationCount; 
        
        /// <summary>
        ///准备阶段获得的奖励
        /// </summary>
        private int readyRewardId; 
        
        /// <summary>
        ///通过类型
        /// </summary>
        private int passType; 
        
        /// <summary>
        ///通过奖励
        /// </summary>
        private int passRewardId; 
        

        
        public string Describe { get => describe; }     
        
        public int ProcessId { get => processId; }     
        
        public int WaveIndex { get => waveIndex; }     
        
        public List<int> WaveNodeIdList { get => waveNodeIdList; }     
        
        public int WaveType { get => waveType; }     
        
        public int ReadyTime { get => readyTime; }     
        
        public int LimitTime { get => limitTime; }     
        
        public int ReadyAddPopulationCount { get => readyAddPopulationCount; }     
        
        public int ReadyRewardId { get => readyRewardId; }     
        
        public int PassType { get => passType; }     
        
        public int PassRewardId { get => passRewardId; }     
        

    } 
}