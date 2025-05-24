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
    //获得队友
    public class RewardEffect_TeamMember_Gain : BaseBattleRewardEffectOption
    {
        //最终结果奖励
        public int teamMemberConfigId;

        // private int skillConfigId;
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
            // teamMemberConfigId = filterIdList[index];

            teamMemberConfigId = GetRandValueByWeights();
        }

        public override void OnGain(BattlePlayer gainer)
        {
            var ret = this.player.TryToAddTeamMember(this.teamMemberConfigId);
            if (ret.type == ResultCodeType.AddTeamMemberFull)
            {
                this.battle.OnSyncReplaceTeamMemberResult(gainer.playerIndex,ret);
            }
        }


        // public override List<int> GetIntValueList()
        // {
        //     return new List<int>()
        //         { teamMemberConfigId };
        // }

        public override BattleRewardEffectDTO GetValues()
        {
            var dto = base.GetValues();
            dto.intArg1 = this.teamMemberConfigId;
            return dto;
        }
    }
}