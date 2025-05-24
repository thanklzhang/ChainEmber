/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
using Config;
namespace Battle
{
    
    
 
    public class TitleBar_Impl : ITitleBar
    {
        private Config.TitleBar config;
        
        public void Init(int id)
        {
            config = ConfigManager.Instance.GetById<Config.TitleBar>(id);
        }
        
        public int Id => config.Id;
        
        /// <summary>
        ///名称
        /// </summary>
        public string Name => config.Name;
        
        /// <summary>
        ///介绍
        /// </summary>
        public string Describe => config.Describe;
        
        /// <summary>
        ///标题资源id
        /// </summary>
        public int TitleRes => config.TitleRes;
        
        /// <summary>
        ///标题显示文本
        /// </summary>
        public string TitleName => config.TitleName;
        
        /// <summary>
        ///标题资源列表(，分割)
        /// </summary>
        public List<int> ResList => config.ResList;
        
        /// <summary>
        ///是否显示关闭按钮
        /// </summary>
        public int IsShowCloseBtn => config.IsShowCloseBtn;
        
        /// <summary>
        ///是否显示背景
        /// </summary>
        public int IsShowBg => config.IsShowBg;
        
        /// <summary>
        ///是否显示线
        /// </summary>
        public int IsShowLine => config.IsShowLine;
        
    } 
}