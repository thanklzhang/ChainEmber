using Battle_Client;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.U2D;
using UnityEngine.UI;

public class LoadBattleViewEntityRequest : LoadObjectRequest
{
    public Action<BattleEntity_Client, GameObject> selfFinishCallback;
    bool isFinish;

    BattleEntity_Client battleEntity;

    public LoadBattleViewEntityRequest(BattleEntity_Client battleEntity)
    {
        this.battleEntity = battleEntity;
    }

    public override void Start()
    {
        //Logx.Log("LoadBattleViewEntityRequest : start load");
        var path = battleEntity.path;

        ResourceManager.Instance.GetObject<GameObject>(path, (gameObject) =>
        {
            //viewEntity.OnLoadModelFinish(gameObject);
            this.OnFinishLoadEntityObj(battleEntity, gameObject);
        });
    }

    public void OnFinishLoadEntityObj(BattleEntity_Client viewEntity, GameObject obj)
    {
        //Logx.Log("LoadBattleViewEntityRequest : OnFinishLoadEntityObj : guid : " + viewEntity.guid);
        isFinish = true;
        selfFinishCallback?.Invoke(viewEntity, obj);
    }

    public override bool CheckFinish()
    {

        return isFinish;
    }

    public override void Finish()
    {

    }

    public override void Unload()
    {

    }

}