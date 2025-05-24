using Battle_Client;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//英雄状态 UI 管理 包括血条等
public class HeroStateUIMgr
{
    public Transform transform;
    public GameObject gameObject;

    BattleUI _battleUICtrl;
    
    public Transform hpRoot;
    public GameObject stateTemp;

    Dictionary<int, HeroStateShowObj> stateShowObjDic = new Dictionary<int, HeroStateShowObj>();

    public void Init(GameObject gameObject, BattleUI ctrl)
    {
        this.gameObject = gameObject;
        this.transform = this.gameObject.transform;

        this._battleUICtrl = ctrl;

        hpRoot = this.transform.Find("root");
        stateTemp = hpRoot.Find("stateItem").gameObject;
        stateTemp.SetActive(false);

        EventDispatcher.AddListener<BattleEntity_Client>(EventIDs.OnCreateEntity, OnCreateEntity);
        EventDispatcher.AddListener<BattleEntity_Client>(EventIDs.OnEntityDestroy, OnEntityDestroy);
        EventDispatcher.AddListener<BattleEntity_Client, int>(EventIDs.OnChangeEntityBattleData,
            OnChangeEntityBattleData);
        EventDispatcher.AddListener<BattleEntity_Client, bool>(EventIDs.OnEntityChangeShowState,
            this.OnEntityChangeShowState);
    }


    public void OnCreateEntity(BattleEntity_Client entity)
    {
        RefreshStateShow(entity, 0);
    }

    public void OnEntityDestroy(BattleEntity_Client entity)
    {
        DestoryHpUI(entity);
    }

    public void OnChangeEntityBattleData(BattleEntity_Client entity, int fromEntityGuid)
    {
        this.RefreshStateShow(entity, fromEntityGuid);
    }

    public void OnEntityChangeShowState(BattleEntity_Client entity, bool isShow)
    {
        this.SetShowState(entity, isShow);
    }


    public void RefreshStateShow(BattleEntity_Client entity, int damageFromEntityGuid)
    {
        HeroStateShowObj showObj = null;
        if (stateShowObjDic.ContainsKey(entity.guid))
        {
            showObj = stateShowObjDic[entity.guid];
        }
        else
        {
            showObj = new HeroStateShowObj();

            GameObject newObj = GameObject.Instantiate(stateTemp, this.hpRoot, false);
            newObj.SetActive(true);
            showObj.Init(newObj, entity,this);

            stateShowObjDic.Add(entity.guid, showObj);
        }

        showObj.Refresh(entity, damageFromEntityGuid);
    }

    public void DestoryHpUI(BattleEntity_Client entity)
    {
        var entityGuid = entity.guid;
        if (stateShowObjDic.ContainsKey(entityGuid))
        {
            var hpShowObj = stateShowObjDic[entityGuid];
            hpShowObj.Destroy();
            stateShowObjDic.Remove(entityGuid);
        }
        else
        {
            Logx.LogWarning(LogxType.UI, "BattleUI DestoryHpUI : the entityGuid is not found : " + entityGuid);
        }
    }

    public HeroStateShowObj FindStateShow(int entityGuid)
    {
        HeroStateShowObj showObj = null;
        if (stateShowObjDic.ContainsKey(entityGuid))
        {
            showObj = stateShowObjDic[entityGuid];
        }
        else
        {
            Logx.LogWarning(LogxType.UI, "BattleUI DestoryHpUI : the entityGuid is not found : " + entityGuid);
        }

        return showObj;
    }

    public void SetShowState(BattleEntity_Client entity, bool isShow)
    {
        var entityGuid = entity.guid;
        var stateUI = FindStateShow(entityGuid);
        if (stateUI != null)
        {
            stateUI.SetShowState(isShow);
        }
    }

    public void ShowFloatWord(FloatWordBean bean)
    {
        this._battleUICtrl.ShowFloatWord(bean);
    }


    public void Update(float timeDelta)
    {
        foreach (var item in stateShowObjDic)
        {
            item.Value.Update(timeDelta);
        }
    }

    public void Release()
    {
        EventDispatcher.RemoveListener<BattleEntity_Client>(EventIDs.OnCreateEntity, OnCreateEntity);
        EventDispatcher.RemoveListener<BattleEntity_Client>(EventIDs.OnEntityDestroy, OnEntityDestroy);
        EventDispatcher.RemoveListener<BattleEntity_Client,int>(EventIDs.OnChangeEntityBattleData,
            OnChangeEntityBattleData);

        EventDispatcher.RemoveListener<BattleEntity_Client, bool>(EventIDs.OnEntityChangeShowState,
            this.OnEntityChangeShowState);
    }
}