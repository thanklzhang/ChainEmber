/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
namespace Config
{
    
    
    
       
    public class TeamStage : BaseConfig
    {
        
        /// <summary>
        ///名称
        /// </summary>
        private string name; 
        
        /// <summary>
        ///描述
        /// </summary>
        private string describe; 
        
        /// <summary>
        ///战斗id
        /// </summary>
        private int battleId; 
        
        /// <summary>
        ///最多玩家数
        /// </summary>
        private int maxPlayerCount; 
        
        /// <summary>
        ///关卡图片资源id
        /// </summary>
        private int iconResId; 
        

        
        public string Name { get => name; }     
        
        public string Describe { get => describe; }     
        
        public int BattleId { get => battleId; }     
        
        public int MaxPlayerCount { get => maxPlayerCount; }     
        
        public int IconResId { get => iconResId; }     
        

    } 
}