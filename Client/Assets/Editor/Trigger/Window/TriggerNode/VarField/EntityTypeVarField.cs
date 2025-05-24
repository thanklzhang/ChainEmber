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
    //public enum Number_GetValueFromType
    //{
    //    [EnumLabel("浮点数固定值")]
    //    FloatFixed = 0,
    //    [EnumLabel("整数固定值")]
    //    IntFixed = 1,
    //    [EnumLabel("表达式")]
    //    CalculateExpression = 2,
    //    [EnumLabel("实体属性")]
    //    EntityAttr = 10,
    //}

    public enum EntityType_GetValueFromType
    {
        [EnumLabel("固定实体的类型(根据 configId)")]
        EntityTypeByConfigId = 0,
        [EnumLabel("触发动作的实体类型")]
        EntityTypeTrigger = 1
    }

    public class EntityTypeVarField : BaseVarField
    {
        EntityType_GetValueFromType getType;
        EntityTypeVar entityTypeVar;

        public override void OnParse(JsonData nodeJsonData)
        {
            getType = (EntityType_GetValueFromType)(int.Parse(nodeJsonData["getType"].ToString()));
            entityTypeVar = EntityTypeVar.ParseEntityTypeValue(nodeJsonData["entityTypeVar"]);
        }

        public override void OnCreate()
        {
            getType = EntityType_GetValueFromType.EntityTypeByConfigId;
            entityTypeVar = new EntityTypeVar();
            entityTypeVar.Create();
        }

        public override string GetDrawContentStr()
        {
            return entityTypeVar.GetDrawContentStr();
        }

        public override void DrawSelectInfo()
        {
            getType = (EntityType_GetValueFromType)EditorGUILayout_Ex.EnumPopup(getType, new GUILayoutOption[] { GUILayout.Width(100) });

            //根据选择的 numberGetType 来进行各自的输入显示
            var numberType = GetNumberClassType(getType);
            if (null != numberType)
            {
                //if (!a.GetType().IsSubclassOf(numberType))
                if (numberType != entityTypeVar.GetType())
                {
                    entityTypeVar = null;
                    entityTypeVar = Activator.CreateInstance(numberType) as EntityTypeVar;
                    entityTypeVar.Create();
                }
            }

            entityTypeVar.DrawSelectInfo();
        }
        public static string NameSpaceName = "BattleTrigger.Editor";
        public Type GetNumberClassType(EntityType_GetValueFromType enumType)
        {
            var enumName = enumType.ToString();
            //Logx.Log("aEnumName:" + enumName);
            var enumfullName = NameSpaceName + "." + enumName + "Var";
            var entityType = Type.GetType(enumfullName);
            if (null == entityType)
            {
                Logx.LogError("the type of entityType is not found : " + enumfullName);
                return null;
            }
            return entityType;
        }

        public override BaseVarField OnClone()
        {
            EntityTypeVarField fi = new EntityTypeVarField();
            fi.getType = this.getType;
            fi.entityTypeVar = this.entityTypeVar.Clone();

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
            jd["entityTypeVar"] = entityTypeVar.ToJson();
            return jd;
        }

        public static EntityTypeVarField ParseVarField(JsonData nodeJsonData)
        {
            if (null == nodeJsonData)
            {
                return null;
            }

            EntityTypeVarField entityTypeVarField = null;

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
                if (type.IsSubclassOf(typeof(EntityTypeVarField)) || type == typeof(EntityTypeVarField))
                {
                    entityTypeVarField = Activator.CreateInstance(type) as EntityTypeVarField;
                    entityTypeVarField.Parse(nodeJsonData);
                }
            }
            else
            {
                Logx.LogError("the type of numberVarField is not found : " + resultClassName);
            }

            return entityTypeVarField;
        }

        public virtual int Get()
        {
            return this.entityTypeVar.Get();
        }
    }
}