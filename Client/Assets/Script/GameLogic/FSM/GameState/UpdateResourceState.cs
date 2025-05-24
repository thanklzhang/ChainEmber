using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateResourceState : BaseState
{
    public override void Init()
    {
        this.state = (int)GameState.UpdateResource;
    }

    public override void Enter(params object[] args)
    {
        base.Enter();
        //Debug.Log("enter update resource state");

        //var finishCallback = (Action<bool>)args[0];
        ////更新资源等
        //GameObject updateResourceView = GlobalObject.Instance.gameStartupPrefab;
        //var updateObjView = GameObject.Instantiate(updateResourceView, GlobalObject.Instance.UIRoot);

        //UpdateResourceUI.Instance.Init(updateObjView);
        //UpdateResourceUI.Instance.Show();

        ////这里可设置进度等显示功能
        //UpdateResource.Instance.Start((info) =>
        //{
        //    UpdateResourceUI.Instance.UpdateState(info);
        //}, () =>
        //{
        //    UpdateResourceUI.Instance.Finish();
        //    //完成
        //    finishCallback?.Invoke(true);
        //});

    }



    public override void Excute()
    {
        base.Excute();
    }

    public override void Exit()
    {
        base.Exit();
    }
}
