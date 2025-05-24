using System.Collections.Generic;

namespace Battle
{
    //附加值类型
    public enum AddedValueType
    {
        //固定伤害值
        Fixed = 0,

        //物理攻击的千分比
        PhysicAttack_Permillage = 1,

        //魔法攻击的千分比数量
        MagicAttack_Permillage = 2,

        //生命值的千分比数量
        MaxHealth_Permillage = 3,

        //护甲的千分比数量
        Defence_Permillage = 4,

        //造成伤害的千分比数量
        HurtDamage_Permillage = 20,

        //已经损失的生命值的千分比值
        LostHealth_Permillage_Rate = 30,

        //召唤兽造成的伤害的千分比值
        SummonEntityHurtDamage_Permilage = 40,

        //召唤兽受到伤害的千分比值
        SummonEntityBeHurtDamage_Permilage = 41,
    }

    //效果伤害类型
    public enum EffectDamageType
    {
        Null = 0,
        Physic = 1,
        Magic = 2
    }

    // public enum CalculateEffectTargetType
    // {
    //     //技能释放者
    //     SkillReleaser = 0,
    //
    //     //技能目标者
    //     SkillTarget = 1,
    //
    //     //技能释放者 的 召唤者（作为召唤物）
    //     BeSummonMaster = 5,
    // }

    //施加效果的实体类型(即将废弃)
    public enum EffectEntityTargetType
    {
        //选取的单位
        Selected = 0,

        //技能释放者
        SkillReleaser = 1,

        //技能目标者
        SkillTarget = 2
    }

    //效果附加值项
    public class CalculateEffectAddedOption
    {
        public AddedValueType addedValueType;

        public int value;

        // public EffectDamageType effectDamageType;
        public AttrCalculateTargetType targetType;
    }


    //伤害计算
    public class DamageCalculate
    {
        public int damageSrcGuid;
        public List<CalculateEffectAddedOption> calculateOptionList;
        public EffectDamageType finalEffectDamageType;

        public static DamageCalculate Create(int damageSrcGuid, int calculateEftId)
        {
            var tableConfig = BattleConfigManager.Instance.GetById<ICalculateEffect>(calculateEftId);
            DamageCalculate damageCalculate = new DamageCalculate();
            damageCalculate.damageSrcGuid = damageSrcGuid;

            damageCalculate.calculateOptionList = new List<CalculateEffectAddedOption>();
            //待优化 不能总是分割字符串 所以存起来或者表格导出那更改逻辑
            var addedValueGroup = tableConfig.AddedValueGroup;
            foreach (var addedValueOption in addedValueGroup)
            {
                var addedType = addedValueOption.Count > 0 ? addedValueOption[0] : 0;
                var addedValue = addedValueOption.Count > 1 ? addedValueOption[1] : 0;
                var addedDamageType = addedValueOption.Count > 2 ? addedValueOption[2] : 0;


                //_Battle_Log.Log(string.Format("addedType:{0} addedValue:{1} addedDamageType:{2}  {3}",
                //    addedType, addedValue, addedDamageType, config.Name));

                CalculateEffectAddedOption calOption = new CalculateEffectAddedOption();
                calOption.addedValueType = (AddedValueType)addedType;
                calOption.value = addedValue;
                calOption.targetType = (AttrCalculateTargetType)addedDamageType;

                damageCalculate.calculateOptionList.Add(calOption);
            }

            damageCalculate.finalEffectDamageType = (EffectDamageType)tableConfig.FinalEffectType;
            return damageCalculate;
        }
    }


    public class CalculateEffect : SkillEffect
    {
        //public BattleEntity target;
        //simulate config
        public ICalculateEffect tableConfig;
        DamageCalculate damageCalculate;

        public void SetDamageCalculate(DamageCalculate damageCal)
        {
            damageCalculate = damageCal;
        }

        internal int targetGuid;

        //
        Battle battle;

        public override void OnInit()
        {
            battle = this.context.battle;
            tableConfig = BattleConfigManager.Instance.GetById<ICalculateEffect>(this.configId);

            isAutoDestroy = true;
            //var config = ConfigManager.Instance.GetById<Config.CalculateEffect>(effectConfigId);
            // var config = BattleConfigManager.Instance.GetById<ICalculateEffectConfig>(configId);
            targetGuid = context.fromSkill.targetGuid;

            //填充伤害结算信息
            // DamageCalculate damageCalculate = new DamageCalculate();
            // damageCalculate.damageSrcGuid = context.fromSkill.releaser.guid;
            //
            // damageCalculate.calculateOptionList = new List<CalculateEffectAddedOption>();
            // //待优化 不能总是分割字符串 所以存起来或者表格导出那更改逻辑
            // var addedValueGroup = tableConfig.AddedValueGroup;
            // foreach (var addedValueOption in addedValueGroup)
            // {
            //     var addedType = addedValueOption.Count > 0 ? addedValueOption[0] : 0;
            //     var addedValue = addedValueOption.Count > 1 ? addedValueOption[1] : 0;
            //     var addedDamageType = addedValueOption.Count > 2 ? addedValueOption[2] : 0;
            //
            //
            //     //_Battle_Log.Log(string.Format("addedType:{0} addedValue:{1} addedDamageType:{2}  {3}",
            //     //    addedType, addedValue, addedDamageType, config.Name));
            //
            //     CalculateEffectAddedOption calOption = new CalculateEffectAddedOption();
            //     calOption.addedValueType = (AddedValueType)addedType;
            //     calOption.value = addedValue;
            //     calOption.targetType = (AttrCalculateTargetType)addedDamageType;
            //
            //     damageCalculate.calculateOptionList.Add(calOption);
            // }
            //
            // damageCalculate.finalEffectDamageType = (EffectDamageType)tableConfig.FinalEffectType;

            var damageCalculate = DamageCalculate.Create(context.fromSkill.releaser.guid, this.configId);

            SetDamageCalculate(damageCalculate);
        }

        public override CreateEffectInfo ToCreateEffectInfo()
        {
            var resId = tableConfig.EffectResId;

            var followEntityGuid = -1;
            var effectPosType = EffectPosType.Custom_Pos;
            if (context.selectEntities.Count > 0)
            {
                var selectEntity = context.selectEntities[0];

                effectPosType = EffectPosType.Hit_Pos;

                if (1 == tableConfig.IsEffectFollowTarget)
                {
                    followEntityGuid = selectEntity.guid;
                }
            }

            CreateEffectInfo createInfo = new CreateEffectInfo();
            createInfo.guid = guid;
            createInfo.resId = resId;
            createInfo.effectPosType = effectPosType;
            createInfo.followEntityGuid = followEntityGuid;
            createInfo.isAutoDestroy = isAutoDestroy;

            return createInfo;
        }


        public override void OnStart()
        {
            this.TriggerEffect();
            this.SetWillEndState();
        }

        public void TriggerEffect()
        {
            if (1 == this.tableConfig.IsForShow)
            {
                return;
            }

            //_G.Log(string.Format("CalculateEffect effect of guid : {0} TriggerEffect", this.guid));
            //根据 calculateList 开始计算最终值

            //TODO:
            //注意这里根据 EffectDamageType 不同要分开计算 
            //并且不同的 EffectAddedType 计算后 总共算一次结算伤害
            //也就是说：例如：造成 150 点 物理伤害 和 100 点魔法伤害
            //经过护甲计算后 目标造成了物理伤害和魔法伤害 但该攻击只算一次结算伤害(攻击特效)

            BattleEntity skillTarget = null;
            if (this.context.selectEntities is { Count: > 0 })
            {
                skillTarget = this.context.selectEntities[0];
            }

            var battle = this.context.battle;


            var releaser = this.context.fromSkill.releaser;
            var contextDamageRate = this.context.damageChangeRate;
            float finalDamage = DamageCalculateTool.Calculate(damageCalculate, releaser, skillTarget, this.context
            ,AddedDamageCalculateType.Normal);
            
            //计算普通伤害附加（算作一次伤害）
            if (this.context.fromSkill.isNormalAttack)
            {
                var addedDamageDic = this.context.fromSkill.releaser.normalAttackAddedDamageDic;

                if (addedDamageDic != null)
                {
                    var total = 0.0f;
                    foreach (var kv in addedDamageDic)
                    {
                        var addedDamage = kv.Value;
                        var damageCalculateList = addedDamage.damageCalculateList;
                        foreach (var dc in damageCalculateList)
                        {
                            float damage = DamageCalculateTool.Calculate(dc, releaser, skillTarget, this.context ,AddedDamageCalculateType.Progress);
                            total += damage;
                        }
                    }

                    finalDamage += total;
                }
            }

            //上下文中的伤害递减
            finalDamage *= (1 + contextDamageRate);

            //计算暴击
            if (releaser != null)
            {
                var rand = BattleRandom.GetRandFloat(0, 1.0f, this.battle);
                if (rand <= releaser.CritRate)
                {
                    var critDamage = releaser.CritDamage;
                    finalDamage = finalDamage * (critDamage);
                }
            }

            if (finalDamage < 0)
            {
                //治疗比率
                finalDamage = finalDamage * releaser.TreatmentRate;
            }
            //将伤害 施加在 实体 上
            var targets = new List<BattleEntity>();

            targets = this.context.selectEntities;
            if (targets.Count > 0)
            {
                //目前一个 calculate 只对应一个 target 造成伤害
                var target = targets[0];
                //检测 link 效果
                CalculateNextLinkEffect(target, finalDamage);
            }
        }

        Dictionary<int, List<BattleEntity>> hasSignDic = new Dictionary<int, List<BattleEntity>>();

        public void CalculateNextLinkEffect(BattleEntity target, float damage)
        {
            //计算分摊伤害
            var buffs = target.GetBuffs();
            var nowDamage = damage;
            bool isCanTrueHurt = true;
            foreach (var kv in buffs)
            {
                var buff = kv.Value;
                var linkGroup = buff.linkGroupEft;
                if (linkGroup != null)
                {
                    LinkEffectType effectType = (LinkEffectType)linkGroup.tableConfig.EffectType;
                    if (effectType == LinkEffectType.ShareDamage || effectType == LinkEffectType.CopyDamage)
                    {
                        if (!TryToGetSign(target, linkGroup))
                        {
                            isCanTrueHurt = false;

                            var entities = linkGroup.linkEntityList;

                            nowDamage = damage;
                            if (effectType == LinkEffectType.ShareDamage)
                            {
                                nowDamage = damage / entities.Count;
                            }
                            else if (effectType == LinkEffectType.CopyDamage)
                            {
                                nowDamage = damage;
                            }

                            for (int i = 0; i < entities.Count; i++)
                            {
                                var entity = entities[i];
                                TryToAddSignDic(entity, linkGroup);
                                this.CalculateNextLinkEffect(entity, nowDamage);
                            }
                        }
                    }
                }
            }

            if (isCanTrueHurt)
            {
                //这个单位所关联的 link 都标记完了 可以直接伤害了
                CalculateDamage(target, nowDamage);
            }
        }


        public void TryToAddSignDic(BattleEntity target, LinkGroupEffect link)
        {
            List<BattleEntity> list = new List<BattleEntity>();
            if (hasSignDic.ContainsKey(link.guid))
            {
                list = hasSignDic[link.guid];
            }
            else
            {
                list = new List<BattleEntity>();
                hasSignDic.Add(link.guid, list);
            }

            if (!list.Contains(target))
            {
                list.Add(target);
            }
        }

        public bool TryToGetSign(BattleEntity target, LinkGroupEffect linkGroup)
        {
            if (null == linkGroup)
            {
                return false;
            }

            if (hasSignDic.ContainsKey(linkGroup.guid))
            {
                var list = hasSignDic[linkGroup.guid];
                if (list.Contains(target))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public void CalculateDamage(BattleEntity target, float finalDamage)
        {
            target.OnBeHurt((int)finalDamage, this.context.fromSkill);
        }
    }
}