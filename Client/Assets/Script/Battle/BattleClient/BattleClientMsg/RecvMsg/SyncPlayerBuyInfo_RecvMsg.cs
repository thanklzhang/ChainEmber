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
    //同步实体的道具栏
    public class SyncPlayerBuyInfo_RecvMsg : ClientRecvMsg
    {
        public override void Handle()
        {
            var arg = this.msgArg as SyncPlayerBuyInfo_RecvMsg_Arg;
            var player = Battle_Client.BattleManager.Instance.GetLocalPlayer();
            if (player != null)
            {
               

                // var localPlayerEntityGuid = BattleManager.Instance.GetLocalCtrlHeroGuid();
                // if (localPlayerEntityGuid == arg.entityGuid)
                // {
                //     UIManager.Instance.Close<BattleReviveUI>();
                // }
                
                player.UpdatePlayerBuyInfo(BattleConvert.ConvertTo(arg.buyInfo));
            }
        }
    }

    public class SyncPlayerBuyInfo_RecvMsg_Arg : BaseClientRecvMsgArg
    {
        public PlayerBuyInfo buyInfo;
    }
}