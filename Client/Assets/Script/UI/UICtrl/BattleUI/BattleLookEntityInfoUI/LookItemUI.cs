using Battle_Client;
using System;
using System.Collections;
using System.Collections.Generic;
using Battle;
using Config;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//查看选中实体的 buff 界面
public class LookItemUI
{
    public GameObject gameObject;
    public Transform transform;

    // Button closeBtn;

    Transform itemListRoot;
    private List<ItemCellUIShowObj> uiShowList;

    public BattleUI BattleUI;

    public void Init(GameObject gameObject, BattleUI battleUIPre)
    {
        this.BattleUI = battleUIPre;

        this.gameObject = gameObject;
        this.transform = this.gameObject.transform;

        itemListRoot = this.transform.Find("group");
        uiShowList = new List<ItemCellUIShowObj>();

        InitList();
    }

    //初始化道具列表
    public void InitList()
    {
        var localHero = BattleManager.Instance.GetLocalCtrlHero();
        var dataList = localHero.GetItemBarCells();

        for (int i = 0; i < dataList.Count; i++)
        {
            var data = dataList[i].itemData;
            GameObject go = null;
            if (i < this.itemListRoot.childCount)
            {
                go = this.itemListRoot.GetChild(i).gameObject;
            }
            else
            {
                go = GameObject.Instantiate(this.itemListRoot.GetChild(0).gameObject,
                    this.itemListRoot, false);
            }

            ItemCellUIShowObj showObj = new ItemCellUIShowObj();
            showObj.Init(go, this.BattleUI, 0);
            showObj.RefreshUI(data, i);
            showObj.gameObject.SetActive(true);

            uiShowList.Add(showObj);
        }
        
        for (int i = dataList.Count; i < this.itemListRoot.childCount; i++)
        {
            this.itemListRoot.GetChild(i).gameObject.SetActive(false);
        }
    }

    public void Show()
    {
        this.gameObject.SetActive(true);
    }

    private BattleEntity_Client entity;

    public void RefreshAllUI(BattleEntity_Client entity)
    {
        this.entity = entity;
        UpdateAllItemShow();
    }

    public void UpdateAllItemShow()
    {
        var dataList = entity.GetItemBarCells();

        for (int i = 0; i < dataList.Count; i++)
        {
            var data = dataList[i];
            var itemData = data.itemData;
            var showObj = this.uiShowList[i];
            showObj.entityGuid = this.entity.guid;
            showObj.RefreshUI(itemData, i, data.isUnlock);
        }
    }

    public void RefreshItem(BattleEntity_Client entity, ItemBarCellData_Client cell)
    {
        for (int i = 0; i < this.uiShowList.Count; i++)
        {
            var showObj = uiShowList[i];
            if (showObj.index == cell.index)
            {
                showObj.RefreshUI(cell.itemData, i, cell.isUnlock);
            }
        }
    }


    public void Update(float deltaTime)
    {
        // foreach (var item in buffShowObjList)
        // {
        //     item.Update(deltaTime);
        // }
    }


    public void Hide()
    {
        this.gameObject.SetActive(false);
    }

    public void Close()
    {
        this.Hide();
    }

    public void Release()
    {
        foreach (var item in this.uiShowList)
        {
            item.Release();
        }
    }
}