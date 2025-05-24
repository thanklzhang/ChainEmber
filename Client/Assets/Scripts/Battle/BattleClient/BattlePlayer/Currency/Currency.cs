using System.Collections.Generic;
using Battle;

namespace Battle_Client
{
    public class Currency
    {
        public Dictionary<int, CurrencyItem> currencyDic =
            new Dictionary<int, CurrencyItem>();

        public void Init()
        {
            
        }

        public void SetCurrencyData(Dictionary<int, CurrencyItem> dic)
        {
            this.currencyDic = dic;
            
            EventDispatcher.Broadcast(EventIDs.OnUpdateBattleCurrencyInfo);

        }

        public int GetCurrencyCount(int itemId)
        {
            if (currencyDic.ContainsKey(itemId))
            {
                return currencyDic[itemId].count;
            }

            return 0;
        }

        public int GetCoinCount()
        {
            return GetCurrencyCount(CurrencyItem.CoinId);
        }
    }
}