using Battle;

public class EntityUpgradeConfigHelper
{
    public static IEntityUpgradeParam GetConfig()
    {
        var configId = 1;
        return BattleConfigManager.Instance.GetById<IEntityUpgradeParam>(configId);
    }

    public static int GetDecomposeStarExp(int level)
    {
        var config = GetConfig();

        var exp = 0;
        if (level <= 0)
        {
            BattleLog.LogWarning("the level must bigger than 0 , level : " + level);
            level = 1;
        }

        if (level - 1 < config.DecomposeExpPerStarLevel.Count)
        {
            exp = config.DecomposeExpPerStarLevel[level - 1];
        }

        return exp;
    }

    public static int GetUpgradeNeedExpByLevel(int level)
    {
        var config = GetConfig();

        var exp = 0;
        if (level - 1< config.UpgradeExpPerStarLevel.Count)
        {
            exp = config.UpgradeExpPerStarLevel[level - 1];
        }

        return exp;
    }

    //?????????????????????
    public static int GetLevelByExp(int exp)
    {
        int level = 1;
        int currTargetExp = 1;//?????? 1
        var config = GetConfig();

        for (int i = 0; i < config.UpgradeExpPerStarLevel.Count; i++)
        {
            currTargetExp += config.UpgradeExpPerStarLevel[i];
            if (exp < currTargetExp)
            {
                level = i;
                break;
            }
            else
            {
                level = i + 1;
            }
        }

        return level;
    }
    //
    // public static int GetSkillGroupId(int skillConfigId)
    // {
    //     return skillConfigId / 100;
    // }
    //
    // public static int GetLevelSkillConfigId(int skillGroupId, int level)
    // {
    //     return skillGroupId * 100 + level;
    // }
    //
    // public static int GetSkillLevel(int skillConfigId)
    // {
    //     var config = BattleConfigManager.Instance.GetById<ISkill>(skillConfigId);
    //     if (null == config)
    //     {
    //         return 1;
    //     }
    //
    //     return config.Level;
    // }
    //
    // public static int GetUpgradeSkillConfigId(int skillConfigId)
    // {
    //     var groupId = GetSkillGroupId(skillConfigId);
    //     var level = GetSkillLevel(skillConfigId);
    //
    //     level += 1;
    //     
    //     var nextId = GetLevelSkillConfigId(groupId,level);
    //     return nextId;
    // }
}