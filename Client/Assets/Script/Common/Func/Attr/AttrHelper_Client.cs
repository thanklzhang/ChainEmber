using System.Collections.Generic;
using Battle;
using Battle_Client;
using Config;
using UnityEngine;
using BattleItem = Config.BattleItem;

public class AttrHelper_Client
{
    public static string GetAttrShowValue(EntityAttrType attrType, int value)
    {
        // if (AttrHelper.IsFixedFloatValue(attrType))
        // {
        //     return value / 1000.0f;
        // }
        //
        // if (AttrHelper.IsPermillage(attrType))
        // {
        //     return value / 1000.0f;
        // }
        //
        // return value;

        var str = BattleNumShowTool.GetNumShow(attrType, value);
        return str;
    }

    public static void GetBattleRewardContent(BattleReward_Client battleReward, bool isMakeSureReward,
        bool isGainResultShow, out string nameStr, out string desStr)
    {
        string resultNameStr = "";
        string resultDesStr = "";
        var options = battleReward.effectOptionList;
        if (options.Count > 0)
        {
            for (int i = 0; i < options.Count; i++)
            {
                var option = options[i];
                GetBattleRewardEffectContent(option, isMakeSureReward, isGainResultShow, out nameStr, out desStr);

                resultNameStr += nameStr;
                resultDesStr += desStr;

                if (options.Count > 1 && i < options.Count - 1)
                {
                    resultNameStr += ",";
                    resultDesStr += "\n";
                }
            }
        }

        nameStr = resultNameStr;
        desStr = resultDesStr;
    }

    //isGainResultShow 是否是获得之后的结果显示
    public static void GetBattleRewardEffectContent(BattleRewardEffectOption_Client battleReward, bool isMakeSureReward,
        bool isGainResultShow, out string nameStr, out string desStr)
    {
        var configId = battleReward.configId;
        var rewardConfig = ConfigManager.Instance.GetById<Config.BattleRewardEffectOption>(configId);

        var type = (BattleRewardEffectType)rewardConfig.Type;

        nameStr = rewardConfig.Name;
        desStr = rewardConfig.Describe;
        // var intValueList = this.data.intValueList;
        if (type == BattleRewardEffectType.Skill_Gain)
        {
            var skillConfigId = battleReward.intArg1;
            nameStr = "获得技能";
            if (isGainResultShow)
            {
                var skillConfig = ConfigManager.Instance.GetById<Config.Skill>(skillConfigId);
                desStr = $"获得技能：{skillConfig.Name}";
            }
            else
            {
                if (isMakeSureReward)
                {
                    var skillConfig = ConfigManager.Instance.GetById<Config.Skill>(skillConfigId);
                    desStr = $"{skillConfig.Name}\n效果：{skillConfig.Describe}";
                }
                else
                {
                    desStr = $"获得一个随机技能";
                }
            }
        }
        else if (type == BattleRewardEffectType.Item_Gain)
        {
            //TODO 这里应该从表格中读取
            nameStr = "获得道具";
            var itemConfigId = battleReward.intArg1;
            var itemConfig = ConfigManager.Instance.GetById<BattleItem>(itemConfigId);
            desStr = "";

            if (isGainResultShow)
            {
                desStr += $"获得道具：{itemConfig.Name}";
            }
            else
            {
                if (isMakeSureReward)
                {
                    desStr += itemConfig.Name + "\n效果：";
                    var attrStr = GetAttrContent(itemConfig.AttrGroupConfigId,
                        battleReward.intListArg1);
                    desStr += attrStr;
                }
                else
                {
                    desStr = "随机获得一个道具";
                }
            }
        }
        else if (type == BattleRewardEffectType.Item_Copy)
        {
            nameStr = "复制道具";
            if (isGainResultShow)
            {
                desStr = "";

                var itemConfigId = battleReward.intArg1;
                var attrStr = "";
                if (itemConfigId > 0)
                {
                    desStr += "复制了道具：";
                    var itemConfig = ConfigManager.Instance.GetById<BattleItem>(itemConfigId);
                    desStr += itemConfig.Name;
                }
                else
                {
                    attrStr = "随机复制一个道具";
                }

                desStr += attrStr;
            }
            else
            {
                if (isMakeSureReward)
                {
                    desStr = "";

                    var itemConfigId = battleReward.intArg1;
                    var attrStr = "";
                    if (itemConfigId > 0)
                    {
                        desStr += "复制了道具：";
                        var itemConfig = ConfigManager.Instance.GetById<BattleItem>(itemConfigId);
                        desStr += itemConfig.Name + "\n效果：";

                        attrStr = GetAttrContent(itemConfig.AttrGroupConfigId,
                            battleReward.intListArg1);
                    }
                    else
                    {
                        attrStr = "随机复制一个道具";
                    }

                    desStr += attrStr;
                }
                else
                {
                    desStr = "随机复制一个已有道具";
                }
            }
        }
        else if (type == BattleRewardEffectType.TeamMember_Gain)
        {
            nameStr = "获得队友";
            if (isGainResultShow)
            {
                var entityConfigId = battleReward.intArg1;
                var entityConfig = ConfigManager.Instance.GetById<EntityInfo>(entityConfigId);
                desStr = $"获得队友：{entityConfig.Name}";
            }
            else
            {
                if (isMakeSureReward)
                {
                    var entityConfigId = battleReward.intArg1;
                    var entityConfig = ConfigManager.Instance.GetById<EntityInfo>(entityConfigId);
                    desStr = $"{entityConfig.Name}\n{entityConfig.Describe2}";
                }
                else
                {
                    desStr = "随机获得一个队友";
                }
            }
        }
        else if (type == BattleRewardEffectType.TeamMember_TeammateAddRandAttr)
        {
            nameStr = "增加属性";
            if (isGainResultShow)
            {
                var attrStr = GetAttrContent(battleReward.intArg1,
                    battleReward.intListArg1,false);
                
                var entityGuid = battleReward.intArg2;
                var entity = BattleEntityManager.Instance.FindEntity(entityGuid);
                if (entity != null)
                {
                    var entityConfig = ConfigManager.Instance.GetById<EntityInfo>(entity.configId);
                    if (entityConfig != null)
                    {
                        desStr = $"{entityConfig.Name} 获得了属性：{attrStr}";
                    }
                }
                else
                {
                    desStr = $"没有找到队友，属性加到队长身上:获得了属性：{attrStr}";
                }
            }
            else
            {
                if (isMakeSureReward)
                {
                    desStr = "一个随机队友获得随机属性：\n";
                    var attrStr = GetAttrContent(battleReward.intArg1,
                        battleReward.intListArg1);
                    desStr += attrStr;
                }
                else
                {
                    desStr = "一个随机队友获得随机属性";
                }
            }
        }
        else if (type == BattleRewardEffectType.TeamMember_AllTeammateAddRandAttr)
        {
            nameStr = "增加属性";
            if (isGainResultShow)
            {
                desStr = "所有队友增加属性：";

                var attrStr = GetAttrContent(battleReward.intArg1,
                    battleReward.intListArg1,false);
                desStr += attrStr;
            }
            else
            {
                if (isMakeSureReward)
                {
                    desStr = "所有队友增加属性：\n";

                    var attrStr = GetAttrContent(battleReward.intArg1,
                        battleReward.intListArg1);
                    desStr += attrStr;
                }
                else
                {
                    desStr = "所有队友增加随机属性";
                }
            }

          
        }
        else if (type == BattleRewardEffectType.TeamMember_RandAddStarExp)
        {
            nameStr = "增加星级经验";

            if (isGainResultShow)
            {
                var entityGuid = battleReward.intArg2;
                var entity = BattleEntityManager.Instance.FindEntity(entityGuid);
                if (entity != null)
                {
                    var entityConfig = ConfigManager.Instance.GetById<EntityInfo>(entity.configId);
                    if (entityConfig != null)
                    {
                        desStr = $"{entityConfig.Name} 获得了星级经验：{battleReward.intArg1}点";
                    }
                }
            }
            else
            {
                if (isMakeSureReward)
                {
                    desStr = $"随机一个英雄增加星级经验\n{battleReward.intArg1}点";
                }
                else
                {
                    desStr = "随机一个英雄增加星级经验";
                }
            }
        }
        else if (type == BattleRewardEffectType.TeamMember_AllAddStarExp)
        {
            nameStr = "增加星级经验";
            if (isGainResultShow)
            {
                desStr += $"所有英雄增加星级经验：{battleReward.intArg1}点";
            }
            else
            {
                if (isMakeSureReward)
                {
                    desStr = $"所有英雄增加星级经验\n{battleReward.intArg1}点";
                }
                else
                {
                    desStr = "所有英雄增加星级经验";
                }
            }
        }
        else if (type == BattleRewardEffectType.Leader_RandAttr)
        {
            nameStr = "增加属性";
            if (isGainResultShow)
            {
                desStr = "队长增加了属性：";

                var attrStr = GetAttrContent(battleReward.intArg1,
                    battleReward.intListArg1,false);
                desStr += attrStr;
            }
            else
            {
                if (isMakeSureReward)
                {
                    desStr = "增加队长属性\n";

                    var attrStr = GetAttrContent(battleReward.intArg1,
                        battleReward.intListArg1);
                    desStr += attrStr;
                }
                else
                {
                    desStr = "增加队长随机属性";
                }
            }
        }
        else if (type == BattleRewardEffectType.Leader_AddBuff)
        {
            nameStr = "获得Buff效果";

            if (isGainResultShow)
            {
                var buffConfigId = battleReward.intArg1;
                var buffConfig = ConfigManager.Instance.GetById<Config.BuffEffect>(buffConfigId);
                desStr = $"队长获得了Buff效果{buffConfig.Name}";
            }
            else
            {
                if (isMakeSureReward)
                {
                    var buffConfigId = battleReward.intArg1;
                    var buffConfig = ConfigManager.Instance.GetById<Config.BuffEffect>(buffConfigId);
                    desStr = $"队长获得Buff效果\n{buffConfig.Name}";
                }
                else
                {
                    desStr = "队长获得特殊Buff效果";
                }
            }
        }
        else if (type == BattleRewardEffectType.Currency_BattleCoin)
        {
            nameStr = "获得战银";
            if (isGainResultShow)
            {
                var str = $"获得 {battleReward.intArg1} 战银";
                desStr += str;
            }
            else
            {
                if (isMakeSureReward)
                {
                    desStr = "";

                    if (rewardConfig.ValueList.Count > 1)
                    {
                        //随机
                        var str =
                            $"{battleReward.intArg1}战银\n({rewardConfig.ValueList[0]}~{rewardConfig.ValueList[1]})\n（获得时计算战银加成）";
                        desStr += str;
                    }
                    else
                    {
                        //固定
                        var str = $"{battleReward.intArg1}（包含战银获得加成）";
                        desStr += str;
                    }
                }
                else
                {
                    desStr = "获得一定的战银（包含战银获得加成）";
                }
            }
        }
        else if (type == BattleRewardEffectType.Currency_Population)
        {
            nameStr = "获得人口";
            if (isGainResultShow)
            {
                var str = $"获得 {battleReward.intArg1} 人口";
                desStr += str;
            }
            else
            {
                if (isMakeSureReward)
                {
                    desStr = "获得 ";

                    if (rewardConfig.ValueList.Count > 1)
                    {
                        //随机
                        var str = $"{battleReward.intArg1}({rewardConfig.ValueList[0]}~{rewardConfig.ValueList[1]})";
                        desStr += str;

                        desStr += " 人口";
                    }
                    else
                    {
                        //固定
                        var str = $"{battleReward.intArg1}";
                        desStr += str;

                        desStr += " 人口";
                    }
                }
                else
                {
                    desStr = "获得一定的人口";
                }
            }
        }
    }

    public static string GetAttrContent(int attrGroupConfigId, List<int> intValueList,bool isShowRand = true)
    {
        var attrGroupConfig = ConfigManager.Instance.GetById<BattleAttributeGroup>(attrGroupConfigId);

        var isRand = attrGroupConfig.AddedValueRand != null && attrGroupConfig.AddedValueRand.Count > 0;
        var attrStr = "";
        for (int i = 0; i < attrGroupConfig.AddedAttrGroup.Count; i++)
        {
            var str = "";
            var attrType = (EntityAttrType)attrGroupConfig.AddedAttrGroup[i];
            var paramsList = attrGroupConfig.AddedValueGroup[i];
            var randList = new List<int>();

            if (isRand)
            {
                randList = attrGroupConfig.AddedValueRand[i];
            }


            var attrValueType = (AddedValueType)paramsList[0];
            var attrValue = 0;
            bool isUseConfig = false;
            if (intValueList != null && i < intValueList.Count)
            {
                //用传来的属性值
                attrValue = intValueList[i];
            }
            else
            {
                //没有运行时数据，那么就走配置，如道具属性
                attrValue = paramsList[1];
                isUseConfig = true;
            }

            var showAttrType = attrType;
            if (attrType >= EntityAttrType.Attack_Permillage)
            {
                showAttrType = attrType - 1000;
            }

            var attrInfo = AttrInfoHelper.Instance.GetAttrInfo(showAttrType);

            //减伤需要更改显示方式
            var attrName = attrInfo.name;
            if (attrType == EntityAttrType.InputDamageRate ||
                attrType == EntityAttrType.InputDamageRate_Permillage)
            {
                attrName = "减伤";
            }

            if (attrValueType == AddedValueType.Fixed)
            {
                var resultValue = AttrHelper_Client.GetAttrShowValue(attrType, attrValue);

                if ((int)attrType < (int)EntityAttrType.Attack_Permillage)
                {
                    if (isRand && isShowRand)
                    {
                        //随机属性项
                        var randMin = BattleNumShowTool.GetNumShow(attrType, randList[0]);
                        var randMax = BattleNumShowTool.GetNumShow(attrType, randList[1]);
                        str += $"{resultValue} 点 {attrName} ({randMin} ~ {randMax})";
                    }
                    else
                    {
                        //固定属性项
                        str += $"{resultValue} 点 {attrName}";
                    }
                }
                else
                {
                    if (isRand && isShowRand)
                    {
                        //随机千分比属性项
                        // resultValue *= 100.0f;
                        // var randMin = randList[0] * 100.0f;
                        // var randMax = randList[1] * 100.0f;

                        var randMin = BattleNumShowTool.GetNumShow(attrType, randList[0]);
                        var randMax = BattleNumShowTool.GetNumShow(attrType, randList[1]);
                        str += $"{resultValue} {attrName} ({randMin} ~ {randMax})";
                    }
                    else
                    {
                        //固定千分比属性项
                        // resultValue *= 100.0f;
                        str += $"{resultValue} {attrName}";
                    }
                }

                attrStr += str;
            }
            else
            {
                //其他的目前没有需求展示
            }
        }

        return attrStr;
    }
}