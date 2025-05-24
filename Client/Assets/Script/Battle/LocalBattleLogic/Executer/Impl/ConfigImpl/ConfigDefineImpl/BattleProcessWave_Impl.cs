/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
using Config;
namespace Battle
{
    
    
 
    public class BattleProcessWave_Impl : IBattleProcessWave
    {
        private Config.BattleProcessWave config;
        
        public void Init(int id)
        {
            config = ConfigManager.Instance.GetById<Config.BattleProcessWave>(id);
        }
        
        public int Id => config.Id;
        
        /// <summary>
        ///介绍
        /// </summary>
        public string Describe => config.Describe;
        
        /// <summary>
        ///流程id
        /// </summary>
        public int ProcessId => config.ProcessId;
        
        /// <summary>
        ///波次索引
        /// </summary>
        public int WaveIndex => config.WaveIndex;
        
        /// <summary>
        ///波次节点
        /// </summary>
        public List<int> WaveNodeIdList => config.WaveNodeIdList;
        
        /// <summary>
        ///波次类型
        /// </summary>
        public int WaveType => config.WaveType;
        
        /// <summary>
        ///准备时间（*1000）
        /// </summary>
        public int ReadyTime => config.ReadyTime;
        
        /// <summary>
        ///限制时间（*1000）（战斗时间）
        /// </summary>
        public int LimitTime => config.LimitTime;
        
        /// <summary>
        ///准备阶段获得的人口数
        /// </summary>
        public int ReadyAddPopulationCount => config.ReadyAddPopulationCount;
        
        /// <summary>
        ///准备阶段获得的奖励
        /// </summary>
        public int ReadyRewardId => config.ReadyRewardId;
        
        /// <summary>
        ///通过类型
        /// </summary>
        public int PassType => config.PassType;
        
        /// <summary>
        ///通过奖励
        /// </summary>
        public int PassRewardId => config.PassRewardId;
        
    } 
}