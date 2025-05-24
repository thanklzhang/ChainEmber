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
   
    public class Number_Check : ConditionCheck
    {
        NumberVarField aField;
        ConditionCompareType op;
        NumberVarField bField;

        public override void OnParse(JsonData nodeJsonData)
        {
            aField = NumberVarField.ParseNumberVarField(nodeJsonData["aField"]);
            op = (ConditionCompareType)(int.Parse(nodeJsonData["op"].ToString()));
            bField = NumberVarField.ParseNumberVarField(nodeJsonData["bField"]);
        }

        public override void OnCreate()
        {
            aField = new NumberVarField();
            aField.Create();
            op = ConditionCompareType.Equal;
            bField = new NumberVarField();
            bField.Create();
        }

        public override string GetDrawContentStr()//Rect childRect
        {
            var str = string.Format("如果 {0} {1} {2}  ", aField.GetDrawContentStr(), ConditionNodeGraph.GetCompareTypeStr(op), bField.GetDrawContentStr());
            return str;
        }
        public override void DrawSelectInfo()
        {
            var style = new GUIStyle();
            style.stretchWidth = false;
            style.normal.textColor = Color.white;

            aField.DrawSelectInfo();

            op = (ConditionCompareType)EditorGUILayout.EnumPopup(op, new GUILayoutOption[] { GUILayout.Width(100) });

            bField.DrawSelectInfo();
        }

        public Type GetNumberClassType(Number_GetValueFromType enumType)
        {
            var enumName = enumType.ToString();
            //Logx.Log("aEnumName:" + enumName);
            var enumfullName = NameSpaceName + "." + enumName + "Var";
            var numberType = Type.GetType(enumfullName);
            if (null == numberType)
            {
                Logx.LogError("the type of numberVar is not found : " + enumfullName);
                return null;
            }
            return numberType;
        }

        public override ConditionCheck OnClone()
        {
            Number_Check judge = new Number_Check();
            judge.aField = (NumberVarField)this.aField.Clone();
            judge.op = this.op;
            judge.bField = (NumberVarField)this.bField.Clone();

            return judge;
        }

        public override JsonData OnToJson()
        {
            JsonData jd = new JsonData();
            var fullTypeName = this.GetType().ToString();
            var splits = fullTypeName.Split('.');
            var typeName = splits[splits.Length - 1];

            jd["__TYPE__"] = typeName;
            jd["aField"] = this.aField.ToJson();
            jd["op"] = (int)op;
            jd["bField"] = this.bField.ToJson();
            return jd;
        }
    }
}