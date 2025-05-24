/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
namespace Config
{
    
    
    
       
    public class Item : BaseConfig
    {
        
        /// <summary>
        ///道具名称
        /// </summary>
        private string name; 
        
        /// <summary>
        ///道具介绍
        /// </summary>
        private string describe; 
        
        /// <summary>
        ///图标显示资源id
        /// </summary>
        private int iconResId; 
        
        /// <summary>
        ///是否可叠加
        /// </summary>
        private int isCanOverlying; 
        

        
        public string Name { get => name; }     
        
        public string Describe { get => describe; }     
        
        public int IconResId { get => iconResId; }     
        
        public int IsCanOverlying { get => isCanOverlying; }     
        

    } 
}