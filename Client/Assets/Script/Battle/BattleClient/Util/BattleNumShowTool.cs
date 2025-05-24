using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Battle;
using Config;
using NetProto;
using UnityEngine;

namespace Battle_Client
{
    public class BattleNumShowTool
    {
        public static string GetNumShow(EntityAttrType attrType, float value)
        {
            if (attrType == EntityAttrType.InputDamageRate ||
                attrType == EntityAttrType.InputDamageRate_Permillage)
            {
                if (value < 0)
                {
                    value = -value;
                }
            }

            if (attrType == EntityAttrType.Attack ||
                attrType == EntityAttrType.Defence ||
                attrType == EntityAttrType.MaxHealth ||
                attrType == EntityAttrType.SkillCD
               )
            {
                //int 部分
                return value.ToString();
            }
            else
            {
                //float 部分
                if (AttrHelper.IsPermillage(attrType))
                {
                    //百分比加成数值
                    return (value * 100 / 1000.0f).ToString("0.00") + "%";
                }
                else if (attrType == EntityAttrType.CritRate ||
                         attrType == EntityAttrType.CritDamage ||
                         attrType == EntityAttrType.OutputDamageRate ||
                         attrType == EntityAttrType.InputDamageRate ||
                         attrType == EntityAttrType.TreatmentRate
                        )
                {
                    //百分比数值
                    return (value * 100 / 1000.0f).ToString("0.00") + "%";
                }
                else
                {
                    //非百分比
                    return (value / 1000.0f).ToString("0.00");
                }
            }


            // //float 部分
            // if (attrType == EntityAttrType.AttackSpeed||
            //     attrType == EntityAttrType.MoveSpeed||
            //     attrType == EntityAttrType.AttackRange||
            //     attrType == EntityAttrType.CritRate ||
            //     attrType == EntityAttrType.CritDamage ||
            //     attrType == EntityAttrType.OutputDamageRate ||
            //     attrType == EntityAttrType.InputDamageRate ||
            //     attrType == EntityAttrType.TreatmentRate ||
            //     attrType == EntityAttrType.HealthRecoverSpeed ||
            //    )
            // {
            //     return value.ToString();
            // }
        }
    }
}