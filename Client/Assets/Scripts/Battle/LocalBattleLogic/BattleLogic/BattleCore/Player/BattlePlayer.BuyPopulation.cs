using System.Collections.Generic;
using System.Linq;

namespace Battle
{
    public class PlayerBuyInfo
    {
        public int hasBuyPopulationCount = 0;
    }
    //购买人口
    public partial class BattlePlayer
    {
        public PlayerBuyInfo buyInfo;

        public void InitBuyInfo()
        {
            buyInfo = new PlayerBuyInfo();
        }

        public int hasBuyPopulationCount =>  buyInfo.hasBuyPopulationCount; 
        public void BuyPopulation()
        {
            var config = BattleConfigManager.Instance.GetById<IBattleCommonParam>(1);
            int index = hasBuyPopulationCount;
            if (index >= config.BuyPopulationCostCoin.Count)
            {
                index = config.BuyPopulationCostCoin.Count - 1;
            }

            var coin = config.BuyPopulationCostCoin[index];
            
            if (this.CoinCount < coin)
            {
                BattleLog.LogError($"玩家金币不足，无法购买人口");
                return;
            }
            
            this.CoinCount -= coin;
            this.AddPopulation(1);

            buyInfo.hasBuyPopulationCount += 1;
            battle.PlayerMsgSender.Notify_SyncPlayerBuyInfo(buyInfo); 
        }

    }
}