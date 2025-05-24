using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public enum GameState
//{
//    Null,
//    Login,
//    Lobby,
//    Room,
//    Hero,
//    Loading,
//    Combat,
//    GameOver,
//}
public abstract class BaseStateManager
{
    Dictionary<int, BaseState> stateDic = new Dictionary<int, BaseState>();
    public BaseState currState;
    
    public virtual void Init()
    {
        //stateDic.Add(GameState.Null, new GameBaseState());
        //stateDic.Add(GameState.Login, new LoginState());
        //stateDic.Add(GameState.Lobby, new LobbyState());
        //stateDic.Add(GameState.Combat, new CombatState());
        //stateDic = new Dictionary<int, BaseState>();
        foreach (var item in stateDic)
        {
            item.Value.Init();
        }


        //currState = stateDic[0];
    }

    protected void AddState(int stateKey, BaseState state)
    {
        if (this.stateDic.ContainsKey(stateKey))
        {
            Debug.LogError("the key is exist : " + stateKey);
            return;
        }
        this.stateDic.Add(stateKey,state);
    }

    public virtual void ChangeState(int nextState, params object[] args)
    {
        if (null == currState)
        {
            //一开始的状态
        }
        else
        {
            if (null == currState)
            {
                Debug.LogError("the currState is null : currState : " + currState);
                return;
            }
            currState.Exit();
        }

        if (!this.stateDic.ContainsKey(nextState))
        {
            Debug.LogError("the nextState key is not exist : " + nextState);
            return;
        }

        currState = stateDic[nextState];

        if (null == currState)
        {
            Debug.LogError("the currState is null : nextState : " + nextState);
            return;
        }

        currState.Enter(args);
    }

    public void Excute()
    {
        if (currState != null)
        {
            currState.Excute();
        }
    }
}
