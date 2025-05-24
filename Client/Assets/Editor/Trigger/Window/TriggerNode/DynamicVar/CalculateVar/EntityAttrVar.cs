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
    public enum TriggerEntityType
    {
        [EnumLabel("攻击的实体")]
        Attacking_Entity = 1,
        [EnumLabel("被攻击的实体")]
        Be_Attacking_Entity = 2,
        [EnumLabel("释放技能的实体")]
        Releasing_Skill_Entity = 10,
        [EnumLabel("死亡的实体")]
        Dead_Entity = 25

    }

    public enum EntityAttrNumberType
    {
        [EnumLabel("攻击力")]
        Attack = 1,
        [EnumLabel("防御力")]
        Defence = 2,
        [EnumLabel("当前生命值")]
        CurrHealth = 3,
        [EnumLabel("最大生命值上限")]
        MaxHealth = 4,
        [EnumLabel("攻击速度")]
        AttackSpeed = 5,
        [EnumLabel("移动速度")]
        MoveSpeed = 6,
        [EnumLabel("攻击范围")]
        AttackRange = 7,

        [EnumLabel("等级")]
        Level = 1000,
        [EnumLabel("星级")]
        Star = 1001
    }

    public class EntityAttrVar : NumberVar
    {
        public TriggerEntityType entityType;
        public EntityAttrNumberType attrType;

        public override float Get()
        {
            return 0;
        }

        public override void OnParse(JsonData nodeJsonData)
        {
            entityType = (TriggerEntityType)int.Parse(nodeJsonData["entityType"].ToString());
            attrType = (EntityAttrNumberType)int.Parse(nodeJsonData["attrType"].ToString());
        }

        public override void OnCreate()
        {
            entityType = TriggerEntityType.Attacking_Entity;
            attrType = EntityAttrNumberType.Attack;
        }

        public override string GetDrawContentStr()
        {
            return entityType.GetLabel() + " 的 " + this.attrType.GetLabel();
        }

        public override void DrawSelectInfo()
        {
            entityType = (TriggerEntityType)EditorGUILayout_Ex.EnumPopup(entityType, new GUILayoutOption[] { GUILayout.Width(100) });
            EditorGUILayout.LabelField(" 的 ", new GUILayoutOption[] { GUILayout.Width(22) });
            attrType = (EntityAttrNumberType)EditorGUILayout_Ex.EnumPopup(attrType, new GUILayoutOption[] { GUILayout.Width(100) });
        }

        public override NumberVar OnClone()
        {
            EntityAttrVar numVar = new EntityAttrVar();
            numVar.entityType = this.entityType;
            numVar.attrType = this.attrType;
            return numVar;
        }

        public override JsonData OnToJson(JsonData jd)
        {
            jd["entityType"] = (int)entityType;
            jd["attrType"] = (int)attrType;
            return jd;
        }
    }
}