using System.Collections.Generic;
using Battle;

namespace Battle_Client
{
    public partial class ClientPlayer
    {
        private BoxShop boxShop;

        public void InitBoxShop()
        {
            boxShop = new BoxShop();
            boxShop.Init();
        }

        public void SetBoxShopItemsData(Dictionary<RewardQuality, BoxShopItem> shopItemDic)
        {
            boxShop.SetBoxShopItems(shopItemDic);
            
            EventDispatcher.Broadcast(EventIDs.OnUpdateShopBoxInfo);

        }

        //购买宝箱
        public void BuyBoxFromShop(RewardQuality quality,int buyCount)
        {
            boxShop.Buy(quality,buyCount);
        }

        //获取所有商店宝箱信息
        public Dictionary<RewardQuality, BoxShopItem> GetAllBoxShopItems()
        {
            return this.boxShop.boxShopItemDic;
        }
        
        
    }
}
