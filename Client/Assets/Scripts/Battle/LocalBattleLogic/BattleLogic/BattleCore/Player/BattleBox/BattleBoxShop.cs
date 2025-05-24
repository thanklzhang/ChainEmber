using System;
using System.Collections.Generic;
using System.Linq;


namespace Battle
{
    //宝箱商店
    public class BattleBoxShop
    {
        private IBattleBoxShop config;

        private Dictionary<RewardQuality, BattleBoxShopItem> shopItemDic;

        private BattlePlayer player;

        public void Init(int shopConfigId, BattlePlayer player)
        {
            config = BattleConfigManager.Instance.GetById<IBattleBoxShop>(shopConfigId);
            this.player = player;
            shopItemDic = new Dictionary<RewardQuality, BattleBoxShopItem>();

            shopItemDic.Add(RewardQuality.Green, new BattleBoxShopItem());
            // shopItemDic.Add(RewardQuality.Blue, new BattleBoxShopItem());
            // shopItemDic.Add(RewardQuality.Purple, new BattleBoxShopItem());
            // shopItemDic.Add(RewardQuality.Orange, new BattleBoxShopItem());
            // shopItemDic.Add(RewardQuality.Red, new BattleBoxShopItem());

            RefreshAllShopItem();
        }

        public Dictionary<RewardQuality, BattleBoxShopItem> GetShopItems()
        {
            return shopItemDic;
        }

        //商店道具全部刷新
        public void RefreshAllShopItem()
        {
            var itemList = config.BoxShopItemList;

            for (int i = 0; i < itemList.Count; i++)
            {
                var shopItemId = itemList[i];
                var itemConfig = BattleConfigManager.Instance.GetById<IBattleBoxShopItem>(shopItemId);
                var quality = itemConfig.Quality;

                var shopItem = shopItemDic[(RewardQuality)quality];
                shopItem.Refresh(shopItemId);
                //最少产出宝箱数 也就是保底个数
                for (int j = 0; j < itemConfig.MinCount; j++)
                {
                    AddBoxToShopItem(itemConfig, shopItem);
                }

                for (int j = itemConfig.MinCount + 1; j <= itemConfig.MaxCount; j++)
                {
                    var randIndex = BattleRandom.Next(0, 1000);
                    if (randIndex < itemConfig.Chance)
                    {
                        AddBoxToShopItem(itemConfig, shopItem);
                    }
                }
            }
        }

        void AddBoxToShopItem(IBattleBoxShopItem itemConfig, BattleBoxShopItem shopItem)
        {
            var index = BattleRandom.GetNextIndexByWeights(itemConfig.BoxWeightList);
            var randBoxConfigId = itemConfig.BoxIdList[index];

            shopItem.AddBox(randBoxConfigId, this.player);
        }

        public void Buy(RewardQuality quality, int count)
        {
            // Battle_Log.LogZxy("BattleBoxShop : Buy : quality : "
            //                   + quality + " , count : " + count);
            var shopItem = shopItemDic[quality];
            var list = shopItem.boxList;
            if (count <= list.Count)
            {
                var totalPrice = shopItem.costCount * count;
                var playerCoin = player.CoinCount;

                if (playerCoin >= totalPrice)
                {
                    // var entity = player.entity;
                    var canBuyBoxList = list.Take(count).ToList();

                    // entity.GainBoxes(canBuyBoxList);
                    shopItem.FinishBuy(canBuyBoxList);
                    player.GainBoxes(canBuyBoxList);
                    list.RemoveRange(0, canBuyBoxList.Count);
                    player.CoinCount -= totalPrice;
                    BattleLog.LogZxy("BattleBoxShop : Buy : will send buy msg : "
                                      + quality + " , count : " + count);
                    
                    player.SyncBoxShop();
                    
                }
                else
                {
                    BattleLog.LogZxy("BattleBoxShop : Buy : fail : "
                                      + quality + " , totalPrice : " + totalPrice +
                                      " , playerCoin : " + playerCoin);
                }
            }
            else
            {
                BattleLog.LogZxy("BattleBoxShop : Buy : the count of shopItem is not enough" +
                                  " the count of shopItemList : " + list.Count);
            }
        }

    }
}