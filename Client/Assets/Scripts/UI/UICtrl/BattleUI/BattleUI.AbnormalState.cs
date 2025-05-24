using Battle_Client;
using System;
using System.Collections;
using System.Collections.Generic;
using Battle;
using Battle_Client.BattleSkillOperate;
using Config;
using GameData;
using UnityEngine;
using UnityEngine.UI;

public partial class BattleUI : BaseUI
{
    public void OnEntityAbnormalEffect(BattleEntity_Client entity, AbnormalStateBean stateBean)
    {
        var go = entity.gameObject;
        string showWord = "";
        Color color = Color.white;

        //TODO : 文字待配置
        var abnormalType = stateBean.stateType;
        if (abnormalType == EntityAbnormalStateType.AvoidNormalAttack)
        {
            if (stateBean.triggerType == AbnormalStateTriggerType.Trigger)
            {
                showWord = "躲避普攻";
                ColorUtility.TryParseHtmlString("#00FCFF", out color);
            }
           
        }
        else if (abnormalType == EntityAbnormalStateType.AvoidProjectile)
        {
            if (stateBean.triggerType == AbnormalStateTriggerType.Trigger)
            {
                showWord = "躲避投掷物";
                ColorUtility.TryParseHtmlString("#00FCFF", out color);
            }
        }
        else if (abnormalType == EntityAbnormalStateType.AvoidDamage)
        {
            if (stateBean.triggerType == AbnormalStateTriggerType.Trigger)
            {
                showWord = "躲避伤害";
                ColorUtility.TryParseHtmlString("#00FCFF", out color);
            }
        }

        FloatWordBean arg = new FloatWordBean();
        arg.color = color;
        arg.showStyle = FloatWordShowStyle.Middle;
        arg.stateType = stateBean.stateType;
        arg.triggerType = stateBean.triggerType;
        arg.wordStr = showWord;
        arg.followGo = go;

        floatWordMgr.ShowFloatWord(arg);
    }

}