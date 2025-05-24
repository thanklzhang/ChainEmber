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

public class ReplaceHeroCell
{
    public GameObject gameObject;
    public Transform transform;

    // private Transform bgTran;
    // private Image icon;
    private TextMeshProUGUI nameText;
    // private TextMeshProUGUI levelText;
    private GameObject selectFlagGo;
    private Button clickBtn;

    public int entityConfigId;

    private BattleReplaceHeroUI parentUI;
    private bool isOpSkill;

    private BattleHeroAvatar heroAvatar;

    public void Init(GameObject gameObject, BattleReplaceHeroUI parentUI, bool isOpSkill)
    {
        this.gameObject = gameObject;
        this.transform = this.gameObject.transform;

        // this.isOpSkill = isOpSkill;

        this.parentUI = parentUI;

        var heroAvatarRoot = this.transform.Find("BattleHeroAvatar");
        heroAvatar = new BattleHeroAvatar();
        heroAvatar.Init(heroAvatarRoot.gameObject);
        // bgTran = transform.Find("bg");
        //
         nameText = transform.Find("name").GetComponent<TextMeshProUGUI>();
        // levelText = bgTran.Find("level").GetComponent<TextMeshProUGUI>();
        // icon = bgTran.Find("icon").GetComponent<Image>();
        selectFlagGo = transform.Find("selectFlag").gameObject;
        selectFlagGo.SetActive(false);
        // clickBtn = icon.GetComponent<Button>();
        //
        // clickBtn.onClick.RemoveAllListeners();
        // clickBtn.onClick.AddListener(OnClick);
        
        heroAvatar.RemoveClickListener();
        heroAvatar.AddClickListener((a) =>
        {
            OnClick();
        });
    }

    protected void OnClick()
    {
        this.parentUI.SelectHero(this.entityConfigId);
    }

    //TODO 参数需要封装
    public void Refresh(int skillConfigId, int level, int starLevel)
    {
        this.entityConfigId = skillConfigId;
        if (this.entityConfigId <= 0)
        {
            return;
        }

        heroAvatar.Refresh(new BattleHeroShowData()
        {
            configId = skillConfigId,
            level = level,
            star = starLevel
        });

        var skillConfig = ConfigManager.Instance.GetById<Config.EntityInfo>(skillConfigId);
        this.nameText.text = skillConfig.Name;
        // this.levelText.text = "等级 " + skillConfig.Level;
        // ResourceManager.Instance.GetObject<Sprite>(skillConfig.IconResId, (sprite) => { this.icon.sprite = sprite; });
    }

    public void SetSelectState(bool isSelect)
    {
        if (this.isOpSkill)
        {
            return;
        }

        if (this.entityConfigId <= 0)
        {
            return;
        }

        selectFlagGo.SetActive(isSelect);
    }
}

public class BattleReplaceHeroUI : BaseUI
{
    private Transform replaceHeroRoot;
    private Transform opHeroRoot;
    private GameObject opHeroGo;
    private Button replaceBtn;
    private Button giveUpBtn;

    private List<ReplaceHeroCell> replaceHeroShowObjList;
    private ReplaceHeroCell opHeroShowObj;

    private ReplaceHeroCell currSelectHeroShowObj;

    protected override void OnInit()
    {
        this.uiResId = (int)ResIds.BattleReplaceHeroUI;
        this.uiShowLayer = UIShowLayer.Top_0;
        this.showMode = CtrlShowMode.Float;
    }

    protected override void OnLoadFinish()
    {
        var panel = transform.Find("Panel");
        var entityArea = panel.Find("entityArea");
        replaceHeroRoot = entityArea.transform.Find("allEntityRoot/scroll/mask/layout");
        opHeroRoot = entityArea.transform.Find("opEntityRoot");
        replaceBtn = panel.transform.Find("ConfirmBtn").GetComponent<Button>();
        giveUpBtn = panel.transform.Find("GiveUpBtn").GetComponent<Button>();
        opHeroGo = opHeroRoot.Find("item").gameObject;

        replaceBtn.onClick.RemoveAllListeners();
        replaceBtn.onClick.AddListener(this.OnClickReplaceBtn);

        giveUpBtn.onClick.RemoveAllListeners();
        giveUpBtn.onClick.AddListener(this.OnClickGiveUpBtn);
    }

    private BattleReplaceHeroUIArgs currUIArgs;

    protected override void OnOpen(UICtrlArgs args)
    {
        currUIArgs = (BattleReplaceHeroUIArgs)args;

        RefreshUI();
    }

    void RefreshUI()
    {
        replaceHeroShowObjList = new List<ReplaceHeroCell>();
        List<BattleEntity_Client> entityList = new List<BattleEntity_Client>();
        var localPlayer = BattleManager.Instance.GetLocalPlayer();
        var teamMemberGuid = localPlayer.teamMemberGuids;
        //除去自己实体
        for (int i = 0; i < teamMemberGuid.Count; i++)
        {
            var guid = teamMemberGuid[i];
            var entity = BattleEntityManager.Instance.FindEntity(guid);
            if (entity != null)
            {
                if (entity.guid != BattleManager.Instance.GetLocalCtrlHeroGuid())
                {
                    entityList.Add(entity);
                }
            }
        }

        // var leaderSkills = new List<BattleSkillInfo>();
        // var _leaderSkills = mySkills.FindAll(skill =>
        // {
        //     var skillConfig = ConfigManager.Instance.GetById<Config.Skill>(skill.configId);
        //     return (SkillCategory)skillConfig.SkillCategory == SkillCategory.LeaderSkill;
        // });
        //
        // var player = BattleManager.Instance.GetLocalPlayer();
        // var CM_1 = player.GetInputCommand(PlayerCommandType.Skill_Leader_1) as SkillCommandModel;
        // var CM_2 = player.GetInputCommand(PlayerCommandType.Skill_Leader_2) as SkillCommandModel;
        // ;
        // var leaderSkill1 = mySkills.Find(skill => { return skill.configId == CM_1.skillConfigId; });
        // leaderSkills.Add(leaderSkill1);
        //
        // var leaderSkill2 = mySkills.Find(skill => { return skill.configId == CM_2.skillConfigId; });
        // leaderSkills.Add(leaderSkill2);

        //目前拥有的英雄

        for (int i = 0; i < entityList.Count; i++)
        {
            var entity = entityList[i];
            GameObject go = null;
            if (i < replaceHeroRoot.childCount)
            {
                var child = replaceHeroRoot.GetChild(i);
                go = child.gameObject;
            }
            else
            {
                var child = GameObject.Instantiate(replaceHeroRoot.GetChild(0).gameObject,
                    replaceHeroRoot, false);
                go = child.gameObject;
            }
            var heroShowObj = new ReplaceHeroCell();
            heroShowObj.Init(go, this, false);
            heroShowObj.Refresh(entity.configId, entity.level, entity.starLv);

            replaceHeroShowObjList.Add(heroShowObj);
        }
        
        
        // for (int i = 0; i < replaceHeroRoot.childCount; i++)
        // {
        //     var child = replaceHeroRoot.GetChild(i);
        //     var skillShowObj = new ReplaceHeroCell();
        //     var entityData = entityList[i];
        //     //理论上不会有空的时候 因为都要替换技能了。。。
        //
        //     skillShowObj.Init(child.gameObject, this, false);
        //     skillShowObj.Refresh(entityData.configId, entityData.level, entityData.starLv);
        //     replaceHeroShowObjList.Add(skillShowObj);
        // }

        SelectHero(replaceHeroShowObjList[0].entityConfigId);

        //将要替换的英雄
        opHeroShowObj = new ReplaceHeroCell();
        opHeroShowObj.Init(opHeroGo, this, true);
        opHeroShowObj.Refresh(currUIArgs.opEntityConfigId, 1, 1);
    }

    public void SelectHero(int skillId)
    {
        if (currSelectHeroShowObj != null)
        {
            currSelectHeroShowObj.SetSelectState(false);
        }

        var skillShowObj = replaceHeroShowObjList.Find(s => s.entityConfigId == skillId);
        currSelectHeroShowObj = skillShowObj;
        currSelectHeroShowObj.SetSelectState(true);
    }

    public void OnClickReplaceBtn()
    {
        var configId = currSelectHeroShowObj.entityConfigId;
        BattleManager.Instance.MsgSender.Send_SelectReplaceHero(configId);

        UIManager.Instance.Close<BattleReplaceHeroUI>();
    }

    public void OnClickGiveUpBtn()
    {
        var configId = -1;
        BattleManager.Instance.MsgSender.Send_SelectReplaceHero(configId);

        UIManager.Instance.Close<BattleReplaceHeroUI>();
    }

    protected override void OnClose()
    {
    }
}

public class BattleReplaceHeroUIArgs : UICtrlArgs
{
    // public List<int> replaceSkillIdList;
    public int opEntityConfigId;
}