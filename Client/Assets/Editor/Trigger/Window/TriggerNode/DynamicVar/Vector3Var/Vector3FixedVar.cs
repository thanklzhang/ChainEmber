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
    public class Vector3FixedVar : Vector3Var
    {
        public Vector3 value;

        public override Vector3 Get()
        {
            return value;
        }

        public override void OnParse(JsonData nodeJsonData)
        {
            float x = float.Parse(nodeJsonData["x"].ToString());
            float y = float.Parse(nodeJsonData["y"].ToString());
            float z = float.Parse(nodeJsonData["z"].ToString());
            value = new Vector3(x, y, z);
        }

        public override void OnCreate()
        {
            value = Vector3.zero;
        }

        public override string GetDrawContentStr()
        {
            return "" + this.value;
        }

        public override void DrawSelectInfo()
        {
            value = EditorGUILayout.Vector3Field("", value, new GUILayoutOption[] { GUILayout.Width(200) });
        }

        public override Vector3Var OnClone()
        {
            Vector3FixedVar v3Var = new Vector3FixedVar();
            v3Var.value = this.value;
            return v3Var;
        }

        public override JsonData OnToJson(JsonData jd)
        {
            jd["x"] = this.value.x;
            jd["y"] = this.value.y;
            jd["z"] = this.value.z;
            return jd;
        }
    }
}