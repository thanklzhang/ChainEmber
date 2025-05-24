using System.Collections.Generic;
using Battle;

namespace Battle_Client
{
    public partial class ClientPlayer
    {
        public Currency currency;

        public void InitCurrency()
        {
            currency = new Currency();
            currency.Init();
        }

        public void SetCurrencyData(Dictionary<int, BattleClient_CurrencyItem> serverData)
        {
            currency.SetCurrencyData(ConvertCurrency(serverData));
        }


        public int GetCoinCount()
        {
            return currency.GetCoinCount();
        }

        public int GetReviveCount()
        {
            return currency.GetCurrencyCount(CurrencyItem.ReviveCoinId);
        }

        public static Dictionary<int, CurrencyItem> ConvertCurrency(
            Dictionary<int, BattleClient_CurrencyItem> serverData)
        {
            Dictionary<int, CurrencyItem> localItemDic = new Dictionary<int, CurrencyItem>();
            if (serverData != null)
            {
                foreach (var kv in serverData)
                {
                    var serverItemId = kv.Key;
                    var serverCurrencyItem = kv.Value;

                    var localItem = new CurrencyItem();
                    localItem.itemId = serverItemId;
                    localItem.count = serverCurrencyItem.count;

                    localItemDic.Add(localItem.itemId, localItem);
                }
            }

            return localItemDic;
        }
    }
}