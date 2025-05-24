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
    //随机给一个队友加属性
    public class RewardEffect_TeamMember_TeammateAddRandAttr : BaseBattleRewardEffectOption
    {
        //最终结果奖励
        public int attrGroupId;

        // private int skillConfigId;
        private List<AttrOption> attrs;
        
        public int randMemberGuid;

        public override void CalculateRealityReward()
        {
            // var idList = rewardConfig.ValueList;
            // var weightList = rewardConfig.WeightList;
            //
            // List<int> filterIdList = new List<int>();
            // List<int> filterWeightList = new List<int>();
            // for (int i = 0; i < idList.Count; i++)
            // {
            //     var skillId = idList[i];
            //     var weight = weightList[i];
            //     // if (!skills.ContainsKey(skillId))
            //     {
            //         filterIdList.Add(skillId);
            //         filterWeightList.Add(weight);
            //     }
            // }
            //
            // var index = BattleRandom.GetNextIndexByWeights(filterWeightList);
            //
            // buffConfigId = filterIdList[index];

            attrGroupId = GetRandValueByWeights();
            
            //根据配置创建属性
            AttrHelper.AttrCreateArg arg = new AttrHelper.AttrCreateArg();
            arg.guid = GenRewardEffectGuid();
            arg.attrGroupType = EntityAttrGroupType.BattleReward;
            arg.attrGroupConfigId = this.attrGroupId;
            
            attrs = AttrHelper.GetAttrOptions(arg,
                this.player.entity.GetBattle());
            
        }

        public override void OnGain(BattlePlayer gainer)
        {
            // AttrHelper.AttrCreateArg arg = new AttrHelper.AttrCreateArg();
            // arg.guid = this.guid;
            // arg.attrGroupType = EntityAttrGroupType.BattleReward;
            // arg.attrGroupConfigId = this.attrGroupId;
            //
             // attrs = this.player.AddAttrToRandMemeber(arg);
             
             var memberList = this.player.entity.GetTeamMemberDic().Values.ToList();
             if (memberList.Count > 0)
             {
                 var index = BattleRandom.GetRandInt(0, memberList.Count, this.player.entity.GetBattle());
                 var randMember = memberList[index];

                 randMemberGuid = randMember.guid;;
                 randMember.AddAttrs(EntityAttrGroupType.BattleReward, attrs);
             }
             else
             {
                 //给自己加
                 randMemberGuid  = this.player.entity.guid;
                 this.player.entity.AddAttrs(EntityAttrGroupType.BattleReward, attrs);
             }
             
            
            // player.entity.AddAttrs(EntityAttrGroupType.BattleReward, attrs);

        }

        public override BattleRewardEffectDTO GetValues()
        {
            var dto = base.GetValues();
            dto.intArg1 = attrGroupId;
            dto.intArg2 = randMemberGuid;
            dto.intListArg1 = attrs.Select(attr => AttrHelper.ToDtoAttrValue(
                attr.entityAttrType, attr.initValue)).ToList();
            return dto;
        }

        // public override List<int> GetIntValueList()
        // {
        //     return new List<int>()
        //         { attrGroupId };
        // }
    }
}