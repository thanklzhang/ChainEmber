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
    public enum Vector3CalculateVarType
    {
        [EnumLabel("+")]
        Plus = 0,
        [EnumLabel("-")]
        Minus = 1,
        //Multi = 2,
        //Divide = 3
    }



    public class Vector3Var : BaseVar
    {
        public virtual Vector3 Get()
        {
            return Vector3.zero;
        }

        public void Parse(JsonData nodeJsonData)
        {
            this.OnParse(nodeJsonData);
        }

        public virtual void OnParse(JsonData nodeJsonData)
        {

        }

        public void Create()
        {
            this.OnCreate();
        }

        public virtual void OnCreate()
        {

        }

        public JsonData ToJson()
        {
            JsonData jsonData = new JsonData();
            var fullTypeName = this.GetType().ToString();
            var splits = fullTypeName.Split('.');
            var typeName = splits[splits.Length - 1];
            jsonData["__TYPE__"] = typeName;
            return this.OnToJson(jsonData);
        }

        public virtual JsonData OnToJson(JsonData jsonData)
        {
            return jsonData;
        }

        public Vector3Var Clone()
        {
            return this.OnClone();
        }

        public virtual Vector3Var OnClone()
        {
            return null;
        }

        public virtual string GetDrawContentStr()
        {
            return "";
        }

        public virtual void DrawSelectInfo()
        {

        }

        public static string NameSpaceName = "BattleTrigger.Editor";
        public static Vector3Var ParseNumberValue(JsonData nodeJsonData)
        {
            if (null == nodeJsonData)
            {
                return null;
            }

            Vector3Var vector3Var = null;

            if (!nodeJsonData.ContainsKey("__TYPE__"))
            {
                return null;
            }
            var nodeJsonType = nodeJsonData["__TYPE__"];
            var typeStr = nodeJsonType.ToString();
            var strs = typeStr.Split('.');
            var str = strs[strs.Length - 1];

            var fullName = NameSpaceName + "." + str;
            //Logx.Log("ParseVector3rValue fullName : " + fullName);
            var resultClassName = fullName;
            var type = Type.GetType(resultClassName);
            if (type != null)
            {
                if (type.IsSubclassOf(typeof(Vector3Var)))
                {
                    vector3Var = Activator.CreateInstance(type) as Vector3Var;
                    vector3Var.Parse(nodeJsonData);
                }
            }
            else
            {
                Logx.LogError("the type of vector3Var is not found : " + resultClassName);
            }

            return vector3Var;
        }

        //public static JsonData ToNumberJsonData(NumberVar numberVar)
        //{

        //}

        public static string GetCompareTypeStr(Vector3CalculateVarType calculateType)
        {
            string str = "";
            if (calculateType == Vector3CalculateVarType.Plus)
            {
                str = "+";
            }
            else if (calculateType == Vector3CalculateVarType.Minus)
            {
                str = "-";
            }

            return str;
        }

    }
}