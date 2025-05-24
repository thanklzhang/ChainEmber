/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
using Config;
namespace Battle
{
    
    
 
    public class BattleSkillItemDraw_Impl : IBattleSkillItemDraw
    {
        private Config.BattleSkillItemDraw config;
        
        public void Init(int id)
        {
            config = ConfigManager.Instance.GetById<Config.BattleSkillItemDraw>(id);
        }
        
        public int Id => config.Id;
        
        /// <summary>
        ///名称
        /// </summary>
        public string Name => config.Name;
        
        /// <summary>
        ///介绍
        /// </summary>
        public string Describe => config.Describe;
        
        /// <summary>
        ///图片资源id
        /// </summary>
        public int IconResId => config.IconResId;
        
        /// <summary>
        ///消耗道具id（，分割）
        /// </summary>
        public List<int> CostItemIds => config.CostItemIds;
        
        /// <summary>
        ///消耗道具数目（，分割）
        /// </summary>
        public List<int> CostItemCounts => config.CostItemCounts;
        
        /// <summary>
        ///抽取权重（，分割 蓝紫橙红）
        /// </summary>
        public List<int> DrawWeights => config.DrawWeights;
        
        /// <summary>
        ///抽取值
        /// </summary>
        public List<int> DrawValues => config.DrawValues;
        
        /// <summary>
        ///蓝色卡池ids（不填则是所有蓝色卡池）
        /// </summary>
        public List<int> Pool1ItemIds => config.Pool1ItemIds;
        
        /// <summary>
        ///蓝色卡池权重（不填的话就是都相同）
        /// </summary>
        public List<int> Pool1ItemWeights => config.Pool1ItemWeights;
        
        /// <summary>
        ///紫色卡池ids
        /// </summary>
        public List<int> Pool2ItemIds => config.Pool2ItemIds;
        
        /// <summary>
        ///紫色卡池权重
        /// </summary>
        public List<int> Pool2ItemWeights => config.Pool2ItemWeights;
        
        /// <summary>
        ///橙色卡池ids
        /// </summary>
        public List<int> Pool3ItemIds => config.Pool3ItemIds;
        
        /// <summary>
        ///橙色卡池权重
        /// </summary>
        public List<int> Pool3ItemWeights => config.Pool3ItemWeights;
        
        /// <summary>
        ///红色卡池ids
        /// </summary>
        public List<int> Pool4ItemIds => config.Pool4ItemIds;
        
        /// <summary>
        ///红色卡池权重
        /// </summary>
        public List<int> Pool4ItemWeights => config.Pool4ItemWeights;
        
    } 
}