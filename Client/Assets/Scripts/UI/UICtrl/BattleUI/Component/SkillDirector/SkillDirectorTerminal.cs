using Battle_Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Battle;
using UnityEngine.UI;
using Vector3 = UnityEngine.Vector3;

public enum SkillDirectorTerminalType
{
    Null = 0,
    RectangleRotate = 1,
    Circle = 2
}

public enum SkillDirectorRotateType
{
    Null = 0,

    //旋转到 释放者到目标的方向
    ReleaserToTarget = 1
}

//终端指示器 适用于施法者指示和目标指示
public class SkillDirectorTerminal : BaseSkillDirector
{
    List<int> param;

    //矩形范围参数
    float rectXWidth;
    float rectZWidth;
    SkillDirectorRotateType rotateType;

    //圆形范围参数
    float radius;

    SkillDirectorTerminalType skillDirectorType;

    public override void OnInit(int _skillDirectorType, List<int> param)
    {
        this.skillDirectorType = (SkillDirectorTerminalType)_skillDirectorType;
        this.param = param;

        RefreshDirectData();
    }


    public override void OnShow()
    {
        RefreshDirectShow();
    }

    public override void OnUpdate(float deltaTime)
    {
        this.transform.position = followEntity.transform.position;

        if (this.rotateType == SkillDirectorRotateType.Null)
        {
        }
        else if (this.rotateType == SkillDirectorRotateType.ReleaserToTarget)
        {
            //旋转到 释放者到目标的方向
            var mousePosition = mousePositionOnGround;
            Vector3 toMouseDir = mousePosition - this.transform.position;
            toMouseDir = new Vector3(toMouseDir.x, 0, toMouseDir.z);
            this.transform.forward = toMouseDir;
        }

        RefreshDirectData();
        RefreshDirectShow();
    }


    public void RefreshDirectData()
    {
        //parse param
        if (skillDirectorType == SkillDirectorTerminalType.RectangleRotate)
        {
            this.resourceId = this.param[0];
            this.rectXWidth = this.param[1] / 1000.0f;
            this.rectZWidth = this.param[2] / 1000.0f;
            this.rotateType = (SkillDirectorRotateType)this.param[3];
        }
        else if (skillDirectorType == SkillDirectorTerminalType.Circle)
        {
            this.resourceId = param[0];

            var skillCategory = (Battle.SkillCategory)skillConfig.SkillCategory;
            var localEntity = BattleManager.Instance.GetLocalCtrlHero();
            if (skillCategory == SkillCategory.NormalAttack)
            {
                this.radius = localEntity.attr.GetValue(EntityAttrType.AttackRange);
            }
            else
            {
                this.radius = param[1] / 1000.0f;
            }
        }
    }

    public void RefreshDirectShow()
    {
        //if (this.rotateType == SkillDirectorRotateType.Null)
        //{

        //}
        var pre = transform.localScale;

        if (skillDirectorType == SkillDirectorTerminalType.RectangleRotate)
        {
            transform.localScale = new Vector3(this.rectXWidth, pre.y, this.rectZWidth);
        }
        else if (skillDirectorType == SkillDirectorTerminalType.Circle)
        {
            transform.localScale = new Vector3(this.radius * 2, pre.y, this.radius * 2);
        }
    }
}