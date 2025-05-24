using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;
using System.Text;
using LitJson;

namespace PlotDesigner.Runtime
{


    //public enum TrackNodeState
    //{
    //    Ready,
    //    Running,
    //    End
    //}
    [System.Serializable]
    [PolymorphismJsonAttribute]
    public class TrackNode
    {
        public int id;//唯一 id 用于查找和重复使用资源
        public int resId;
        //public int type;

        public float startTime;
        public float endTime;

        //public TrackNodeState state;

        public void Start()
        {
            //state = TrackNodeState.Running;
            this.OnStart();
        }

        public void Update(float currTime)
        {
            this.OnUpdate(currTime);
        }

        public void End()
        {
            this.OnEnd();
        }

        public virtual void OnStart() { }

        public virtual void OnUpdate(float currTime) { }

        public virtual void OnEnd() { }


    }
}