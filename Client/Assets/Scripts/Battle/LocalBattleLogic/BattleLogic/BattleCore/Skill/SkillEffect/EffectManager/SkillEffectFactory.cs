namespace Battle
{
    public enum SkillEffectType
    {
        ProjectileEffect = 3,
        CalculateEffect = 4,
        AreaEffect = 13,
        BuffEffect = 14,
        MoveEffect = 16,
        CollisionGroupEffect = 17,
        PassiveEffect = 18,
        SummonEffect = 26,
        GeneralEffect = 27,
        LinkGroupEffect = 28,
        ConditionActionEffect = 29,
        TimeSequenceEffect = 30,
    }

    public class SkillEffectFactory
    {
        public static SkillEffectType GetTypeByConfigId(int configId)
        {
            //这里也可以根据表格提前存数据 然后查找是否存在字典中
            var type = (SkillEffectType)(configId / 1000000);
            return type;
        }

        public static SkillEffect GetInstanceByType(SkillEffectType type)
        {
            SkillEffect effect = null;
            if (type == SkillEffectType.ProjectileEffect)
            {
                effect = new ProjectileEffect();
            }
            else if (type == SkillEffectType.BuffEffect)
            {
                effect = new BuffEffect();
            }
            else if (type == SkillEffectType.AreaEffect)
            {
                effect = new AreaEffect();
            }
            else if (type == SkillEffectType.MoveEffect)
            {
                effect = new MoveEffect();
            }
            else if (type == SkillEffectType.CalculateEffect)
            {
                effect = new CalculateEffect();
            }
            else if (type == SkillEffectType.CollisionGroupEffect)
            {
                effect = new CollisionGroupEffect();
            }
            else if (type == SkillEffectType.PassiveEffect)
            {
                effect = new PassiveEffect();
            }
            else if (type == SkillEffectType.SummonEffect)
            {
                effect = new SummonEffect();
            }
            else if (type == SkillEffectType.GeneralEffect)
            {
                effect = new GeneralEffect();
            }
            else if (type == SkillEffectType.LinkGroupEffect)
            {
                effect = new LinkGroupEffect();
            }
            else if (type == SkillEffectType.ConditionActionEffect)
            {
                effect = new ConditionActionEffect();
            }
            else if (type == SkillEffectType.TimeSequenceEffect)
            {
                effect = new TimeSequenceEffect();
            }

            if (null == effect)
            {
                BattleLog.LogZxy("effect is null : " + type);
            }

            return effect;
        }
    }
}