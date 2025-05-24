using Battle_Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Config;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BattleSkillUIShowObj// : BaseUIShowObj<BattleSkillUI>
{
    public GameObject gameObject;
    public Transform transform;
    
    protected Image icon;
    protected GameObject canUseMaskGo;
    protected GameObject cdRootGo;
    protected Image cdImg;
    protected Text cdTimeText;
    private Slider expSlider;
    private TextMeshProUGUI expText;
    private TextMeshProUGUI levelText;
    protected UIEventTrigger evetnTrigger;
    public BattleSkillInfo uiData;

    float currCDTimer = 0;

    private BattleSkillUI skillUI;
    private Transform normalRoot;
    private Transform lockRoot;
    private Transform emptyRoot;
    public void Init(GameObject gameObject,BattleSkillUI skillUI)
    {
        this.gameObject = gameObject;
        this.transform = this.gameObject.transform;
        this.skillUI = skillUI;

        normalRoot = this.transform.Find("normal");
        emptyRoot = this.transform.Find("empty");
        
        lockRoot = this.normalRoot.Find("lock");
      
        canUseMaskGo = normalRoot.Find("cantUse").gameObject;
        cdRootGo = normalRoot.Find("CDRoot").gameObject;
        cdTimeText = normalRoot.Find("CDRoot/CDShow/cd_text").GetComponent<Text>();
        cdImg = normalRoot.Find("CDRoot/CDShow").GetComponent<Image>();
        icon = normalRoot.Find("icon").GetComponent<Image>();
        expSlider = normalRoot.Find("expSlider").GetComponent<Slider>();
        expText = expSlider.transform.Find("exp_text").GetComponent<TextMeshProUGUI>();
        levelText = normalRoot.Find("lv_text").GetComponent<TextMeshProUGUI>();

        evetnTrigger = icon.GetComponent<UIEventTrigger>();

        canUseMaskGo.SetActive(false);
        cdRootGo.SetActive(false);
        
        normalRoot.gameObject.SetActive(false);
        emptyRoot.gameObject.SetActive(true);
        
        lockRoot.gameObject.SetActive(false);

        evetnTrigger.OnPointEnterEvent -= OnPointEnter;
        evetnTrigger.OnPointerExitEvent -= OnPointExit;
        
        evetnTrigger.OnPointEnterEvent += OnPointEnter;
        evetnTrigger.OnPointerExitEvent += OnPointExit;
    }

    public void Refresh(BattleSkillInfo data, int index)
    {
        if (null == data)
        {
            return;
        }

        this.uiData = (BattleSkillInfo)data;

        if (this.uiData.configId > 0)
        {
            normalRoot.gameObject.SetActive(true);
            emptyRoot.gameObject.SetActive(false);
            
            if (this.uiData.configId > 0)
            {
                lockRoot.gameObject.SetActive(false);
            }
            else
            {
                lockRoot.gameObject.SetActive(true);
            }

            //技能图标
            var skillId = this.uiData.configId;
            var skillConfig = Config.ConfigManager.Instance.GetById<Config.Skill>(skillId);
            ResourceManager.Instance.GetObject<Sprite>(skillConfig.IconResId, (sprite) => { this.icon.sprite = sprite; });

            //冷却显示
            currCDTimer =  this.uiData.currCDTime;

            if (currCDTimer <= 0)
            {
                //可以使用技能了
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
            
            RefreshLevelExpShow();
        }
        else
        {
            normalRoot.gameObject.SetActive(false);
            emptyRoot.gameObject.SetActive(true);
        }

    }

    public void RefreshLevelExpShow()
    {
        if (this.uiData.configId > 0)
        {
            var skillId = this.uiData.configId;
            var skillConfig = Config.ConfigManager.Instance.GetById<Config.Skill>(skillId);
            var currExp = this.uiData.exp;
            var skillUpdateConfig = Config.ConfigManager.Instance.GetById<SkillUpgradeParam>(1);
            var maxExp = skillUpdateConfig.UpgradeExpPerLevel.Sum() + 1;
            this.expSlider.value = currExp / (float)maxExp;
            this.expText.text = $"{currExp}/{maxExp}";
        
            this.levelText.text = "" + skillConfig.Level;
        }
    }

    internal void UpdateInfo(BattleSkillInfo skillInfo)
    {
        currCDTimer = skillInfo.currCDTime;

        if (currCDTimer <= 0)
        {
            //可以使用技能了
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

        RefreshLevelExpShow();
    }


    public void Update(float deltaTime)
    {
        if (currCDTimer < 0)
        {
            currCDTimer = 0;
        }

        if (currCDTimer > 0)
        {
            currCDTimer -= deltaTime;

            float showTime = 0.0f;
            var showStr = "";
            if (currCDTimer >= 0.50f)
            {
                showTime = (float)Math.Ceiling(currCDTimer);

                showStr = string.Format("{0}", (int)showTime);
            }
            else
            {
                showTime = currCDTimer;
                showStr = string.Format("{0:F}", showTime);
            }


            //Debug.LogError("currCDTimer " + currCDTimer + " " + uiData.maxCDTime);
            cdTimeText.text = showStr;
            //Debug.Log("currCDTimer" + currCDTimer);
            //Debug.Log("uiData.maxCDTime" + uiData.maxCDTime);
            cdImg.fillAmount = currCDTimer / uiData.maxCDTime;
        }
    }

    internal int GetSkillConfigId()
    {
        return this.uiData.configId;
    }

    public void OnPointEnter(PointerEventData e)
    {
        //转换成点在 BattleUI 中的 localPosition

        if (this.uiData.configId <= 0)
        {
            return;
        }

        var camera3D = CameraManager.Instance.GetCamera3D();
        var cameraUI = CameraManager.Instance.GetCameraUI();

        var screenPos = e.position;

        Vector2 uiPos;
        var battleUIRect = this.skillUI.BattleUI.gameObject.GetComponent<RectTransform>();
        RectTransformUtility.ScreenPointToLocalPointInRectangle(battleUIRect, screenPos, cameraUI.camera, out uiPos);

        EventDispatcher.Broadcast<int, Vector2>(EventIDs.On_UISkillOption_PointEnter, this.uiData.configId, uiPos);
    }

    public void OnPointExit(PointerEventData e)
    {
        if (this.uiData.configId <= 0)
        {
            return;
        }
        
        EventDispatcher.Broadcast<int>(EventIDs.On_UISkillOption_PointExit, this.uiData.configId);
    }

    public void OnRemove()
    {
        this.uiData.configId = 0;
        this.Refresh(this.uiData, 0);
    }
    
    public void Release()
    {
        evetnTrigger.OnPointEnterEvent -= OnPointEnter;
        evetnTrigger.OnPointerExitEvent -= OnPointExit;
    }

  
}


// public class BattleSkillUIArgs : UICtrlArgs
// {
//     public List<BattleSkillUIData> battleSkillList;
// }

// public class BattleSkillUIData
// {
//     public int skillId;
//     public int iconResId;
//     public float maxCDTime;
//     public int exp;
//     // public int showIndex;
//     public bool isUnlock;
// }