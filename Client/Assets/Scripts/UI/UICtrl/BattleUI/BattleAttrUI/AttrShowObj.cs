using Battle_Client;
using System;
using System.Collections;
using System.Collections.Generic;
using Battle;
using Config;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Vector2 = UnityEngine.Vector2;

// public enum AttrValueShowType
// {
//     Int = 0,
//
//     //浮点型 保留两位
//     Float_2 = 1
// }

public enum AttrValueShowType
{
    Int = 0,

    //浮点型 保留两位
    Float_2 = 1
}

public class AttrUIData
{
    public EntityAttrType type;

    // public string name;
    // public int iconResId;
    public float value;

    // public string describe;
    //public AttrValueShowType valueShowType;

    public static AttrUIData Create(EntityAttrType type, float value,
        AttrValueShowType valueShowType)
    {
        return null;
    }
}

public class AttrShowObj
{
    public GameObject gameObject;
    public Transform transform;

    Text nameText;
    Text valueText;
    Image iconImg;
    public AttrUIData uiData;
    UIEventTrigger evetnTrigger;

    public void Init(GameObject gameObject)
    {
        this.gameObject = gameObject;
        this.transform = this.gameObject.transform;

        nameText = this.transform.Find("name_text").GetComponent<Text>();
        valueText = this.transform.Find("value_text").GetComponent<Text>();
        evetnTrigger = this.transform.GetComponent<UIEventTrigger>();

        if (evetnTrigger != null)
        {
            evetnTrigger.OnPointEnterEvent += OnPointEnter;
            evetnTrigger.OnPointerExitEvent += OnPointExit;
        }

        //icon
        iconImg = this.transform.Find("icon").GetComponent<Image>();
    }

    public void Refresh(object data, int index)
    {
        this.uiData = (AttrUIData)data;

        var attrInfo = AttrInfoHelper.Instance.GetAttrInfo(this.uiData.type);

        nameText.text = "" + attrInfo.name;

        valueText.text = "" + this.uiData.value;

        AttrValueShowType showType = AttrValueShowType.Int;
        if (this.uiData.type == EntityAttrType.AttackSpeed)
        {
            showType = AttrValueShowType.Float_2;
        }
        else if (this.uiData.type == EntityAttrType.AttackRange)
        {
            showType = AttrValueShowType.Float_2;
        }
        else if (this.uiData.type == EntityAttrType.MoveSpeed)
        {
            showType = AttrValueShowType.Float_2;
        }

        if (showType == AttrValueShowType.Int)
        {
            valueText.text = "" + (int)this.uiData.value;
        }
        else if (showType == AttrValueShowType.Float_2)
        {
            valueText.text = string.Format("{0:F}", this.uiData.value);
        }

        var iconResId = attrInfo.iconResId;
        //TODO：注意 iconResId
        ResourceManager.Instance.GetObject<Sprite>(iconResId,
            (sprite) => { this.iconImg.sprite = sprite; });
    }

    public void Show()
    {
        this.gameObject.SetActive(true);
    }

    public void Update(float deltaTime)
    {
        
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }

    public void OnPointEnter(PointerEventData e)
    {
        //转换成点在 BattleUI 中的 localPosition
        
        var camera3D = CameraManager.Instance.GetCamera3D();
        var cameraUI = CameraManager.Instance.GetCameraUI();
        
        var screenPos = e.position;
        
        Vector2 uiPos;
        var battleUIRect = UIManager.Instance.uiRootRectTran;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(battleUIRect, screenPos, cameraUI.camera, out uiPos);
        
        EventDispatcher.Broadcast<EntityAttrType, Vector2>(EventIDs.On_UIAttrOption_PointEnter, this.uiData.type,
            uiPos);
    }

    public void OnPointExit(PointerEventData e)
    {
        EventDispatcher.Broadcast<EntityAttrType>(EventIDs.On_UIAttrOption_PointExit, this.uiData.type);
    }


    public void Release()
    {
        if (evetnTrigger != null)
        {
            evetnTrigger.OnPointEnterEvent -= OnPointEnter;
            evetnTrigger.OnPointerExitEvent -= OnPointExit;
        }
    }
}