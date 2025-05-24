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
    public enum CalculateVarType
    {
        [EnumLabel("+")]
        Plus = 0,
        [EnumLabel("-")]
        Minus = 1,
        [EnumLabel("*")]
        Multi = 2,
        [EnumLabel("÷")]
        Divide = 3
    }

   

    public class NumberVar : BaseVar
    {
        public virtual float Get()
        {
            return 0;
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

        public NumberVar Clone()
        {
            return this.OnClone();
        }

        public virtual NumberVar OnClone()
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
        public static NumberVar ParseNumberValue(JsonData nodeJsonData)
        {
            if (null == nodeJsonData)
            {
                return null;
            }

            NumberVar numberVar = null;

            if (!nodeJsonData.ContainsKey("__TYPE__"))
            {
                return null;
            }
            var nodeJsonType = nodeJsonData["__TYPE__"];
            var typeStr = nodeJsonType.ToString();
            var strs = typeStr.Split('.');
            var str = strs[strs.Length - 1];

            var fullName = NameSpaceName + "." + str;
            //Logx.Log("ParseNumberValue fullName : " + fullName);
            var resultClassName = fullName;
            var type = Type.GetType(resultClassName);
            if (type != null)
            {
                if (type.IsSubclassOf(typeof(NumberVar)))
                {
                    numberVar = Activator.CreateInstance(type) as NumberVar;
                    numberVar.Parse(nodeJsonData);
                }
            }
            else
            {
                Logx.LogError("the type of numberVar is not found : " + resultClassName);
            }

            return numberVar;
        }

        //public static JsonData ToNumberJsonData(NumberVar numberVar)
        //{

        //}

        public static string GetCompareTypeStr(CalculateVarType calculateType)
        {
            string str = "";
            if (calculateType == CalculateVarType.Plus)
            {
                str = "+";
            }
            else if (calculateType == CalculateVarType.Minus)
            {
                str = "-";
            }
            else if (calculateType == CalculateVarType.Multi)
            {
                str = "*";
            }
            else if (calculateType == CalculateVarType.Divide)
            {
                str = "/";
            }

            return str;
        }

    }
}