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
    [Serializable]
    public class DelayTimeNodeGraph : TriggerNodeGraph
    {
        //public float delayTime;

        public NumberVarField delayTime;

        public override void OnParse(JsonData nodeJsonData)
        {
            //delayTime = (float.Parse(nodeJsonData["delayTime"]["value"].ToString()));
            delayTime = NumberVarField.ParseNumberVarField(nodeJsonData["delayTime"]);
        }

        public override void OnCreate()
        {
            delayTime = new NumberVarField();
            delayTime.Create();
        }

        public override string GetDrawContentStr()//Rect childRect
        {
            var str = "等待 " + delayTime.GetDrawContentStr() + " 秒";
            return str;
        }

        public override void DrawSelectInfo()
        {
            var style = new GUIStyle();
            style.stretchWidth = false;
            style.normal.textColor = Color.white;

            GUILayout.BeginHorizontal();
            GUILayout.Label("等待", style);

            delayTime.DrawSelectInfo();

            GUILayout.Label("秒", style);
            GUILayout.EndHorizontal();
        }

        public override TriggerNodeGraph OnClone()
        {
            TriggerNodeGraph node = new DelayTimeNodeGraph();
            var delayTimeNode = node as DelayTimeNodeGraph;
            delayTimeNode.delayTime = (NumberVarField)this.delayTime.Clone();

            return delayTimeNode;
        }

        public override JsonData OnToJson(JsonData jd)
        {
            jd["delayTime"] = delayTime.ToJson();
            return jd;
        }

    }
}