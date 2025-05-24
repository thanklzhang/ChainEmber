using Battle_Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.UI;

public enum SkillDirectorType
{
    Null = 0,
    DirectorProjectile,
    DirectorReleaserTerminal,
    DirectorTargetTerminal
}

public class SkillDirectorGroup
{
    public int skillId;
    public BaseSkillDirector projectileDirector;
    public BaseSkillDirector releaserDirector;
    public BaseSkillDirector targetDirector;

    public void Init(BaseSkillDirector projectileDirector,
        BaseSkillDirector releaserDirector,
        BaseSkillDirector targetDirector)
    {
        this.projectileDirector = projectileDirector;
        this.releaserDirector = releaserDirector;
        this.targetDirector = targetDirector;
    }

    public void Show(GameObject followGo)
    {
        projectileDirector?.Show(followGo);
        releaserDirector?.Show(followGo);
        targetDirector?.Show(followGo);
    }

    public void Update(float deltaTime)
    {   
        projectileDirector.Update(deltaTime);
        releaserDirector.Update(deltaTime);
        targetDirector.Update(deltaTime);
    }

    public void UpdateMousePosition(Vector3 mousePos)
    {
        projectileDirector.UpdateMousePosition(mousePos);
        releaserDirector.UpdateMousePosition(mousePos);
        targetDirector.UpdateMousePosition(mousePos);
    }

    public void Hide()
    {
        projectileDirector.Hide();
        releaserDirector.Hide();
        targetDirector.Hide();
    }

    public void Release()
    {
        projectileDirector.Release();
        releaserDirector.Release();
        targetDirector.Release();
    }
}

