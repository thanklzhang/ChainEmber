// using GameData;
// using Google.Protobuf.Collections;
// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Text;
// using System.Threading.Tasks;
// using UnityEngine;
// using ServerSimulation.Account.Models;
//
// namespace GameData
// {
//
//
//     public class HeroGameData : BaseGameData
//     {
//         //public int flag;//待定
//         List<HeroData> heroList = new List<HeroData>();
//         Dictionary<int, HeroData> heroDic = new Dictionary<int, HeroData>();
//
//         public List<HeroData> HeroList { get => heroList; set => heroList = value; }
//         public Dictionary<int, HeroData> HeroDic { get => heroDic; }
//
//         //public void SetHeroDataList(List<HeroData> heroList)
//         //{
//         //    //增删改查
//         //    this.heroList = heroList;
//         //    this.heroDic = this.heroList.ToDictionary(hero => hero.id);
//         //}
//
//         public void SetHeroDataList(RepeatedField<NetProto.HeroProto> heroList)//List<HeroData> heroList
//         {
//             foreach (var serverHero in heroList)
//             {
//                 UpdateOneHero(serverHero);
//             }
//
//             this.heroDic = this.heroList.ToDictionary(hero => hero.guid);
//
//             EventDispatcher.Broadcast(EventIDs.OnRefreshHeroListData);
//         }
//
//         /// <summary>
//         /// 从后台数据同步英雄列表到客户端
//         /// </summary>
//         /// <param name="serverHeroList">后台英雄数据列表</param>
//         public void SyncHeroDataFromBackend(List<UserHero> serverHeroList)
//         {
//             // 清空现有英雄列表
//             this.heroList.Clear();
//             this.heroDic.Clear();
//
//             if (serverHeroList == null || serverHeroList.Count == 0)
//             {
//                 Debug.Log("[HeroGameData] 没有英雄数据需要同步");
//                 return;
//             }
//
//             int heroCount = 0;
//
//             foreach (var serverHero in serverHeroList)
//             {
//                 if (serverHero == null)
//                     continue;
//
//                 // 创建客户端英雄数据
//                 HeroData heroData = new HeroData
//                 {
//                     guid = serverHero.Guid, // 直接使用int类型的HeroId
//                     configId = serverHero.ConfigId,
//                     level = serverHero.Level
//                 };
//
//                 this.heroList.Add(heroData);
//                 this.heroDic[heroData.guid] = heroData;
//                 heroCount++;
//             }
//
//             Debug.Log($"[HeroGameData] 已同步 {heroCount} 个英雄数据");
//
//             // 触发英雄列表刷新事件
//             EventDispatcher.Broadcast(EventIDs.OnRefreshHeroListData);
//         }
//
//         public void UpdateOneHero(NetProto.HeroProto serverHero)
//         {
//             var currLocalHero = this.GetDataByGuid(serverHero.Guid);
//             if (null == currLocalHero)
//             {
//                 currLocalHero = new HeroData();
//
//                 currLocalHero = HeroConvert.ToHeroData(serverHero);
//                 this.heroList.Add(currLocalHero);
//                 this.heroDic.Add(serverHero.Guid, currLocalHero);
//                 //add
//             }
//             else
//             {
//                 //update
//
//                 currLocalHero.guid = serverHero.Guid;
//                 currLocalHero.configId = serverHero.ConfigId;
//                 currLocalHero.level = serverHero.Level;
//             }
//
//
//
//         }
//
//         public HeroData GetDataByGuid(int guid)
//         {
//             if (HeroDic.ContainsKey(guid))
//             {
//                 return HeroDic[guid];
//             }
//             else
//             {
//                 return null;
//             }
//
//         }
//
//         public HeroData GetDataByConfigId(int configId)
//         {
//             foreach (var hero in HeroList)
//             {
//                 if (hero.configId == configId)
//                 {
//                     return hero;
//                 }
//             }
//             return null;
//         }
//        
//     }
// }