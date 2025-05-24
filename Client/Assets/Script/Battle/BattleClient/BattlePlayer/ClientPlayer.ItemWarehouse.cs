using System.Collections.Generic;
using Battle;

namespace Battle_Client
{
    //玩家道具仓库
    public partial class ClientPlayer
    {
        public ItemWarehouse itemWarehouse;

        public void InitItemWarehouse()
        {
            itemWarehouse = new ItemWarehouse();
            itemWarehouse.Init();
        }

        public void SyncWarehouseItem(BattleItemData_Client itemData, int index)
        {
            this.itemWarehouse.SetWarehouseItemData(itemData,index);
        }
    }
}