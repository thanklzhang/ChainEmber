namespace Battle
{
    public enum AddedDamageCalculateType
    {
        //a *= (1 + b)
        Normal = 0,

        //a *= b
        Progress = 1
    }

    public class DamageCalculateTool
    {
        public static BattleEntity GetCalculateTarget(AttrCalculateTargetType type, BattleEntity releaser,
            BattleEntity skillTarget)
        {
            var calculateTargetEntity = releaser;

            var calculateTargetType = type;

            if (calculateTargetType == AttrCalculateTargetType.Releaser)
            {
                calculateTargetEntity = releaser;
            }
            else if (calculateTargetType == AttrCalculateTargetType.EffectTarget)
            {
                calculateTargetEntity = skillTarget;
            }
            else if (calculateTargetType == AttrCalculateTargetType.BeSummonMaster)
            {
                calculateTargetEntity = releaser.beSummonMaster;
            }
            else if (calculateTargetType == AttrCalculateTargetType.TeamLeader)
            {
                calculateTargetEntity = releaser.teamLeader;
            }
            else
            {
                calculateTargetEntity = releaser;
            }

            return calculateTargetEntity;
        }

        public static float Calculate(DamageCalculate damageCalculate, BattleEntity releaser, BattleEntity skillTarget,
            SkillEffectContext context, AddedDamageCalculateType calculateType = AddedDamageCalculateType.Normal)
        {
            float finalDamage = 0;
            foreach (var item in damageCalculate.calculateOptionList)
            {
                var damageOption = item;
                var type = damageOption.addedValueType;
                var value = damageOption.value;
                var targetType = damageOption.targetType;
                var calculateTargetSmallEntity = GetCalculateTarget(targetType, releaser, skillTarget);
                if (null == calculateTargetSmallEntity)
                {
                    continue;
                }

                //先默认一次攻击只是一种伤害
                //var dd = damageOption.effectDamageType;
                if (type == AddedValueType.Fixed)
                {
                    finalDamage += value;
                }
                else if (type == AddedValueType.PhysicAttack_Permillage)
                {
                    finalDamage += calculateTargetSmallEntity.Attack * value / 1000.0f;
                    //_G.Log("releaser.Attack : " + releaser.Attack);
                    //_G.Log("releaser.value : " + value);
                }
                //else if (type == AddedValueType.MagicAttackPercent)
                //{
                //    finalDamage += releaser.FinalAttak * value / 100;
                //}
                else if (type == AddedValueType.Defence_Permillage)
                {
                    finalDamage += calculateTargetSmallEntity.Defence * value / 1000.0f;
                }
                else if (type == AddedValueType.MaxHealth_Permillage)
                {
                    finalDamage += calculateTargetSmallEntity.MaxHealth * value / 1000.0f;
                }
                else if (type == AddedValueType.HurtDamage_Permillage)
                {
                    finalDamage += context.damage * value / 1000.0f;
                }
                else if (type == AddedValueType.LostHealth_Permillage_Rate)
                {
                    //比值 不参与这里的额伤害计算 一般只会在附加值效果中生效
                    // var maxHealth =
                    //     calculateTargetSmallEntity.MaxHealth;
                    // var currHealth = calculateTargetSmallEntity.GetCurrHp();
                    // var ratio = (maxHealth - currHealth) / (float)maxHealth;
                    // var resultValue = ratio * value;
                    // finalDamage += resultValue;
                }

                // if (this.context.fromSkill.isNormalAttack)
                // {
                //     var addedDamageDic = this.context.fromSkill.releaser.normalAttackAddedDamageDic;
                //
                //     if (addedDamageDic != null)
                //     {
                //         var total = 0.0f;
                //         foreach (var kv in addedDamageDic)
                //         {
                //             var addedDamage = kv.Value;
                //             var damageCalculateList = addedDamage.damageCalculateList;
                //             foreach (var damageCalculate in damageCalculateList)
                //             {
                //                 damageCalculate.calculateOptionList
                //
                //                 total += damageCalculate.;
                //             }
                //         }
                //
                //         finalDamage *= (1.0f + total);
                //     }


                // if (this.tableConfig.IsAddedNormalAttackAddedDamage)
                // {
                //     var addedNormalAttackAddedDamage = this.context.fromSkill.GetAddedNormalAttackAddedDamage();
                //     if (addedNormalAttackAddedDamage != null)
                //     {
                //         finalDamage += addedNormalAttackAddedDamage.value;
                //     }
                // }
                //}
            }

            //附加强化伤害（伤害倍数的增加在这里）
            var totalScale = 0.0f;
            foreach (var item in damageCalculate.calculateOptionList)
            {
                var damageOption = item;
                var type = damageOption.addedValueType;
                var value = damageOption.value;
                var targetType = damageOption.targetType;
                var calculateTargetSmallEntity = GetCalculateTarget(targetType, releaser, skillTarget);
                if (null == calculateTargetSmallEntity)
                {
                    continue;
                }
                else if (type == AddedValueType.LostHealth_Permillage_Rate)
                {
                    var maxHealth =
                        calculateTargetSmallEntity.MaxHealth;
                    var currHealth = calculateTargetSmallEntity.GetCurrHp();
                    var ratio = (maxHealth - currHealth) / (float)maxHealth;
                    var resultValue = ratio * value;
                    totalScale += resultValue;
                }
            }

            if (calculateType == AddedDamageCalculateType.Normal)
            {
                finalDamage *= (1 + totalScale);
            }
            else if (calculateType == AddedDamageCalculateType.Progress)
            {
                finalDamage *= totalScale;
            }

            return finalDamage;
        }
    }
}