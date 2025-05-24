using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;
using System.Text;
using LitJson;
using System;
using System.Reflection;
using PlotDesigner.Runtime;

namespace BattleTrigger.Editor
{
    public class AssShowStrGraph : AssistantShowNodeGraph
    {
        public override string GetDrawContentStr()//Rect childRect
        {
            var str = string.Format(this.showStr);
            return str;
        }

        public override void Draw()
        {
            GUILayout.Space(3);
            base.Draw();
        }

        public override void OnCreate()
        {
            //headOffsetX = -10.0f;
        }

        public override void DrawSelectInfo()
        {

        }

        public override TriggerNodeGraph OnClone()
        {
            TriggerNodeGraph node = new AssShowStrGraph();
            var actionShowNode = node as AssShowStrGraph;
            return actionShowNode;
        }

        public override JsonData OnToJson(JsonData jd)
        {
            return null;
        }

    }
}