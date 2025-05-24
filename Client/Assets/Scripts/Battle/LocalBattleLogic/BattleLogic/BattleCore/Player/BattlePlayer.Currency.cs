using System.Collections.Generic;
using System.Linq;

namespace Battle
{
    //货币
    public class BattleCurrency

    {
        public static int CoinId = 500001;
        public static int PopulationId = 500101;
        public static int reviveCoinId = 500005;

        public int itemConfigId;
        public int count;

        //获取的速率（千分比）
        public int addRate = 1000;
    }


    //该玩家的货币资源（玩家公用的货币 ）
    public partial class BattlePlayer
    {
        public int CoinCount
        {
            get
            {
                var cu = GetCurrency(BattleCurrency.CoinId);
                if (cu != null)
                {
                    return cu.count;
                }

                return 0;
            }
            set { SetCurrencyCount(BattleCurrency.CoinId, value); }
        }

        public void SetCurrencyCount(int itemConfigId, int count)
        {
            var cu = GetCurrency(itemConfigId);
            if (cu != null)
            {
                cu.count = count;
            }

            SyncCurrency();
        }

        public void AddCurrencyAddRate(int itemConfigId, int addRate)
        {
            var cu = GetCurrency(itemConfigId);
            if (cu != null)
            {
                cu.addRate += addRate;
            }
        }

        public int GetCurrencyRate(int itemConfigId)
        {
            var cu = GetCurrency(itemConfigId);
            if (cu == null)
            {
                return 0;
            }

            return cu.addRate;
        }

        public void GainCurrency(int itemConfigId, int count, bool isSync = true)
        {
            var cu = GetCurrency(itemConfigId);
            if (cu != null)
            {
                cu.count += (int)(count * cu.addRate / 1000.0f);
            }


            if (isSync)
            {
                SyncCurrency();
            }
        }

        public void GainCurrency(List<int> itemConfigIds, List<int> counts)
        {
            for (int i = 0; i < itemConfigIds.Count; i++)
            {
                var configId = itemConfigIds[i];
                var count = 0;
                if (i < counts.Count)
                {
                    count = counts[i];
                }

                GainCurrency(configId, count, false);
            }

            SyncCurrency();
        }

        public void CostCurrency(int itemConfigId, int costCount)
        {
            var cu = GetCurrency(itemConfigId);
            if (cu != null)
            {
                cu.count -= costCount;
            }

            SyncCurrency();
        }

        public void SyncCurrency()
        {
            //currencyDic sync

            this.battle.OnNotifyUpdateCurrency(this.playerIndex, currencyDic);
        }

        public BattleCurrency GetCurrency(int itemConfigId)
        {
            BattleCurrency currency = null;

            if (currencyDic.ContainsKey(itemConfigId))
            {
                currency = currencyDic[itemConfigId];
            }
            else
            {
                currency = new BattleCurrency()
                {
                    itemConfigId = itemConfigId,
                    count = 0,
                };

                currencyDic.Add(itemConfigId, currency);
            }

            return currency;
        }

        public void InitCurrency()
        {
            var paramConfig = BattleConfigManager.Instance.GetById<IBattleCommonParam>(1);
            //初始资源
            GainCurrency(BattleCurrency.CoinId, paramConfig.InitCoin, false);
            GainCurrency(BattleCurrency.PopulationId, paramConfig.InitPopulation, false);
            GainCurrency(BattleCurrency.reviveCoinId, paramConfig.InitReviveCoin, false);

            // //初始货币
            // currencyDic.Add(BattleCurrency.CoinId, new BattleCurrency()
            // {
            //     itemConfigId = BattleCurrency.CoinId,
            //     count = 1000000,
            // });
            //
            // //初始人口
            // currencyDic.Add(BattleCurrency.PopulationId, new BattleCurrency()
            // {
            //     itemConfigId = BattleCurrency.PopulationId,
            //     count = 2,
            // });
            //
            // //初始复活币
            // currencyDic.Add(BattleCurrency.reviveCoinId, new BattleCurrency()
            // {
            //     itemConfigId = BattleCurrency.reviveCoinId,
            //     count = 1,
            // });
        }
    }
}