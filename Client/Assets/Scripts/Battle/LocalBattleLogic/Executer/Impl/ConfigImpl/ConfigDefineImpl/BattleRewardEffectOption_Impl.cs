/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
using Config;
namespace Battle
{
    
    
 
    public class BattleRewardEffectOption_Impl : IBattleRewardEffectOption
    {
        private Config.BattleRewardEffectOption config;
        
        public void Init(int id)
        {
            config = ConfigManager.Instance.GetById<Config.BattleRewardEffectOption>(id);
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
        ///获得时机类型
        /// </summary>
        public int GainTimingType => config.GainTimingType;
        
        /// <summary>
        ///类型
        /// </summary>
        public int Type => config.Type;
        
        /// <summary>
        ///效果值列表
        /// </summary>
        public List<int> ValueList => config.ValueList;
        
        /// <summary>
        ///奖励中数值的权重
        /// </summary>
        public List<int> WeightList => config.WeightList;
        
        /// <summary>
        ///参数，类型不同意义也不同
        /// </summary>
        public List<int> ParamIntList => config.ParamIntList;
        
        /// <summary>
        ///最大获得次数
        /// </summary>
        public int MaxGainTimesType => config.MaxGainTimesType;
        
        /// <summary>
        ///奖励效果是否对新生成的队友也生效（目前适用于全员加属性和buff）
        /// </summary>
        public int ApplyToNewTeamMembers => config.ApplyToNewTeamMembers;
        
    } 
}