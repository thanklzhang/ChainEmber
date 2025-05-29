// using System;
// using UnityEngine;
// using UnityEngine.Serialization;
//
// namespace ServerSimulation.Account.Models
// {
//     [Serializable]
//     public class UserHero
//     {
//         [FormerlySerializedAs("HeroId")] public int Guid;        // 英雄唯一ID，改为int类型
//         public int ConfigId;       // 英雄配置ID，对应配置表中的英雄
//         public int Level;          // 英雄等级
//         public int Star;           // 英雄星级
//         public int Experience;     // 英雄经验值
//         public DateTime ObtainTime;// 获取时间
//         
//         // 空构造函数，用于反序列化
//         public UserHero() 
//         {
//         }
//         
//         public UserHero(int configId, int level = 1, int star = 1)
//         {
//             // 生成一个唯一的int类型ID
//             Guid = GenerateUniqueId();
//             ConfigId = configId;
//             Level = level;
//             Star = star;
//             Experience = 0;
//             ObtainTime = DateTime.Now;
//         }
//         
//         // 生成唯一的int类型ID
//         private int GenerateUniqueId()
//         {
//             // 使用GUID的哈希码作为基础，保证唯一性
//             string guid = System.Guid.NewGuid().ToString();
//             int hash = guid.GetHashCode();
//             return Math.Abs(hash); // 取绝对值避免负数
//         }
//     }
// } 