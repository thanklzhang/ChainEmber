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
    public class TimePassEventGraph : TriggerEventGraph
    {
        public float targetTime;

        public override string GetDrawContentStr()//Rect childRect
        {
            var str = string.Format("当 游戏时间过去 {0} 秒 的时候", targetTime);
            return str;
        }

        public override void OnParse(JsonData nodeJsonData)
        {
            targetTime = (float.Parse(nodeJsonData["targetTime"].ToString()));

        }

        public override void DrawSelectInfo()
        {
            var style = new GUIStyle();
            style.stretchWidth = false;
            style.normal.textColor = Color.white;

            GUILayout.BeginHorizontal();
            GUILayout.Label("当游戏时间过去 ", style);
            targetTime = EditorGUILayout.FloatField(targetTime, new GUILayoutOption[] { GUILayout.Width(40) });
            GUILayout.Label("秒 的时候", style);
            GUILayout.EndHorizontal();
        }

        public override JsonData OnToJson(JsonData jd)
        {
            jd["targetTime"] = new JsonData();
            jd["targetTime"] = targetTime;
            return jd;
        }


    }
}