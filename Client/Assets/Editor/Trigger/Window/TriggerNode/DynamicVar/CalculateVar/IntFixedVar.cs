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
    public class IntFixedVar : NumberFixedVar
    {
        public int value;

        public override float Get()
        {
            return value;
        }

        public int GetInt()
        {
            return value;
        }

        public override void OnParse(JsonData nodeJsonData)
        {
            value = int.Parse(nodeJsonData["value"].ToString());
        }

        public override JsonData OnToJson(JsonData jd)
        {
            jd["value"] = value;
            return jd;
        }
        public override NumberVar OnClone()
        {
            IntFixedVar numVar = new IntFixedVar();
            numVar.value = this.value;
            return numVar;
        }

        public override string GetDrawContentStr()
        {
            return "" + this.value;
        }

        public override void DrawSelectInfo()
        {
            value = EditorGUILayout.IntField(value, new GUILayoutOption[] { GUILayout.Width(100) });
        }

    }
}