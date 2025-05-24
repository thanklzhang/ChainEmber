using Battle_Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.UI;


public class SkillTrackGroup
{
    BattleEntity_Client entity;
    List<BaseSkillTrack> skillTrackList;
    public void Init()
    {
        skillTrackList = new List<BaseSkillTrack>();
    }
    public void OnInit()
    {

    }
    internal void AddTrack(BaseSkillTrack track)
    {
        skillTrackList.Add(track);
    }

    public void Update(float deltaTime)
    {
        foreach (var track in skillTrackList)
        {
            track.Update(deltaTime);
        }

        for (int i = skillTrackList.Count - 1; i >= 0; i--)
        {
            var track = skillTrackList[i];
            if (track.isWillDelete)
            {
                track.Release();
                skillTrackList.RemoveAt(i);
            }
        }
    }

    public bool IsEmpty()
    {
        return 0 == skillTrackList.Count;
    }

    internal void DeleteTrack(int trackConfigId)
    {
        for (int i = skillTrackList.Count - 1; i >= 0; i--)
        {
            var track = skillTrackList[i];
            if (track.configId == trackConfigId)
            {
                track.isWillDelete = true;
            }
        }
    }

    public void Release()
    {
        for (int i = skillTrackList.Count - 1; i >= 0; i--)
        {
            var track = skillTrackList[i];
            track.Release();
        }
    }


    public void OnBattleEnd()
    {
        for (int i = skillTrackList.Count - 1; i >= 0; i--)
        {
            var track = skillTrackList[i];
            track.OnBattleEnd();
        }
    }
}
