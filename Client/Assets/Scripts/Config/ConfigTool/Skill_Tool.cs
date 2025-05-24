using System.Collections.Generic;
using Battle;

namespace Config
{
    public static class Skill_Tool
    {
        // public static List<int> GetSkilConfigIdsByLevels(List<int> skillLevels,int count = 1)
        // {
        //     List<int> list = new List<int>();
        //
        //     var allSkill = Config.ConfigManager.Instance.GetList<Config.Skill>();
        //     
        //     
        //     for (int i = 0; i < allSkill.Count; i++)
        //     {
        //         var fromSkillConfig = allSkill[i];
        //         if(skillLevels.Contains(fromSkillConfig.level))
        //     }
        //     
        //     var chapter = Config.ConfigManager.Instance.GetById<Config.MainTaskChapter>(chapterId);
        //     if (chapter != null)
        //     {
        //         var strs = chapter.StageList.Split(',');
        //         foreach (var idStr in strs)
        //         {
        //             var id = int.Parse(idStr);
        //             list.Add(id);
        //         }
        //     }
        //     return list;
        // }

        //
        
        //获取释放时候产生关键效果的区域效果 id
        public static int GetKeyAreaEft(int skillId)
        {
            var skillConfig = BattleConfigManager.Instance.GetById<ISkill>(skillId);
            if (skillConfig != null)
            {
                if (skillConfig.EffectList.Count > 0)
                {
                    for (int i = 0; i < skillConfig.EffectList.Count; i++)
                    {
                        var eftId = skillConfig.EffectList[i];
                        var type = SkillEffectFactory.GetTypeByConfigId(eftId);
                        if (type == SkillEffectType.AreaEffect)
                        {
                            return eftId;
                        }
                        else if (type == SkillEffectType.ConditionActionEffect)
                        {
                            //如果有影响释放目标单位等的条件判断 如释放经的时候生命值小于50%给自己套盾，否则释放周围区域伤害   这种目前暂定，
                            //如需拓展 那么需要深度检测条件 目前没那必要 有必要再加
                        }
                    }
                }
            }

            return -1;
        }
    }
}