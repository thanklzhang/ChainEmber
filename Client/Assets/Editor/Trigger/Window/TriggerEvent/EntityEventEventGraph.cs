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
    public enum EntityEventType
    {
        [EnumLabel("攻击")]
        Attack = 1,
        [EnumLabel("被攻击")]
        BeAttack = 2,
        [EnumLabel("释放技能")]
        ReleasingSkill = 10,
        [EnumLabel("死亡")]
        Dead = 20
    }

    public class EntityEventEventGraph : TriggerEventGraph
    {
        EntityEventType entityEventType;

        public override string GetDrawContentStr()//Rect childRect
        {
            var str = string.Format("当 实体 {0} 的时候", entityEventType);
            return str;
        }

        public override void OnParse(JsonData nodeJsonData)
        {
            entityEventType = (EntityEventType)(int.Parse(nodeJsonData["entityEventType"].ToString()));

        }

        public override void DrawSelectInfo()
        {
            var style = new GUIStyle();
            style.stretchWidth = false;
            style.normal.textColor = Color.white;

            GUILayout.BeginHorizontal();
            GUILayout.Label("当 实体 ", style);
            entityEventType = (EntityEventType)EditorGUILayout_Ex.EnumPopup(entityEventType, new GUILayoutOption[] { GUILayout.Width(100) });

            GUILayout.Label(" 的时候", style);
            GUILayout.EndHorizontal();
        }

        public override JsonData OnToJson(JsonData jd)
        {
            jd["entityEventType"] = new JsonData();
            jd["entityEventType"] = (int)entityEventType;
            return jd;
        }


    }
}