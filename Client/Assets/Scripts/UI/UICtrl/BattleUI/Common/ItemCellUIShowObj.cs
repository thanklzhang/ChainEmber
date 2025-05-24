using Battle_Client;
using System;
using System.Collections;
using System.Collections.Generic;
using Battle;
using Config;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Vector2 = UnityEngine.Vector2;


public class ItemCellUIShowObj
{
    public GameObject gameObject;
    public Transform transform;

    protected Transform itemTran;
    // protected Image itemIconImg;
    // private TextMeshProUGUI countText;
    private GameObject lockFlagGo;
    private UIEventTrigger evetnTrigger;

    //runtime
    public int index;
    public BattleItemData_Client itemData;
    private BattleUI battleUI;
    public bool isUnlock;

    public int entityGuid;

    private CommonItem itemShowObj;

    public ItemLocationType locationType;
    
    public virtual void Init(GameObject gameObject, BattleUI battleUI, int entityGuid)
    {
        this.gameObject = gameObject;
        this.transform = this.gameObject.transform;
        this.battleUI = battleUI;

        this.entityGuid = entityGuid;

        itemTran = this.transform.Find("CommonItem");

        itemShowObj = new CommonItem();
        itemShowObj.Init(itemTran.gameObject);
        // this.itemIconImg = itemTran.Find("icon").GetComponent<Image>();
        //
        // countText = itemTran.transform.Find("countText").GetComponent<TextMeshProUGUI>();

        lockFlagGo = transform.Find("lock").gameObject;

        var itemIconImg = itemShowObj.GetIconImage();
        evetnTrigger = itemIconImg.GetComponent<UIEventTrigger>();
        evetnTrigger.OnPointEnterEvent += OnPointEnter;
        evetnTrigger.OnPointerExitEvent += OnPointExit;
    }

    public void RefreshUI(BattleItemData_Client data, int index, bool isUnlock = true)
    {
        this.index = index;
        this.itemData = data;
        this.isUnlock = isUnlock;

        RefreshItemShow();
    }

    void RefreshItemShow()
    {
        if (this.isUnlock)
        {
            lockFlagGo.SetActive(false);
            
            if (itemData != null)
            {    
                this.itemTran.gameObject.SetActive(true);

                // var itemConfig = ConfigManager.Instance.GetById<Config.BattleItem>(this.data.configId);
                // var iconResId = itemConfig.IconResId;
                // ResourceManager.Instance.GetObject<Sprite>(iconResId,
                //     (sprite) => { this.itemIconImg.sprite = sprite; });
                //
                // var count = this.data.count;
                // countText.text = "" + this.data.count;
                // if (count > 1)
                // {
                //     countText.gameObject.SetActive(true);
                // }
                // else
                // {
                //     countText.gameObject.SetActive(false);
                // }
                itemShowObj.RefreshUI(itemData);
            }
            else
            {
                this.itemTran.gameObject.SetActive(false);
            }
        }
        else
        {
            this.itemTran.gameObject.SetActive(false);
            lockFlagGo.SetActive(true);
        }
    }

    public void Update(float deltaTime)
    {
    }

    public void OnPointEnter(PointerEventData e)
    {
        if (null == this.itemData)
        {
            return;
        }
        //转换成点在 BattleUI 中的 localPosition

        var camera3D = CameraManager.Instance.GetCamera3D();
        var cameraUI = CameraManager.Instance.GetCameraUI();

        var screenPos = e.position;

        Vector2 uiPos;

        var battleUIRect = battleUI.gameObject.GetComponent<RectTransform>();
        RectTransformUtility.ScreenPointToLocalPointInRectangle(battleUIRect, screenPos, cameraUI.camera, out uiPos);

        EventDispatcher.Broadcast<int, Vector2>(EventIDs.On_UIItemOption_PointEnter, this.itemData.configId, uiPos);
    }

    public void OnPointExit(PointerEventData e)
    {
        if (null == this.itemData)
        {
            return;
        }

        EventDispatcher.Broadcast<int>(EventIDs.On_UIItemOption_PointExit, this.itemData.configId);
    }


    public virtual void Release()
    {
        itemShowObj.Release();
        
        evetnTrigger.OnPointEnterEvent -= OnPointEnter;
        evetnTrigger.OnPointerExitEvent -= OnPointExit;
        
    }

    
    //获取移动中的定位参数
    public virtual ItemMoveLocationArg_Client GetItemMoveLocationArg()
    {
        return null;
    }
}


// public class BattleSkillUIArgs : UICtrlArgs
// {
//     public List<BattleSkillUIData> battleSkillList;
// }

// public class BattleItemUIData
// {
//     public int index;
//     public int configId;
//     public int count;
//
//     public float currCDTime;
//     public float maxCDTime;
//     
//     // public int skillId;
//     // public int iconResId;
//     
// }