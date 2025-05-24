using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace Battle
{
    //道具部分 这里道具不参与玩家操作指令列表中
    public partial class BattleEntity : IOpItemBar
    {
        public ItemBar itemBar;

        public void InitItemBar()
        {
            var config = BattleCommonConfigHelper.GetConfig();
            
            itemBar = new ItemBar();
            itemBar.Init(config.MaxEntityItemBarCellCount);
            
            //初始解锁
            int initCount = config.InitEntityItemBarCellUnlockCount;
            itemBar.UnlockCell(initCount);
        }

        public BattleItem MoveItem(int index)
        {
            return this.itemBar.MoveItem(index);
        }
        
        //获得一个道具
        public void GainItem(BattleItem item,int putIndex = -1)
        {
            var index = this.itemBar.GainItem(item, putIndex);
            if (index >= 0)
            {
                //TODO: 处理叠加
                item.OnGain(this, false);
                
                SyncItemBarItem(index,item);
            }
            else
            {
                BattleLog.LogWarningZxy($"gain item failed , itemConfigId : {item.tableConfig.Id} , putIndex : {putIndex}");
            }
           
        }

        public int GetItemIndex(BattleItem item)
        {
            var cell = itemBar.GetItemBarCellByItem(item);
            if (cell != null)
            {
                return cell.Index;
            }

            return -1;
        }

        public void UseItem(Battle_ItemUseArg arg)
        {
            var index = arg.itemIndex;
            if (index < 0)
            {
                return;
            }

            var cell = itemBar.GetItemBarCell(index);
            if (cell != null)
            {
                var item = cell.GetItem();
                if (item != null)
                {
                    CheckAndUseItem(item, arg.targetGuid, arg.targetPos,arg.mousePos);
                    //判断是否用完了
                    if (item.count <= 0)
                    {
                        var removeItem = cell.RemoveItem();
                        //TODO Sync
                    }
                }
            }
        }

        private void CheckAndUseItem(BattleItem item, int targetGuid, Vector3 targetPos,
            Vector3 mousePos)
        {
            var skill = item.skill;

            bool isCanRelease = Skill.IsCanRelease(skill, targetGuid, targetPos);
            if (isCanRelease)
            {
                BattleLog.Log(string.Format("entity {0} success to use item {1}", this.infoConfig.Name,
                    item.tableConfig.Name));
                item.SuccessToUse(targetGuid, targetPos,mousePos);
                if (item.owner != null)
                {
                    this.battle.OnItemInfoUpdate(item);
                }
                //SuccessToReleaseSkill(skill, targetGuid, targetPos);
            }
        }

        public void UnlockItemBarCell(int unlockCount = 1)
        {
            var list = this.itemBar.UnlockCell(unlockCount);
            if (list != null)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    var cell = list[i];
                    SyncItemBarItem(cell.Index,cell.GetItem(),true);
                }
            }
        }

        public void GainItemToItemBar(BattleItem item, int putIndex = -1)
        {
            this.GainItem(item,putIndex);
        }

        public void RemoveItemFromItemBar(int index)
        {
            var item = this.itemBar.RemoveItem(index);

            if (item != null)
            {
                item.Remove();
            }

            SyncItemBarItem(index,null);
        }

        public void MoveItemInternal(int srcIndex, int desIndex)
        {
            var srcCell = this.itemBar.GetItemBarCell(srcIndex);
            var desCell = this.itemBar.GetItemBarCell(desIndex);
            srcCell.Swap(desCell);

            SyncItemBarItem(srcCell.Index,srcCell.GetItem());
            SyncItemBarItem(desCell.Index,desCell.GetItem());
        }

        public BattleItem GetItemFromItemBar(int index)
        {
            return this.itemBar.GetItem(index);
        }

        public ItemBarCell GetCellFromItemBar(int index)
        {
            return this.itemBar.GetItemBarCell(index);
        }

        public void SyncItemBarItem(int index,BattleItem item,bool isUnlock = true)
        {
            battle.PlayerMsgSender.SyncEntityItemBarItem(this.guid, index,item ,isUnlock);
        }
    }
}