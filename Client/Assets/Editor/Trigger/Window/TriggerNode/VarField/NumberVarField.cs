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
    public enum Number_GetValueFromType
    {
        [EnumLabel("浮点数固定值")]
        FloatFixed = 0,
        [EnumLabel("整数固定值")]
        IntFixed = 1,
        [EnumLabel("表达式")]
        CalculateExpression = 2,
        [EnumLabel("实体属性")]
        EntityAttr = 10,
        [EnumLabel("队伍索引")]
        EntityTeamIndex = 11,
        [EnumLabel("队伍中英雄的存活数量")]
        TeamHeroAliveCount = 12,
    }

    public class NumberVarField : BaseVarField
    {
        Number_GetValueFromType getType;
        NumberVar numberVar;

        public override void OnParse(JsonData nodeJsonData)
        {
            getType = (Number_GetValueFromType)(int.Parse(nodeJsonData["getType"].ToString()));
            numberVar = NumberVar.ParseNumberValue(nodeJsonData["numberVar"]);
        }

        public override void OnCreate()
        {
            getType = Number_GetValueFromType.FloatFixed;
            numberVar = new FloatFixedVar();
            numberVar.Create();
        }

        public override string GetDrawContentStr()
        {
            return numberVar.GetDrawContentStr();
        }

        public override void DrawSelectInfo()
        {
            getType = (Number_GetValueFromType)EditorGUILayout_Ex.EnumPopup(getType, new GUILayoutOption[] { GUILayout.Width(100) });

            //根据选择的 numberGetType 来进行各自的输入显示
            var numberType = GetNumberClassType(getType);
            if (null != numberType)
            {
                //if (!a.GetType().IsSubclassOf(numberType))
                if (numberType != numberVar.GetType())
                {
                    numberVar = null;
                    numberVar = Activator.CreateInstance(numberType) as NumberVar;
                    numberVar.Create();
                }
            }

            numberVar.DrawSelectInfo();
        }
        public static string NameSpaceName = "BattleTrigger.Editor";
        public Type GetNumberClassType(Number_GetValueFromType enumType)
        {
            var enumName = enumType.ToString();
            //Logx.Log("aEnumName:" + enumName);
            var enumfullName = NameSpaceName + "." + enumName + "Var";
            var numberType = Type.GetType(enumfullName);
            if (null == numberType)
            {
                Logx.LogError("the type of numberVar is not found : " + enumfullName);
                return null;
            }
            return numberType;
        }

        public override BaseVarField OnClone()
        {
            NumberVarField fi = new NumberVarField();
            fi.getType = this.getType;
            fi.numberVar = this.numberVar.Clone();

            return fi;
        }

        public override JsonData OnToJson()
        {
            JsonData jd = new JsonData();

            var fullTypeName = this.GetType().ToString();
            var splits = fullTypeName.Split('.');
            var typeName = splits[splits.Length - 1];

            jd["__TYPE__"] = typeName;
            jd["getType"] = (int)this.getType;
            jd["numberVar"] = numberVar.ToJson();
            return jd;
        }

        public static NumberVarField ParseNumberVarField(JsonData nodeJsonData)
        {
            if (null == nodeJsonData)
            {
                return null;
            }

            NumberVarField numberVarField = null;

            if (!nodeJsonData.ContainsKey("__TYPE__"))
            {
                return null;
            }
            var nodeJsonType = nodeJsonData["__TYPE__"];
            var typeStr = nodeJsonType.ToString();
            var strs = typeStr.Split('.');
            var str = strs[strs.Length - 1];

            var fullName = NameSpaceName + "." + str;
            //Logx.Log("ParseNumberVarField fullName : " + fullName);
            var resultClassName = fullName;
            var type = Type.GetType(resultClassName);
            if (type != null)
            {
                if (type.IsSubclassOf(typeof(NumberVarField)) || type == typeof(NumberVarField))
                {
                    numberVarField = Activator.CreateInstance(type) as NumberVarField;
                    numberVarField.Parse(nodeJsonData);
                }
            }
            else
            {
                Logx.LogError("the type of numberVarField is not found : " + resultClassName);
            }

            return numberVarField;
        }

        public virtual float Get()
        {
            return this.numberVar.Get();
        }
    }
}