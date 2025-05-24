using Battle_Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 矩形 track , 默认按照 z 方向为进度
/// </summary>
public class RectangleSkillTrack : BaseSkillTrack
{
    float length;
    float width;

    Transform showTran;
    Transform progressTran;

    protected override void OnInit()
    {
        //初始化配置
        this.length = this.config.Length / 1000.0f;
        this.width = this.config.Width / 1000.0f;
    }

    protected override void OnStart()
    {

    }

    protected override void OnLoadResFinish()
    {
        //资源加载完成 可以开始自定义的资源相关的显示和进度的初始化 
        showTran = this.trackResGo.transform.Find("show");
        progressTran = this.trackResGo.transform.Find("progress");

        var releaser = BattleEntityManager.Instance.FindEntity(this.trackBean.releaserGuid);
        if (null == releaser)
        {
            return;
        }

        //设置长度和宽度
        showTran.localScale = new Vector3(width, 1, length);
        progressTran.localScale = new Vector3(width, 1, 0);

        //设置方向
        var directType = (SkillTrackDirectType)this.config.DirectType;
        var dir = Vector3.forward;
        if (directType == SkillTrackDirectType.ReleaserToTargetPos)
        {
            var targetPos = GetTargetPos();
            dir = (targetPos - releaser.GetPosition()).normalized;
            dir.y = 0;
        }

        showTran.forward = dir;
        progressTran.forward = dir;

       
        //开始点位置
        var startPosType = (SkillStartPosType)this.config.StartPosType;
        if (startPosType == SkillStartPosType.ReleaserPos)
        {

            showTran.position = new Vector3(releaser.GetPosition().x, showTran.position.y, releaser.GetPosition().z);
            progressTran.position = new Vector3(releaser.GetPosition().x, showTran.position.y, releaser.GetPosition().z);
        }
        else if (startPosType == SkillStartPosType.TargetPos)
        {
            var targetPos = GetTargetPos();
            showTran.position = new Vector3(targetPos.x, showTran.position.y, targetPos.z);;
            progressTran.position = new Vector3(targetPos.x, showTran.position.y, targetPos.z);;
        }
    }

    protected override void OnUpdate(float deltaTime)
    {
        var releaser = BattleEntityManager.Instance.FindEntity(this.trackBean.releaserGuid);
        if (null == releaser)
        {
            return;
        }

        //按照进度显示
        var progress = (this.totalProgressTime - this.currProgressTimer) / this.totalProgressTime;
        var currLength = this.length * progress;
        progressTran.localScale = new Vector3(width, 1, currLength);

        //是否跟随
        if (1 == this.config.IsFollow)
        {
            showTran.position = releaser.GetPosition();
            progressTran.position = releaser.GetPosition();
        }

    }

    protected override void OnFinish()
    {

    }

    protected override void OnRelease()
    {

    }
}
