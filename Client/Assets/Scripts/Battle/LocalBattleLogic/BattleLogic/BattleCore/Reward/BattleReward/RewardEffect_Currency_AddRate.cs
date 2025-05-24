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
    //增加获取战银的比率（千分比）
    public class RewardEffect_Currency_AddCoinGainRate : BaseBattleRewardEffectOption
    {
        //最终结果奖励
        public int rate;
        
        public override void CalculateRealityReward()
        {
            if (rewardOptionConfig.ValueList.Count > 1)
            {
                //随机给战银
                var a = rewardOptionConfig.ValueList[0];
                var b = rewardOptionConfig.ValueList[1];
                rate =  BattleRandom.GetRandInt(a,b,this.battle);
            }
            else
            {
                //直接给固定的战银
                rate = rewardOptionConfig.ValueList[0];
            }
        }

        public override void OnGain(BattlePlayer gainer)
        {
            gainer.AddCurrencyAddRate(BattleCurrency.CoinId,rate);
            // gainer.GainCurrency(BattleCurrency.CoinId,battleCoinCount);
        }

        // public override List<int> GetIntValueList()
        // {
        //     return new List<int>()
        //         { battleCoinCount };
        // }

        public override BattleRewardEffectDTO GetValues()
        {
            var dto = base.GetValues();
            dto.intArg1 = rate;
            return dto;
        }
    }
}