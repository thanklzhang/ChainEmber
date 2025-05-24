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
    public enum WordShowType
    {
        Null = 0,
        TypeWriter = 1,
    }

    public class WordTrackNode : TrackNode
    {
        public string word;
        public WordShowType showType;

    }

}
