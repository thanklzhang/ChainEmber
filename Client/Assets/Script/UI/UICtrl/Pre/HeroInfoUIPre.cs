// using System;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UI;
//
// public class HeroInfoUIPre : BaseUI
// {
//     public Action onCloseClickEvent;
//
//     Button closeBtn;
//     protected override void OnInit()
//     {
//         closeBtn = this.transform.Find("closeBtn").GetComponent<Button>();
//
//         closeBtn.onClick.AddListener(() =>
//         {
//             onCloseClickEvent?.Invoke();
//         });
//     }
// }
//
