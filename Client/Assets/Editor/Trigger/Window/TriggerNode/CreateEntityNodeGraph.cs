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
    public class CreateEntityType
    {
        public int configId;
    }
    [Serializable]
    public class CreateEntityNodeGraph : TriggerNodeGraph
    {
        public NumberVarField count;
        //public CreateEntityType createEntityType;
        //TODO 根据 configId 显示头像等缩略信息
        public int configId;
        public Vector3VarField createPosition;

        public override void OnParse(JsonData nodeJsonData)
        {

            count = NumberVarField.ParseNumberVarField(nodeJsonData["count"]);

            configId = (int.Parse(nodeJsonData["entityType"]["configId"].ToString()));

            createPosition = Vector3VarField.ParseVector3VarField(nodeJsonData["createPosition"]);
        }

        public override void OnCreate()
        {
            count = new NumberVarField();
            count.Create();

            configId = 0;

            createPosition = new Vector3VarField();
            createPosition.Create();
        }

        public override string GetDrawContentStr()//Rect childRect
        {
            var str = string.Format("创建 {0} 个 '配置 id 为 {1}' 的实体 在 {2} 点上", count.GetDrawContentStr(), configId, createPosition.GetDrawContentStr());
            return str;
        }

        public override void DrawSelectInfo()
        {
            var style = new GUIStyle();
            style.stretchWidth = false;
            style.normal.textColor = Color.white;

            GUILayout.BeginHorizontal();
            GUILayout.Label("创建 ", style, new GUILayoutOption[] { GUILayout.Width(30) });
            //count = EditorGUILayout.IntField(count, new GUILayoutOption[] { GUILayout.Width(40) });
            count.DrawSelectInfo();
            GUILayout.Label(" 个", style, new GUILayoutOption[] { GUILayout.Width(25) });
            GUILayout.EndHorizontal();

            GUILayout.Space(10);

            GUILayout.BeginHorizontal();
            GUILayout.Label("配置 id 为", style, new GUILayoutOption[] { GUILayout.Width(100) });
            configId = EditorGUILayout.IntField(configId, new GUILayoutOption[] { GUILayout.Width(120) });
            GUILayout.Label("的实体", style, new GUILayoutOption[] { GUILayout.Width(60) });
            GUILayout.EndHorizontal();

            GUILayout.Space(10);

            GUILayout.BeginHorizontal();
            GUILayout.Label("在", style, new GUILayoutOption[] { GUILayout.Width(15) });
            createPosition.DrawSelectInfo();
            GUILayout.Label("点上", style, new GUILayoutOption[] { GUILayout.Width(50) });
            GUILayout.EndHorizontal();
        }

        public override TriggerNodeGraph OnClone()
        {
            TriggerNodeGraph node = new CreateEntityNodeGraph();
            var createEntityNode = node as CreateEntityNodeGraph;
            createEntityNode.count = (NumberVarField)this.count.Clone();
            createEntityNode.configId = this.configId;
            createEntityNode.createPosition = (Vector3VarField)this.createPosition.Clone();
            return createEntityNode;
        }

        public override JsonData OnToJson(JsonData jd)
        {
            jd["count"] = new JsonData();
            jd["count"] = count.ToJson();

            jd["entityType"] = new JsonData();
            jd["entityType"]["configId"] = configId;

            jd["createPosition"] = this.createPosition.ToJson();

            return jd;
        }

    }
}