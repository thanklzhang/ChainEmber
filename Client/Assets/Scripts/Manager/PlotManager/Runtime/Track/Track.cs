using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;
using System.Text;
using LitJson;
using System;

namespace PlotDesigner.Runtime
{

    public enum TrackType
    {
        Animation = 0,
        Dialog = 1,
        Transform = 2,
        Word = 3,

    }


    [PolymorphismJsonAttribute]
    public class Track
    {
        //public TrackType type;
        public List<TrackNode> trackNodeList;

        public void Start()
        {
            this.OnStart();
        }

        public void Update()
        {
            this.OnUpdate();
        }

        internal void Init()
        {
            if (null == trackNodeList)
            {
                trackNodeList = new List<TrackNode>();
            }
        }

        public void End()
        {
            this.OnEnd();
        }

        public virtual void OnStart() { }

        public virtual void OnUpdate() { }

        internal void AddTrackNode(TrackNode insObj)
        {
            if (null == trackNodeList)
            {
                trackNodeList = new List<TrackNode>();
            }
            trackNodeList.Add(insObj);

        }

        internal void DeleteTrackNode(TrackNode insObj)
        {
            if (null != trackNodeList)
            {
                trackNodeList.Remove(insObj);
            }
            

        }

        public virtual void OnEnd() { }

    }
}