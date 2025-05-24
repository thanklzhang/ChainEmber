/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
namespace Config
{
    
    
    
       
    public class BattleProcessWaveReward : BaseConfig
    {
        
        /// <summary>
        ///介绍
        /// </summary>
        private string describe; 
        
        /// <summary>
        ///道具奖励
        /// </summary>
        private List<int> itemIdList; 
        
        /// <summary>
        ///道具数目
        /// </summary>
        private List<int> itemCountList; 
        
        /// <summary>
        ///固定箱子奖励
        /// </summary>
        private List<int> fixedBoxIdList; 
        
        /// <summary>
        ///固定箱子奖励数目
        /// </summary>
        private List<int> fixedBoxCountList; 
        
        /// <summary>
        ///概率箱子奖励
        /// </summary>
        private List<int> randBoxIdList; 
        
        /// <summary>
        ///概率箱子权重
        /// </summary>
        private List<int> randBoxWeightList; 
        
        /// <summary>
        ///概率箱子数目
        /// </summary>
        private int randBoxCount; 
        

        
        public string Describe { get => describe; }     
        
        public List<int> ItemIdList { get => itemIdList; }     
        
        public List<int> ItemCountList { get => itemCountList; }     
        
        public List<int> FixedBoxIdList { get => fixedBoxIdList; }     
        
        public List<int> FixedBoxCountList { get => fixedBoxCountList; }     
        
        public List<int> RandBoxIdList { get => randBoxIdList; }     
        
        public List<int> RandBoxWeightList { get => randBoxWeightList; }     
        
        public int RandBoxCount { get => randBoxCount; }     
        

    } 
}