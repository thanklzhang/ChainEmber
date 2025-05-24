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
    public enum ConditionCheckType
    {
        Number_Check,
        Bool_Check,
        EntityType_Check
    }

    public class ConditionCheck
    {
        public void Parse(JsonData jsonData)
        {
            this.OnParse(jsonData);
        }

        public virtual void OnParse(JsonData jsonData)
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
            return this.OnToJson();
        }
        public virtual JsonData OnToJson()
        {
            return new JsonData();
        }

        public ConditionCheck Clone()
        {
            return OnClone();
        }

        public virtual ConditionCheck OnClone()
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
        public static ConditionCheck ParseConditionCheck(JsonData nodeJsonData)
        {
            if (null == nodeJsonData)
            {
                return null;
            }

            ConditionCheck conditionJudge = null;

            if (!nodeJsonData.ContainsKey("__TYPE__"))
            {
                return null;
            }
            var nodeJsonType = nodeJsonData["__TYPE__"];
            var typeStr = nodeJsonType.ToString();
            var strs = typeStr.Split('.');
            var str = strs[strs.Length - 1];

            var fullName = NameSpaceName + "." + str;
            //Logx.Log("ParseConditionJudge fullName : " + fullName);
            var resultClassName = fullName;
            var type = Type.GetType(resultClassName);
            if (type != null)
            {
                if (type.IsSubclassOf(typeof(ConditionCheck)))
                {
                    conditionJudge = Activator.CreateInstance(type) as ConditionCheck;
                    conditionJudge.Parse(nodeJsonData);
                }
            }
            else
            {
                Logx.LogError("the type of ConditionCheck is not found : " + resultClassName);
            }

            return conditionJudge;
        }

    }
}