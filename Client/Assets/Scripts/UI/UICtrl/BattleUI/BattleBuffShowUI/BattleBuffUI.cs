using System;
using System.Collections;
using System.Collections.Generic;
using Battle_Client;
using UnityEngine;
using UnityEngine.UI;

public class BattleBuffUI
{
    public GameObject gameObject;
    public Transform transform;

    Transform buffListRoot;

    //Text skillTipText;
    //float skillTipShowTimer;
    //float skillTipMaxShowTime = 1.6f;
    public BattleUI BattleUIPre;
    //List<BuffEffectInfo_Client> buffDataList = new List<BuffEffectInfo_Client>();
    List<BuffShowObj> buffShowObjList = new List<BuffShowObj>();

    private GameObject itemTemp;
    public void Init(GameObject gameObject, BattleUI battleUIPre)
    {
        this.gameObject = gameObject;
        this.transform = this.gameObject.transform;

        buffListRoot = this.transform.Find("group");

        this.BattleUIPre = battleUIPre;
        
        itemTemp = this.transform.Find("item").gameObject;
        itemTemp.SetActive(false);

        // buffDataList = new List<BuffEffectInfo_Client>();
        //this.skillTipText = this.transform.Find("skillTipText").GetComponent<Text>();

        EventDispatcher.AddListener<BuffEffectInfo_Client>(EventIDs.OnBuffInfoUpdate, OnBuffInfoUpdate);

        InitShowList();
    }

    public void Show()
    {
        this.gameObject.SetActive(true);
    }

    public void RefreshAllUI()
    {
        // BattleBuffUIArgs uiDataArgs = (BattleBuffUIArgs)args;

        // this.buffDataList = uiDataArgs.battleBuffList;
        // if (null == this.buffDataList)
        // {
        //     this.buffDataList = new List<BuffEffectInfo_Client>();
        // }

        //fill data
        
        
        // this.RefreshBuffList();
    }

    // void RefreshBuffList()
    // {
    //     // UIListArgs<BattleBuffUIShowObj, BattleBuffUI> args = new UIListArgs<BattleBuffUIShowObj, BattleBuffUI>();
    //     // args.dataList = buffDataList;
    //     // args.showObjList = buffShowObjList;
    //     // args.root = buffListRoot;
    //     // args.parentObj = this;
    //     // UIFunc.DoUIList(args);
    //     
    //     buffShowObjList.Clear();
    //     
    //     for (int i = 0; i < buffDataList.Count; i++)
    //     {
    //         var data = buffDataList[i];
    //         GameObject go = null;
    //         if (i < this.buffListRoot.childCount)
    //         {
    //             go = this.buffListRoot.GetChild(i).gameObject;
    //         }
    //         else
    //         {
    //             go = GameObject.Instantiate(this.buffListRoot.GetChild(0).gameObject,
    //                 this.buffListRoot, false);
    //         }
    //
    //         BuffShowObj cell = new BuffShowObj();
    //         cell.Init(go);
    //         cell.Show();
    //         cell.Refresh(data,i);
    //
    //         buffShowObjList.Add(cell);
    //     }
    //     
    //     for (int i = buffDataList.Count; i < this.buffListRoot.childCount; i++)
    //     {
    //         var go = this.buffListRoot.GetChild(i).gameObject;
    //         go.SetActive(false);
    //     }
    // }
    
    public void InitShowList()
    {
        // foreach (var showObj in buffShowObjList)
        // {
        //     showObj.Hide();
        //     showObj.Release();
        // }
        buffShowObjList.Clear();
        
        var entity = BattleManager.Instance.GetLocalCtrlHero();
        //buffDataList.Sort((a, b) => { return a.guid.CompareTo(b.guid); });
        var buffDataList = entity.GetBuffList();
        for (int i = 0; i < buffDataList.Count; i++)
        {
            var data = buffDataList[i];
            GameObject go = null;
            if (i < this.buffListRoot.childCount)
            {
                go = this.buffListRoot.GetChild(i).gameObject;
            }
            else
            {
                go = GameObject.Instantiate(itemTemp.gameObject,
                    this.buffListRoot, false);
            }

            BuffShowObj cell = new BuffShowObj();
            cell.Init(go);
            cell.Show();
            cell.Refresh(data, i);

            buffShowObjList.Add(cell);
        }

        for (int i = buffDataList.Count; i < this.buffListRoot.childCount; i++)
        {
            var go = this.buffListRoot.GetChild(i).gameObject;
            go.SetActive(false);
        }
    }
    

    // public void RemoveBuffFromDataList(int guid)
    // {
    //     for (int i = this.buffDataList.Count - 1; i >= 0; --i)
    //     {
    //         var buffData = this.buffDataList[i];
    //         if (buffData.guid == guid)
    //         {
    //             this.buffDataList.RemoveAt(i);
    //             return;
    //         }
    //     }
    // }

    // public void UpdateBuffInfo(BuffEffectInfo_Client buffInfo)
    // {
    //
    //     var entity = BattleManager.Instance.GetLocalCtrlHero();
    //
    //     buffDataList = entity.GetBuffList();
    //     this.RefreshBuffList();
    //
    //     
    //     // var buffShowObj = FindBuff(buffInfo.guid);
    //     // if (buffShowObj != null)
    //     // {
    //     //     if (buffInfo.isRemove)
    //     //     {
    //     //         RemoveBuffFromDataList(buffInfo.guid);
    //     //         this.RefreshBuffList();
    //     //     }
    //     //     else
    //     //     {
    //     //         buffShowObj.Refresh(buffInfo);
    //     //     }
    //     // }
    //     // else
    //     // {
    //     //     var eft = BattleSkillEffectManager_Client.Instance.FindSkillEffect(buffInfo.guid);
    //     //     if (eft != null)
    //     //     {
    //     //         //如果在 技能效果中找到了 那么应该是非 buff 的显示特效 需要删除 (这块逻辑待修改)
    //     //         BattleSkillEffectManager_Client.Instance.DestorySkillEffect(buffInfo.guid);
    //     //     }
    //     //     else
    //     //     {
    //     //         buffDataList.Add(buffInfo);
    //     //         this.RefreshBuffList();
    //     //     }
    //     // }
    // }
    
    public void OnBuffInfoUpdate(BuffEffectInfo_Client buffInfo)
    {
        var ctrlHeroGuid = BattleManager.Instance.GetLocalPlayer().ctrlHeroGuid;
            
        // if (null == this.currShowEntity)
        // {
        //     return;
        // }

        if (ctrlHeroGuid == buffInfo.targetEntityGuid)
        {
            this.UpdateBuff(buffInfo);
        }
        
        // if (BattleManager.Instance.GetLocalPlayer().ctrlHeroGuid == buffInfo.targetEntityGuid)
        // {
        //     UpdateBuffInfo(buffInfo);
        // }
    }
    
     public void UpdateBuff(BuffEffectInfo_Client buffInfo)
    {
        //  RefreshDataList(this.entity);
        // this.RefreshShowList();

        if (buffInfo.isRemove)
        {
            var findShowObj = this.buffShowObjList.Find(showObj => showObj.Guid == buffInfo.guid);
            if (findShowObj != null)
            {
                findShowObj.Hide();
                findShowObj.Release();
                this.buffShowObjList.Remove(findShowObj);
            }
        }
        else
        {
            var findShowObj = this.buffShowObjList.Find(showObj => showObj.Guid == buffInfo.guid);
            if (null == findShowObj)
            {
                var go = GameObject.Instantiate(itemTemp.gameObject,
                    this.buffListRoot, false);

                BuffShowObj cell = new BuffShowObj();
                cell.Init(go);
                cell.Show();
                cell.Refresh(buffInfo);

                buffShowObjList.Add(cell);
            }
            else
            {
                findShowObj.Refresh(buffInfo);
            }
        }
      
    }

    public void Update(float deltaTime)
    {
        foreach (var item in buffShowObjList)
        {
            item.Update(deltaTime);
        }
    }

    public BuffShowObj FindBuff(int buffId)
    {
        foreach (var buff in buffShowObjList)
        {
            if (buff.Guid == buffId)
            {
                return buff;
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

    // public void OnBuffInfoUpdate(BuffEffectInfo_Client buffInfo)
    // {
    //     if (BattleManager.Instance.GetLocalPlayer().ctrlHeroGuid == buffInfo.targetEntityGuid)
    //     {
    //         UpdateBuffInfo(buffInfo);
    //     }
    // }

    public void Release()
    {
        foreach (var item in buffShowObjList)
        {
            item.Release();
        }

        buffShowObjList = null;
        // buffDataList = null;

        EventDispatcher.RemoveListener<BuffEffectInfo_Client>(EventIDs.OnBuffInfoUpdate, OnBuffInfoUpdate);
    }
}