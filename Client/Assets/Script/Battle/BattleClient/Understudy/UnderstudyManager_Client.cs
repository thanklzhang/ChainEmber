// using System.Collections.Generic;
// using UnityEngine;
//
// namespace Battle_Client
// {
//     public partial class UnderstudyManager_Client : Singleton<UnderstudyManager_Client>
//     {
//         private int maxCount = 6;
//         private List<UnderstudyLocation_Client> locationList;
//         private Transform locationRoot;
//         private Transform posRoot;
//         public void Init(Transform locationRoot)
//         {
//             this.locationRoot = locationRoot;
//             posRoot = locationRoot.Find("posList");
//             locationList = new List<UnderstudyLocation_Client>();
//            
//             //根据场景填充
//             for (int i = 0; i < maxCount; i++)
//             {
//                 var newLoc = new UnderstudyLocation_Client();
//                 var posRoot = this.posRoot.GetChild(i);
//                 newLoc.Init(posRoot.gameObject);
//                 locationList.Add(newLoc);
//             }
//         }
//
//         public UnderstudyLocation_Client Get(int index)
//         {
//             return locationList[index];
//         }
//
//         public int GetLocationByEntityGuid(int guid)
//         {
//             return locationList.FindIndex(t => t.entity.guid == guid);
//         }
//
//         public void SetLocation(BattleEntity_Client entity,int index)
//         {
//             locationList[index].SetEntity(entity);
//         }
//
//         public void Release()
//         {
//             
//         }
//     }
// }