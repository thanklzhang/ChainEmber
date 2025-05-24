/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
namespace Config
{
    
    
    
       
    public class Attribute : BaseConfig
    {
        
        /// <summary>
        ///属性类型
        /// </summary>
        private int type; 
        
        /// <summary>
        ///名称
        /// </summary>
        private string name; 
        
        /// <summary>
        ///描述
        /// </summary>
        private string describe; 
        
        /// <summary>
        ///图标资源id
        /// </summary>
        private int iconResId; 
        

        
        public int Type { get => type; }     
        
        public string Name { get => name; }     
        
        public string Describe { get => describe; }     
        
        public int IconResId { get => iconResId; }     
        

    } 
}