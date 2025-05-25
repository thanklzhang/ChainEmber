using System;
using System.Collections;
using System.Collections.Generic;
using Battle;
using Config;
using GameData;
using ServerSimulation;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeroInfoUI : BaseUI
{
    // 英雄数据
    private HeroData heroData;
    private EntityInfo heroConfig;
    
    // UI元素
    private TextMeshProUGUI nameText;
    private Image heroFullBodyImage;
    private TextMeshProUGUI descriptionText;
    private TextMeshProUGUI levelText;
    private Button upgradeButton;
    // private Button closeButton;
    
    // 属性UI元素
    private Transform attributesRoot;
    private GameObject attributeItemPrefab;
    private List<GameObject> attributeItems = new List<GameObject>();
    
    protected override void OnInit()
    {
        this.uiResId = (int)ResIds.HeroInfoUI;
        this.uiShowLayer = UIShowLayer.Floor_0;
        this.showMode = CtrlShowMode.Fixed;
    }
    
    protected override void OnLoadFinish()
    {
        // 获取主要UI组件
        var root = transform.Find("root");
        nameText = root.Find("Info/name").GetComponent<TextMeshProUGUI>();
        heroFullBodyImage = root.Find("rolePic/img").GetComponent<Image>();
        descriptionText = root.Find("describe").GetComponent<TextMeshProUGUI>();
        levelText = root.Find("Info/level").GetComponent<TextMeshProUGUI>();
        upgradeButton = root.Find("Info/upgradeBtn").GetComponent<Button>();
        //closeButton = root.Find("closeBtn").GetComponent<Button>();
        
        // 获取属性部分
        attributesRoot = root.Find("Info/AttrRoot");
        attributeItemPrefab = attributesRoot.Find("attrOption").gameObject;
        attributeItemPrefab.SetActive(false);
        
        // 设置按钮监听
        // closeButton.onClick.AddListener(() =>
        // {
        //     UIManager.Instance.Close<HeroInfoUI>();
        // });
        
        upgradeButton.onClick.AddListener(() =>
        {
            UpgradeHero();
        });
    }
    
    protected override void OnOpen(UICtrlArgs args)
    {
        HeroInfoUIArgs heroInfoArgs = args as HeroInfoUIArgs;
        if (heroInfoArgs != null && heroInfoArgs.heroData != null)
        {
            heroData = heroInfoArgs.heroData;
            heroConfig = ConfigManager.Instance.GetById<EntityInfo>(heroData.configId);
            
            RefreshUI();
        }
        
        UIManager.Instance.Open<TitleBarUI>(new TitleBarUIArgs()
        {
            titleBarId = 2
        });
    }
    
    private void RefreshUI()
    {
        // 更新英雄基本信息
        nameText.text = heroConfig.Name;
        levelText.text = "Lv." + heroData.level;
        descriptionText.text = heroConfig.Describe;
        
        // 加载英雄全身图片
        ResourceManager.Instance.GetObject<Sprite>(heroConfig.AllBodyResId, (sprite) =>
        {
            heroFullBodyImage.sprite = sprite;
        });
        
        // 刷新属性
        RefreshAttributes();
    }
    
    private void RefreshAttributes()
    {
        // 清除现有属性项
        foreach (var item in attributeItems)
        {
            if (item != null)
            {
                GameObject.Destroy(item);
            }
        }
        attributeItems.Clear();
        
        // 获取英雄的基础属性
        var baseAttrId = heroConfig.BaseAttrId;
        var baseAttr = ConfigManager.Instance.GetById<EntityAttrBase>(baseAttrId);
        
        if (baseAttr != null)
        {
            // 显示所有属性
            AddAttributeItem("生命值", baseAttr.Health + (heroData.level - 1) * 100);
            AddAttributeItem("攻击力", baseAttr.Attack + (heroData.level - 1) * 10);
            AddAttributeItem("防御力", baseAttr.Defence + (heroData.level - 1) * 5);
            AddAttributeItem("攻击速度", baseAttr.AttackSpeed / 1000f);
            AddAttributeItem("移动速度", baseAttr.MoveSpeed / 1000f);
            AddAttributeItem("攻击范围", baseAttr.AttackRange / 1000f);
            AddAttributeItem("暴击率", baseAttr.CritRate / 10f + "%");
            AddAttributeItem("暴击伤害", (100 + baseAttr.CritDamage / 10f) + "%");
            AddAttributeItem("输出伤害率", baseAttr.OutputDamageRate / 10f + "%");
            AddAttributeItem("承受伤害率", baseAttr.InputDamageRate / 10f + "%");
            AddAttributeItem("技能冷却", baseAttr.SkillCD + "秒");
            AddAttributeItem("治疗比率", baseAttr.TreatmentRate / 10f + "%");
            AddAttributeItem("生命恢复速度", baseAttr.HealthRecoverSpeed / 1000f + "/秒");
        }
    }
    
    private void AddAttributeItem(string name, object value)
    {
        // 实例化属性项
        GameObject item = GameObject.Instantiate(attributeItemPrefab, attributesRoot);
        item.SetActive(true);
        
        // 设置属性名和值
        TextMeshProUGUI nameText = item.transform.Find("name").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI valueText = item.transform.Find("value").GetComponent<TextMeshProUGUI>();
        
        nameText.text = name;
        valueText.text = value.ToString();
        
        // 添加到列表中
        attributeItems.Add(item);
    }
    
    private void UpgradeHero()
    {
        // 调用英雄服务来升级英雄
        var heroService = ServerServiceManager.Instance.HeroService;
        bool success = heroService.LevelUpHero(heroData.guid);
        
        if (success)
        {
            // 刷新UI显示新的等级
            heroData.level += 1;
            RefreshUI();
        }
    }
    
    protected override void OnClose()
    {
        // closeButton.onClick.RemoveAllListeners();
        upgradeButton.onClick.RemoveAllListeners();
        
        // 清理属性项
        foreach (var item in attributeItems)
        {
            if (item != null)
            {
                GameObject.Destroy(item);
            }
        }
        attributeItems.Clear();
    }
}

// 打开HeroInfoUI的参数
public class HeroInfoUIArgs : UICtrlArgs
{
    public HeroData heroData;
    
    public HeroInfoUIArgs(HeroData heroData)
    {
        this.heroData = heroData;
    }
} 