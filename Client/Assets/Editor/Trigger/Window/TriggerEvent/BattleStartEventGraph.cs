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
    public class BattleStartEventGraph : TriggerEventGraph
    {


        public override string GetDrawContentStr()//Rect childRect
        {
            var str = string.Format("当 战斗开始 的时候");
            return str;
        }

        public override void OnParse(JsonData nodeJsonData)
        {

        }

        public override void DrawSelectInfo()
        {
            var style = new GUIStyle();
            style.stretchWidth = false;
            style.normal.textColor = Color.white;

            GUILayout.BeginHorizontal();
            GUILayout.Label("当 战斗开始 的时候", style);
            GUILayout.EndHorizontal();
        }

        public override JsonData OnToJson(JsonData jd)
        {
            return jd;
        }


    }
}