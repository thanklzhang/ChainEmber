using System;
using System.Collections.Generic;
using System.Linq;


namespace Battle
{
    public class BattleBoxShopItem
    {
        //可购买的最大上限
        public int maxBoxCount;

        //当前购买宝箱列表
        public List<BattleBox> boxList = new List<BattleBox>();

        //已经买的宝箱列表
        public List<BattleBox> hasBuyBoxList = new List<BattleBox>();

        //这里所有宝箱都是一个价格(目前都是消耗银子)
        public int costCount;

        public int costItemId = BattleCurrency.CoinId;

        public int configId;
        public IBattleBoxShopItem config;

        public void Refresh(int configId)
        {
            this.configId = configId;
            config = BattleConfigManager.Instance.GetById<IBattleBoxShopItem>(this.configId);
            this.costCount = config.CostCount;
            this.costItemId = config.CostItemId;
        }

        public int GetCanBuyCount()
        {
            return (maxBoxCount - hasBuyBoxList.Count);
        }

        public int GetHasBuyCount()
        {
            return hasBuyBoxList.Count;
        }

        public int GetMaxBuyCount()
        {
            return maxBoxCount;
        }

        public void AddBox(int boxConfigId, BattlePlayer player)
        {
            BattleBox box = new BattleBox();
            box.Init(boxConfigId, player);

            boxList.Add(box);

            maxBoxCount += 1;
        }

        public void FinishBuy(List<BattleBox> list)
        {
            hasBuyBoxList.AddRange(list);
        }
    }
}