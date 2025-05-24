// using System;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.SceneManagement;
// using UnityEngine.U2D;
// using UnityEngine.UI;
//
// public class LoadUIRequest<T> : LoadObjectRequest where T : BaseUI, new()
// {
//     public Action<T> selfFinishCallback;
//     bool isFinish;
//     public LoadUIRequest()
//     {
//
//     }
//
//     public override void Start()
//     {
//         UIManager.Instance.GetUICache<T>((ui) =>
//         {
//             selfFinishCallback?.Invoke(ui);
//             isFinish = true;
//         });
//     }
//
//     public override bool CheckFinish()
//     {
//
//         return isFinish;
//     }
//
//     public override void Finish()
//     {
//
//     }
//
//     public override void Unload()
//     {
//         UIManager.Instance.UnloadUI<T>();
//     }
//
// }