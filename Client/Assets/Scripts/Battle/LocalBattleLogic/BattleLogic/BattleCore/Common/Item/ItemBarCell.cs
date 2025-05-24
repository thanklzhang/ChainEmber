using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace Battle
{
    public class ItemBarCell
    {
        private int index;
        public int Index => index;

        private bool isUnlock;
        public bool IsUnlock => isUnlock;

        private BattleItem item;

        public void Init(int index, bool isUnlock)
        {
            this.index = index;
            this.isUnlock = isUnlock;
        }

        public bool IsHaveItem()
        {
            return this.item != null;
        }

        public void PutItem(BattleItem item)
        {
            if (this.item != null)
            {
                BattleLog.LogWarning("PutItem : the item is not null");
            }

            this.item = item;
        }

        public bool IsContainItem(BattleItem item)
        {
            if (null == item || null == this.item)
            {
                return false;
            }

            return this.item == item;
        }

        public BattleItem RemoveItem()
        {
            var item = this.item;
            this.item = null;
            return item;
        }

        public BattleItem GetItem()
        {
            return this.item;
        }

        public void Swap(ItemBarCell desCell)
        {
            // (desCell.isUnlock, this.isUnlock) = (this.isUnlock, desCell.isUnlock);
            (desCell.item, this.item) = (this.item, desCell.item);
        }

        public void Unlock()
        {
            this.isUnlock = true;
        }

        public virtual void SyncItem()
        {
            
        }
    }

  
}