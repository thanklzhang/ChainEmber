using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.U2D;
using UnityEngine.UI;
namespace PlotDesigner.Runtime
{

    public enum PlotTrackNodePlayerState
    {
        Null = 0,
        Ready = 1,
        Running = 2,
        End = 3,
        Destroy = 4
    }

    //单个剧情轨道节点播放者
    public class PlotTrackNodePlayer
    {

        protected TrackNode trackNode;
        public PlotTrackNodePlayerState state = PlotTrackNodePlayerState.Null;
        PlotTrackPlayer trackPlayer;
        public void Init(TrackNode node, PlotTrackPlayer trackPlayer)
        {
            this.trackNode = node;
            this.trackPlayer = trackPlayer;
            state = PlotTrackNodePlayerState.Ready;
            this.OnInit();
        }

        public void Start()
        {
            state = PlotTrackNodePlayerState.Running;
            this.OnStart();
        }

        public void Update(float timeDelta)
        {
            this.OnUpdate(timeDelta);
        }

        internal void End()
        {
            state = PlotTrackNodePlayerState.End;
        }

        public virtual void OnInit()
        {

        }

        public virtual void OnStart()
        {

        }

        public virtual void OnUpdate(float timeDelta)
        {

        }

        public virtual void OnEnd()
        {
            
        }

        // get function
        public float GetStartTime()
        {
            return trackNode.startTime;
        }

        internal float GetEndTime()
        {
            return trackNode.endTime;
        }

        public PlotEntity GetPlotEntityById(int id)
        {
            return trackPlayer.GetPlotEntityById(id);
        }

        public float GetCurrTime()
        {
            return this.trackPlayer.GetCurrTime();
        }

        public Transform GetPlotUIRoot()
        {
            return this.trackPlayer.GetPlotUIRoot();
        }

        //

       
    }
}

