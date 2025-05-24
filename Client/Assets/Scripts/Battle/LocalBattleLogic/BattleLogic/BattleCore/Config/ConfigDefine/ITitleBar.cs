/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
namespace Battle
{
    
    
 
    public interface ITitleBar : IConfig
    {
        
        /// <summary>
        ///名称
        /// </summary>
        string Name {get;} 
        
        /// <summary>
        ///介绍
        /// </summary>
        string Describe {get;} 
        
        /// <summary>
        ///标题资源id
        /// </summary>
        int TitleRes {get;} 
        
        /// <summary>
        ///标题显示文本
        /// </summary>
        string TitleName {get;} 
        
        /// <summary>
        ///标题资源列表(，分割)
        /// </summary>
        List<int> ResList {get;} 
        
        /// <summary>
        ///是否显示关闭按钮
        /// </summary>
        int IsShowCloseBtn {get;} 
        
        /// <summary>
        ///是否显示背景
        /// </summary>
        int IsShowBg {get;} 
        
        /// <summary>
        ///是否显示线
        /// </summary>
        int IsShowLine {get;} 
        
    } 
}