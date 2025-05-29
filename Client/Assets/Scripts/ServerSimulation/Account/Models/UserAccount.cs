// using System;
// using UnityEngine;
//
// namespace ServerSimulation.Account.Models
// {
//     [Serializable]
//     public class UserAccount
//     {
//         public string Username;
//         public string Password; // 实际应用中应该是加密的哈希值
//         public string UserId;
//         public DateTime RegistrationDate;
//         public DateTime LastLoginDate;
//         
//         // 空构造函数，用于反序列化
//         public UserAccount() 
//         {
//         }
//         
//         public UserAccount(string username, string password)
//         {
//             Username = username;
//             Password = password;
//             UserId = Guid.NewGuid().ToString(); // 生成唯一ID
//             RegistrationDate = DateTime.Now;
//             LastLoginDate = DateTime.Now;
//         }
//     }
// } 