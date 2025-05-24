using System;
using System.Collections;
using System.Collections.Generic;
using Battle;
using Battle_Client;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//玩家道具仓库
public class BattleItemWarehouseUI
{
    public GameObject gameObject;
    public Transform transform;

    Transform itemListRoot;

    private Button closeBtn;

    private List<WarehouseCellUIShowObj> uiShowList;

    public BattleUI battleUI;

    public void Init(GameObject gameObject, BattleUI battleUI)
    {
        this.gameObject = gameObject;
        this.transform = this.gameObject.transform;

        this.battleUI = battleUI;
        itemListRoot = this.transform.Find("root/scroll/mask/content");
        closeBtn = this.transform.Find("root/closeBtn").GetComponent<Button>();
        closeBtn.onClick.AddListener(() =>
        {
            this.Close();
            this.battleUI.OnWarehouseClose();
        });

        uiShowList = new List<WarehouseCellUIShowObj>();

        InitList();

        EventDispatcher.AddListener<WarehouseItemCellData_Client>(EventIDs.OnUpdateWarehouseItemData,
            OnUpdateWarehouseItemData);
        // EventDispatcher.AddListener<string>(EventIDs.OnItemTips,OnItemTips);
    }

    public void OnUpdateWarehouseItemData(WarehouseItemCellData_Client cellData)
    {
        var cell = FindItemCell(cellData.index);
        cell.RefreshUI(cellData.itemData, cellData.index, cellData.isUnlock);
    }

    public void OnItemInfoUpdate(BattleItemData_Client itemData)
    {
        //var entity = BattleEntityManager.Instance.FindEntity(entityGuid);

        var myEntityGuid = BattleManager.Instance.GetLocalCtrlHeroGuid();

        // if (myEntityGuid == itemInfo.ownerGuid)
        // {
        //     this.UpdateItemInfo(itemInfo);
        // }
    }

    void OnItemTips(string str)
    {
        this.SetItemTipText(str);
    }

    public void Show()
    {
        this.gameObject.SetActive(true);
    }

    //初始化仓库道具列表
    public void InitList()
    {
        var localPlayer = BattleManager.Instance.GetLocalPlayer();
        var dataList = localPlayer.itemWarehouse.itemCellList;

        for (int i = 0; i < dataList.Count; i++)
        {
            var cellData = dataList[i];
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

            WarehouseCellUIShowObj showObj = new WarehouseCellUIShowObj();
            showObj.Init(go, this.battleUI,0);
            showObj.RefreshUI(data, i, cellData.isUnlock);
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
        var localPlayer = BattleManager.Instance.GetLocalPlayer();
        var dataList = localPlayer.itemWarehouse.itemCellList;

        for (int i = 0; i < dataList.Count; i++)
        {
            var cellData = dataList[i];
            var data = dataList[i].itemData;
            var showObj = this.uiShowList[i];
            showObj.RefreshUI(data, i, cellData.isUnlock);
        }
    }

    public void UpdateItemInfo(BattleItemData_Client itemInfo, int index, bool isUnlock)
    {
        var itemShowObj = FindItemCell(index);
        if (itemShowObj != null)
        {
            itemShowObj.RefreshUI(itemInfo, index, isUnlock);
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

    public WarehouseCellUIShowObj FindItemCell(int index)
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

        EventDispatcher.RemoveListener<WarehouseItemCellData_Client>(EventIDs.OnUpdateWarehouseItemData,
            OnUpdateWarehouseItemData);

        foreach (var item in this.uiShowList)
        {
            item.Release();
        }

        // itemShowObjList = null;
        // itemDataList = null;
    }

    // //仓库间拖动
    // public void OnItemEndDrag(WarehouseItemUIShowObj warehouseItemUIShowObj, PointerEventData eventData)
    // {
    //     foreach (var showObj in this.uiShowList)
    //     {
    //         var rectTran = showObj.transform.GetComponent<RectTransform>();
    //
    //         var parentRectTran = showObj.transform.parent.GetComponent<RectTransform>();
    //         var uiCamera = CameraManager.Instance.GetCameraUI().camera;
    //         RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTran, eventData.position, uiCamera,
    //             out var outPos);
    //
    //         if (rectTran.rect.Contains(outPos))
    //         {
    //             // BattleItemData_Client data = new BattleItemData_Client();
    //             // data.configId = warehouseItemUIShowObj.data.configId;
    //             // data.count = warehouseItemUIShowObj.data.count;
    //             // showObj.RefreshUI(data,showObj.index);
    //
    //             ItemMoveArg srcMoveArg = new ItemMoveArg();
    //             srcMoveArg.locationType = ItemLocationType.Warehouse;
    //             srcMoveArg.itemIndex = warehouseItemUIShowObj.index;
    //             
    //             ItemMoveArg desMoveArg = new ItemMoveArg();
    //             desMoveArg.locationType = ItemLocationType.Warehouse;
    //             desMoveArg.itemIndex = showObj.index;
    //             
    //             
    //             BattleManager.Instance.MsgSender.Send_MoveItemTo(srcMoveArg,desMoveArg);
    //             
    //             break;
    //         }
    //     }
    // }
}