using Battle;

public class SkillUpgradeConfigHelper
{
    public static ISkillUpgradeParam GetConfig()
    {
        var configId = 1;
        return BattleConfigManager.Instance.GetById<ISkillUpgradeParam>(configId);
    }

    public static int GetDecomposeExp(int level)
    {
        var config = GetConfig();

        var exp = 0;
        if (level <= 0)
        {
            BattleLog.LogWarning("the level must bigger than 0 , level : " + level);
            level = 1;
        }

        if (level - 1 < config.DecomoseExpPerLevel.Count)
        {
            exp = config.DecomoseExpPerLevel[level - 1];
        }

        return exp;
    }

    public static int GetUpgradeNeedExpByLevel(int level)
    {
        var config = GetConfig();

        var exp = 0;
        if (level - 1< config.UpgradeExpPerLevel.Count)
        {
            exp = config.UpgradeExpPerLevel[level - 1];
        }

        return exp;
    }

    //获得技能经验所在的技能等级
    public static int GetLevelByExp(int exp)
    {
        int level = 1;
        int currTargetExp = 1;//本身是 1
        var config = GetConfig();

        for (int i = 0; i < config.UpgradeExpPerLevel.Count; i++)
        {
            currTargetExp += config.UpgradeExpPerLevel[i];
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

    public static int GetSkillGroupId(int skillConfigId)
    {
        return skillConfigId / 100;
    }

    public static int GetLevelSkillConfigId(int skillGroupId, int level)
    {
        return skillGroupId * 100 + level;
    }

    public static int GetSkillLevel(int skillConfigId)
    {
        var config = BattleConfigManager.Instance.GetById<ISkill>(skillConfigId);
        if (null == config)
        {
            return 1;
        }

        return config.Level;
    }

    public static int GetUpgradeSkillConfigId(int skillConfigId)
    {
        var groupId = GetSkillGroupId(skillConfigId);
        var level = GetSkillLevel(skillConfigId);

        level += 1;
        
        var nextId = GetLevelSkillConfigId(groupId,level);
        return nextId;
    }
}