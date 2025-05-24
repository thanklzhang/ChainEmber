using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;

namespace Battle
{
    //获得道具
    public class RewardEffect_Item_Gain : BaseBattleRewardEffectOption
    {
        //最终结果奖励
        private int itemConfigId;

        private BattleItem item;

        public override void CalculateRealityReward()
        {
            itemConfigId = GetRandValueByWeights();
            
        }

        public override void OnGain(BattlePlayer gainer)
        {
            item = battle.battleItemMgr.GenerateItem(itemConfigId);
            gainer.GainItem(item);
        }

        // public override List<int> GetIntValueList()
        // {
        //     return new List<int>()
        //         { skillConfigId };
        // }

        public override BattleRewardEffectDTO GetValues()
        {
            //装备这里只给出独立属性即可 不用跟实体挂钩 直接走配置即可
            var dto = base.GetValues();
            dto.intArg1 = itemConfigId;
            // var config = BattleConfigManager.Instance.GetById<IBattleItem>(itemConfigId);
            // dto.intArg1 = config.AttrGroupConfigId;
            // dto.intListArg1 = attrs.Select(attr => AttrHelper.ToDtoAttrValue(
            //     attr.entityAttrType, attr.initValue)).ToList();
            return dto;
        }
    }
}