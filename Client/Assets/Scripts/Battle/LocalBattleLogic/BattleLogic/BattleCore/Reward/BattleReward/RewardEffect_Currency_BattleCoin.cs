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
    //获得战银
    public class RewardEffect_Currency_BattleCoin : BaseBattleRewardEffectOption
    {
        //最终结果奖励
        public int battleCoinCount;

        public override void CalculateRealityReward()
        {
            if (rewardOptionConfig.ValueList.Count > 1)
            {
                //随机给战银
                var a = rewardOptionConfig.ValueList[0];
                var b = rewardOptionConfig.ValueList[1];
                battleCoinCount = BattleRandom.GetRandInt(a, b, this.battle);
            }
            else
            {
                //直接给固定的战银
                battleCoinCount = rewardOptionConfig.ValueList[0];
            }
        }

        public override void OnGain(BattlePlayer gainer)
        {
            gainer.GainCurrency(BattleCurrency.CoinId, battleCoinCount);
        }

        // public override List<int> GetIntValueList()
        // {
        //     return new List<int>()
        //         { battleCoinCount };
        // }

        public override BattleRewardEffectDTO GetValues()
        {
            var dto = base.GetValues();
            var resultCount = battleCoinCount * 
                player.GetCurrencyRate(BattleCurrency.CoinId) / 1000.0f;
            dto.intArg1 = (int)resultCount;
            return dto;
        }
    }
}