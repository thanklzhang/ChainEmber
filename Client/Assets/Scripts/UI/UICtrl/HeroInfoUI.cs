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

        upgradeButton.onClick.AddListener(() => { UpgradeHero(); });
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

        // 添加英雄数据更新事件监听
        EventDispatcher.AddListener<int>(EventIDs.OnRefreshHeroData, OnHeroDataRefreshed);
    }

    // 响应英雄数据刷新事件
    private void OnHeroDataRefreshed(int guid)
    {
        // 获取最新的英雄数据
        HeroData updatedHeroData = HeroService.Instance.GetHero(guid);

        if (updatedHeroData != null)
        {
            heroData = updatedHeroData;
            RefreshUI();
            Debug.Log($"[HeroInfoUI] 英雄数据已更新并刷新UI: ID={heroData.guid}, 等级={heroData.level}");
        }
    }

    private void RefreshUI()
    {
        // 更新英雄基本信息
        nameText.text = heroConfig.Name;
        levelText.text = "Lv." + heroData.level;
        descriptionText.text = heroConfig.Describe;

        // 加载英雄全身图片
        ResourceManager.Instance.GetObject<Sprite>(heroConfig.AllBodyResId,
            (sprite) => { heroFullBodyImage.sprite = sprite; });

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

        // 获取英雄的等级属性
        var levelAttrId = heroConfig.LevelAttrId;
        var levelAttrTemplateId = levelAttrId;

        // 查找当前等级的等级属性
        EntityAttrLevel levelAttr = null;
        var allLevelAttrs = ConfigManager.Instance.GetList<EntityAttrLevel>();

        foreach (var attr in allLevelAttrs)
        {
            if (attr.TemplateId == levelAttrTemplateId && attr.Level == heroData.level)
            {
                levelAttr = attr;
                break;
            }
        }

        if (baseAttr != null)
        {
            // 基础生命值 + 等级附加生命值
            int healthAdd = levelAttr != null ? levelAttr.Health : 0;
            AddAttributeItem("生命值", baseAttr.Health + healthAdd);

            // 基础攻击力 + 等级附加攻击力
            int attackAdd = levelAttr != null ? levelAttr.Attack : 0;
            AddAttributeItem("攻击力", baseAttr.Attack + attackAdd);

            // 基础防御力 + 等级附加防御力
            int defenceAdd = levelAttr != null ? levelAttr.Defence : 0;
            AddAttributeItem("防御力", baseAttr.Defence + defenceAdd);

            // 攻击速度 (可能会随等级提升)
            int attackSpeedAdd = levelAttr != null ? levelAttr.AttackSpeed : 0;
            float attackSpeed = (baseAttr.AttackSpeed + attackSpeedAdd) / 1000f;
            AddAttributeItem("攻击速度", attackSpeed);

            // 移动速度 (可能会随等级提升)
            int moveSpeedAdd = levelAttr != null ? levelAttr.MoveSpeed : 0;
            float moveSpeed = (baseAttr.MoveSpeed + moveSpeedAdd) / 1000f;
            AddAttributeItem("移动速度", moveSpeed);

            // 攻击范围 (可能会随等级提升)
            int attackRangeAdd = levelAttr != null ? levelAttr.AttackRange : 0;
            float attackRange = (baseAttr.AttackRange + attackRangeAdd) / 1000f;
            AddAttributeItem("攻击范围", attackRange);

            // 暴击率 (可能会随等级提升)
            int critRateAdd = levelAttr != null ? levelAttr.CritRate : 0;
            float critRate = (baseAttr.CritRate + critRateAdd) / 10f;
            AddAttributeItem("暴击率", critRate + "%");

            // 暴击伤害 (可能会随等级提升)
            int critDamageAdd = levelAttr != null ? levelAttr.CritDamage : 0;
            float critDamage = (100 + (baseAttr.CritDamage + critDamageAdd) / 10f);
            AddAttributeItem("暴击伤害", critDamage + "%");

            // 输出伤害率 (可能会随等级提升)
            int outputDamageRateAdd = levelAttr != null ? levelAttr.OutputDamageRate : 0;
            float outputDamageRate = (baseAttr.OutputDamageRate + outputDamageRateAdd) / 10f;
            AddAttributeItem("输出伤害率", outputDamageRate + "%");

            // 承受伤害率 (可能会随等级提升)
            int inputDamageRateAdd = levelAttr != null ? levelAttr.InputDamageRate : 0;
            float inputDamageRate = (baseAttr.InputDamageRate + inputDamageRateAdd) / 10f;
            AddAttributeItem("承受伤害率", inputDamageRate + "%");

            // 技能冷却 (可能会随等级提升)
            int skillCDAdd = levelAttr != null ? levelAttr.SkillCD : 0;
            float skillCD = baseAttr.SkillCD + skillCDAdd;
            AddAttributeItem("技能冷却", skillCD + "秒");

            // 治疗比率 (通常不随等级变化)
            AddAttributeItem("治疗比率", baseAttr.TreatmentRate / 10f + "%");

            // 生命恢复速度 (通常不随等级变化)
            AddAttributeItem("生命恢复速度", baseAttr.HealthRecoverSpeed / 1000f + "/秒");

            // 如果找到了等级属性数据，记录日志
            if (levelAttr != null)
            {
                Debug.Log(
                    $"[HeroInfoUI] 应用了等级{heroData.level}的属性加成: 生命+{levelAttr.Health}, 攻击+{levelAttr.Attack}, 防御+{levelAttr.Defence}");
            }
            else
            {
                Debug.LogWarning(
                    $"[HeroInfoUI] 未找到英雄(ID:{heroData.guid}, 配置ID:{heroData.configId})等级{heroData.level}的属性数据");
            }
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
        // 显示加载中状态
        upgradeButton.interactable = false;

        // 调用英雄服务来升级英雄
        var heroService = HeroService.Instance;

        // 使用回调方式处理升级结果
        heroService.UpgradeHero(heroData.guid, (response) =>
        {
            // 恢复按钮状态
            upgradeButton.interactable = true;

            var hero = heroService.GetHero(heroData.guid);
            if (response.err == ErrorCode.Success)
            {
                Logx.Log($"[HeroInfoUI] 英雄升级成功: configId={heroData.configId}, 新等级={hero.level}");
            }
            else
            {
                Logx.LogError($"[HeroInfoUI] 英雄升级失败: configId={heroData.configId}, 错误码={response.err}");
            }
        });
    }

    protected override void OnClose()
    {
        // closeButton.onClick.RemoveAllListeners();
        upgradeButton.onClick.RemoveAllListeners();

        // 移除事件监听
        EventDispatcher.RemoveListener<int>(EventIDs.OnRefreshHeroData, OnHeroDataRefreshed);

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