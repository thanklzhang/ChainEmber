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
    public class BattleWavePass_RecvMsg : ClientRecvMsg
    {
        public override void Handle()
        {
            var arg = this.msgArg as BattleWavePass_RecvMsg_Arg;

            var player = BattleManager.Instance.GetLocalPlayer();
            if (player.team == arg.passTeam)
            {
                //客户端显示结算界面
                BattleManager.Instance.OnBattleWavePass(arg);
            }
        }
    }

    public class BattleWavePass_RecvMsg_Arg : BaseClientRecvMsgArg
    {
        public int passTeam;
        public Dictionary<int, WavePassCurrency_RecvMsg> currencyDic;
        public Dictionary<RewardQuality, List<WavePassBox_RecvMsg>> boxDic;
        
    }

    public class WavePassCurrency_RecvMsg
    {
        public int itemConfigId;
        public int count;
    }
    public class WavePassBox_RecvMsg
    {
        public int boxConfigId;
    }
}