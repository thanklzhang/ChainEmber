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
    public class EntityTeamIndexVar : NumberVar
    {
        public TriggerEntityType entityType;

        public override float Get()
        {
            return 0;
        }

        public override void OnParse(JsonData nodeJsonData)
        {
            entityType = (TriggerEntityType)int.Parse(nodeJsonData["entityType"].ToString());
        }

        public override void OnCreate()
        {
            entityType = TriggerEntityType.Attacking_Entity;
        }

        public override string GetDrawContentStr()
        {
            return entityType.GetLabel() + " 的 队伍索引";
        }

        public override void DrawSelectInfo()
        {
            entityType =
                (TriggerEntityType)EditorGUILayout_Ex.EnumPopup(entityType,
                    new GUILayoutOption[] { GUILayout.Width(100) });
            EditorGUILayout.LabelField(" 的 队伍索引", new GUILayoutOption[] { GUILayout.Width(100) });
        }

        public override NumberVar OnClone()
        {
            EntityTeamIndexVar numVar = new EntityTeamIndexVar();
            numVar.entityType = this.entityType;
            return numVar;
        }

        public override JsonData OnToJson(JsonData jd)
        {
            jd["entityType"] = (int)entityType;
            return jd;
        }
    }
}