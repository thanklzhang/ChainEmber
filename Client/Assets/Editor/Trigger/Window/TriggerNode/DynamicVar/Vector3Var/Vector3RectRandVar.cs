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
    public class Vector3RectRandVar : Vector3Var
    {
        public Vector3VarField randMin;
        public Vector3VarField randMax;

        public override void OnParse(JsonData nodeJsonData)
        {
            randMin = Vector3VarField.ParseVector3VarField(nodeJsonData["randMin"]);
            randMax = Vector3VarField.ParseVector3VarField(nodeJsonData["randMax"]);
        }

        public override void OnCreate()
        {
            randMin = new Vector3VarField();
            randMin.Create();

            randMax = new Vector3VarField();
            randMax.Create();
        }

        public override JsonData OnToJson(JsonData jd)
        {
            jd["randMin"] = randMin.ToJson();
            jd["randMax"] = randMax.ToJson();
            return jd;
        }

        public override Vector3Var OnClone()
        {
            Vector3RectRandVar v3Var = new Vector3RectRandVar();
            v3Var.randMin = (Vector3VarField)this.randMin.Clone();
            v3Var.randMax = (Vector3VarField)this.randMax.Clone();
            return v3Var;
        }

        public override string GetDrawContentStr()
        {
            var str = "";
            var aShowStr = randMin.GetDrawContentStr();
            var bShowStr = randMax.GetDrawContentStr();

            str = string.Format("( 左下角点为 {0} 且 右下角点为 {1} 的矩形中的随机点 )", aShowStr, bShowStr);

            return str;
        }

        public override void DrawSelectInfo()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("( 左下角点为", new GUILayoutOption[] { GUILayout.Width(70) });
            randMin.DrawSelectInfo();
            GUILayout.Label("且 右下角点为", new GUILayoutOption[] { GUILayout.Width(80) });
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label(" 的矩形中的随机点 )", new GUILayoutOption[] { GUILayout.Width(90) });
            GUILayout.EndHorizontal();
        }
    }
}