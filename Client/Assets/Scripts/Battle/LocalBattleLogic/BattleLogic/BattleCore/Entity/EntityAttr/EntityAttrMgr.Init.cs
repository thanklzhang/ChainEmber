using System;
using System.Collections.Generic;
using System.Linq;

namespace Battle
{
    public partial class EntityAttrMgr
    {
        public void InitAttrInfo()
        {
            //人物基础属性
            RefreshEntityBaseAttr();

            //人物等级属性
            RefreshEntityLevelAttr();

            //人物星级属性
            RefreshEntityStarAttr();

            //人物上动态的 buff 属性
            RefreshEntityBuffAttr();
        }

        public void RefreshEntityBaseAttr()
        {
            var baseInfo = BattleConfigManager.Instance.GetById<IEntityAttrBase>(battleEntity.infoConfig.BaseAttrId);

            this.AddAttrValue(EntityAttrGroupType.Base, new AttrOption()
            {
                entityAttrType = EntityAttrType.Attack,
                guid = (int)EntityAttrType.Attack,
                initValue = baseInfo.Attack
            });

            this.AddAttrValue(EntityAttrGroupType.Base, new AttrOption()
            {
                entityAttrType = EntityAttrType.Defence,
                guid = (int)EntityAttrType.Defence,
                initValue = baseInfo.Defence
            });

            this.AddAttrValue(EntityAttrGroupType.Base, new AttrOption()
            {
                entityAttrType = EntityAttrType.MaxHealth,
                guid = (int)EntityAttrType.MaxHealth,
                initValue = baseInfo.Health
            });

            this.AddAttrValue(EntityAttrGroupType.Base, new AttrOption()
            {
                entityAttrType = EntityAttrType.AttackSpeed,
                guid = (int)EntityAttrType.AttackSpeed,
                initValue = baseInfo.AttackSpeed / 1000.0f
            });

            this.AddAttrValue(EntityAttrGroupType.Base, new AttrOption()
            {
                entityAttrType = EntityAttrType.MoveSpeed,
                guid = (int)EntityAttrType.MoveSpeed,
                initValue = baseInfo.MoveSpeed / 1000.0f
            });

            this.AddAttrValue(EntityAttrGroupType.Base, new AttrOption()
            {
                entityAttrType = EntityAttrType.AttackRange,
                guid = (int)EntityAttrType.AttackRange,
                initValue = baseInfo.AttackRange / 1000.0f
            });

            this.AddAttrValue(EntityAttrGroupType.Base, new AttrOption()
            {
                entityAttrType = EntityAttrType.InputDamageRate,
                guid = (int)EntityAttrType.InputDamageRate,
                initValue = baseInfo.InputDamageRate / 1000.0f
            });

            this.AddAttrValue(EntityAttrGroupType.Base, new AttrOption()
            {
                entityAttrType = EntityAttrType.OutputDamageRate,
                guid = (int)EntityAttrType.OutputDamageRate,
                initValue = baseInfo.OutputDamageRate / 1000.0f
            });

            this.AddAttrValue(EntityAttrGroupType.Base, new AttrOption()
            {
                entityAttrType = EntityAttrType.CritRate,
                guid = (int)EntityAttrType.CritRate,
                initValue = baseInfo.CritRate / 1000.0f
            });

            this.AddAttrValue(EntityAttrGroupType.Base, new AttrOption()
            {
                entityAttrType = EntityAttrType.CritDamage,
                guid = (int)EntityAttrType.CritDamage,
                initValue = baseInfo.CritDamage / 1000.0f
            });


            this.AddAttrValue(EntityAttrGroupType.Base, new AttrOption()
            {
                entityAttrType = EntityAttrType.SkillCD,
                guid = (int)EntityAttrType.SkillCD,
                initValue = baseInfo.SkillCD
            });

            this.AddAttrValue(EntityAttrGroupType.Base, new AttrOption()
            {
                entityAttrType = EntityAttrType.TreatmentRate,
                guid = (int)EntityAttrType.TreatmentRate,
                initValue = baseInfo.TreatmentRate / 1000.0f
            });
            
            this.AddAttrValue(EntityAttrGroupType.Base, new AttrOption()
            {
                entityAttrType = EntityAttrType.HealthRecoverSpeed,
                guid = (int)EntityAttrType.HealthRecoverSpeed,
                initValue = baseInfo.HealthRecoverSpeed / 1000.0f
            });
        }

        public void RefreshEntityLevelAttr()
        {
            var allData = BattleConfigManager.Instance.GetList<IEntityAttrLevel>();
            IEntityAttrLevel levelInfo = null;
            foreach (var item in allData)
            {
                if (item.TemplateId == battleEntity.infoConfig.LevelAttrId && item.Level == battleEntity.level)
                {
                    levelInfo = item;
                }
            }

            if (levelInfo != null)
            {
                this.AddAttrValue(EntityAttrGroupType.Level, new AttrOption()
                {
                    entityAttrType = EntityAttrType.Attack,
                    guid = (int)EntityAttrType.Attack,
                    initValue = levelInfo.Attack
                });

                this.AddAttrValue(EntityAttrGroupType.Level, new AttrOption()
                {
                    entityAttrType = EntityAttrType.Defence,
                    guid = (int)EntityAttrType.Defence,
                    initValue = levelInfo.Defence
                });

                this.AddAttrValue(EntityAttrGroupType.Level, new AttrOption()
                {
                    entityAttrType = EntityAttrType.MaxHealth,
                    guid = (int)EntityAttrType.MaxHealth,
                    initValue = levelInfo.Health
                });

                this.AddAttrValue(EntityAttrGroupType.Level, new AttrOption()
                {
                    entityAttrType = EntityAttrType.AttackSpeed,
                    guid = (int)EntityAttrType.AttackSpeed,
                    initValue = levelInfo.AttackSpeed / 1000.0f
                });

                this.AddAttrValue(EntityAttrGroupType.Level, new AttrOption()
                {
                    entityAttrType = EntityAttrType.MoveSpeed,
                    guid = (int)EntityAttrType.MoveSpeed,
                    initValue = levelInfo.MoveSpeed / 1000.0f
                });

                this.AddAttrValue(EntityAttrGroupType.Level, new AttrOption()
                {
                    entityAttrType = EntityAttrType.AttackRange,
                    guid = (int)EntityAttrType.AttackRange,
                    initValue = levelInfo.AttackRange / 1000.0f
                });

                this.AddAttrValue(EntityAttrGroupType.Level, new AttrOption()
                {
                    entityAttrType = EntityAttrType.InputDamageRate,
                    guid = (int)EntityAttrType.InputDamageRate,
                    initValue = levelInfo.InputDamageRate / 1000.0f
                });

                this.AddAttrValue(EntityAttrGroupType.Level, new AttrOption()
                {
                    entityAttrType = EntityAttrType.OutputDamageRate,
                    guid = (int)EntityAttrType.OutputDamageRate,
                    initValue = levelInfo.OutputDamageRate / 1000.0f
                });

                this.AddAttrValue(EntityAttrGroupType.Level, new AttrOption()
                {
                    entityAttrType = EntityAttrType.CritRate,
                    guid = (int)EntityAttrType.CritRate,
                    initValue = levelInfo.CritRate / 1000.0f
                });

                this.AddAttrValue(EntityAttrGroupType.Level, new AttrOption()
                {
                    entityAttrType = EntityAttrType.CritDamage,
                    guid = (int)EntityAttrType.CritDamage,
                    initValue = levelInfo.CritDamage / 1000.0f
                });

                this.AddAttrValue(EntityAttrGroupType.Level, new AttrOption()
                {
                    entityAttrType = EntityAttrType.SkillCD,
                    guid = (int)EntityAttrType.SkillCD,
                    initValue = levelInfo.SkillCD
                });
            }
            else
            {
                BattleLog.LogWarningZxy("the levelInfo is not found : " + battleEntity.configId + " " +
                                         battleEntity.level);
            }
        }

        public void RefreshEntityStarAttr()
        {
            var preMaxHP = (int)this.battleEntity.MaxHealth;
            var preHpRatio = this.battleEntity.CurrHealth / this.battleEntity.MaxHealth;
            
            var allData = BattleConfigManager.Instance.GetList<IEntityAttrStar>();
            IEntityAttrStar levelInfo = null;
            foreach (var item in allData)
            {
                if (item.TemplateId == battleEntity.infoConfig.StarAttrId && item.StarLevel == battleEntity.starLevel)
                {
                    levelInfo = item;
                }
            }

            if (levelInfo != null)
            {
                this.AddAttrValue(EntityAttrGroupType.Star, new AttrOption()
                {
                    entityAttrType = EntityAttrType.Attack,
                    guid = (int)EntityAttrType.Attack,
                    initValue = levelInfo.Attack
                });

                this.AddAttrValue(EntityAttrGroupType.Star, new AttrOption()
                {
                    entityAttrType = EntityAttrType.Defence,
                    guid = (int)EntityAttrType.Defence,
                    initValue = levelInfo.Defence
                });

                this.AddAttrValue(EntityAttrGroupType.Star, new AttrOption()
                {
                    entityAttrType = EntityAttrType.MaxHealth,
                    guid = (int)EntityAttrType.MaxHealth,
                    initValue = levelInfo.Health
                });

                this.AddAttrValue(EntityAttrGroupType.Star, new AttrOption()
                {
                    entityAttrType = EntityAttrType.AttackSpeed,
                    guid = (int)EntityAttrType.AttackSpeed,
                    initValue = levelInfo.AttackSpeed / 1000.0f
                });

                this.AddAttrValue(EntityAttrGroupType.Star, new AttrOption()
                {
                    entityAttrType = EntityAttrType.MoveSpeed,
                    guid = (int)EntityAttrType.MoveSpeed,
                    initValue = levelInfo.MoveSpeed / 1000.0f
                });

                this.AddAttrValue(EntityAttrGroupType.Star, new AttrOption()
                {
                    entityAttrType = EntityAttrType.AttackRange,
                    guid = (int)EntityAttrType.AttackRange,
                    initValue = levelInfo.AttackRange / 1000.0f
                });

                this.AddAttrValue(EntityAttrGroupType.Star, new AttrOption()
                {
                    entityAttrType = EntityAttrType.InputDamageRate,
                    guid = (int)EntityAttrType.InputDamageRate,
                    initValue = levelInfo.InputDamageRate
                });
                this.AddAttrValue(EntityAttrGroupType.Star, new AttrOption()
                {
                    entityAttrType = EntityAttrType.OutputDamageRate,
                    guid = (int)EntityAttrType.OutputDamageRate,
                    initValue = levelInfo.OutputDamageRate
                });
                this.AddAttrValue(EntityAttrGroupType.Star, new AttrOption()
                {
                    entityAttrType = EntityAttrType.CritRate,
                    guid = (int)EntityAttrType.CritRate,
                    initValue = levelInfo.CritRate
                });
                this.AddAttrValue(EntityAttrGroupType.Star, new AttrOption()
                {
                    entityAttrType = EntityAttrType.CritDamage,
                    guid = (int)EntityAttrType.CritDamage,
                    initValue = levelInfo.CritDamage
                });
                this.AddAttrValue(EntityAttrGroupType.Star, new AttrOption()
                {
                    entityAttrType = EntityAttrType.InputDamageRate,
                    guid = (int)EntityAttrType.InputDamageRate,
                    initValue = levelInfo.InputDamageRate
                });
            }
            else
            {
                BattleLog.LogWarningZxy("the starLevelInfo is not found : " + battleEntity.configId + " " +
                                         battleEntity.starLevel);
            }
            
            //最大生命值的更改需要重新计算
            var nowMaxHp = (int)this.battleEntity.MaxHealth;

            if (nowMaxHp != preMaxHP)
            {
                //前后保持当前生命百分比相同
                var result = nowMaxHp * preHpRatio;
                if (result < 1) result = 1;
                this.battleEntity.SetCurrHp(result);
            }
        }

        public void RefreshEntityBuffAttr()
        {
        }
    }
}