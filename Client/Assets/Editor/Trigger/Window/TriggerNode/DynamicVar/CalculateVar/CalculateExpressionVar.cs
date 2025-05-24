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
    public class CalculateExpressionVar : NumberVar
    {
        NumberVarField aField;
        public CalculateVarType op;
        NumberVarField bField;

        public override float Get()
        {
            float result = 0.0f;
            if (op == CalculateVarType.Plus)
            {
                result = aField.Get() + bField.Get();
            }
            else if (op == CalculateVarType.Minus)
            {
                result = aField.Get() - bField.Get();
            }
            else if (op == CalculateVarType.Multi)
            {
                result = aField.Get() - bField.Get();
            }
            else if (op == CalculateVarType.Divide)
            {
                result = aField.Get() - bField.Get();
            }

            return result;
        }

        public override void OnParse(JsonData nodeJsonData)
        {
            aField = NumberVarField.ParseNumberVarField(nodeJsonData["aField"]);
            op = (CalculateVarType)int.Parse(nodeJsonData["op"].ToString());
            bField = NumberVarField.ParseNumberVarField(nodeJsonData["bField"]);
        }

        public override void OnCreate()
        {
            aField = new NumberVarField();
            aField.Create();

            bField = new NumberVarField();
            bField.Create();
        }

        public override JsonData OnToJson(JsonData jd)
        {
            jd["aField"] = aField.ToJson();
            jd["op"] = (int)op;
            jd["bField"] = bField.ToJson();
            return jd;
        }

        public override NumberVar OnClone()
        {
            CalculateExpressionVar numVar = new CalculateExpressionVar();
            numVar.aField = (NumberVarField)this.aField.Clone();
            numVar.op = this.op;
            numVar.bField = (NumberVarField)this.bField.Clone();
            return numVar;
        }

        public override string GetDrawContentStr()
        {
            var str = "";
            var aShowStr = aField.GetDrawContentStr();
            var opStr = NumberVar.GetCompareTypeStr(this.op);
            var bShowStr = bField.GetDrawContentStr();

            str = string.Format("( {0} {1} {2} )",aShowStr,opStr,bShowStr);

            return str;
        }

        public override void DrawSelectInfo()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("(", new GUILayoutOption[] { GUILayout.Width(10) });
            aField.DrawSelectInfo();
            GUILayout.EndHorizontal();
            
            op = (CalculateVarType)EditorGUILayout_Ex.EnumPopup(op, new GUILayoutOption[] { GUILayout.Width(35) });

            GUILayout.BeginHorizontal();
            bField.DrawSelectInfo();
            GUILayout.Label(")",new GUILayoutOption[] { GUILayout.Width(10) });
            GUILayout.EndHorizontal();
        }
    }
}