using System.Collections.Generic;
using System.Linq;

namespace Battle
{
    public interface IOpItemBar
    {
        void GainItemToItemBar(BattleItem item,int putIndex = -1);
        void RemoveItemFromItemBar(int index);
        void MoveItemInternal(int srcIndex,int desIndex);
        BattleItem GetItemFromItemBar(int index);
        
        ItemBarCell GetCellFromItemBar(int index);
    }

    //玩家道具部分 包括仓库和对道具的操作等
    public partial class BattlePlayer : IOpItemBar
    {
        // public PlayerWarehouse playerWarehouse;

        public ItemBar warehouseItemBar;

        public void InitItemWarehouseList()
        {
            var config = BattleCommonConfigHelper.GetConfig();

            // playerWarehouse = new PlayerWarehouse();
            // playerWarehouse.Init(this);
            warehouseItemBar = new ItemBar();
            warehouseItemBar.Init(config.MaxPlayerWarhouseCellCount);
            
            //初始解锁
            int initCount = config.InitPlayerWarhouseCellUnlockCount;
            warehouseItemBar.UnlockCell(initCount);
            
        }

        public void GainItem(BattleItem item, int putIndex = -1)
        {
            var index = warehouseItemBar.GainItem(item, putIndex);
            if (index >= 0)
            {
                item.OnGain(null, false);

                SyncWarehouseItem(index,item);
            }
            else
            {
                BattleLog.LogWarningZxy($"gain item failed , itemConfigId : {item.tableConfig.Id} , putIndex : {putIndex}");
            }
        }

        public void SyncWarehouseItem(int index,BattleItem item)
        {
            battle.PlayerMsgSender.SyncPlayerWarehouseItem(this.playerIndex, index,item );
        }

        public BattleItem GetItemFromItemBar(int index)
        {
            return this.warehouseItemBar.GetItem(index);
        }

        public ItemBarCell GetCellFromItemBar(int index)
        {
            return this.warehouseItemBar.GetItemBarCell(index);
        }

        public void GainItemToItemBar(BattleItem item, int putIndex = -1)
        {
            GainItem(item , putIndex);
        }

        public void RemoveItemFromItemBar(int index)
        {
            warehouseItemBar.RemoveItem(index);
            SyncWarehouseItem(index,null);
        }

        public void MoveItemInternal(int srcIndex, int desIndex)
        {
            var srcCell = this.warehouseItemBar.GetItemBarCell(srcIndex);
            var desCell = this.warehouseItemBar.GetItemBarCell(desIndex);
            srcCell.Swap(desCell);
            
            SyncWarehouseItem(srcIndex,srcCell.GetItem());
            SyncWarehouseItem(desIndex,desCell.GetItem());
        }

        

        public void MoveItemTo(MoveItemOpLocation srcLocation, MoveItemOpLocation desLocation)
        {
            IOpItemBar src = GetOpItemBarByType(srcLocation);
            IOpItemBar des = GetOpItemBarByType(desLocation);
            
            if (null == src || null == des)
            {
                BattleLog.LogZxy("error : null == src || null == des");
                return;
            }
            
            var srcCell = src.GetCellFromItemBar(srcLocation.index);
            var desCell = des.GetCellFromItemBar(desLocation.index);

           

            if (!srcCell.IsUnlock || !desCell.IsUnlock)
            {
                return;
            }

            bool isInternal = false;
            if (srcLocation.type == desLocation.type)
            {
                if (srcLocation.type == ItemLocationType.Warehouse)
                {
                    isInternal = true;
                }
                else if (srcLocation.type == ItemLocationType.EntityItemBar)
                {
                    if (srcLocation.entityGuid == desLocation.entityGuid)
                    {
                        isInternal = true;
                    }
                }
            }

            if (isInternal)
            {
                src.MoveItemInternal(srcLocation.index, desLocation.index);
            }
            else
            {
                var srcItem = src.GetItemFromItemBar(srcLocation.index);
                src.RemoveItemFromItemBar(srcLocation.index);
                
                var desItem = des.GetItemFromItemBar(desLocation.index);

                if (desItem != null)
                {
                    des.RemoveItemFromItemBar(desLocation.index);
                    src.GainItemToItemBar(desItem,srcLocation.index);
                }

                des.GainItemToItemBar(srcItem,desLocation.index);
            }
        }
        
        public IOpItemBar GetOpItemBarByType(MoveItemOpLocation itemLocation)
        {
            var type = itemLocation.type;
            if (type == ItemLocationType.Warehouse)
            {
                return this;
            }
            else if (type == ItemLocationType.EntityItemBar)
            {
                var entities = this.GetAllMemberAndSelf();
                var entity = entities.Find(en => en.guid == itemLocation.entityGuid);


                if (entity != null)
                {
                    return entity;
                }
            }

            return null;
        }
        
    }
}