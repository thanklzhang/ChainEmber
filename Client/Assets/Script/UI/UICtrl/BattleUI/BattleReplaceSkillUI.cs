using System;
using System.Collections;
using System.Collections.Generic;
using Battle;
using Battle_Client;
using Config;
using GameData;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ReplaceSkillCell
{
    public GameObject gameObject;
    public Transform transform;

    private Transform bgTran;
    private Image icon;
    private TextMeshProUGUI nameText;
    private TextMeshProUGUI levelText;
    private GameObject selectFlagGo;
    private Button clickBtn;

    public int skillId;

    private BattleReplaceSkillUI parentUI;
    private bool isOpSkill;

    public void Init(GameObject gameObject, BattleReplaceSkillUI parentUI, bool isOpSkill)
    {
        this.gameObject = gameObject;
        this.transform = this.gameObject.transform;

        this.isOpSkill = isOpSkill;

        this.parentUI = parentUI;

        bgTran = transform.Find("bg");

        nameText = bgTran.Find("name").GetComponent<TextMeshProUGUI>();
        levelText = bgTran.Find("level").GetComponent<TextMeshProUGUI>();
        icon = bgTran.Find("icon").GetComponent<Image>();
        selectFlagGo = bgTran.Find("selectFlag").gameObject;
        selectFlagGo.SetActive(false);
        clickBtn = icon.GetComponent<Button>();

        clickBtn.onClick.RemoveAllListeners();
        clickBtn.onClick.AddListener(OnClick);
    }

    protected void OnClick()
    {
        if (this.isOpSkill)
        {
            return;
        }
        
        this.parentUI.SelectSkill(this.skillId);
    }

    public void Refresh(int skillConfigId)
    {
        this.skillId = skillConfigId;
        if (this.skillId <= 0)
        {
            return;
        }

        var skillConfig = ConfigManager.Instance.GetById<Config.Skill>(skillConfigId);
        this.nameText.text = skillConfig.Name;
        this.levelText.text = "等级 " + skillConfig.Level;
        ResourceManager.Instance.GetObject<Sprite>(skillConfig.IconResId, (sprite) => { this.icon.sprite = sprite; });
    }

    public void SetSelectState(bool isSelect)
    {
        if (this.isOpSkill)
        {
            return;
        }

        if (this.skillId <= 0)
        {
            return;
        }

        selectFlagGo.SetActive(isSelect);
    }
}

public class BattleReplaceSkillUI : BaseUI
{
    private Transform replaceSkillRoot;
    private Transform opSkillRoot;
    private GameObject opSkillGo;
    private Button replaceBtn;
    private Button giveUpBtn;

    private List<ReplaceSkillCell> replaceSkillShowObjList;
    private ReplaceSkillCell opSkillShowObj;

    private ReplaceSkillCell currSelectSkillShowObj;

    protected override void OnInit()
    {
        this.uiResId = (int)ResIds.BattleReplaceSkillUI;
        this.uiShowLayer = UIShowLayer.Top_0;
        this.showMode = CtrlShowMode.Float;
    }

    protected override void OnLoadFinish()
    {
        var panel = transform.Find("Panel");
        var skillArea = panel.Find("skillArea");
        replaceSkillRoot = skillArea.transform.Find("leaderSkillRoot/layout");
        opSkillRoot = skillArea.transform.Find("opSkillRoot");
        replaceBtn = panel.transform.Find("ConfirmBtn").GetComponent<Button>();
        giveUpBtn = panel.transform.Find("GiveUpBtn").GetComponent<Button>();
        opSkillGo = opSkillRoot.Find("skillItem").gameObject;

        replaceBtn.onClick.RemoveAllListeners();
        replaceBtn.onClick.AddListener(this.OnClickReplaceBtn);

        giveUpBtn.onClick.RemoveAllListeners();
        giveUpBtn.onClick.AddListener(this.OnClickGiveUpBtn);
    }

    private BattleReplaceSkillUIArgs currUIArgs;

    protected override void OnOpen(UICtrlArgs args)
    {
        currUIArgs = (BattleReplaceSkillUIArgs)args;

        RefreshUI();
    }

    void RefreshUI()
    {
        replaceSkillShowObjList = new List<ReplaceSkillCell>();
        var mySkills = BattleManager.Instance.GetLocalCtrlHeroSkills();

        var leaderSkills = new List<BattleSkillInfo>();
        var _leaderSkills = mySkills.FindAll(skill =>
        {
            var skillConfig = ConfigManager.Instance.GetById<Config.Skill>(skill.configId);
            return (SkillCategory)skillConfig.SkillCategory == SkillCategory.LeaderSkill;
        });

        var player = BattleManager.Instance.GetLocalPlayer();
        var CM_1 = player.GetInputCommand(PlayerCommandType.Skill_Leader_1) as SkillCommandModel;
        var CM_2 = player.GetInputCommand(PlayerCommandType.Skill_Leader_2) as SkillCommandModel;
        ;
        var leaderSkill1 = mySkills.Find(skill => { return skill.configId == CM_1.skillConfigId; });
        leaderSkills.Add(leaderSkill1);

        var leaderSkill2 = mySkills.Find(skill => { return skill.configId == CM_2.skillConfigId; });
        leaderSkills.Add(leaderSkill2);

        //目前拥有的队长技能
        for (int i = 0; i < replaceSkillRoot.childCount; i++)
        {
            var child = replaceSkillRoot.GetChild(i);
            var skillShowObj = new ReplaceSkillCell();
            var skillData = leaderSkills[i];
            //理论上不会有空的时候 因为都要替换技能了。。。

            skillShowObj.Init(child.gameObject, this, false);
            skillShowObj.Refresh(skillData?.configId ?? 0);
            replaceSkillShowObjList.Add(skillShowObj);
        }

        SelectSkill(replaceSkillShowObjList[0].skillId);

        //将要替换的技能
        opSkillShowObj = new ReplaceSkillCell();
        opSkillShowObj.Init(opSkillGo, this, true);
        opSkillShowObj.Refresh(currUIArgs.opSkillId);
    }

    public void SelectSkill(int skillId)
    {
        if (currSelectSkillShowObj != null)
        {
            currSelectSkillShowObj.SetSelectState(false);
        }

        var skillShowObj = replaceSkillShowObjList.Find(s => s.skillId == skillId);
        currSelectSkillShowObj = skillShowObj;
        currSelectSkillShowObj.SetSelectState(true);
    }

    public void OnClickReplaceBtn()
    {
        var skillId = currSelectSkillShowObj.skillId;
        BattleManager.Instance.MsgSender.Send_SelectReplaceSkill(skillId);

        UIManager.Instance.Close<BattleReplaceSkillUI>();
    }

    public void OnClickGiveUpBtn()
    {
        var skillId = -1;
        BattleManager.Instance.MsgSender.Send_SelectReplaceSkill(skillId);

        UIManager.Instance.Close<BattleReplaceSkillUI>();
    }

    protected override void OnClose()
    {
    }
}

public class BattleReplaceSkillUIArgs : UICtrlArgs
{
    // public List<int> replaceSkillIdList;
    public int opSkillId;
}