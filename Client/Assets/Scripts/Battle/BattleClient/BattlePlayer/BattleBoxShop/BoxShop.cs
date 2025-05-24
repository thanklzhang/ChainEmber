using System.Collections.Generic;
using Battle;

namespace Battle_Client
{
    public class BoxShop
    {
        public Dictionary<RewardQuality, BoxShopItem> boxShopItemDic
            = new Dictionary<RewardQuality, BoxShopItem>();

        public void Init()
        {
            boxShopItemDic = new Dictionary<RewardQuality, BoxShopItem>();
            boxShopItemDic.Add(RewardQuality.Green, new BoxShopItem());
            boxShopItemDic.Add(RewardQuality.Blue, new BoxShopItem());
            boxShopItemDic.Add(RewardQuality.Purple, new BoxShopItem());
            boxShopItemDic.Add(RewardQuality.Orange, new BoxShopItem());
            boxShopItemDic.Add(RewardQuality.Red, new BoxShopItem());
        }

        public void SetBoxShopItems(Dictionary<RewardQuality, BoxShopItem> shopItemDic)
        {
            this.boxShopItemDic = shopItemDic;
        }

        public void Buy(RewardQuality quality,int buyCount)
        {
            //send msg
            BattleManager.Instance.MsgSender.Send_BuyBoxFromShop(quality,buyCount);
        }
    }
}