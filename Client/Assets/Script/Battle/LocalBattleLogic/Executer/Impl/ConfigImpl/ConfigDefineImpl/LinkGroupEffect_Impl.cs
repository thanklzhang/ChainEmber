/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
using Config;
namespace Battle
{
    
    
 
    public class LinkGroupEffect_Impl : ILinkGroupEffect
    {
        private Config.LinkGroupEffect config;
        
        public void Init(int id)
        {
            config = ConfigManager.Instance.GetById<Config.LinkGroupEffect>(id);
        }
        
        public int Id => config.Id;
        
        /// <summary>
        ///技能名称
        /// </summary>
        public string Name => config.Name;
        
        /// <summary>
        ///介绍
        /// </summary>
        public string Describe => config.Describe;
        
        /// <summary>
        ///链接类型
        /// </summary>
        public int LinkType => config.LinkType;
        
        /// <summary>
        ///持续时间（*1000）
        /// </summary>
        public int LastTime => config.LastTime;
        
        /// <summary>
        ///效果类型
        /// </summary>
        public int EffectType => config.EffectType;
        
        /// <summary>
        ///效果参数
        /// </summary>
        public List<string> EffectParam => config.EffectParam;
        
        /// <summary>
        ///最大链接实体数（0为所有全连）
        /// </summary>
        public int MaxLinkEntityCount => config.MaxLinkEntityCount;
        
        /// <summary>
        /// 是否链接释放者（自己）
        /// </summary>
        public int IsAddReleaser => config.IsAddReleaser;
        
        /// <summary>
        ///开始时候的技能效果列表
        /// </summary>
        public List<int> StartEffectList => config.StartEffectList;
        
    } 
}