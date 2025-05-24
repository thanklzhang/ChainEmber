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
    public enum EntityPointType
    {
        [EnumLabel("位置点")]
        Position = 1,
        [EnumLabel("头部点")]
        HeadPos = 30,
    }

    public class EntityPointVar : Vector3Var
    {
        public TriggerEntityType entityType;
        public EntityPointType pointType;

        public override void OnParse(JsonData nodeJsonData)
        {
            entityType = (TriggerEntityType)int.Parse(nodeJsonData["entityType"].ToString());
            pointType = (EntityPointType)int.Parse(nodeJsonData["pointType"].ToString());
        }

        public override void OnCreate()
        {
            entityType = TriggerEntityType.Attacking_Entity;
            pointType = EntityPointType.Position;
        }

        public override string GetDrawContentStr()
        {
            return entityType.GetLabel() + " 的 " + this.pointType.GetLabel();
        }

        public override void DrawSelectInfo()
        {
            entityType = (TriggerEntityType)EditorGUILayout_Ex.EnumPopup(entityType, new GUILayoutOption[] { GUILayout.Width(100) });
            EditorGUILayout.LabelField(" 的 ", new GUILayoutOption[] { GUILayout.Width(22) });
            pointType = (EntityPointType)EditorGUILayout_Ex.EnumPopup(pointType, new GUILayoutOption[] { GUILayout.Width(100) });
        }

        public override Vector3Var OnClone()
        {
            EntityPointVar v3Var = new EntityPointVar();
            v3Var.pointType = this.pointType;
            return v3Var;
        }

        public override JsonData OnToJson(JsonData jd)
        {
            jd["entityType"] = (int)entityType;
            jd["pointType"] = (int)pointType;
            return jd;
        }
    }
}