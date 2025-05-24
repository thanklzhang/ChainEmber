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
    public class CurrencyUpdate_RecvMsg : ClientRecvMsg
    {
        public override void Handle()
        {
            var arg = this.msgArg as CurrencyUpdate_RecvMsg_Arg;

            var player = BattleManager.Instance.GetLocalPlayer();
            if (player.playerIndex == arg.playerIndex)
            {
                var dic = BattleConvert.ConvertTo(arg.currencyItemDic);
                player.SetCurrencyData(dic);
            }
        }
    }

    public class CurrencyUpdate_RecvMsg_Arg : BaseClientRecvMsgArg
    {
        public int playerIndex;
        public Dictionary<int, BattleCurrency> currencyItemDic;
    }
}