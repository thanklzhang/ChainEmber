using Battle_Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.UI;
public enum SkillDirectorProjectileType
{
    Null = 0,
    Rectangle = 1
}

public class SkillDirectorProjectile : BaseSkillDirector
{
    List<int> param;
    //矩形参数
    float rectXWidth;
    float rectZWidth;

    SkillDirectorProjectileType skillDirectorType;

    public override void OnInit(int _skillDirectorType, List<int> param)
    {
        this.skillDirectorType = (SkillDirectorProjectileType)_skillDirectorType;
        this.param = param;

        ParseParam(skillDirectorType, param);
    }

    public void ParseParam(SkillDirectorProjectileType skillDirectorType, List<int> param)
    {
        if (skillDirectorType == SkillDirectorProjectileType.Rectangle)
        {
            var paramsStrs = param;
            this.resourceId = paramsStrs[0];
            this.rectXWidth = paramsStrs[1] / 1000.0f;
            this.rectZWidth = paramsStrs[2] / 1000.0f;
        }
    }

    public override void OnShow()
    {
        var pre = transform.localScale;
        transform.localScale = new Vector3(this.rectXWidth, pre.y, this.rectZWidth);
    }


    public override void OnUpdate(float deltaTime)
    {
        this.transform.position = followEntity.transform.position;

        //跟随鼠标
        var mousePosition = mousePositionOnGround;
        Vector3 toMouseDir = mousePosition - this.transform.position;
        toMouseDir = new Vector3(toMouseDir.x, 0, toMouseDir.z);
        this.transform.forward = toMouseDir;
    }



}
