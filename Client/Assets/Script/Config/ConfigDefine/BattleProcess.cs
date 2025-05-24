/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
namespace Config
{
    
    
    
       
    public class BattleProcess : BaseConfig
    {
        
        /// <summary>
        ///介绍
        /// </summary>
        private string describe; 
        
        /// <summary>
        ///胜利条件
        /// </summary>
        private int winType; 
        
        /// <summary>
        ///失败条件
        /// </summary>
        private int loseType; 
        

        
        public string Describe { get => describe; }     
        
        public int WinType { get => winType; }     
        
        public int LoseType { get => loseType; }     
        

    } 
}