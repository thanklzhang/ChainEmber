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
    public class TransformTrackNode : TrackNode
    {
        public Vector3 startPos;
        public Vector3 endPos;

        public Vector3 startForward;
        public Vector3 endForward;

    }

}

