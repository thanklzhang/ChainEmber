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
    public class EntityTypeByConfigIdVar : EntityTypeVar
    {
        public int configId;

        public override int Get()
        {
            return configId;
        }

        public override void OnParse(JsonData nodeJsonData)
        {
            configId = int.Parse(nodeJsonData["configId"].ToString());
        }

        public override void OnCreate()
        {
            configId = 0;
        }

        public override string GetDrawContentStr()
        {
            return "配置 Id 为 " + this.configId + " 的类型";
        }

        public override void DrawSelectInfo()
        {
            GUILayout.Label("配置 Id 为 )", new GUILayoutOption[] { GUILayout.Width(100) });
            configId = EditorGUILayout.IntField("", configId, new GUILayoutOption[] { GUILayout.Width(200) });
            GUILayout.Label(" 的类型 )", new GUILayoutOption[] { GUILayout.Width(90) });
        }

        public override EntityTypeVar OnClone()
        {
            EntityTypeByConfigIdVar var = new EntityTypeByConfigIdVar();
            var.configId = this.configId;
            return var;
        }

        public override JsonData OnToJson(JsonData jd)
        {
            jd["configId"] = configId;
            return jd;
        }
    }
}