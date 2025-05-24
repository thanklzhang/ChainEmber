using System.Collections.Generic;

namespace Battle
{
    //道具栏
    public class ItemBar // : IItemBarAction
    {
        public List<ItemBarCell> itemBarCellList;

        public void Init(int maxCount)
        {
            itemBarCellList = new List<ItemBarCell>();
            for (int i = 0; i < maxCount; i++)
            {
                var itemCell = new ItemBarCell();
                itemCell.Init(i, false);
                itemBarCellList.Add(itemCell);
            }
        }

        public int GainItem(BattleItem item, int putIndex = -1)
        {
            //TODO 堆叠问题 需要先判断堆叠 之后再说
            ItemBarCell cell = null;
            cell = putIndex < 0
                ? GetCanUseItemBarCell()
                : GetItemBarCell(putIndex);

            // if (putIndex >= 0)
            // {
            //     cell = GetCanUseItemBarCell();
            // }
            // else
            // {
            //     cell = GetItemBarCell(putIndex);
            // }

            if (cell != null)
            {
                cell.PutItem(item);
                return cell.Index;
            }
            else
            {
                BattleLog.LogWarning("not found the cell of itemBar");
                return -1;
            }
        }

        public BattleItem GetItem(int index)
        {
            var cell = GetItemBarCell(index);
            if (cell != null)
            {
                return cell.GetItem();
            }

            return null;
        }

        public BattleItem MoveItem(int srcIndex)
        {
            var cell = GetItemBarCell(srcIndex);
            if (cell != null)
            {
                var item = cell.GetItem();
                cell.RemoveItem();
                return item;
            }

            return null;
        }

        // public BattleItem PutItem(int desIndex, BattleItem item)
        // {
        //     var cell = GetItemBarCell(desIndex);
        //     if (cell != null)
        //     {
        //         var preItem = cell.GetItem();
        //         cell.PutItem(item);
        //         return preItem;
        //     }
        //
        //     return null;
        // }

        public List<ItemBarCell> UnlockCell(int unlockCount = 1)
        {
            int currUnlockCount = 0;
            List<ItemBarCell> cellList = new List<ItemBarCell>();
            for (int i = 0; i < itemBarCellList.Count; i++)
            {
                var cell = itemBarCellList[i];
                if (!cell.IsUnlock)
                {
                    cell.Unlock();
                    cellList.Add(cell);
                    currUnlockCount += 1;
                    if (currUnlockCount >= unlockCount)
                    {
                        break;
                    }
                }
            }

            return cellList;
        }

        public ItemBarCell GetItemBarCell(int index)
        {
            if (index < itemBarCellList.Count)
            {
                return itemBarCellList[index];
            }

            return null;
        }

        public BattleItem RemoveItem(int index)
        {
            var cell = GetItemBarCell(index);
            if (cell != null)
            {
                return cell.RemoveItem();
            }
            else
            {
                BattleLog.LogWarning("not found the index of cell , index : " + index);
            }

            return null;
        }

        public ItemBarCell GetCanUseItemBarCell()
        {
            for (int i = 0; i < itemBarCellList.Count; i++)
            {
                var cell = itemBarCellList[i];
                if (cell.IsUnlock && !cell.IsHaveItem())
                {
                    return cell;
                }
            }

            return null;
        }

        public ItemBarCell GetItemBarCellByItem(BattleItem item)
        {
            for (int i = 0; i < itemBarCellList.Count; i++)
            {
                var cell = itemBarCellList[i];
                if (cell.IsContainItem(item))
                {
                    return cell;
                }
            }

            return null;
        }

        public List<int> GetAllItemIdList()
        {
            List<int> itemIdList = new List<int>();
            for (int i = 0; i < itemBarCellList.Count; i++)
            {
                var cell = itemBarCellList[i];
                if (cell.IsHaveItem())
                {
                    var item = cell.GetItem();
                    if (item != null)
                    {
                        itemIdList.Add(item.configId);
                    }
                }
            }

            return itemIdList;
        }
    }
}