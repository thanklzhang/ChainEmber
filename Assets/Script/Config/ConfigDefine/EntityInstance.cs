/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
namespace Config
{
    
    
    
       
    public class EntityInstance : BaseConfig
    {
        
        /// <summary>
        ///名称
        /// </summary>
        private string name; 
        
        /// <summary>
        ///实体模版Id
        /// </summary>
        private int entityConfigId; 
        
        /// <summary>
        ///等级
        /// </summary>
        private int level; 
        
        /// <summary>
        ///星级
        /// </summary>
        private int star; 
        
        /// <summary>
        ///技能等级
        /// </summary>
        private List<int> skillLevels; 
        

        
        public string Name { get => name; }     
        
        public int EntityConfigId { get => entityConfigId; }     
        
        public int Level { get => level; }     
        
        public int Star { get => star; }     
        
        public List<int> SkillLevels { get => skillLevels; }     
        

    } 
}