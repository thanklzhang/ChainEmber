// using System.Collections.Generic;
// using System.Linq;
//
// namespace Battle
// {
//     public class PlayerWarehouse 
//     {
//         private BattlePlayer player;
//         // private List<PlayerItemWarehouseCell> itemCellList;
//         
//         
//         public ListTypeItemBar itemBar;
//         
//         
//         public void Init(BattlePlayer player)
//         {
//             this.player = player;
//             itemBar.Init(BattleDefine.maxWarehouseItemBarCellCount);
//         }
//
//         public void GainItem(BattleItem item)
//         {
//             var index = itemBar.GainItem(item);
//             if (index >= 0)
//             {
//                 SyncWarehouseItem(item,index);
//             }
//             else
//             {
//                 Battle_Log.LogWarning("GainItem fail");
//             }
//         }
//
//         public void GainItems(List<BattleItem> items)
//         {
//             for (int i = 0; i < items.Count; i++)
//             {
//                 var item = items[i];
//                 GainItem(item);
//             }
//             //TODO
//             // SyncWarehouseItems();
//         }
//
//         public void RemoveItem(BattleItem item)
//         {
//         }
//
//         //同步一个库中的道具
//         public void SyncWarehouseItem(BattleItem item,int index)
//         {
//             player.battle.PlayerMsgSender.SyncPlayerWarehouseItem(this.player.playerIndex, item, index);
//         }
//
//         // public BattleItem MoveItem(int srcIndex)
//         // {
//         //     return itemBar.MoveItem(srcIndex);
//         // }
//         //
//         //
//         // public BattleItem PutItem(int recvIndex,BattleItem item)
//         // {
//         //     return itemBar.PutItem(recvIndex,item);
//         // }
//
//     }
//
// }