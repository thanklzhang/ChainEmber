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
    public class TeamHeroAliveCountVar : NumberVar
    {
        public NumberVarField teamIndexField;

        public override float Get()
        {
            return 0;
        }

        public override void OnParse(JsonData nodeJsonData)
        {
            teamIndexField = NumberVarField.ParseNumberVarField(nodeJsonData["teamIndexField"]);
        }

        public override void OnCreate()
        {
            teamIndexField = new NumberVarField();
            teamIndexField.Create();
        }

        public override string GetDrawContentStr()
        {
            var aShowStr = teamIndexField.GetDrawContentStr();
            var str = string.Format("队伍索引为 {0} 的存活英雄数量", aShowStr);
            return str;
            
        }

        public override void DrawSelectInfo()
        {
            GUILayout.Label("队伍索引为 ", new GUILayoutOption[] { GUILayout.Width(60) });
            
            teamIndexField.DrawSelectInfo();
            
            GUILayout.Label(" 的存活英雄数量 ", new GUILayoutOption[] { GUILayout.Width(80) });
        }

        public override NumberVar OnClone()
        {
            TeamHeroAliveCountVar tVar = new TeamHeroAliveCountVar();
            tVar.teamIndexField = (NumberVarField)this.teamIndexField.Clone();
            return tVar;
        }

        public override JsonData OnToJson(JsonData jd)
        {
            jd["teamIndexField"] = teamIndexField.ToJson();
            return jd;
        }
    }
}