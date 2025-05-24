using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Battle;
using Battle_Client;
using GameData;
using NetProto;
using UnityEngine;
using UnityEngine.UI;

namespace Battle_Client
{
    //同步玩家仓库的道具
    public class SyncWarehouseItem_RecvMsg : ClientRecvMsg
    {
        public override void Handle()
        {
            var arg = this.msgArg as SyncWarehouseItem_RecvMsg_Arg;

            var player = BattleManager.Instance.GetLocalPlayer();
            if (player != null && arg.playerIndex == player.playerIndex)
            {
                player.SyncWarehouseItem(arg.itemData, arg.index);
            }
        }
    }

    public class SyncWarehouseItem_RecvMsg_Arg : BaseClientRecvMsgArg
    {
        public int playerIndex;
        public BattleItemData_Client itemData;
        public int index;
    }
}