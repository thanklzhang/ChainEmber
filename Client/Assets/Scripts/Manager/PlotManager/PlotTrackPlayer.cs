using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.U2D;
using UnityEngine.UI;
namespace PlotDesigner.Runtime
{
    //单个剧情轨道播放者
    public class PlotTrackPlayer
    {
        Track trackData;

        List<PlotTrackNodePlayer> trackNodePlayerList;

        PlotPlayer plotPlayer;

        string assembleName;

        public virtual void Init(Track trackData, PlotPlayer plotPlayer)
        {
            assembleName = "Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null";

            this.trackData = trackData;
            this.plotPlayer = plotPlayer;

            trackNodePlayerList = new List<PlotTrackNodePlayer>();

            for (int i = 0; i < trackData.trackNodeList.Count; i++)
            {
                var trackNodeData = trackData.trackNodeList[i];

                PlotTrackNodePlayer trackNodePlayer = CreateTrackNodePlayerByTrackNodeDataType(trackNodeData.GetType());
                if (trackNodePlayer != null)
                {
                    trackNodePlayer.Init(trackNodeData, this);
                    trackNodePlayerList.Add(trackNodePlayer);
                }
                else
                {
                    var trackNodePlayerName = "Plot" + trackNodeData.GetType().Name + "Player";
                    Logx.LogError("PlotTrackNodePlayer : Init : create object fail , the type of src is : " + trackNodeData.GetType() + ", perhaps the trackPlayer is not exist : " + trackNodePlayerName);
                }
            }
        }

        private PlotTrackNodePlayer CreateTrackNodePlayerByTrackNodeDataType(Type type)
        {
            var trackPlayerName = "Plot" + type.Name + "Player";
            var nameSpaceName = "PlotDesigner.Runtime";
            var fullName = nameSpaceName + "." + trackPlayerName;
            PlotTrackNodePlayer trackPlayer = null;
            //Logx.Log("CreateInstance : " + assembleName + " : " + fullName);
            trackPlayer = Assembly.Load(assembleName).CreateInstance(fullName) as PlotTrackNodePlayer;
            return trackPlayer;
        }

        internal Transform GetPlotUIRoot()
        {
            return this.plotPlayer.GetPlotUIRoot();
        }

        internal PlotEntity GetPlotEntityById(int id)
        {
            return this.plotPlayer.GetPlotEntityById(id);
        }


        //生命周期

        public void StartExecute()
        {
            this.OnStartExecute();
        }

        public void Update(float timeDelta)
        {
            //List<PlotTrackNodePlayer> willEndTrackNodeList = new List<PlotTrackNodePlayer>();

            var currTime = this.GetCurrTime();
            foreach (var trackNodePlayer in trackNodePlayerList)
            {
                //这里可能同帧调用 start 和 end


                if (trackNodePlayer.state == PlotTrackNodePlayerState.Ready)
                {
                    if (currTime >= trackNodePlayer.GetStartTime())
                    {
                        trackNodePlayer.Start();
                    }
                }

                if (trackNodePlayer.state == PlotTrackNodePlayerState.Running)
                {
                    trackNodePlayer.Update(timeDelta);

                    if (currTime >= trackNodePlayer.GetEndTime())
                    {
                        trackNodePlayer.End();
                    }
                }
            }
            this.OnUpdate(timeDelta);
        }

        public virtual void OnStartExecute()
        {

        }

        public virtual void OnUpdate(float timeDelta)
        {

        }

        //

        public float GetCurrTime()
        {
            return plotPlayer.GetCurrTime();
        }

        internal float GetMaxEndTime()
        {
            float maxEndTime = 0;
            foreach (var nodePlayer in this.trackNodePlayerList)
            {
                if (nodePlayer.GetEndTime() >= maxEndTime)
                {
                    maxEndTime = nodePlayer.GetEndTime();
                }
            }
            return maxEndTime;
        }
    }
}

