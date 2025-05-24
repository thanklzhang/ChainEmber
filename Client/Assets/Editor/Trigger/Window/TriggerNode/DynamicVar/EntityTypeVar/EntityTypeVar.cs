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
    //public enum EntityTypeVarType
    //{
    //    [EnumLabel("根据 configId 获得的固定类型")]
    //    ByConfigId = 0,
    //    [EnumLabel("触发单位类型")]
    //    TriggerEntity = 1,
    //}



    public class EntityTypeVar : BaseVar
    {
        public virtual int Get()
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

        public EntityTypeVar Clone()
        {
            return this.OnClone();
        }

        public virtual EntityTypeVar OnClone()
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
        public static EntityTypeVar ParseEntityTypeValue(JsonData nodeJsonData)
        {
            if (null == nodeJsonData)
            {
                return null;
            }

            EntityTypeVar entityTypeVar = null;

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
                if (type.IsSubclassOf(typeof(EntityTypeVar)))
                {
                    entityTypeVar = Activator.CreateInstance(type) as EntityTypeVar;
                    entityTypeVar.Parse(nodeJsonData);
                }
            }
            else
            {
                Logx.LogError("the type of EntityTypeVar is not found : " + resultClassName);
            }

            return entityTypeVar;
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