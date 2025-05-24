/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
namespace Battle
{
    
    
 
    public interface IBattleSkillItemDraw : IConfig
    {
        
        /// <summary>
        ///名称
        /// </summary>
        string Name {get;} 
        
        /// <summary>
        ///介绍
        /// </summary>
        string Describe {get;} 
        
        /// <summary>
        ///图片资源id
        /// </summary>
        int IconResId {get;} 
        
        /// <summary>
        ///消耗道具id（，分割）
        /// </summary>
        List<int> CostItemIds {get;} 
        
        /// <summary>
        ///消耗道具数目（，分割）
        /// </summary>
        List<int> CostItemCounts {get;} 
        
        /// <summary>
        ///抽取权重（，分割 蓝紫橙红）
        /// </summary>
        List<int> DrawWeights {get;} 
        
        /// <summary>
        ///抽取值
        /// </summary>
        List<int> DrawValues {get;} 
        
        /// <summary>
        ///蓝色卡池ids（不填则是所有蓝色卡池）
        /// </summary>
        List<int> Pool1ItemIds {get;} 
        
        /// <summary>
        ///蓝色卡池权重（不填的话就是都相同）
        /// </summary>
        List<int> Pool1ItemWeights {get;} 
        
        /// <summary>
        ///紫色卡池ids
        /// </summary>
        List<int> Pool2ItemIds {get;} 
        
        /// <summary>
        ///紫色卡池权重
        /// </summary>
        List<int> Pool2ItemWeights {get;} 
        
        /// <summary>
        ///橙色卡池ids
        /// </summary>
        List<int> Pool3ItemIds {get;} 
        
        /// <summary>
        ///橙色卡池权重
        /// </summary>
        List<int> Pool3ItemWeights {get;} 
        
        /// <summary>
        ///红色卡池ids
        /// </summary>
        List<int> Pool4ItemIds {get;} 
        
        /// <summary>
        ///红色卡池权重
        /// </summary>
        List<int> Pool4ItemWeights {get;} 
        
    } 
}