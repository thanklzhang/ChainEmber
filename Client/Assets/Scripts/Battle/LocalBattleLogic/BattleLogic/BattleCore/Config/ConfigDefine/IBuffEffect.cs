/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
namespace Battle
{
    
    
 
    public interface IBuffEffect : IConfig
    {
        
        /// <summary>
        ///技能名称
        /// </summary>
        string Name {get;} 
        
        /// <summary>
        ///buff介绍
        /// </summary>
        string Describe {get;} 
        
        /// <summary>
        ///图标资源
        /// </summary>
        int IconResId {get;} 
        
        /// <summary>
        ///施加效果的目标类型
        /// </summary>
        int EffectTargetType {get;} 
        
        /// <summary>
        ///异常状态类型列表(,分割)
        /// </summary>
        List<int> AbnormalStateTypeList {get;} 
        
        /// <summary>
        ///持续时间(ms)
        /// </summary>
        int LastTime {get;} 
        
        /// <summary>
        ///是否能够被驱散
        /// </summary>
        int IsCanBeClear {get;} 
        
        /// <summary>
        ///影响类型
        /// </summary>
        int AffectType {get;} 
        
        /// <summary>
        ///是否死亡时候删除
        /// </summary>
        int IsDeleteOnDead {get;} 
        
        /// <summary>
        ///初始层数
        /// </summary>
        int InitLayerCount {get;} 
        
        /// <summary>
        ///满层数
        /// </summary>
        int MaxLayerCount {get;} 
        
        /// <summary>
        ///叠层类型
        /// </summary>
        int AddLayerType {get;} 
        
        /// <summary>
        ///是否满层移除
        /// </summary>
        int IsMaxLayerRemove {get;} 
        
        /// <summary>
        ///满层触发效果列表
        /// </summary>
        List<int> MaxLayerTriggerEffectList {get;} 
        
        /// <summary>
        ///满层触发时施加效果的目标类型
        /// </summary>
        int MaxLayerTriggerTargetType {get;} 
        
        /// <summary>
        ///效果间隔时间(ms) 
        /// </summary>
        int IntervalTime {get;} 
        
        /// <summary>
        ///持续时间附加组(ms)
        /// </summary>
        List<List<int>> LastTimeAddedGroup {get;} 
        
        /// <summary>
        ///开始的时候触发的效果列表
        /// </summary>
        List<int> StartEffectList {get;} 
        
        /// <summary>
        ///间隔触发的效果列表
        /// </summary>
        List<int> IntervalEffectList {get;} 
        
        /// <summary>
        ///结束的时候触发的效果列表
        /// </summary>
        List<int> EndEffectList {get;} 
        
        /// <summary>
        ///增加的属性组配置Id
        /// </summary>
        int AttrGroupConfigId {get;} 
        
        /// <summary>
        ///结束的时候移除的效果列表
        /// </summary>
        List<int> EndRemoveEffectList {get;} 
        
        /// <summary>
        ///普通攻击附加伤害比值
        /// </summary>
        List<int> NormalAttackAddedEffectIds {get;} 
        
        /// <summary>
        ///效果资源id
        /// </summary>
        int EffectResId {get;} 
        
        /// <summary>
        ///显示类型
        /// </summary>
        int ShowType {get;} 
        
    } 
}