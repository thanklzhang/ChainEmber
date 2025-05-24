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
    //给所有队友加随机属性(包括自身)
    public class RewardEffect_TeamMember_AllTeammateAddRandAttr : BaseBattleRewardEffectOption
    {
        //最终结果奖励
        public int attrGroupId;
        // private int skillConfigId;
        public List<AttrOption> attrs;
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
            var memberList = this.player.entity.GetTeamMemberDic().Values.ToList();
            memberList.Add(this.player.entity);
            
            if (memberList != null)
            {
                foreach (var member in memberList)
                {
                    member.AddAttrs(EntityAttrGroupType.BattleReward, attrs);
                }
            }
        }

        // public override List<int> GetIntValueList()
        // {
        //     return new List<int>()
        //         { attrGroupId };
        // }


        public override BattleRewardEffectDTO GetValues()
        {
            var dto = base.GetValues();
            dto.intArg1 = attrGroupId;
            dto.intListArg1 = attrs.Select(attr => AttrHelper.ToDtoAttrValue(
                attr.entityAttrType, attr.initValue)).ToList();
            return dto;
            
        }

        public override void ApplyToNewTeamMembers(BattleEntity newTeamMember)
        {
            newTeamMember.AddAttrs(EntityAttrGroupType.BattleReward, attrs);
        }

        // public override void OnDiscard()
        // {
        //     //TODO: 需要得到相关的 entity ， 然后移除属性
        // }
    }
}