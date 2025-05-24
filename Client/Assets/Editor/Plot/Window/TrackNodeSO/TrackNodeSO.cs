using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;
using System.Text;
using LitJson;
using System;
using PlotDesigner.Runtime;

namespace PlotDesigner.Editor
{
    public class TrackNodeSO : ScriptableObject
    { 
        public TrackNode trackNode;
        public bool isSelect;
    }
}