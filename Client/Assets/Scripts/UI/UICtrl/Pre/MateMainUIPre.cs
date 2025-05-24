// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UI;
//
// public class MateMainUIPre : BaseUI
// {
//     //Text stateText;
//
//     //Button startMateBtn;
//     //Button cancelMateBtn;
//
//
//     //Button closeBtn;
//
//     //MateNetHandler mateNetHandler;
//
//     //public override void Init()
//     //{
//     //    this.resId = UIResIds.MateMainUI;
//     //}
//
//     //public override void LoadFinish(UIContext context)
//     //{
//     //    base.LoadFinish(context);
//
//     //    mateNetHandler = NetHandlerManager.Instance.GetHandler<MateNetHandler>();
//
//     //    stateText = root.Find("state_text").GetComponent<Text>();
//
//     //    startMateBtn = root.Find("start_mate_btn").GetComponent<Button>();
//     //    cancelMateBtn = root.Find("cancel_mate_btn").GetComponent<Button>();
//
//     //    closeBtn = root.Find("closeBtn").GetComponent<Button>();
//
//     //    closeBtn.onClick.AddListener(() =>
//     //    {
//     //        this.Close();
//     //    });
//
//     //    startMateBtn.onClick.AddListener(() =>
//     //    {
//     //        mateNetHandler.ReqStartMateCombat(() =>
//     //        {
//     //            Debug.Log("success to start to mate ...");
//
//     //        }, () =>
//     //         {
//     //            //服务端创建战斗初始信息成功 跳转到战斗界面
//     //            //这里需要更改 改成由事件触发 或者做成统一管理
//     //            GoToCombat();
//     //         });
//     //    });
//
//     //    cancelMateBtn.onClick.AddListener(() =>
//     //    {
//
//     //    });
//
//
//     //}
//
//     //void GoToCombat()
//     //{
//     //    //UIManager.Instance.ShowUI<CombatUI>();
//     //    GameStateManager.Instance.ChangeState(GameState.Combat);
//     //}
//
//     //void RefreshInfo()
//     //{
//
//     //}
//
//     //public override void Show()
//     //{
//     //    base.Show();
//     //    //var handler = NetHandlerManager.Instance.GetHandler<HeroListHandler>();
//     //    //handler.ReqServerHeroList(() =>
//     //    //{
//     //    //    RefreshInfo();
//     //    //});
//     //}
//
//     //public override void Hide()
//     //{
//     //    base.Hide();
//     //}
//
//
//
// }
