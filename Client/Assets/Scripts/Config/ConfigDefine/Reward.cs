/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
namespace Config
{
    
    
    
       
    public class Reward : BaseConfig
    {
        
        /// <summary>
        ///名称
        /// </summary>
        private string name; 
        
        /// <summary>
        ///介绍
        /// </summary>
        private string describe; 
        
        /// <summary>
        ///奖励道具id列表
        /// </summary>
        private List<int> rewardItemIdList; 
        
        /// <summary>
        ///获得数目列表
        /// </summary>
        private List<int> rewardItemCountList; 
        

        
        public string Name { get => name; }     
        
        public string Describe { get => describe; }     
        
        public List<int> RewardItemIdList { get => rewardItemIdList; }     
        
        public List<int> RewardItemCountList { get => rewardItemCountList; }     
        

    } 
}