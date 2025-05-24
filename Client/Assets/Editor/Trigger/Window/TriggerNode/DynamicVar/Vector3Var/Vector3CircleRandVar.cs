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
    public class Vector3CircleRandVar : Vector3Var
    {
        Vector3VarField center;
        NumberVarField radius;

        public override void OnParse(JsonData nodeJsonData)
        {
            center = Vector3VarField.ParseVector3VarField(nodeJsonData["center"]);
            radius = NumberVarField.ParseNumberVarField(nodeJsonData["radius"]);
        }

        public override void OnCreate()
        {
            center = new Vector3VarField();
            center.Create();

            radius = new NumberVarField();
            radius.Create();
        }

        public override JsonData OnToJson(JsonData jd)
        {
            jd["center"] = center.ToJson();
            jd["radius"] = radius.ToJson();
            return jd;
        }

        public override Vector3Var OnClone()
        {
            Vector3CircleRandVar v3Var = new Vector3CircleRandVar();
            v3Var.center = (Vector3VarField)this.center.Clone();
            v3Var.radius = (NumberVarField)this.radius.Clone();
            return v3Var;
        }

        public override string GetDrawContentStr()
        {
            var str = "";
            var aShowStr = center.GetDrawContentStr();
            var bShowStr = radius.GetDrawContentStr();

            str = string.Format("( 中心点为 {0} 且半径为 {1} 的圆中的随机点 )", aShowStr, bShowStr);

            return str;
        }

        public override void DrawSelectInfo()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("( 中心点为 ", new GUILayoutOption[] { GUILayout.Width(60) });
            center.DrawSelectInfo();
            GUILayout.Label("且半径为  ", new GUILayoutOption[] { GUILayout.Width(60) });
            radius.DrawSelectInfo();
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label(" 的圆中的随机点 )", new GUILayoutOption[] { GUILayout.Width(90) });
            GUILayout.EndHorizontal();
        }
    }
}