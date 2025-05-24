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
    public class RewardEffect_Item_Copy : BaseBattleRewardEffectOption
    {
        //最终结果奖励
        private int itemConfigId;

        private BattleItem item;

        public override void CalculateRealityReward()
        {
            // itemConfigId = GetRandValueByWeights();
            List<int> allItemIds = new List<int>();
            var warehouseItemIds = player.warehouseItemBar.GetAllItemIdList();
            var entityItemIds = player.entity.itemBar.GetAllItemIdList();
            var member = player.entity.GetTeamMemberList();
            if (warehouseItemIds.Count > 0)
            {
                allItemIds.AddRange(warehouseItemIds);
            }

            if (entityItemIds.Count > 0)
            {
                allItemIds.AddRange(entityItemIds);
            }

            if (member.Count > 0)
            {
                foreach (var memberEntity in member)
                {
                    var memberItemIds = memberEntity.itemBar.GetAllItemIdList();
                    if (memberItemIds.Count > 0)
                    {
                        allItemIds.AddRange(memberItemIds);
                    }
                }
            }
            
       

            if (allItemIds.Count == 0)
            {
                return;
            }
            //从 allItemIds 中随机取一个
            var rendIndex = BattleRandom.Next(0, allItemIds.Count);
            itemConfigId = allItemIds[rendIndex];

        }

        public override void OnGain(BattlePlayer gainer)
        {
            if (itemConfigId == 0)
            {
                return;
            }

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