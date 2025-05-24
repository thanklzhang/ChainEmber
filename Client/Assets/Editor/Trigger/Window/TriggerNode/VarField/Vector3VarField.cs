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
    public enum Vector3_GetValueFromType
    {
        [EnumLabel("固定值")]
        Vector3Fixed = 0,
        [EnumLabel("表达式")]
        Vector3CalculateExpression = 2,
        [EnumLabel("实体点")]
        EntityPoint = 10,
        [EnumLabel("矩形内随机点")]
        Vector3RectRand = 20,
        [EnumLabel("圆形内随机点")]
        Vector3CircleRand = 21,
    }

    public class Vector3VarField : BaseVarField
    {
        Vector3_GetValueFromType getType;
        Vector3Var vector3Var;

        public override void OnParse(JsonData nodeJsonData)
        {
            getType = (Vector3_GetValueFromType)(int.Parse(nodeJsonData["getType"].ToString()));
            vector3Var = Vector3Var.ParseNumberValue(nodeJsonData["vector3Var"]);
        }

        public override void OnCreate()
        {
            getType = Vector3_GetValueFromType.Vector3Fixed;
            vector3Var = new Vector3FixedVar();
            vector3Var.Create();
        }

        public override string GetDrawContentStr()
        {
            return vector3Var.GetDrawContentStr();
        }

        public override void DrawSelectInfo()
        {
            getType = (Vector3_GetValueFromType)EditorGUILayout_Ex.EnumPopup(getType, new GUILayoutOption[] { GUILayout.Width(100) });

            //根据选择的 numberGetType 来进行各自的输入显示
            var vector3Type = GetNumberClassType(getType);
            if (null != vector3Type)
            {
                //if (!a.GetType().IsSubclassOf(numberType))
                if (vector3Type != vector3Var.GetType())
                {
                    vector3Var = null;
                    vector3Var = Activator.CreateInstance(vector3Type) as Vector3Var;
                    vector3Var.Create();
                }
            }

            vector3Var.DrawSelectInfo();
        }
        public static string NameSpaceName = "BattleTrigger.Editor";
        public Type GetNumberClassType(Vector3_GetValueFromType enumType)
        {
            var enumName = enumType.ToString();
            //Logx.Log("aEnumName:" + enumName);
            var enumfullName = NameSpaceName + "." + enumName + "Var";
            var numberType = Type.GetType(enumfullName);
            if (null == numberType)
            {
                Logx.LogError("the type of vector3Var is not found : " + enumfullName);
                return null;
            }
            return numberType;
        }

        public override BaseVarField OnClone()
        {
            Vector3VarField v3VF = new Vector3VarField();
            v3VF.getType = this.getType;
            v3VF.vector3Var = this.vector3Var.Clone();

            return v3VF;
        }

        public override JsonData OnToJson()
        {
            JsonData jd = new JsonData();

            var fullTypeName = this.GetType().ToString();
            var splits = fullTypeName.Split('.');
            var typeName = splits[splits.Length - 1];

            jd["__TYPE__"] = typeName;
            jd["getType"] = (int)this.getType;
            jd["vector3Var"] = vector3Var.ToJson();
            return jd;
        }

        public static Vector3VarField ParseVector3VarField(JsonData nodeJsonData)
        {
            if (null == nodeJsonData)
            {
                return null;
            }

            Vector3VarField vector3VarField = null;

            if (!nodeJsonData.ContainsKey("__TYPE__"))
            {
                return null;
            }
            var nodeJsonType = nodeJsonData["__TYPE__"];
            var typeStr = nodeJsonType.ToString();
            var strs = typeStr.Split('.');
            var str = strs[strs.Length - 1];

            var fullName = NameSpaceName + "." + str;
            //Logx.Log("ParseVector3VarField fullName : " + fullName);
            var resultClassName = fullName;
            var type = Type.GetType(resultClassName);
            if (type != null)
            {
                if (type.IsSubclassOf(typeof(Vector3VarField)) || type == typeof(Vector3VarField))
                {
                    vector3VarField = Activator.CreateInstance(type) as Vector3VarField;
                    vector3VarField.Parse(nodeJsonData);
                }
            }
            else
            {
                Logx.LogError("the type of vector3VarField is not found : " + resultClassName);
            }

            return vector3VarField;
        }

        public virtual Vector3 Get()
        {
            return this.vector3Var.Get();
        }
    }
}