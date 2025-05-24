using System.Collections.Generic;
using Battle;
using Config;

namespace Battle_Client
{
    public class WarehouseItemCellData_Client : ItemBarCellData_Client
    {
        
    }
    

    public class ItemWarehouse
    {
        public List<WarehouseItemCellData_Client> itemCellList;

        //public int maxCellCount = 100;

        public void Init()
        {
            itemCellList = new List<WarehouseItemCellData_Client>();
            
            var config = ConfigManager.Instance.GetById<Config.BattleCommonParam>(1);
            var maxCellCount = config.MaxPlayerWarhouseCellCount;
            var initCount = config.InitPlayerWarhouseCellUnlockCount;
            
            for (int i = 0; i < maxCellCount; i++)
            {
                var cell = new WarehouseItemCellData_Client();
                cell.index = i;
                cell.isUnlock = i < initCount;
                itemCellList.Add(cell);
            }
        }

        public WarehouseItemCellData_Client GetCell(int index)
        {
            return itemCellList[index];
        }

        // public void SetItemWarehouseData(List<WarehouseItemCellData_Client> list)
        // {
        //     foreach (var cell in list)
        //     {
        //         var currCell = GetCell(cell.index);
        //         //update
        //         currCell.itemData = cell.itemData;
        //     }
        //
        //     //EventDispatcher.Broadcast(EventIDs.OnUpdateBattleCurrencyInfo);
        // }

        public void SetWarehouseItemData(BattleItemData_Client itemData,int index)
        {
            var cell = GetCell(index);
            cell.itemData = itemData;
            
            EventDispatcher.Broadcast<WarehouseItemCellData_Client>(EventIDs.OnUpdateWarehouseItemData,
                cell);
            
        }
    }
}