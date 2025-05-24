using Battle_Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Config;
using UnityEngine;
using UnityEngine.UI;

public class TrackBean
{
    public int releaserGuid;
    public int trackConfigId;
    public int targetEntityGuid;
    public Vector3 targetPos;
}

public enum SkillTrackType
{
    Circle = 0,
    Rectangle = 1,
    Sector = 2
}

public class SkillTrackModule
{
    // BattleCtrlPre _battleCtrlPre;
    public Dictionary<int, SkillTrackGroup> trackGroupDic;

    public void Init()
    {
        // this._battleCtrlPre = battleCtrlPre;
        trackGroupDic = new Dictionary<int, SkillTrackGroup>();
    }

    public void AddTrack(TrackBean trackBean)
    {
        BaseSkillTrack track = null;
        SkillTrackGroup group = null;
        var guid = trackBean.releaserGuid;
        if (!trackGroupDic.ContainsKey(guid))
        {
            group = new SkillTrackGroup();
            group.Init();

            trackGroupDic.Add(guid, group);
        }
        else
        {
            group = trackGroupDic[guid];
        }

        track = CreateTrack(trackBean);
        group.AddTrack(track);

        track.Start();
    }

    BaseSkillTrack CreateTrack(TrackBean trackBean)
    {
        BaseSkillTrack skillTrack = null;
        var config = ConfigManager.Instance.GetById<Config.SkillTrack>(trackBean.trackConfigId);

        var type = (SkillTrackType)config.Type;
        if (type == SkillTrackType.Rectangle)
        {
            skillTrack = new RectangleSkillTrack();
            skillTrack.Init(trackBean);
        }
        else if (type == SkillTrackType.Circle)
        {
            skillTrack = new CircleSkillTrack();
            skillTrack.Init(trackBean);
        }
        else if (type == SkillTrackType.Sector)
        {
            skillTrack = new SectorSkillTrack();
            skillTrack.Init(trackBean);
        }

        return skillTrack;
    }

    public void DeleteTrack(int entityGuid, int trackConfigId)
    {
        if (trackGroupDic.ContainsKey(entityGuid))
        {
            var group = trackGroupDic[entityGuid];
            group.DeleteTrack(trackConfigId);
        }
        else
        {
            //Logx.LogWarning("the tarck is not found : entityGuid : " + entityGuid);
        }
    }

    public void Update(float deltaTime)
    {
        foreach (var item in trackGroupDic)
        {
            var trackGroup = item.Value;
            trackGroup.Update(deltaTime);
        }

    }

    public void OnBattleEnd()
    {
        foreach (var item in trackGroupDic)
        {
            var trackGroup = item.Value;
            trackGroup.OnBattleEnd();
        }
    }

    public void Release()
    {
        foreach (var item in trackGroupDic)
        {
            var trackGroup = item.Value;
            trackGroup.Release();
        }
    }
}