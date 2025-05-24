/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
namespace Battle
{
    
    
 
    public interface IConditionActionEffect : IConfig
    {
        
        /// <summary>
        ///名称
        /// </summary>
        string Name {get;} 
        
        /// <summary>
        ///条件
        /// </summary>
        int Condition {get;} 
        
        /// <summary>
        ///条件参数(intList)
        /// </summary>
        List<int> ConditionParamIntList {get;} 
        
        /// <summary>
        ///操作符
        /// </summary>
        string Operate {get;} 
        
        /// <summary>
        ///操作比较值(int)
        /// </summary>
        int OpIntValue {get;} 
        
        /// <summary>
        ///行为类型
        /// </summary>
        int ActionType {get;} 
        
        /// <summary>
        ///行为参数
        /// </summary>
        List<int> ActionParamIntList {get;} 
        
    } 
}