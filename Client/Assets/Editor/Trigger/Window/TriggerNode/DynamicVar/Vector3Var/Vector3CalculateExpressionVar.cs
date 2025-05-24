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
    public class Vector3CalculateExpressionVar : Vector3Var
    {
        Vector3VarField aField;
        public Vector3CalculateVarType op;
        Vector3VarField bField;

        public override Vector3 Get()
        {
            Vector3 result = Vector3.zero;
            if (op == Vector3CalculateVarType.Plus)
            {
                result = aField.Get() + bField.Get();
            }
            else if (op == Vector3CalculateVarType.Minus)
            {
                result = aField.Get() - bField.Get();
            }

            return result;
        }

        public override void OnParse(JsonData nodeJsonData)
        {
            aField = Vector3VarField.ParseVector3VarField(nodeJsonData["aField"]);
            op = (Vector3CalculateVarType)int.Parse(nodeJsonData["op"].ToString());
            bField = Vector3VarField.ParseVector3VarField(nodeJsonData["bField"]);
        }

        public override void OnCreate()
        {
            aField = new Vector3VarField();
            aField.Create();

            bField = new Vector3VarField();
            bField.Create();
        }

        public override JsonData OnToJson(JsonData jd)
        {
            jd["aField"] = aField.ToJson();
            jd["op"] = (int)op;
            jd["bField"] = bField.ToJson();
            return jd;
        }

        public override Vector3Var OnClone()
        {
            Vector3CalculateExpressionVar v3Var = new Vector3CalculateExpressionVar();
            v3Var.aField = (Vector3VarField)this.aField.Clone();
            v3Var.op = this.op;
            v3Var.bField = (Vector3VarField)this.bField.Clone();
            return v3Var;
        }

        public override string GetDrawContentStr()
        {
            var str = "";
            var aShowStr = aField.GetDrawContentStr();
            var opStr = Vector3Var.GetCompareTypeStr(this.op);
            var bShowStr = bField.GetDrawContentStr();

            str = string.Format("( {0} {1} {2} )", aShowStr, opStr, bShowStr);

            return str;
        }

        public override void DrawSelectInfo()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("(", new GUILayoutOption[] { GUILayout.Width(10) });
            aField.DrawSelectInfo();
            GUILayout.EndHorizontal();

            op = (Vector3CalculateVarType)EditorGUILayout_Ex.EnumPopup(op, new GUILayoutOption[] { GUILayout.Width(35) });

            GUILayout.BeginHorizontal();
            bField.DrawSelectInfo();
            GUILayout.Label(")", new GUILayoutOption[] { GUILayout.Width(10) });
            GUILayout.EndHorizontal();
        }
    }
}