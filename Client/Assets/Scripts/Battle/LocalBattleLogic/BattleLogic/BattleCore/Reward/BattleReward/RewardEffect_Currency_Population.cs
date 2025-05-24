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
    //获得人口
    public class RewardEffect_Currency_Population : BaseBattleRewardEffectOption
    {
        //最终结果奖励
        public int populationCount;
        
        public override void CalculateRealityReward()
        {
            if (rewardOptionConfig.ValueList.Count > 1)
            {
                //随机给
                var a = rewardOptionConfig.ValueList[0];
                var b = rewardOptionConfig.ValueList[1];
                populationCount =  BattleRandom.GetRandInt(a,b,this.battle);
            }
            else
            {
                //直接给固定的
                populationCount = rewardOptionConfig.ValueList[0];
            }
        }

        public override void OnGain(BattlePlayer gainer)
        {
            gainer.AddPopulation(populationCount);
        }

        // public override List<int> GetIntValueList()
        // {
        //     return new List<int>()
        //         { battleCoinCount };
        // }

        public override BattleRewardEffectDTO GetValues()
        {
            var dto = base.GetValues();
            dto.intArg1 = populationCount;
            return dto;
        }
    }
}