/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
namespace Config
{
    
    
    
       
    public class BattleItem : BaseConfig
    {
        
        /// <summary>
        ///名称
        /// </summary>
        private string name; 
        
        /// <summary>
        ///备注
        /// </summary>
        private string describe; 
        
        /// <summary>
        ///图片资源id
        /// </summary>
        private int iconResId; 
        
        /// <summary>
        ///道具类型
        /// </summary>
        private int itemType; 
        
        /// <summary>
        ///道具品质
        /// </summary>
        private int itemQuality; 
        
        /// <summary>
        ///叠加类型
        /// </summary>
        private int addType; 
        
        /// <summary>
        ///使用后是否销毁
        /// </summary>
        private int isDestroyAfterUse; 
        
        /// <summary>
        ///包含的技能id（包含主动效果和被动效果）
        /// </summary>
        private int skillId; 
        
        /// <summary>
        ///增加的属性组配置Id
        /// </summary>
        private int attrGroupConfigId; 
        

        
        public string Name { get => name; }     
        
        public string Describe { get => describe; }     
        
        public int IconResId { get => iconResId; }     
        
        public int ItemType { get => itemType; }     
        
        public int ItemQuality { get => itemQuality; }     
        
        public int AddType { get => addType; }     
        
        public int IsDestroyAfterUse { get => isDestroyAfterUse; }     
        
        public int SkillId { get => skillId; }     
        
        public int AttrGroupConfigId { get => attrGroupConfigId; }     
        

    } 
}