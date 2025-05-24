using System.Collections.Generic;
using Battle_Client;
using GameData;
using TMPro;
using UnityEngine;

//操作实体装备的实体组界面
public class OpEntityItemGroup
{
    public GameObject gameObject;
    public Transform transform;

    private BattleHeroAvatar avatar;
    private TextMeshProUGUI heroNameText;

    protected Transform itemListRoot;

    private List<OpEntitiesItemUIShowObj> itemShowObjList;

    private BattleUI battleUI;

    public void Init(GameObject gameObject, BattleUI battleUI)
    {
        this.gameObject = gameObject;
        this.transform = this.gameObject.transform;
        this.battleUI = battleUI;


        //填充 avatar
        var avatarRootGo = this.transform.Find("BattleHeroAvatar").gameObject;
        avatar = new BattleHeroAvatar();
        avatar.Init(avatarRootGo);

        heroNameText = this.transform.Find("nameText").GetComponent<TextMeshProUGUI>();

        itemListRoot = transform.Find("itemRoot");

        itemShowObjList = new List<OpEntitiesItemUIShowObj>();
    }

    public BattleEntity_Client entity;
    public int index;
    public void RefreshUI(int entityGuid, int index)
    {
        this.index = index;
        entity = BattleEntityManager.Instance.FindEntity(entityGuid);

        avatar.Refresh(new BattleHeroShowData()
        {
            configId = entity.configId,
            level = entity.level,
            star = entity.starLv
        });

        var info = Config.ConfigManager.Instance.GetById<Config.EntityInfo>(entity.configId);
        heroNameText.text = info.Name;

        RefreshAllItemsShow();
    }

    public void RefreshItem(BattleItemData_Client itemData,int index)
    {
        var cell = itemShowObjList.Find(item => item.index == index);
        cell.RefreshUI(itemData,index);
    }

    void RefreshAllItemsShow()
    {
        for (int i = 0; i < itemShowObjList.Count; i++)
        {
            var showObj = itemShowObjList[i];
            showObj.Release();
        }

        itemShowObjList.Clear();

        var dataList = entity.GetItemBarCells();

        for (int i = 0; i < dataList.Count; i++)
        {
            var data = dataList[i];

            var itemData = dataList[i].itemData;
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

            var showObj = new OpEntitiesItemUIShowObj();
            showObj.Init(go, this.battleUI,this.entity.guid);
            showObj.RefreshUI(itemData, i,data.isUnlock);
            showObj.gameObject.SetActive(true);

            itemShowObjList.Add(showObj);
        }

        for (int i = dataList.Count; i < this.itemListRoot.childCount; i++)
        {
            this.itemListRoot.GetChild(i).gameObject.SetActive(false);
        }
    }

    public void Update(float deltaTime)
    {
    }


    public virtual void Release()
    {
        this.avatar.Release();
        for (int i = 0; i < itemShowObjList.Count; i++)
        {
            var itemShowObj = itemShowObjList[i];
            itemShowObj.Release();
        }
    }
}