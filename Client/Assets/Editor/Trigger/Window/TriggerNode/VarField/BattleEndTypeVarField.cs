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
    public enum BattleEndType
    {
        [EnumLabel("胜利")]
        Win = 0,
        [EnumLabel("失败")]
        Fail = 1,
    }

    public class BattleEndTypeVarField : BaseVarField
    {
        BattleEndType endType;
        public override void OnParse(JsonData nodeJsonData)
        {
            endType = (BattleEndType)(int.Parse(nodeJsonData["endType"].ToString()));
          
        }

        public override void OnCreate()
        {
            endType = BattleEndType.Win;
        }

        public override string GetDrawContentStr()
        {
            return endType.GetLabel();
        }

        public override void DrawSelectInfo()
        {
            endType = (BattleEndType)EditorGUILayout_Ex.EnumPopup(endType, new GUILayoutOption[] { GUILayout.Width(100) });

        }
      
        public override BaseVarField OnClone()
        {
            BattleEndTypeVarField fi = new BattleEndTypeVarField();
            fi.endType = this.endType;
            return fi;
        }

        public override JsonData OnToJson()
        {
            JsonData jd = new JsonData();

            var fullTypeName = this.GetType().ToString();
            var splits = fullTypeName.Split('.');
            var typeName = splits[splits.Length - 1];

            jd["__TYPE__"] = typeName;
            jd["endType"] = (int)this.endType;
            return jd;
        }

    }
}