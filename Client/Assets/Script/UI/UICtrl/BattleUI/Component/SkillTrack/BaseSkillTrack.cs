using Battle_Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.UI;

public enum SkillTrackDirectType
{
    Null = 0,
    //施法者 到 目标点 的 方向
    ReleaserToTargetPos = 1,
}

public enum SkillStartPosType
{
    Null = 0,
    ReleaserPos = 1,
    TargetPos = 2
}

public class BaseSkillTrack
{
    public int configId;
    protected Config.SkillTrack config;
    protected BattleEntity_Client entity;

    protected int resourceId;
    protected GameObject trackResGo;

    protected float currProgressTimer;
    protected float totalProgressTime;

    protected bool isFinishLoad;
    protected bool isFinish;

    public bool isWillDelete;
    protected TrackBean trackBean;

    private bool isBattleEnd;
    public void Init(TrackBean trackBean)
    {
        this.trackBean = trackBean;

        this.configId = this.trackBean.trackConfigId;
        this.config = Config.ConfigManager.Instance.GetById<Config.SkillTrack>(this.configId);
        totalProgressTime = this.config.ProgressFinishTime / 1000.0f;
        this.resourceId = config.EffectResId;
        OnInit();
    }
    protected virtual void OnInit()
    {

    }
    public void Start()
    {
        isFinishLoad = false;
        isFinish = false;
        currProgressTimer = totalProgressTime;

        StartLoad();

        OnStart();
    }

    protected virtual void StartLoad()
    {
        if (this.resourceId > 0)
        {
            ResourceManager.Instance.GetObject<GameObject>(this.resourceId, LoadResFinish);
        }

    }

    protected void LoadResFinish(GameObject gameObject)
    {
        isFinishLoad = true;
        this.trackResGo = gameObject;
        OnLoadResFinish();
    }

    protected virtual void OnLoadResFinish()
    {

    }

    protected virtual void OnStart()
    {

    }

    public void Update(float deltaTime)
    {
        if (isFinish)
        {
            
            if (isBattleEnd)
            {
                //只有战斗结束的时候 本地 track 完成后直接删除
                //否则正常情况下是服务端通知删除
                isWillDelete = true;
            }
            
            return;
        }

        if (isFinishLoad)
        {
            OnUpdate(deltaTime);
        }

        currProgressTimer -= deltaTime;
        if (currProgressTimer <= 0)
        {
            this.Finish();
        }
    }
    protected virtual void OnUpdate(float deltaTime)
    {

    }
    public void Finish()
    {
        isFinish = true;
        OnFinish();
      
    }
    protected virtual void OnFinish()
    {

    }
    public void Release()
    {
        if (trackResGo != null)
        {
            if (this.resourceId > 0)
            {
                ResourceManager.Instance.ReturnObject(this.resourceId, this.trackResGo);
            }
            trackResGo = null;
        }
       
        OnRelease();
    }
    protected virtual void OnRelease()
    {

    }

    //--Get--------------------------
    public Vector3 GetTargetPos()
    {
        var targetPos = this.trackBean.targetPos;
        if (this.trackBean.targetEntityGuid > 0)
        {
            var targetEntity = BattleEntityManager.Instance.FindEntity(this.trackBean.targetEntityGuid);
            if (targetEntity != null)
            {
                targetPos = targetEntity.GetPosition();
            }
        }

        return targetPos;
    }

    //-------------------------------
    public void OnBattleEnd()
    {
        isBattleEnd = true;
    }
}
