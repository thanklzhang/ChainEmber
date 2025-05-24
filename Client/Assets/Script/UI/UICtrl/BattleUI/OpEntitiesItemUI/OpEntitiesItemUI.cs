using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Battle;
using Battle_Client;
using UnityEngine;
using UnityEngine.UI;

//操作实体的道具界面
public class OpEntitiesItemUI
{
    public GameObject gameObject;
    public Transform transform;

    Transform groupRoot;

    // private Button closeBtn;

    private List<OpEntityItemGroup> uiShowList;

    public BattleUI battleUI;

    public Button closeBtn;

    public void Init(GameObject gameObject, BattleUI battleUI)
    {
        this.gameObject = gameObject;
        this.transform = this.gameObject.transform;

        this.battleUI = battleUI;
        groupRoot = this.transform.Find("root/scroll/mask/content");

        closeBtn = this.transform.Find("root/closeBtn").GetComponent<Button>();
        closeBtn.onClick.AddListener(() => { this.Close(); });

        uiShowList = new List<OpEntityItemGroup>();

        // InitList();

        EventDispatcher.AddListener<BattleEntity_Client, List<ItemBarCellData_Client>>(EventIDs.OnEntityItemsUpdate,
            OnUpdateItemsData);
        EventDispatcher.AddListener<BattleEntity_Client, ItemBarCellData_Client>(EventIDs.OnEntityItemInfoUpdate,
            OnEntityItemInfoUpdate);
        EventDispatcher.AddListener<List<int>>(EventIDs.OnUpdatePlayerTeamMembersInfo,
            OnUpdatePlayerTeamMembersInfo);

        // EventDispatcher.AddListener<string>(EventIDs.OnItemTips,OnItemTips);
    }

    public void OnUpdateItemsData(BattleEntity_Client entity, List<ItemBarCellData_Client> barCellList)
    {
        // if (entity.guid == BattleManager.Instance.GetLocalCtrlHeroGuid())
        // {
        //     foreach (var barCell in barCellList)
        //     {
        //         var cell = FindShowItemCell(barCell.index);
        //         cell.RefreshUI(barCell.itemData, barCell.index);
        //     }
        // }
    }

    public void OnEntityItemInfoUpdate(BattleEntity_Client entity, ItemBarCellData_Client itemBarCell)
    {
        // if (entity.guid == BattleManager.Instance.GetLocalCtrlHeroGuid())
        // {
        //     var showObjCel = FindShowItemCell(itemBarCell.index);
        //     showObjCel.RefreshUI(itemBarCell.itemData, itemBarCell.index);
        // }

        var groupShow = FindEntityItemGroupInfo(entity.guid);
        if (groupShow != null)
        {
            groupShow.RefreshItem(itemBarCell.itemData, itemBarCell.index);
        }
    }

    public void Show()
    {
        this.gameObject.SetActive(true);
    }

    public void RefreshAllUI()
    {
        RefreshEntityItemInfoList();

        // var localHero = BattleManager.Instance.GetLocalCtrlHero();
        // var dataList = localHero.GetItemBarCells();
        //
        // for (int i = 0; i < dataList.Count; i++)
        // {
        //     var data = dataList[i].itemData;
        //     var showObj = this.uiShowList[i];
        //     showObj.RefreshUI(data, i);
        // }
    }

    public void RefreshEntityItemInfoList()
    {
        // var localHero = BattleManager.Instance.GetLocalCtrlHero();
        // var dataList = localHero.GetItemBarCells();

        var localPlayer = BattleManager.Instance.GetLocalPlayer();

        List<int> entityGuids = localPlayer.teamMemberGuids;

        entityGuids.RemoveAll(guid => null == BattleEntityManager.Instance.FindEntity(guid));

        entityGuids = entityGuids
            .OrderBy(x => x == BattleManager.Instance.GetLocalCtrlHeroGuid() ? 0 : 1)
            .ToList();


        foreach (var showObj in uiShowList)
        {
            showObj.Release();
        }

        uiShowList.Clear();

        for (int i = 0; i < entityGuids.Count; i++)
        {
            var entityGuid = entityGuids[i];
            GameObject go = null;
            if (i < this.groupRoot.childCount)
            {
                go = this.groupRoot.GetChild(i).gameObject;
            }
            else
            {
                go = GameObject.Instantiate(this.groupRoot.GetChild(0).gameObject,
                    this.groupRoot, false);
            }

            OpEntityItemGroup showObj = new OpEntityItemGroup();
            showObj.Init(go, this.battleUI);
            showObj.RefreshUI(entityGuid, i);
            showObj.gameObject.SetActive(true);

            uiShowList.Add(showObj);
        }

        for (int i = entityGuids.Count; i < this.groupRoot.childCount; i++)
        {
            this.groupRoot.GetChild(i).gameObject.SetActive(false);
        }
    }


    public void UpdateItemInfo(BattleItemData_Client itemInfo, int index)
    {
        // var itemShowObj = FindShowItemCell(index);
        // if (itemShowObj != null)
        // {
        //     itemShowObj.RefreshUI(itemInfo, index);
        // }
        // else
        // {
        //     //Logx.LogWarning("BattleSkillUI : UpdateSkillInfo : the skillId is not found : " + skillId);
        // }
    }

    public void SetItemTipText(string str)
    {
        // this.tipText.text = str;
        // this.tipText.gameObject.SetActive(true);
        // this.tipShowTimer = this.tipMaxShowTime;
    }

    public void Update(float deltaTime)
    {
    }

    public ItemCellUIShowObj FindShowItemCell(int index)
    {
        // foreach (var showObj in this.uiShowList)
        // {
        //     if (showObj.index == index)
        //     {
        //         return showObj;
        //     }
        // }

        return null;
    }

    public OpEntityItemGroup FindEntityItemGroupInfo(int entityGuid)
    {
        for (int i = 0; i < uiShowList.Count; i++)
        {
            var entityItemInfo = uiShowList[i];
            if (entityItemInfo.entity.guid == entityGuid)
            {
                return entityItemInfo;
            }
        }

        return null;
    }

    public void OnUpdatePlayerTeamMembersInfo(List<int> entityGuids)
    {
        this.RefreshAllUI();
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
        EventDispatcher.RemoveListener<BattleEntity_Client, List<ItemBarCellData_Client>>(EventIDs.OnEntityItemsUpdate,
            OnUpdateItemsData);
        EventDispatcher.RemoveListener<BattleEntity_Client, ItemBarCellData_Client>(EventIDs.OnEntityItemInfoUpdate,
            OnEntityItemInfoUpdate);
        EventDispatcher.RemoveListener<List<int>>(EventIDs.OnUpdatePlayerTeamMembersInfo,
            OnUpdatePlayerTeamMembersInfo);

        foreach (var item in this.uiShowList)
        {
            item.Release();
        }
    }
}