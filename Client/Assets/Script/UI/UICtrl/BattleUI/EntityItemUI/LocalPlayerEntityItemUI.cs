using System;
using System.Collections;
using System.Collections.Generic;
using Battle;
using Battle_Client;
using UnityEngine;
using UnityEngine.UI;

//本地玩家实体的道具部分整体 UI
public class LocalPlayerEntityItemUI
{
    public GameObject gameObject;
    public Transform transform;

    Transform itemListRoot;

    // private Button closeBtn;

    private List<ItemCellUIShowObj> uiShowList;

    public BattleUI battleUI;

    public void Init(GameObject gameObject, BattleUI battleUI)
    {
        this.gameObject = gameObject;
        this.transform = this.gameObject.transform;

        this.battleUI = battleUI;
        itemListRoot = this.transform.Find("itemRoot");

        // closeBtn = this.transform.Find("root/closeBtn").GetComponent<Button>();
        // closeBtn.onClick.AddListener(() => { this.Close(); });

        uiShowList = new List<ItemCellUIShowObj>();

        InitList();

        EventDispatcher.AddListener<BattleEntity_Client, List<ItemBarCellData_Client>>(EventIDs.OnEntityItemsUpdate,
            OnUpdateItemsData);
        EventDispatcher.AddListener<BattleEntity_Client, ItemBarCellData_Client>(EventIDs.OnEntityItemInfoUpdate,
            OnEntityItemInfoUpdate);
        // EventDispatcher.AddListener<string>(EventIDs.OnItemTips,OnItemTips);
    }

    public void OnUpdateItemsData(BattleEntity_Client entity, List<ItemBarCellData_Client> barCellList)
    {
        if (entity.guid == BattleManager.Instance.GetLocalCtrlHeroGuid())
        {
            foreach (var barCell in barCellList)
            {
                var cell = FindShowItemCell(barCell.index);
                cell.RefreshUI(barCell.itemData, barCell.index, barCell.isUnlock);
            }
        }
    }

    public void OnEntityItemInfoUpdate(BattleEntity_Client entity, ItemBarCellData_Client itemBarCell)
    {
        if (entity.guid == BattleManager.Instance.GetLocalCtrlHeroGuid())
        {
            var showObjCel = FindShowItemCell(itemBarCell.index);
            showObjCel.RefreshUI(itemBarCell.itemData, itemBarCell.index, itemBarCell.isUnlock);
        }
    }

    void OnItemTips(string str)
    {
        this.SetItemTipText(str);
    }

    public void Show()
    {
        this.gameObject.SetActive(true);
    }

    //初始化道具列表
    public void InitList()
    {
        var localHero = BattleManager.Instance.GetLocalCtrlHero();
        var dataList = localHero.GetItemBarCells();

        for (int i = 0; i < dataList.Count; i++)
        {
            var data = dataList[i]; //.itemData;
            var itemData = data.itemData;
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

            ItemCellUIShowObj showObj = new LocalPlayerItemUIShowObj();
            showObj.Init(go, this.battleUI, localHero.guid);
            showObj.RefreshUI(itemData, i, data.isUnlock);
            showObj.gameObject.SetActive(true);

            uiShowList.Add(showObj);
        }


        for (int i = dataList.Count; i < this.itemListRoot.childCount; i++)
        {
            this.itemListRoot.GetChild(i).gameObject.SetActive(false);
        }
    }

    public void RefreshAllUI()
    {
        var localHero = BattleManager.Instance.GetLocalCtrlHero();
        var dataList = localHero.GetItemBarCells();

        for (int i = 0; i < dataList.Count; i++)
        {
            var data = dataList[i];
            var itemdData = dataList[i].itemData;
            var showObj = this.uiShowList[i];
            showObj.RefreshUI(itemdData, i, data.isUnlock);
        }
    }

    public void UpdateItemInfo(BattleItemData_Client itemInfo, int index)
    {
        var itemShowObj = FindShowItemCell(index);
        if (itemShowObj != null)
        {
            itemShowObj.RefreshUI(itemInfo, index);
        }
        else
        {
            //Logx.LogWarning("BattleSkillUI : UpdateSkillInfo : the skillId is not found : " + skillId);
        }
    }

    public void SetItemTipText(string str)
    {
        // this.tipText.text = str;
        // this.tipText.gameObject.SetActive(true);
        // this.tipShowTimer = this.tipMaxShowTime;
    }

    public void Update(float deltaTime)
    {
        // foreach (var item in itemShowObjList)
        // {
        //     item.Update(deltaTime);
        // }
        //
        // if (this.tipShowTimer > 0)
        // {
        //     this.tipShowTimer -= deltaTime;
        //     if (this.tipShowTimer <= 0)
        //     {
        //         this.tipText.gameObject.SetActive(false);
        //     }
        // }
    }

    public ItemCellUIShowObj FindShowItemCell(int index)
    {
        foreach (var showObj in this.uiShowList)
        {
            if (showObj.index == index)
            {
                return showObj;
            }
        }

        return null;
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
        // EventDispatcher.RemoveListener<BattleItemInfo>(EventIDs.OnItemInfoUpdate, OnItemInfoUpdate);
        // EventDispatcher.RemoveListener<string>(EventIDs.OnItemTips, OnItemTips);

        EventDispatcher.RemoveListener<BattleEntity_Client, List<ItemBarCellData_Client>>(EventIDs.OnEntityItemsUpdate,
            OnUpdateItemsData);
        EventDispatcher.RemoveListener<BattleEntity_Client, ItemBarCellData_Client>(EventIDs.OnEntityItemInfoUpdate,
            OnEntityItemInfoUpdate);

        foreach (var item in this.uiShowList)
        {
            item.Release();
        }

        // itemShowObjList = null;
        // itemDataList = null;
    }
}