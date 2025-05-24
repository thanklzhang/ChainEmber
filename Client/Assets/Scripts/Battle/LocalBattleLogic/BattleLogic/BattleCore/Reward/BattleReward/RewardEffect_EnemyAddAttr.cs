using System;
using System.Collections.Generic;
using System.Linq;

namespace Battle
{
    //敌人属性增强效果实现
    public class RewardEffect_EnemyAddAttr : BaseBattleRewardEffectOption
    {
        //最终结果奖励
        public int attrGroupId;

        // private int skillConfigId;
        private List<AttrOption> attrs;

        private List<List<AttrOption>> totalAttrs = new List<List<AttrOption>>();

        public override void CalculateRealityReward()
        {
            GenAttrs();
        }

        private void GenAttrs()
        {
            attrGroupId = GetRandValueByWeights();

            //根据配置创建属性
            AttrHelper.AttrCreateArg arg = new AttrHelper.AttrCreateArg();
            arg.guid = GenRewardEffectGuid();
            arg.attrGroupType = EntityAttrGroupType.BattleReward;
            arg.attrGroupConfigId = this.attrGroupId;

            attrs = AttrHelper.GetAttrOptions(arg,
                this.player.entity.GetBattle());

            totalAttrs.Add(attrs);
        }

        public override void OnGain(BattlePlayer gainer)
        {
            base.OnGain(gainer);

            // //TODO 将这个比例存起来，在其他处应用
            // battle.SetEnemyAttrAddedRate(attrAddedRate);

            if (hasGainEffectTimes > 0)
            {
                //保证每次获得都是新的属性 并且 保证 attr 的 guid 也是新的
                GenAttrs();
            }

            totalAttrs.Add(attrs);

            battle.AddEnemyAddedAttr(attrs);
        }

        public override void OnWaveReadyProcessStart()
        {
            var paramList = this.rewardOptionConfig.ParamIntList;
            bool isClear = false;
            if (paramList.Count > 0)
            {
                isClear = paramList[0] == 1;
            }

            if (isClear)
            {
                totalAttrs.Remove(attrs);
                battle.RemoveEnemyAddedAttr(attrs);
            }
        }

        public override BattleRewardEffectDTO GetValues()
        {
            BattleRewardEffectDTO dto = base.GetValues();
            dto.intArg1 = attrGroupId; // 存储增强比例
            dto.intListArg1 = attrs.Select(attr => AttrHelper.ToDtoAttrValue(
                attr.entityAttrType, attr.initValue)).ToList();
            return dto;
        }
    }
}