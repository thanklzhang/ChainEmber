// using System.Collections;
// using System.Collections.Generic;
// using Battle;
// using UnityEngine;
// using Quaternion = UnityEngine.Quaternion;
// using Vector3 = UnityEngine.Vector3;
//
// namespace Battle_Client
// {
//     public class UnderstudyLocation_Client
//     {
//         public GameObject gameObject;
//         public Transform transform;
//
//         public BattleEntity_Client entity;
//
//         public void Init(GameObject gameObject)
//         {
//             this.gameObject = gameObject;
//             this.transform = this.gameObject.transform;
//         }
//
//         public void SetEntity(BattleEntity_Client entity)
//         {
//             this.entity = entity;
//             if (this.entity != null)
//             {
//                 var targetPos = transform.position;
//                 var resultPos = new Vector3(targetPos.x, this.entity.GetPosition().y
//                     , targetPos.z);
//                 this.entity.SetPosition(resultPos);
//             }
//             else
//             {
//                 //离开替补
//             }
//         }
//     }
// }