using System;
using System.Collections;
using System.Collections.Generic;
using Battle_Client;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuffShowObj
{
    public GameObject gameObject;
    public Transform transform;

    Image icon;
    GameObject canUseMaskGo;
    GameObject cdRootGo;
    Image cdImg;
    Text stackCountText;


    public BuffEffectInfo_Client uiData;
    UIEventTrigger evetnTrigger;
    float currCDTimer = 0;

    public void Init(GameObject gameObject)
    {
        this.gameObject = gameObject;
        this.transform = this.gameObject.transform;
        
        canUseMaskGo = this.transform.Find("cantUse").gameObject;
        cdRootGo = this.transform.Find("CDRoot").gameObject;
        //cdTimeText = this.transform.Find("CDRoot/CDShow/cd_text").GetComponent<Text>();
        cdImg = this.transform.Find("CDRoot/CDShow").GetComponent<Image>();
        icon = this.transform.Find("icon").GetComponent<Image>();
        stackCountText = this.transform.Find("count_text").GetComponent<Text>();

        evetnTrigger = icon.GetComponent<UIEventTrigger>();

        if (evetnTrigger != null)
        {
            evetnTrigger.OnPointEnterEvent += OnPointEnter;
            evetnTrigger.OnPointerExitEvent += OnPointExit;
        }
    }

    public bool isPointEnter = false;

    // public void Refresh(BuffEffectInfo_Client data, int index)
    // {
    //     this.uiData = data;
    //
    //     this.UpdateInfo(this.uiData);
    // }

    public void Refresh(BuffEffectInfo_Client buffUIData,int index = 0)
    {
        this.uiData = buffUIData;

        var cdTime = buffUIData.currCDTime;
        currCDTimer = cdTime; /// 1000.0f;

        if (cdTime <= 0)
        {
            //-1 时 为没有时间概念的的 buff 例：目前时永久性 buff
            cdRootGo.SetActive(false);
            canUseMaskGo.SetActive(false);
        }
        else
        {
            //在 cd 中 ，刷新 cd 时间
            cdRootGo.SetActive(true);
            //mask 是代表不能用 并不是只有 cd 这个因素 尽管现在只有 cd
            canUseMaskGo.SetActive(true);
        }

        var buffConfig = Config.ConfigManager.Instance.GetById<Config.BuffEffect>(this.uiData.configId);
//        Logx.Log("zxy : buffConfig.IconResId : " + buffConfig.IconResId);
        ResourceManager.Instance.GetObject<Sprite>(buffConfig.IconResId, (sprite) => { icon.sprite = sprite; });

        stackCountText.text = "" + this.uiData.stackCount;
    }

    public void Show()
    {
        this.gameObject.SetActive(true);
    }

    public void Update(float deltaTime)
    {
        if (currCDTimer < 0)
        {
            currCDTimer = 0;

            if (isPointEnter)
            {
                EventDispatcher.Broadcast<int>(EventIDs.On_UIBuffOption_PointExit, this.uiData.configId);
                isPointEnter = false;
            }
        }

        if (currCDTimer > 0)
        {
            //float showTime = 0.0f;
            //var showStr = "";
            //if (currCDTimer >= 0.50f)
            //{
            //    showTime = (float)Math.Ceiling(currCDTimer);

            //    showStr = string.Format("{0}", (int)showTime);
            //}
            //else
            //{
            //    showTime = currCDTimer;
            //    showStr = string.Format("{0:F}", showTime);
            //}

            currCDTimer -= deltaTime;

            this.uiData.currCDTime = currCDTimer; // * 1000.0f
            //cdTimeText.text = showStr;
            cdImg.fillAmount = currCDTimer / (uiData.maxCDTime); // / 1000.0f
        }
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

        isPointEnter = true;

        EventDispatcher.Broadcast<int, Vector2>(EventIDs.On_UIBuffOption_PointEnter, this.uiData.configId, uiPos);
    }

    public void OnPointExit(PointerEventData e)
    {
        isPointEnter = false;
        EventDispatcher.Broadcast<int>(EventIDs.On_UIBuffOption_PointExit, this.uiData.configId);
    }

    public void Hide()
    {
        if (isPointEnter)
        {
            isPointEnter = false;
            EventDispatcher.Broadcast<int>(EventIDs.On_UIBuffOption_PointExit, this.uiData.configId);
        }
        this.gameObject.SetActive(false);
    }

    internal int Guid => this.uiData.guid;

    public void Release()
    {
       this.Hide();
       
        if (evetnTrigger != null)
        {
            evetnTrigger.OnPointEnterEvent -= OnPointEnter;
            evetnTrigger.OnPointerExitEvent -= OnPointExit;
        }

       
    }
}

//
// public class BattleBuffUIArgs : UICtrlArgs
// {
//     public List<BuffUIData> battleBuffList;
// }
//
// public class BuffUIData
// {
//     public int guid;
//
//     public int configId;
//
//     //public int iconResId;
//     public float currCDTime;
//     public float maxCDTime;
//     public int stackCount;
//
//     public bool isRemove;
// }