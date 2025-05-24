using Battle_Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 圆形 track , 默认按照 从内向外 方向为进度 , width 和 height 配成一样即为圆
/// </summary>
public class SectorSkillTrack : BaseSkillTrack
{
    float length;
    float width;
    float angle; // 扇形角度

    Transform showTran;
    Transform progressTran;
    Material sectorMaterial; // 用于控制扇形进度效果
    Material showMaterial; // 用于控制显示扇形效果

    protected override void OnInit()
    {
        //初始化配置
        this.length = this.config.Length / 1000.0f;
        this.width = this.config.Width / 1000.0f;
        this.angle = this.config.Angle > 0 ? this.config.Angle : 180; // 获取配置角度，默认为360度
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
        progressTran.localScale = new Vector3(0, 1, 0);
   
        // 设置显示扇形(show)的材质参数
        MeshRenderer showRenderer = showTran.GetComponentInChildren<MeshRenderer>();
        if (showRenderer != null)
        {
            showMaterial = showRenderer.material;
            if (showMaterial != null && showMaterial.HasProperty("_Angle"))
            {
                showMaterial.SetFloat("_Angle", angle / 180.0f);
                // 显示扇形的进度始终为1
                if (showMaterial.HasProperty("_Progress"))
                {
                    showMaterial.SetFloat("_Progress", 1.0f);
                }
            }
        }
        
        // 设置进度扇形(progress)的材质参数
        MeshRenderer progressRenderer = progressTran.GetComponentInChildren<MeshRenderer>();
        if (progressRenderer != null)
        {
            sectorMaterial = progressRenderer.material;
            if (sectorMaterial != null && sectorMaterial.HasProperty("_Angle"))
            {
                sectorMaterial.SetFloat("_Angle", angle / 180.0f);
                sectorMaterial.SetFloat("_Progress", 0);
            }
        }

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
        
        if (sectorMaterial != null && sectorMaterial.HasProperty("_Progress"))
        {
            // 更新扇形材质的进度
            sectorMaterial.SetFloat("_Progress", progress);
            // 根据进度设置缩放值
            progressTran.localScale = new Vector3(width * progress, 1, length * progress);
        }
        else
        {
            // 原来的线性缩放方式作为备选
            var currLength = this.length * progress;
            var currWidth = this.width * progress;
            progressTran.localScale = new Vector3(currWidth, 1, currLength);
        }

        //是否跟随
        if (1 == this.config.IsFollow)
        {
            showTran.position = releaser.GetPosition();
            progressTran.position = releaser.GetPosition();
            
            // 如果有目标，更新朝向
            var directType = (SkillTrackDirectType)this.config.DirectType;
            if (directType == SkillTrackDirectType.ReleaserToTargetPos)
            {
                var targetPos = GetTargetPos();
                var dir = (targetPos - releaser.GetPosition()).normalized;
                dir.y = 0;
                
                showTran.forward = dir;
                progressTran.forward = dir;
            }
            // 否则保持与释放者朝向一致
            else
            {
                // 使用释放者的GameObject的forward方向
                Vector3 releaserForward = releaser.gameObject.transform.forward;
                releaserForward.y = 0;
                
                showTran.forward = releaserForward;
                progressTran.forward = releaserForward;
            }
        }
    }

    protected override void OnFinish()
    {
        if (sectorMaterial != null && sectorMaterial.HasProperty("_Progress"))
        {
            // 更新扇形材质的进度
            sectorMaterial.SetFloat("_Progress", 1.0f);
        }
    }

    protected override void OnRelease()
    {

    }
}
