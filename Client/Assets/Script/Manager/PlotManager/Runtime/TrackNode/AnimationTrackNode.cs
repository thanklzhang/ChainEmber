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
    [System.Serializable]
    public class AnimationTrackNode : TrackNode
    {
        [SerializeField]
        public string aniName;
        [SerializeField]
        public bool isLoop;
    }

}
