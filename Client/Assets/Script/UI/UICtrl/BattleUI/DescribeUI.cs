using System;
using System.Collections;
using System.Collections.Generic;
using Battle;
using Config;
using UnityEngine;
using UnityEngine.UI;
using Vector2 = UnityEngine.Vector2;

public class DescribeUIArgs : UICtrlArgs
{
    public int iconResId;
    public string name;
    public string content;

    public Vector2 pos;
}

public class DescribeUI
{
    public GameObject gameObject;
    public Transform transform;

    Image icon;
    Text nameText;
    Text contentText;

    DescribeUIArgs uiDataArgs;

    public void Init(GameObject gameObject, BattleUI battleUIPre)
    {
        this.gameObject = gameObject;
        this.transform = this.gameObject.transform;

        icon = this.transform.Find("Layout/Up/iconBg/icon").GetComponent<Image>();
        nameText = this.transform.Find("Layout/Up/name_text").GetComponent<Text>();
        contentText = this.transform.Find("Layout/Down/content_text").GetComponent<Text>();

        //buffDataList = new List<BattleBuffUIData>();
        //this.skillTipText = this.transform.Find("skillTipText").GetComponent<Text>();
        
        //属性说明
        EventDispatcher.AddListener<EntityAttrType, Vector2>(EventIDs.On_UIAttrOption_PointEnter,
            OnUIAttrOptionPointEnter);
        EventDispatcher.AddListener<EntityAttrType>(EventIDs.On_UIAttrOption_PointExit, OnUIAttrOptionPointExit);
        
        //技能说明
        EventDispatcher.AddListener<int, Vector2>(EventIDs.On_UISkillOption_PointEnter, OnUISkillOptionPointEnter);
        EventDispatcher.AddListener<int>(EventIDs.On_UISkillOption_PointExit, OnUISkillOptionPointExit);
        
        //buff 说明
        EventDispatcher.AddListener<int, Vector2>(EventIDs.On_UIBuffOption_PointEnter, OnUIBuffOptionPointEnter);
        EventDispatcher.AddListener<int>(EventIDs.On_UIBuffOption_PointExit, OnUIBuffOptionPointExit);
        
        //道具说明
        EventDispatcher.AddListener<int, Vector2>(EventIDs.On_UIItemOption_PointEnter, OnUIItemOptionPointEnter);
        EventDispatcher.AddListener<int>(EventIDs.On_UIItemOption_PointExit, OnUIItemOptionPointExit);
        
        //技能道具说明
        EventDispatcher.AddListener<int, Vector2>(EventIDs.On_UISkillItemOption_PointEnter, OnUISkillItemOptionPointEnter);
        EventDispatcher.AddListener<int>(EventIDs.On_UISkillItemOption_PointExit, OnUISkillItemOptionPointExit);
    }
    

    public void Show()
    {
        this.gameObject.SetActive(true);
    }

    public void Refresh(UICtrlArgs args)
    {
        uiDataArgs = (DescribeUIArgs)args;

        this.RefreshInfo();
    }

    void RefreshInfo()
    {
        int iconResId = uiDataArgs.iconResId;
        string name = uiDataArgs.name;
        string content = uiDataArgs.content;
        //相对于 BattleUI 的位置
        var pos = uiDataArgs.pos;

        nameText.text = name;
        contentText.text = content;
        //icon
        ResourceManager.Instance.GetObject<Sprite>(this.uiDataArgs.iconResId,
            (sprite) => { this.icon.sprite = sprite; });

        this.transform.localPosition = pos;
    }

    public void Update(float deltaTime)
    {
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }

    public void Close()
    {
        this.Hide();
    }

    public void OnUIAttrOptionPointEnter(EntityAttrType type, Vector2 pos)
    {
        var attrOption = AttrInfoHelper.Instance.GetAttrInfo(type);
        var args = new DescribeUIArgs()
        {
            name = attrOption.name,
            content = attrOption.describe,
            pos = pos + Vector2.right * 50,
            iconResId = attrOption.iconResId
        };
        this.Refresh(args);
        this.Show();
    }

    public void OnUIAttrOptionPointExit(EntityAttrType type)
    {
        this.Hide();
    }

    public void OnUISkillOptionPointEnter(int skillId, Vector2 pos)
    {
        var skillConfig = Config.ConfigManager.Instance.GetById<Config.Skill>(skillId);

        var des = skillConfig.Describe;
        var args = new DescribeUIArgs()
        {
            name = skillConfig.Name,
            content = des,
            pos = pos + Vector2.right * 50,
            iconResId = skillConfig.IconResId
        };
        this.Refresh(args);
        this.Show();
    }

    public void OnUISkillOptionPointExit(int skillId)
    {
        this.Hide();
    }

    public void OnUIBuffOptionPointEnter(int buffConfigId, Vector2 pos)
    {
        var buffConfig = Config.ConfigManager.Instance.GetById<Config.BuffEffect>(buffConfigId);

        var des = buffConfig.Describe;
        var args = new DescribeUIArgs()
        {
            name = buffConfig.Name,
            content = des,
            pos = pos + Vector2.right * 50,
            iconResId = buffConfig.IconResId
        };
        this.Refresh(args);
        this.Show();
    }

    public void OnUIBuffOptionPointExit(int buffId)
    {
        this.Hide();
    }
    
    public void OnUIItemOptionPointEnter(int itemConfigId, Vector2 pos)
    {
        var itemConfig = Config.ConfigManager.Instance.GetById<Config.BattleItem>(itemConfigId);

        var des = itemConfig.Describe;
        var args = new DescribeUIArgs()
        {
            name = itemConfig.Name,
            content = des,
            pos = pos + Vector2.right * 50,
            iconResId = itemConfig.IconResId
        };
        this.Refresh(args);
        this.Show();
    }

    public void OnUISkillItemOptionPointExit(int itemId)
    {
        this.Hide();
    }

    public void OnUISkillItemOptionPointEnter(int itemConfigId, Vector2 pos)
    {
        var itemConfig = Config.ConfigManager.Instance.GetById<Config.BattleItem>(itemConfigId);

        var des = itemConfig.Describe;
        var args = new DescribeUIArgs()
        {
            name = itemConfig.Name,
            content = des,
            pos = pos + Vector2.right * 50,
            iconResId = itemConfig.IconResId
        };
        this.Refresh(args);
        this.Show();
    }

    public void OnUIItemOptionPointExit(int itemId)
    {
        this.Hide();
    }

    public void Release()
    {
        EventDispatcher.RemoveListener<EntityAttrType, Vector2>(EventIDs.On_UIAttrOption_PointEnter,
            OnUIAttrOptionPointEnter);
        EventDispatcher.RemoveListener<EntityAttrType>(EventIDs.On_UIAttrOption_PointExit, OnUIAttrOptionPointExit);
        
        EventDispatcher.RemoveListener<int, Vector2>(EventIDs.On_UISkillOption_PointEnter, OnUISkillOptionPointEnter);
        EventDispatcher.RemoveListener<int>(EventIDs.On_UISkillOption_PointExit, OnUISkillOptionPointExit);
        
        EventDispatcher.RemoveListener<int, Vector2>(EventIDs.On_UIBuffOption_PointEnter, OnUIBuffOptionPointEnter);
        EventDispatcher.RemoveListener<int>(EventIDs.On_UIBuffOption_PointExit, OnUIBuffOptionPointExit);
        
        EventDispatcher.RemoveListener<int, Vector2>(EventIDs.On_UIItemOption_PointEnter, OnUIItemOptionPointEnter);
        EventDispatcher.RemoveListener<int>(EventIDs.On_UIItemOption_PointExit, OnUIItemOptionPointExit);
    }
}