using System.Collections.Generic;
using System.Linq;

namespace Battle
{
    //宝箱商店 每个玩家都有一个宝箱商店
    public partial class BattlePlayer
    {
      
        public BattleBoxShop boxShop;

        void InitBoxShop()
        {
            boxShop = new BattleBoxShop();
            var initShopConfigId = 1000;
            boxShop.Init(1000,this);
        }

        public void BuyBox(RewardQuality quality, int count)
        {
            boxShop.Buy(quality,count);
        }


        public void SyncBoxShop()
        {
            //boxShop sync
            var shopItemsDic = boxShop.GetShopItems();
            
            this.battle.OnNotifyUpdateBoxShop(this.playerIndex,shopItemsDic);
            
        }

    }
}