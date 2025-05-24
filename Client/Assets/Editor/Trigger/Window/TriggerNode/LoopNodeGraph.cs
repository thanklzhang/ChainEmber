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
    [Serializable]
    public class LoopNodeGraph : TriggerNodeGraph
    {
        public NumberVarField loopCount;

        public TriggerNodeGraph actionShowNodeGraph;

        public TriggerGroupGraph loopExecuteGroup;

        public bool isCollapse;

        public override string GetDrawContentStr()//Rect childRect
        {
            var str = string.Format("循环 {0} 次", loopCount.GetDrawContentStr());
            return str;
        }

        public override void Draw()//Rect childRect
        {
            base.Draw();
            if (!isCollapse)
            {
                actionShowNodeGraph.Draw();
                loopExecuteGroup.Draw(new Rect());
            }
        }

        public override void OnParse(JsonData nodeJsonData)
        {
            loopCount = NumberVarField.ParseNumberVarField(nodeJsonData["loopCount"]);

            var group = nodeJsonData["loopExecuteGroup"];
            var nodeGraphList = TriggerWindow.instance.ParseTriggerNodeList(group, this.floor + 1);
            loopExecuteGroup = new TriggerGroupGraph();
            loopExecuteGroup.Init(nodeGraphList, this.floor + 1);

            actionShowNodeGraph = new AssShowStrGraph();
            actionShowNodeGraph.Create(this.floor + 1);
            actionShowNodeGraph.groupGraph = this.loopExecuteGroup;
            actionShowNodeGraph.SetShowStr("循环体-------------");
        }

        public override void OnCreate()
        {
            loopCount = new NumberVarField();
            loopCount.Create();

            loopExecuteGroup = new TriggerGroupGraph();
            var nodeGraphList = new List<TriggerNodeGraph>();
            loopExecuteGroup.Init(nodeGraphList, this.floor + 1);

            actionShowNodeGraph = new AssShowStrGraph();
            actionShowNodeGraph.Create(this.floor + 1);
            actionShowNodeGraph.groupGraph = this.loopExecuteGroup;
            actionShowNodeGraph.SetShowStr("循环体-------------");
        }

        public override void DrawSelectInfo()
        {
            var style = new GUIStyle();
            style.stretchWidth = false;
            style.normal.textColor = Color.white;

            GUILayout.BeginHorizontal();
            GUILayout.Label("循环 ", style);
            //loopCount = EditorGUILayout.IntField(loopCount, new GUILayoutOption[] { GUILayout.Width(40) });
            loopCount.DrawSelectInfo();
            GUILayout.Label(" 次", style);
            GUILayout.EndHorizontal();
        }

        public override TriggerNodeGraph OnClone()
        {
            TriggerNodeGraph node = new LoopNodeGraph();
            var loopNode = node as LoopNodeGraph;
            loopNode.loopCount = (NumberVarField)this.loopCount.Clone();
            loopNode.actionShowNodeGraph = this.actionShowNodeGraph.Clone();
            loopNode.loopExecuteGroup = this.loopExecuteGroup.Clone();

            return loopNode;
        }


        public override JsonData OnToJson(JsonData jd)
        {
            jd["loopCount"] = new JsonData();
            jd["loopCount"] = loopCount.ToJson();
            jd["loopExecuteGroup"] = loopExecuteGroup.ToJson();

            return jd;
        }

        public override void SetFloorIncludeChildren(int floor)
        {
            this.floor = floor;

            loopExecuteGroup.SetFloor(this.floor + 1);
            actionShowNodeGraph.SetFloorIncludeChildren(this.floor + 1);

        }

        public override void DrawLeft()
        {
            var style = new GUIStyle();
            style.normal.background = Texture2D.grayTexture;
            style.alignment = TextAnchor.MiddleCenter;
            style.padding = new RectOffset();
            var sign = isCollapse ? "+" : "-";
            if (GUILayout.Button(sign, style, new GUILayoutOption[] { GUILayout.Height(14), GUILayout.Width(14) }))
            {
                isCollapse = !isCollapse;
            }
            GUILayout.Space(6);
        }
    }
}