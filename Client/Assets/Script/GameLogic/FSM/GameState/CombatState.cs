using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatState : BaseState
{


    public override void Init()
    {
        this.state = (int)GameState.Combat;
    }

    //public override void Enter(params object[] args)
    //{
    //    base.Enter();
    //    Debug.Log("enter combat state");
    //    EventManager.AddListener((int)GameEvent.CombatStart, CombatStart);


    //    CoroutineManager.Instance.StartCoroutine(StartLoad());

    //}

    //IEnumerator StartLoad()
    //{

    //    //loading ...
    //    UIManager.Instance.ShowUI<LoadingUI>();



    //    float progress = 1.0f;

    //    //预加载资源


    //    progress = 0.5f;

    //    //加载场景
    //    //之后这个 lobby_scene_001 会变成 resId
    //    var isSceneLoadFinishFromAB = false;
    //    AsyncOperation currOp = null;
    //    ResourceManager.Instance.LoadScene("Scenes/combat_scene_001", (isSuccess, op) =>
    //    {
    //        //场景的 assetBundle 加载完成  此时 正在加载场景资源
    //        isSceneLoadFinishFromAB = true;
    //        currOp = op;
    //    });

    //    Debug.Log("load scene (from assetBundle)... 2");

    //    while (!isSceneLoadFinishFromAB)
    //    {
    //        yield return null;
    //    }

    //    Debug.Log("load scene (not from assetBundle) ... " + currOp.progress);
    //    while (!currOp.isDone)
    //    {

    //        yield return null;
    //    }

    //    //加载场景完成

    //    //加载角色 asset 先跳过 直接加载并实例化 之后加载和实例化分开
    //    Debug.Log("load hero objs start ...");
    //    var combatPlayers = CombatManager.Instance.GetCombatPlayers();
    //    foreach (var player in combatPlayers)
    //    {
    //        int i = 0;

    //        var posRoot = GameObject.Find("positions_" + i).transform;

    //        foreach (var heroPair in player.entityDic)
    //        {
    //            var heroGuid = heroPair.Key;
    //            var heroEntity = heroPair.Value;

    //            var modelId = heroEntity.config.combatModelId;
    //            Debug.Log("zxy : " + "load entity obj : " + modelId);
    //            ResourceManager.Instance.CreateGameObjectById(modelId, (isSuccess, obj) =>
    //            {
    //                SetEntityInfo(heroEntity, obj, posRoot, i);
    //            }, false);

    //            i += 1;
    //        }
    //    }

    //    Debug.Log("load hero objs finish...");

    //    progress = 1.0f;
    //    Debug.Log("load scene  ... finish");
    //    OnLoadFinish();
    //}

    //void SetEntityInfo(CombatEntity entity, GameObject obj, Transform root, int posIndex)
    //{
    //    entity.Init(obj);
    //    if (posIndex < root.childCount)
    //    {
    //        var currTra = root.GetChild(posIndex);
    //        entity.SetPosition(currTra.position);
    //        entity.SetRotation(currTra.rotation);
    //    }
    //    else
    //    {
    //        Debug.Log("zxy : " + "the posIndex is greater than root.childCount");
    //    }
      
    //}

    //public void OnLoadFinish()
    //{
    //    CombatNetHandler handler = NetHandlerManager.Instance.GetHandler<CombatNetHandler>();

    //    //假设在这预加载完 应该是场景转换的时候加载资源
    //    handler.ReqPlayerLoadCombatFinish(() =>
    //    {
    //        Debug.Log("req load combat finish success ");
    //    });

    //}


    //void CombatStart()
    //{
    //    //loading UI 可以独立出来
    //    UIManager.Instance.CloseUI();

    //    UIManager.Instance.ShowUI<CombatUI>();
    //}


    //public override void Excute()
    //{
    //    base.Excute();
    //    CombatShow.Instance.Update(Time.deltaTime);//这里之后可以注册到一个公用的地方
    //}

    //public override void Exit()
    //{
    //    base.Exit();
    //    EventManager.RemoveListener((int)GameEvent.CombatStart, CombatStart);

    //}
}
