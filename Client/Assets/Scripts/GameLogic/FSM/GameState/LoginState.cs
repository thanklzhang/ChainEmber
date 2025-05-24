using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginState : BaseState
{
    public override void Init()
    {
        this.state = (int)GameState.Login;
    }

    public override void Enter(params object[] args)
    {
        base.Enter();
        //Debug.Log("enter login state");
        
        //UIManager.Instance.ShowUI<LoginUI>();
       
        //NetworkManager.Instance.ConnectLoginServer();
        
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
