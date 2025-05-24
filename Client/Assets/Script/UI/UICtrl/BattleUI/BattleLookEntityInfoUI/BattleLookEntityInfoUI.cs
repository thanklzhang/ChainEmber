using Battle_Client;
using System;
using System.Collections.Generic;
using Battle;
using UnityEngine;
using UnityEngine.UI;


//选中实体的信息界面
public class BattleLookEntityInfoUI
{
    public GameObject gameObject;
    public Transform transform;

    public BattleUI battleUI;

    private LookAttrUI lookAttrUI;
    private LookBuffUI lookBuffUI;
    private LookItemUI lookItemUI;

    public void Init(GameObject gameObject, BattleUI battleUI)
    {
        this.battleUI = battleUI;
        this.gameObject = gameObject;
        this.transform = this.gameObject.transform;

        EventDispatcher.AddListener<BattleEntity_Client, int>(EventIDs.OnChangeEntityBattleData,
            OnChangeEntityBattleData);

        EventDispatcher.AddListener<BattleEntity_Client>(EventIDs.OnSelectEntity,
            OnSelectEntity);

        EventDispatcher.AddListener(EventIDs.OnCancelSelectEntity,
            OnCancelSelectEntity);

        EventDispatcher.AddListener<BuffEffectInfo_Client>(EventIDs.OnBuffInfoUpdate, OnBuffInfoUpdate);

        EventDispatcher.AddListener<BattleEntity_Client, List<ItemBarCellData_Client>>(EventIDs.OnEntityItemsUpdate,
            OnUpdateItemsData);
        
        EventDispatcher.AddListener<BattleEntity_Client, ItemBarCellData_Client>(EventIDs.OnEntityItemInfoUpdate,
            OnEntityItemInfoUpdate);
        
        


        lookAttrUI = new LookAttrUI();
        lookAttrUI.Init(this.transform.Find("lookAttrBar").gameObject, this.battleUI);

        lookBuffUI = new LookBuffUI();
        lookBuffUI.Init(this.transform.Find("lookBuffBar").gameObject, this.battleUI);

        lookItemUI = new LookItemUI();
        lookItemUI.Init(this.transform.Find("lookItemBar").gameObject, this.battleUI);
    }

    public void OnChangeEntityBattleData(BattleEntity_Client entity, int fromEntityGuid)
    {
        if (this.gameObject.activeSelf)
        {
            if (currShowEntity != null && currShowEntity.guid == entity.guid)
            {
                // RefreshAllUI();
                lookAttrUI.RefreshAllUI(currShowEntity);
            }
        }
    }

    public void OnSelectEntity(BattleEntity_Client entity)
    {
        currShowEntity = entity;
        this.Show();
        this.RefreshAllUI();
    }

    public void OnCancelSelectEntity()
    {
        this.Hide();
    }

    public void Show()
    {
        this.gameObject.SetActive(true);
    }

    private BattleEntity_Client currShowEntity;

    public void RefreshAllUI()
    {
        lookAttrUI.RefreshAllUI(currShowEntity);
        lookBuffUI.RefreshAllUI(currShowEntity);
        lookItemUI.RefreshAllUI(currShowEntity);
    }

    public void Update(float deltaTime)
    {
        this.lookAttrUI.Update(deltaTime);
        this.lookBuffUI.Update(deltaTime);
        this.lookItemUI.Update(deltaTime);
    }

    public void OnBuffInfoUpdate(BuffEffectInfo_Client buffInfo)
    {
        if (null == this.currShowEntity)
        {
            return;
        }

        if (this.currShowEntity.guid == buffInfo.targetEntityGuid)
        {
            lookBuffUI.UpdateBuff(buffInfo);
        }
    }


    public void OnUpdateItemsData(BattleEntity_Client entity, List<ItemBarCellData_Client> barCellList)
    {
        if (null == this.currShowEntity)
        {
            return;
        }

        if (this.currShowEntity.guid == entity.guid)
        {
            lookItemUI.RefreshAllUI(entity);
        }
    }

    public void OnEntityItemInfoUpdate(BattleEntity_Client entity, ItemBarCellData_Client itemBarCell)
    {
        if (null == this.currShowEntity)
        {
            return;
        }
        
        if (this.currShowEntity.guid == entity.guid)
        {
            lookItemUI.RefreshItem(entity,itemBarCell);
        }
    }

    public void Hide()
    {
        currShowEntity = null;
        this.gameObject.SetActive(false);
    }

    public void Close()
    {
        this.Hide();
    }

    public void Release()
    {
        EventDispatcher.RemoveListener<BattleEntity_Client, int>(EventIDs.OnChangeEntityBattleData,
            OnChangeEntityBattleData);
        EventDispatcher.RemoveListener<BattleEntity_Client>(EventIDs.OnSelectEntity,
            OnSelectEntity);
        EventDispatcher.RemoveListener(EventIDs.OnCancelSelectEntity,
            OnCancelSelectEntity);

        EventDispatcher.RemoveListener<BuffEffectInfo_Client>(EventIDs.OnBuffInfoUpdate, OnBuffInfoUpdate);

        EventDispatcher.RemoveListener<BattleEntity_Client, List<ItemBarCellData_Client>>(EventIDs.OnEntityItemsUpdate,
            OnUpdateItemsData);
        EventDispatcher.RemoveListener<BattleEntity_Client, ItemBarCellData_Client>(EventIDs.OnEntityItemInfoUpdate,
            OnEntityItemInfoUpdate);
        

        lookAttrUI.Release();
        lookBuffUI.Release();
        lookItemUI.Release();
    }
}