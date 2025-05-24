/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
namespace Config
{
    
    
    
       
    public class BattleMyBoxItem : BaseConfig
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
        ///图标资源id
        /// </summary>
        private int iconResId; 
        
        /// <summary>
        ///宝箱组品质（绿 - 红）
        /// </summary>
        private int quality; 
        

        
        public string Name { get => name; }     
        
        public string Describe { get => describe; }     
        
        public int IconResId { get => iconResId; }     
        
        public int Quality { get => quality; }     
        

    } 
}