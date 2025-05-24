using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Battle;
using Battle_Client;
using Config;
using UnityEngine;
using UnityEngine.UI;
using Skill = Config.Skill;

public class BattleSkillUI
{
    public GameObject gameObject;
    public Transform transform;

    Transform skillListRoot;

    Text skillTipText;
    float skillTipShowTimer;
    float skillTipMaxShowTime = 1.6f;

    // List<BattleSkillUIData> skillDataList = new List<BattleSkillUIData>();
    List<BattleSkillUIShowObj> skillShowObjList = new List<BattleSkillUIShowObj>();

    // private BattleSkillUIData bigSkillData;
    // private BattleBigSkillUIShowObj bigSkillShowObj;

    private GameObject bigSkillGo;

    public BattleUI BattleUI;

    public void Init(GameObject gameObject, BattleUI battleUIPre)
    {
        this.gameObject = gameObject;
        this.transform = this.gameObject.transform;

        this.BattleUI = battleUIPre;

        skillListRoot = this.transform.Find("group");
        this.skillTipText = this.transform.Find("skillTipText").GetComponent<Text>();

        // bigSkillGo = this.transform.Find("bigSkill").gameObject;
        // bigSkillShowObj = new BattleBigSkillUIShowObj();
        // bigSkillShowObj.Init(bigSkillGo,this);

        EventDispatcher.AddListener<int, BattleSkillInfo>(EventIDs.OnSkillInfoUpdate, OnSkillInfoUpdate);
        EventDispatcher.AddListener<string>(EventIDs.OnSkillTips, OnSkillTips);
        EventDispatcher.AddListener(EventIDs.OnSkillRefreshAll, OnSkillRefreshAll);

        InitShowList();
    }

    public void InitShowList()
    {
        var temp = skillListRoot.GetChild(0).gameObject;
        temp.gameObject.SetActive(false);

        for (int i = 0; i < 4; i++)
        {
            GameObject go = GameObject.Instantiate(temp, skillListRoot, false);
            var showObj = new BattleSkillUIShowObj();
            showObj.Init(go, this);
            showObj.gameObject.SetActive(true);
            this.skillShowObjList.Add(showObj);
        }

        var localEntity = BattleManager.Instance.GetLocalCtrlHero();
        var entityConfig = ConfigManager.Instance.GetById<Config.EntityInfo>(localEntity.configId);
        var skills = BattleManager.Instance.GetLocalCtrlHeroSkills();

        List<BattleSkillInfo> GetSkills(SkillCategory skillCategory)
        {
            return skills.FindAll(skill =>
            {
                var skillConfig = ConfigManager.Instance.GetById<Config.Skill>(skill.configId);
                return (SkillCategory)skillConfig.SkillCategory == skillCategory;
            });
        }

        var minorSkills = GetSkills(SkillCategory.MinorSkill);
        var leaderSkills = GetSkills(SkillCategory.LeaderSkill);
        var ultimateSkills = GetSkills(SkillCategory.UltimateSkill);

        var minorShowObj = this.skillShowObjList[0];
        var defaultSkill = new BattleSkillInfo();
        minorShowObj.Refresh(minorSkills.Any() ? minorSkills[0] : defaultSkill, 0);

        var leaderShowObj1 = this.skillShowObjList[1];
        var leaderShowObj2 = this.skillShowObjList[2];

        // 使用ElementAtOrDefault防止索引越界
        leaderShowObj1.Refresh(leaderSkills.Count > 0 ? leaderSkills[0] : new BattleSkillInfo(), 1);
        leaderShowObj2.Refresh(leaderSkills.Count > 1 ? leaderSkills[1] : new BattleSkillInfo(), 2);


        var ultimateShowObj = this.skillShowObjList[3];
        defaultSkill = new BattleSkillInfo();
        ultimateShowObj.Refresh(ultimateSkills.Any() ? ultimateSkills[0] : defaultSkill, 3);
    }

    public void OnSkillInfoUpdate(int entityGuid, BattleSkillInfo skillInfo)
    {
        //var entity = BattleEntityManager.Instance.FindEntity(entityGuid);

        var myEntityGuid = BattleManager.Instance.GetLocalCtrlHeroGuid();

        if (myEntityGuid == entityGuid)
        {
            this.UpdateSkillInfo(skillInfo);

            //RefreshAllUI();
        }
    }

    void OnSkillTips(string str)
    {
        this.SetSkillTipText(str);
    }

    public void Show()
    {
        this.gameObject.SetActive(true);
    }

    public void RefreshAllUI()
    {
        // RefreshSkillShowList();
    }


    BattleSkillInfo GetLeaderSkill(List<BattleSkillInfo> leaderSkills, PlayerCommandType commandType)
    {
        var player = BattleManager.Instance.GetLocalPlayer();
        var leaderSkill1 = leaderSkills.Find(skill =>
        {
            var commandMode = player.GetInputCommand(commandType);
            if (commandMode != null)
            {
                var skillCM = commandMode as SkillCommandModel;
                if (skillCM.skillConfigId == skill.configId)
                {
                    return true;
                }
            }

            return false;
        });

        return leaderSkill1;
    }

    public int GetSkillGroupId(int skillId)
    {
        return SkillUpgradeConfigHelper.GetSkillGroupId(skillId);
    }

    private Dictionary<PlayerCommandType, int> skillCommandTypeDic = new Dictionary<PlayerCommandType, int>()
    {
        { PlayerCommandType.Skill_Minor, 0 },
        { PlayerCommandType.Skill_Leader_1, 1 },
        { PlayerCommandType.Skill_Leader_2, 2 },
        { PlayerCommandType.Skill_Ultimate, 3 },
    };

    public void UpdateSkillInfo(BattleSkillInfo skillInfo)
    {
        if (skillInfo.isDelete)
        {
            var skillShowObj = FindSkill(skillInfo.configId);
            if (skillShowObj != null)
            {
                skillShowObj.OnRemove();
            }
        }
        else
        {
            //此时数据已经刷新了

            var skillConfig = ConfigManager.Instance.GetById<Config.Skill>(skillInfo.configId);
            var player = BattleManager.Instance.GetLocalPlayer();
            var skillCommandModel = player.GetCommandModelBySkillId(skillInfo.configId);

            if (skillCommandTypeDic.TryGetValue(skillCommandModel.commandType, out var skillIndex))
            {
                var skillShowObj = this.skillShowObjList [skillIndex];
                skillShowObj.Refresh(skillInfo, skillInfo.showIndex);
            }

         


            //var groupId = GetSkillGroupId(skillInfo.configId);
        }
    }

    public void SetSkillTipText(string str)
    {
        this.skillTipText.text = str;
        this.skillTipText.gameObject.SetActive(true);
        this.skillTipShowTimer = this.skillTipMaxShowTime;
    }

    public void Update(float deltaTime)
    {
        foreach (var item in skillShowObjList)
        {
            item.Update(deltaTime);
        }

        //bigSkillShowObj.Update(deltaTime);

        if (this.skillTipShowTimer > 0)
        {
            this.skillTipShowTimer -= deltaTime;
            if (this.skillTipShowTimer <= 0)
            {
                this.skillTipText.gameObject.SetActive(false);
            }
        }
    }

    public BattleSkillUIShowObj FindSkill(int skillId)
    {
        foreach (var skill in skillShowObjList)
        {
            if (skill.GetSkillConfigId() == skillId)
            {
                return skill;
            }
        }

        // if (this.bigSkillData != null && skillId == this.bigSkillData.skillId)
        // {
        //     return this.bigSkillShowObj;
        // }

        return null;
    }

    public void OnSkillRefreshAll()
    {
        this.RefreshAllUI();
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
        EventDispatcher.RemoveListener<int, BattleSkillInfo>(EventIDs.OnSkillInfoUpdate, OnSkillInfoUpdate);
        EventDispatcher.RemoveListener(EventIDs.OnSkillRefreshAll, OnSkillRefreshAll);

        EventDispatcher.RemoveListener<string>(EventIDs.OnSkillTips, OnSkillTips);

        foreach (var item in skillShowObjList)
        {
            item.Release();
        }

        skillShowObjList = null;
        // skillDataList = null;
    }
}