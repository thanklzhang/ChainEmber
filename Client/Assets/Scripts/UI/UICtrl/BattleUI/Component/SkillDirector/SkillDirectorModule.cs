using Battle_Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Config;
using UnityEngine;
using UnityEngine.UI;


public class SkillDirectorModule
{
    // BattleCtrlPre _battleCtrlPre;
    bool selectState;
    private Config.Skill skillConfig;

    public Dictionary<int, SkillDirectorGroup> directorGroupDic;
    SkillDirectorGroup currDirectorGroup;

    public void Init()
    {
        // this._battleCtrlPre = ctrlPre;
        directorGroupDic = new Dictionary<int, SkillDirectorGroup>();
    }

    public void StartSelect(int skillId, GameObject originGameObject)
    {
        selectState = true;

        //生成投掷物指示器
        if (directorGroupDic.ContainsKey(skillId))
        {
            currDirectorGroup = directorGroupDic[skillId];
        }
        else
        {
            this.skillConfig = Config.ConfigManager.Instance.GetById<Config.Skill>(skillId);

            var skillDirId = skillConfig.SkillDirectionId;
            if (0 == skillDirId)
            {
                return;
            }
            
            var pDirector = GenNewDirector(SkillDirectorType.DirectorProjectile);
            var rDirector = GenNewDirector(SkillDirectorType.DirectorReleaserTerminal);
            var tDirector = GenNewDirector(SkillDirectorType.DirectorTargetTerminal);

          
            
            SkillDirectorGroup group = new SkillDirectorGroup();
            group.Init(pDirector, rDirector, tDirector);


            directorGroupDic.Add(skillId, group);
            currDirectorGroup = group;
        }
        
     
        

        currDirectorGroup?.Show(originGameObject);

        OperateViewManager.Instance.SetCursor(CursorType.Select);


    }

    public BaseSkillDirector GenNewDirector(SkillDirectorType type)
    {
        BaseSkillDirector skillDirector = null;
        var skillDirId = skillConfig.SkillDirectionId;
        if (0 == skillDirId)
        {
            return null;
        }

        var skillDirConfig = ConfigManager.Instance.GetById<Config.SkillDirection>(skillDirId);
        if (null == skillDirConfig)
        {
            return null;
        }

        if (type == SkillDirectorType.DirectorProjectile)
        {
            skillDirector = new SkillDirectorProjectile();
            skillDirector.Init(skillDirConfig.SkillDirectorProjectileType, skillDirConfig.SkillDirectorProjectileParam,this.skillConfig);
        }
        else if (type == SkillDirectorType.DirectorReleaserTerminal)
        {
            skillDirector = new SkillDirectorTerminal();
            skillDirector.Init(skillDirConfig.SkillReleaserDirectType, skillDirConfig.SkillReleaserDirectParam,this.skillConfig);
        }
        else if (type == SkillDirectorType.DirectorTargetTerminal)
        {
            skillDirector = new SkillDirectorTerminal();
            skillDirector.Init(skillDirConfig.SkillTargetDirectType, skillDirConfig.SkillTargetDirectParam,this.skillConfig);

        }

        return skillDirector;
    }

    public void FinishSelect()
    {
        selectState = false;

        currDirectorGroup?.Hide();

        OperateViewManager.Instance.SetCursor(CursorType.Normal);

    }

    public void Update(float deltaTime)
    {
        foreach (var item in directorGroupDic)
        {
            var directorGroup = item.Value;
            directorGroup.Update(deltaTime);
        }
    }

    internal void UpdateMousePosition(Vector3 resultPos)
    {
        foreach (var item in directorGroupDic)
        {
            var directorGroup = item.Value;
            directorGroup.UpdateMousePosition(resultPos);
        }
    }

    public bool GetSelectState()
    {
        return this.selectState;
    }


    public void Release()
    {
        foreach (var item in directorGroupDic)
        {
            item.Value.Release();
        }
    }


}
