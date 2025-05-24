using System.Collections.Generic;
using System.Threading;
using UnityEngine.UIElements;

namespace Battle
{
    //通用效果 有些简单的效果可在这里写
    public class GeneralEffect : SkillEffect
    {
        //public Config.BuffEffect tableConfig;
        public IGeneralEffect tableConfig;

        //BuffType buffType;
        Battle battle;

        public override void OnInit()
        {
            battle = this.context.battle;
            tableConfig = BattleConfigManager.Instance.GetById<IGeneralEffect>(this.configId);
        }

        public override CreateEffectInfo ToCreateEffectInfo()
        {
            CreateEffectInfo createInfo = new CreateEffectInfo();
            createInfo.guid = guid;
            // createInfo.resId = tableConfig.EffectResId;
            createInfo.effectPosType = EffectPosType.Custom_Pos;
            createInfo.followEntityGuid = -1;
            createInfo.isSync = false;
            // if (createInfo.resId > 0)
            // {
            //     createInfo.isAutoDestroy = true;
            // }

            return createInfo;
        }

        public override void OnStart()
        {
            base.OnStart();
            var triggerType = (GeneralEffectTriggerType)this.tableConfig.TriggerEventType;
            if (triggerType == GeneralEffectTriggerType.ChangeBuffLayoutValue)
            {
                var buffConfigId = int.Parse(tableConfig.TriggerParamList[0]);
                var changeValue = int.Parse(tableConfig.TriggerParamList[1]);
                ChangeBuffLayoutValue(buffConfigId, changeValue);
            }
            else if (triggerType == GeneralEffectTriggerType.DispelAllNegativeBuff)
            {
                DispelAllNegativeBuff();
            }
            // else if (triggerType == GeneralEffectTriggerType.AddNormalAttackAddedDamage)
            // {
            //     var calculateEftId = int.Parse(tableConfig.TriggerParamList[0]);
            //     AddNormalAttackAddedDamage(calculateEftId);
            // }
        }

        BattleEntity GetTarget()
        {
            if (this.context.selectEntities != null &&
                this.context.selectEntities.Count > 0)
            {
                return this.context.selectEntities[0];
            }

            return null;
        }

        public void ChangeBuffLayoutValue(int buffId, int changeValue)
        {
            var entity = GetTarget();
            entity?.ChangeBuffLayer(buffId, changeValue);
        }

        public void DispelAllNegativeBuff()
        {
            var entity = GetTarget();
            if (entity != null)
            {
                var buffs = entity.GetBuffs();
                foreach (var kv in buffs)
                {
                    var buff = kv.Value;
                    var affectType = (SkillAffectType)buff.tableConfig.AffectType;
                    if (1 == buff.tableConfig.IsCanBeClear)
                    {
                        if (affectType == SkillAffectType.Negative)
                        {
                            buff.ForceDelete();
                        }
                    }
                }
            }
        }
        
        // public void AddNormalAttackAddedDamage(int calculateEftId)
        // { 
        //     var config = BattleConfigManager.Instance.GetById<ICalculateEffect>(calculateEftId);
        //     if (config != null)
        //     {
        //         var target = GetTarget();
        //         var damageCalculate = DamageCalculate.Create(context.fromSkill.releaser.guid, calculateEftId);
        //
        //         target.AddNormalAttackAddedDamage(damageCalculate);
        //
        //
        //     }
        // }

        public override void OnUpdate(float timeDelta)
        {
            //CheckCollider();
        }


        public override void OnEnd()
        {
        }
    }

    public enum GeneralEffectTriggerType
    {
        ChangeBuffLayoutValue = 5,
        DispelAllNegativeBuff = 11,
        // AddNormalAttackAddedDamage = 20
    }
}