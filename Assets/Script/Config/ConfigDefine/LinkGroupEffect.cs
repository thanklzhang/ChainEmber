/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
namespace Config
{
    
    
    
       
    public class LinkGroupEffect : BaseConfig
    {
        
        /// <summary>
        ///技能名称
        /// </summary>
        private string name; 
        
        /// <summary>
        ///介绍
        /// </summary>
        private string describe; 
        
        /// <summary>
        ///链接类型
        /// </summary>
        private int linkType; 
        
        /// <summary>
        ///持续时间（*1000）
        /// </summary>
        private int lastTime; 
        
        /// <summary>
        ///效果类型
        /// </summary>
        private int effectType; 
        
        /// <summary>
        ///效果参数
        /// </summary>
        private List<string> effectParam; 
        
        /// <summary>
        ///最大链接实体数（0为所有全连）
        /// </summary>
        private int maxLinkEntityCount; 
        
        /// <summary>
        /// 是否链接释放者（自己）
        /// </summary>
        private int isAddReleaser; 
        
        /// <summary>
        ///开始时候的技能效果列表
        /// </summary>
        private List<int> startEffectList; 
        

        
        public string Name { get => name; }     
        
        public string Describe { get => describe; }     
        
        public int LinkType { get => linkType; }     
        
        public int LastTime { get => lastTime; }     
        
        public int EffectType { get => effectType; }     
        
        public List<string> EffectParam { get => effectParam; }     
        
        public int MaxLinkEntityCount { get => maxLinkEntityCount; }     
        
        public int IsAddReleaser { get => isAddReleaser; }     
        
        public List<int> StartEffectList { get => startEffectList; }     
        

    } 
}