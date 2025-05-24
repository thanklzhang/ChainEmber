/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
namespace Config
{
    
    
    
       
    public class BuffEffect : BaseConfig
    {
        
        /// <summary>
        ///技能名称
        /// </summary>
        private string name; 
        
        /// <summary>
        ///buff介绍
        /// </summary>
        private string describe; 
        
        /// <summary>
        ///图标资源
        /// </summary>
        private int iconResId; 
        
        /// <summary>
        ///施加效果的目标类型
        /// </summary>
        private int effectTargetType; 
        
        /// <summary>
        ///异常状态类型列表(,分割)
        /// </summary>
        private List<int> abnormalStateTypeList; 
        
        /// <summary>
        ///持续时间(ms)
        /// </summary>
        private int lastTime; 
        
        /// <summary>
        ///是否能够被驱散
        /// </summary>
        private int isCanBeClear; 
        
        /// <summary>
        ///影响类型
        /// </summary>
        private int affectType; 
        
        /// <summary>
        ///是否死亡时候删除
        /// </summary>
        private int isDeleteOnDead; 
        
        /// <summary>
        ///初始层数
        /// </summary>
        private int initLayerCount; 
        
        /// <summary>
        ///满层数
        /// </summary>
        private int maxLayerCount; 
        
        /// <summary>
        ///叠层类型
        /// </summary>
        private int addLayerType; 
        
        /// <summary>
        ///是否满层移除
        /// </summary>
        private int isMaxLayerRemove; 
        
        /// <summary>
        ///满层触发效果列表
        /// </summary>
        private List<int> maxLayerTriggerEffectList; 
        
        /// <summary>
        ///满层触发时施加效果的目标类型
        /// </summary>
        private int maxLayerTriggerTargetType; 
        
        /// <summary>
        ///效果间隔时间(ms) 
        /// </summary>
        private int intervalTime; 
        
        /// <summary>
        ///持续时间附加组(ms)
        /// </summary>
        private List<List<int>> lastTimeAddedGroup; 
        
        /// <summary>
        ///开始的时候触发的效果列表
        /// </summary>
        private List<int> startEffectList; 
        
        /// <summary>
        ///间隔触发的效果列表
        /// </summary>
        private List<int> intervalEffectList; 
        
        /// <summary>
        ///结束的时候触发的效果列表
        /// </summary>
        private List<int> endEffectList; 
        
        /// <summary>
        ///增加的属性组配置Id
        /// </summary>
        private int attrGroupConfigId; 
        
        /// <summary>
        ///结束的时候移除的效果列表
        /// </summary>
        private List<int> endRemoveEffectList; 
        
        /// <summary>
        ///普通攻击附加伤害比值
        /// </summary>
        private List<int> normalAttackAddedEffectIds; 
        
        /// <summary>
        ///效果资源id
        /// </summary>
        private int effectResId; 
        
        /// <summary>
        ///显示类型
        /// </summary>
        private int showType; 
        

        
        public string Name { get => name; }     
        
        public string Describe { get => describe; }     
        
        public int IconResId { get => iconResId; }     
        
        public int EffectTargetType { get => effectTargetType; }     
        
        public List<int> AbnormalStateTypeList { get => abnormalStateTypeList; }     
        
        public int LastTime { get => lastTime; }     
        
        public int IsCanBeClear { get => isCanBeClear; }     
        
        public int AffectType { get => affectType; }     
        
        public int IsDeleteOnDead { get => isDeleteOnDead; }     
        
        public int InitLayerCount { get => initLayerCount; }     
        
        public int MaxLayerCount { get => maxLayerCount; }     
        
        public int AddLayerType { get => addLayerType; }     
        
        public int IsMaxLayerRemove { get => isMaxLayerRemove; }     
        
        public List<int> MaxLayerTriggerEffectList { get => maxLayerTriggerEffectList; }     
        
        public int MaxLayerTriggerTargetType { get => maxLayerTriggerTargetType; }     
        
        public int IntervalTime { get => intervalTime; }     
        
        public List<List<int>> LastTimeAddedGroup { get => lastTimeAddedGroup; }     
        
        public List<int> StartEffectList { get => startEffectList; }     
        
        public List<int> IntervalEffectList { get => intervalEffectList; }     
        
        public List<int> EndEffectList { get => endEffectList; }     
        
        public int AttrGroupConfigId { get => attrGroupConfigId; }     
        
        public List<int> EndRemoveEffectList { get => endRemoveEffectList; }     
        
        public List<int> NormalAttackAddedEffectIds { get => normalAttackAddedEffectIds; }     
        
        public int EffectResId { get => effectResId; }     
        
        public int ShowType { get => showType; }     
        

    } 
}