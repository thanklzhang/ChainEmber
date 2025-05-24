/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
using Config;
namespace Battle
{
    
    
 
    public class MainTaskStage_Impl : IMainTaskStage
    {
        private Config.MainTaskStage config;
        
        public void Init(int id)
        {
            config = ConfigManager.Instance.GetById<Config.MainTaskStage>(id);
        }
        
        public int Id => config.Id;
        
        /// <summary>
        ///所属章节
        /// </summary>
        public int ChapterId => config.ChapterId;
        
        /// <summary>
        ///描述(仅供预览)
        /// </summary>
        public string Des_ => config.Des_;
        
        /// <summary>
        ///章节名称
        /// </summary>
        public string Name => config.Name;
        
        /// <summary>
        ///描述
        /// </summary>
        public string Describe => config.Describe;
        
        /// <summary>
        ///战斗id
        /// </summary>
        public int BattleId => config.BattleId;
        
    } 
}