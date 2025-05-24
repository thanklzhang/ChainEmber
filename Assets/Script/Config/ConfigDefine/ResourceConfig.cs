/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
namespace Config
{
    
    
    
       
    public class ResourceConfig : BaseConfig
    {
        
        /// <summary>
        ///资源名称
        /// </summary>
        private string name; 
        
        /// <summary>
        ///备注
        /// </summary>
        private string des; 
        
        /// <summary>
        ///资源路径
        /// </summary>
        private string path; 
        
        /// <summary>
        ///后缀
        /// </summary>
        private string ext; 
        
        /// <summary>
        ///资源类型
        /// </summary>
        private int type; 
        
        /// <summary>
        ///资源标签
        /// </summary>
        private string tag; 
        

        
        public string Name { get => name; }     
        
        public string Des { get => des; }     
        
        public string Path { get => path; }     
        
        public string Ext { get => ext; }     
        
        public int Type { get => type; }     
        
        public string Tag { get => tag; }     
        

    } 
}