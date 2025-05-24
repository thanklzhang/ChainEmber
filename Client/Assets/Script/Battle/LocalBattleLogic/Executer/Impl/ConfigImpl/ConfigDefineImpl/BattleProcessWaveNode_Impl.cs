/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
using Config;
namespace Battle
{
    
    
 
    public class BattleProcessWaveNode_Impl : IBattleProcessWaveNode
    {
        private Config.BattleProcessWaveNode config;
        
        public void Init(int id)
        {
            config = ConfigManager.Instance.GetById<Config.BattleProcessWaveNode>(id);
        }
        
        public int Id => config.Id;
        
        /// <summary>
        ///介绍
        /// </summary>
        public string Describe => config.Describe;
        
        /// <summary>
        ///介绍2
        /// </summary>
        public string Describe2 => config.Describe2;
        
        /// <summary>
        ///波次索引
        /// </summary>
        public int Index => config.Index;
        
        /// <summary>
        ///是否是本波结束节点（如果是怪物死亡胜利的条件，那么这波怪物只有一个，怪物死亡就胜利）
        /// </summary>
        public int IsEndNode => config.IsEndNode;
        
        /// <summary>
        ///初始延迟（*1000）
        /// </summary>
        public int DelayTime => config.DelayTime;
        
        /// <summary>
        ///间隔时间（*1000）
        /// </summary>
        public int IntervalTime => config.IntervalTime;
        
        /// <summary>
        ///触发次数
        /// </summary>
        public int TriggerCount => config.TriggerCount;
        
        /// <summary>
        ///实体实例id
        /// </summary>
        public int EntityInstanceId => config.EntityInstanceId;
        
        /// <summary>
        ///实体个数
        /// </summary>
        public int EntityCount => config.EntityCount;
        
        /// <summary>
        ///生成位置类型
        /// </summary>
        public int PosType => config.PosType;
        
    } 
}