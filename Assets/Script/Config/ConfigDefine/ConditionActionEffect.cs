/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
namespace Config
{
    
    
    
       
    public class ConditionActionEffect : BaseConfig
    {
        
        /// <summary>
        ///名称
        /// </summary>
        private string name; 
        
        /// <summary>
        ///条件
        /// </summary>
        private int condition; 
        
        /// <summary>
        ///条件参数(intList)
        /// </summary>
        private List<int> conditionParamIntList; 
        
        /// <summary>
        ///操作符
        /// </summary>
        private string operate; 
        
        /// <summary>
        ///操作比较值(int)
        /// </summary>
        private int opIntValue; 
        
        /// <summary>
        ///行为类型
        /// </summary>
        private int actionType; 
        
        /// <summary>
        ///行为参数
        /// </summary>
        private List<int> actionParamIntList; 
        

        
        public string Name { get => name; }     
        
        public int Condition { get => condition; }     
        
        public List<int> ConditionParamIntList { get => conditionParamIntList; }     
        
        public string Operate { get => operate; }     
        
        public int OpIntValue { get => opIntValue; }     
        
        public int ActionType { get => actionType; }     
        
        public List<int> ActionParamIntList { get => actionParamIntList; }     
        

    } 
}