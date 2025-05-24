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
    public class BattleEndNodeGraph : TriggerNodeGraph
    {
        public NumberVarField teamIndex;
        public BattleEndType endType;

        public override void OnParse(JsonData nodeJsonData)
        {
            //delayTime = (float.Parse(nodeJsonData["delayTime"]["value"].ToString()));
            teamIndex = NumberVarField.ParseNumberVarField(nodeJsonData["teamIndex"]);
            endType = (BattleEndType)int.Parse(nodeJsonData["endType"].ToString());
        }

        public override void OnCreate()
        {
            teamIndex = new NumberVarField();
            teamIndex.Create();
        }

        public override string GetDrawContentStr()//Rect childRect
        {
            var str = "队伍 " + teamIndex.GetDrawContentStr() + " " + endType.ToString();
            return str;
        }

        public override void DrawSelectInfo()
        {
            var style = new GUIStyle();
            style.stretchWidth = false;
            style.normal.textColor = Color.white;

            GUILayout.BeginHorizontal();
            GUILayout.Label("队伍 ", style);

            teamIndex.DrawSelectInfo();
           
           endType = (BattleEndType)EditorGUILayout_Ex.EnumPopup(endType, new GUILayoutOption[] { GUILayout.Width(100) });

            //GUILayout.Label(" 获胜", style);
            GUILayout.EndHorizontal();
        }

        public override TriggerNodeGraph OnClone()
        {
            TriggerNodeGraph node = new BattleEndNodeGraph();
            var _node = node as BattleEndNodeGraph;
            _node.teamIndex = (NumberVarField)this.teamIndex.Clone();
            _node.endType = this.endType;

            return _node;
        }

        public override JsonData OnToJson(JsonData jd)
        {
            jd["teamIndex"] = teamIndex.ToJson();
            jd["endType"] = (int)endType;
            return jd;
        }

    }
}