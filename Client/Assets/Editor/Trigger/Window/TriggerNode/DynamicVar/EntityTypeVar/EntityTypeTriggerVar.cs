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
    public class EntityTypeTriggerVar : EntityTypeVar
    {
        public TriggerEntityType entityType;

        public override int Get()
        {
            return 0;
        }

        public override void OnParse(JsonData nodeJsonData)
        {
            entityType = (TriggerEntityType)(int.Parse(nodeJsonData["entityType"].ToString()));
        }

        public override void OnCreate()
        {
            entityType = TriggerEntityType.Attacking_Entity;
        }

        public override string GetDrawContentStr()
        {
            return "" + this.entityType + " 的类型";
        }

        public override void DrawSelectInfo()
        {
            entityType = (TriggerEntityType)EditorGUILayout_Ex.EnumPopup(entityType, new GUILayoutOption[] { GUILayout.Width(100) });
            GUILayout.Label(" 的类型 )", new GUILayoutOption[] { GUILayout.Width(90) });
        }

        public override EntityTypeVar OnClone()
        {
            EntityTypeTriggerVar var = new EntityTypeTriggerVar();
            var.entityType = this.entityType;
            return var;
        }

        public override JsonData OnToJson(JsonData jd)
        {
            jd["entityType"] = (int)entityType;
            return jd;
        }
    }
}