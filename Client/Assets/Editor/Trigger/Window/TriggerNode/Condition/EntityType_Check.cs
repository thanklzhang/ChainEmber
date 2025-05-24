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
    public enum EntityType_ConditionCompareType
    {
        Equal = 0,
        NotEqual = 1,
    }

    public class EntityType_Check : ConditionCheck
    {
        EntityTypeVarField aField;
        ConditionCompareType_Bool op;
        EntityTypeVarField bField;

        public override void OnParse(JsonData nodeJsonData)
        {
            aField = EntityTypeVarField.ParseVarField(nodeJsonData["aField"]);
            op = (ConditionCompareType_Bool)(int.Parse(nodeJsonData["op"].ToString()));
            bField = EntityTypeVarField.ParseVarField(nodeJsonData["bField"]);
        }

        public override void OnCreate()
        {
            aField = new EntityTypeVarField();
            aField.Create();
            op = ConditionCompareType_Bool.Equal;
            bField = new EntityTypeVarField();
            bField.Create();
        }

        public override string GetDrawContentStr()//Rect childRect
        {
            var str = string.Format("如果 {0} {1} {2}  ", aField.GetDrawContentStr(), ConditionNodeGraph.GetCompareTypeStr_Bool(op), bField.GetDrawContentStr());
            return str;
        }


        public override void DrawSelectInfo()
        {
            var style = new GUIStyle();
            style.stretchWidth = false;
            style.normal.textColor = Color.white;

            aField.DrawSelectInfo();

            op = (ConditionCompareType_Bool)EditorGUILayout.EnumPopup(op, new GUILayoutOption[] { GUILayout.Width(100) });


            bField.DrawSelectInfo();
        }


        public override ConditionCheck OnClone()
        {
            EntityType_Check judge = new EntityType_Check();
            judge.aField = (EntityTypeVarField)this.aField.Clone();
            judge.op = this.op;
            judge.bField = (EntityTypeVarField)this.bField.Clone();

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