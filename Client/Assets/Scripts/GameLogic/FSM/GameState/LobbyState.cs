using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyState : BaseState
{
    public override void Init()
    {
        this.state = (int)GameState.Lobby;
    }

    public override void Enter(params object[] args)
    {
        base.Enter();
        Debug.Log("enter lobby state");

        //CoroutineManager.Instance.StartCoroutine(StartLoad());

    }

    //IEnumerator StartLoad()
    //{
    //    ////loading ...
    //    //UIManager.Instance.ShowUI<LoadingUI>();



    //    //float progress = 1.0f;

    //    ////预加载资源
        

    //    //progress = 0.5f;

    //    ////加载场景
    //    ////之后这个 lobby_scene_001 会变成 resId
    //    //var isSceneLoadFinishFromAB = false;
    //    //AsyncOperation currOp = null;
    //    //ResourceManager.Instance.LoadScene("Scenes/lobby_scene_001", (isSuccess,op) =>
    //    //{
    //    //    //场景的 assetBundle 加载完成  此时 正在加载场景资源
    //    //    isSceneLoadFinishFromAB = true;
    //    //    currOp = op;
    //    //});



    //    //while (!isSceneLoadFinishFromAB)
    //    //{
    //    //    yield return null;
    //    //}

    //    //while (!currOp.isDone)
    //    //{
    //    //    Debug.Log("load scene (not from assetBundle) ... " + currOp.progress);
    //    //    yield return null;
    //    //}

    //    //progress = 1.0f;

    //    //OnLoadFinish();
    //}

    public void OnLoadFinish()
    {
        //UIManager.Instance.CloseUI();

        //UIManager.Instance.ShowUI<LobbyUI>();

        //var ip = LoginData.Instance.GateServerIp;
        //var port = LoginData.Instance.GateServerPort;
        //Debug.Log("connecting gate server ... : " + ip + ":" + port);
        //NetworkManager.Instance.ConnectGateServer(ip, port);

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
