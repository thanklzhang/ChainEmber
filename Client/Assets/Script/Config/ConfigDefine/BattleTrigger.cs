/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
namespace Config
{
    
    
    
       
    public class BattleTrigger : BaseConfig
    {
        
        /// <summary>
        ///触发器名称
        /// </summary>
        private string name; 
        
        /// <summary>
        ///脚本文件目录(相对于打包根目录 文件夹) (server则是相对于 资源中 Resource 文件夹)
        /// </summary>
        private string scriptPath; 
        

        
        public string Name { get => name; }     
        
        public string ScriptPath { get => scriptPath; }     
        

    } 
}