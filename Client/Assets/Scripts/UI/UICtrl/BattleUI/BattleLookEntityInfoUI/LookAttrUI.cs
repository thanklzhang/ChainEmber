using Battle_Client;
using System;
using System.Collections;
using System.Collections.Generic;
using Battle;
using Config;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//查看选中实体的 属性 界面
public class LookAttrUI
{
    public GameObject gameObject;
    public Transform transform;

    // Button closeBtn;

    Transform attrListRoot;

    // List<BattleAttrUIData> attrDataList = new List<BattleAttrUIData>();
    List<AttrUIData> attrDataList = new List<AttrUIData>();

    
    // List<BattleAttrUIShowObj> attrShowObjList = new List<BattleAttrUIShowObj>();

    private List<AttrShowObj> attrShowObjList = new List<AttrShowObj>();
    
    public BattleUI BattleUI;

    public void Init(GameObject gameObject, BattleUI battleUI)
    {
        this.BattleUI = battleUI;
        this.gameObject = gameObject;
        this.transform = this.gameObject.transform;

        //closeBtn = this.transform.Find("closeBtn").GetComponent<Button>();
        attrListRoot = this.transform.Find("group");

        // closeBtn.onClick.AddListener(() =>
        // {
        //     this.Close();
        // });
        
        // EventDispatcher.AddListener<BattleEntity_Client, int>(EventIDs.OnChangeEntityBattleData,
        //     OnChangeEntityBattleData);
        

    }

    // public void OnChangeEntityBattleData(BattleEntity_Client entity,int fromEntityGuid)
    // {
    //     RefreshAllUI(entity);
    // }

    public void Show()
    {
        this.gameObject.SetActive(true);
    }

    public void RefreshAllUI(BattleEntity_Client entity)
    {
        RefreshDataList(entity);
        RefreshShowList();
    }
    
    // public void RemoveBuffFromDataList(int guid)
    // {
    //     for (int i = this.attrDataList.Count - 1; i >= 0; --i)
    //     {
    //         var buffData = this.attrDataList[i];
    //         if (buffData.guid == guid)
    //         {
    //             this.attrDataList.RemoveAt(i);
    //             return;
    //         }
    //     }
    // }
    //
    // public BuffShowObj FindBuff(int buffId)
    // {
    //     foreach (var buff in attrShowObjList)
    //     {
    //         if (buff.Guid == buffId)
    //         {
    //             return buff;
    //         }
    //     }
    //
    //     return null;
    // }
    //
    public void RefreshDataList(BattleEntity_Client entity)
    {
        attrDataList = new List<AttrUIData>();

        //var attr = BattleManager.Instance.GetLocalCtrlHeroAttrs();

        var attr = entity.attr;
        List<EntityAttrType> types = new List<EntityAttrType>()
        {
            EntityAttrType.Attack,
            EntityAttrType.Defence,
            EntityAttrType.MaxHealth,
            EntityAttrType.AttackSpeed,
            EntityAttrType.AttackRange,
            EntityAttrType.MoveSpeed,
        };

        for (int i = 0; i < types.Count; i++)
        {
            var attrType = types[i];
            float value = attr.GetValue(attrType);
            AttrUIData uiData = new AttrUIData()
            {
                type = attrType,
                value = value
            };
            attrDataList.Add(uiData);
        }
    }

    //用这个开始显示
    public void RefreshShowList()
    {
        attrShowObjList.Clear();

        for (int i = 0; i < attrDataList.Count; i++)
        {
            var data = attrDataList[i];
            GameObject go = null;
            if (i < this.attrListRoot.childCount)
            {
                go = this.attrListRoot.GetChild(i).gameObject;
            }
            else
            {
                go = GameObject.Instantiate(this.attrListRoot.GetChild(0).gameObject,
                    this.attrListRoot, false);
            }

            AttrShowObj cell = new AttrShowObj();
            cell.Init(go);
            cell.Show();
            cell.Refresh(data, i);

            attrShowObjList.Add(cell);
        }
        
        for (int i = attrDataList.Count; i < this.attrListRoot.childCount; i++)
        {
            var go = this.attrListRoot.GetChild(i).gameObject;
            go.SetActive(false);
        }
    }

    public void Update(float deltaTime)
    {
        foreach (var item in attrShowObjList)
        {
            item.Update(deltaTime);
        }
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
        
        // EventDispatcher.RemoveListener<BattleEntity_Client, int>(EventIDs.OnChangeEntityBattleData,
        //     OnChangeEntityBattleData);
        
        
        // closeBtn.onClick.RemoveAllListeners();

        foreach (var item in attrShowObjList)
        {
            item.Release();
        }

        attrShowObjList = null;
        attrDataList = null;

    }
}