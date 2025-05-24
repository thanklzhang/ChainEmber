/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
using Config;
namespace Battle
{
    
    
 
    public class ConditionActionEffect_Impl : IConditionActionEffect
    {
        private Config.ConditionActionEffect config;
        
        public void Init(int id)
        {
            config = ConfigManager.Instance.GetById<Config.ConditionActionEffect>(id);
        }
        
        public int Id => config.Id;
        
        /// <summary>
        ///名称
        /// </summary>
        public string Name => config.Name;
        
        /// <summary>
        ///条件
        /// </summary>
        public int Condition => config.Condition;
        
        /// <summary>
        ///条件参数(intList)
        /// </summary>
        public List<int> ConditionParamIntList => config.ConditionParamIntList;
        
        /// <summary>
        ///操作符
        /// </summary>
        public string Operate => config.Operate;
        
        /// <summary>
        ///操作比较值(int)
        /// </summary>
        public int OpIntValue => config.OpIntValue;
        
        /// <summary>
        ///行为类型
        /// </summary>
        public int ActionType => config.ActionType;
        
        /// <summary>
        ///行为参数
        /// </summary>
        public List<int> ActionParamIntList => config.ActionParamIntList;
        
    } 
}