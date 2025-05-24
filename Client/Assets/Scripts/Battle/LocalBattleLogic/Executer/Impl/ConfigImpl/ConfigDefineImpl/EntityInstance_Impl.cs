/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
using Config;
namespace Battle
{
    
    
 
    public class EntityInstance_Impl : IEntityInstance
    {
        private Config.EntityInstance config;
        
        public void Init(int id)
        {
            config = ConfigManager.Instance.GetById<Config.EntityInstance>(id);
        }
        
        public int Id => config.Id;
        
        /// <summary>
        ///名称
        /// </summary>
        public string Name => config.Name;
        
        /// <summary>
        ///实体模版Id
        /// </summary>
        public int EntityConfigId => config.EntityConfigId;
        
        /// <summary>
        ///等级
        /// </summary>
        public int Level => config.Level;
        
        /// <summary>
        ///星级
        /// </summary>
        public int Star => config.Star;
        
        /// <summary>
        ///技能等级
        /// </summary>
        public List<int> SkillLevels => config.SkillLevels;
        
    } 
}