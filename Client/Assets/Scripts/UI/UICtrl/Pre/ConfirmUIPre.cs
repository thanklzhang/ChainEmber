// using System;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UI;
//
// public class ConfirmUIPre : BaseUI
// {
//     public Action onCloseClickEvent;
//     public Action onClickYesBtn;
//     public Action onClickNoBtn;
//
//     Button closeBtn;
//     Button yesBtn;
//     Button noBtn;
//     protected override void OnInit()
//     {
//         closeBtn = this.transform.Find("closeBtn").GetComponent<Button>();
//         yesBtn = this.transform.Find("yesBtn").GetComponent<Button>();
//         noBtn = this.transform.Find("noBtn").GetComponent<Button>();
//
//         closeBtn.onClick.AddListener(() =>
//         {
//             onCloseClickEvent?.Invoke();
//         });
//         yesBtn.onClick.AddListener(() =>
//         {
//             onClickYesBtn?.Invoke();
//         });
//         noBtn.onClick.AddListener(() =>
//         {
//             onClickNoBtn?.Invoke();
//         });
//     }
// }
//
