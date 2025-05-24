/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
using Config;
namespace Battle
{
    
    
 
    public class MainTaskChapter_Impl : IMainTaskChapter
    {
        private Config.MainTaskChapter config;
        
        public void Init(int id)
        {
            config = ConfigManager.Instance.GetById<Config.MainTaskChapter>(id);
        }
        
        public int Id => config.Id;
        
        /// <summary>
        ///章节名称
        /// </summary>
        public string Name => config.Name;
        
        /// <summary>
        ///描述
        /// </summary>
        public string Describe => config.Describe;
        
        /// <summary>
        ///关卡
        /// </summary>
        public List<int> StageList => config.StageList;
        
    } 
}