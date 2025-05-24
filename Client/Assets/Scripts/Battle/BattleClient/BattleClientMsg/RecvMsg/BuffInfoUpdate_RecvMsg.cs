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
    public class BuffInfoUpdate_RecvMsg : ClientRecvMsg
    {
        public override void Handle()
        {
            var arg = this.msgArg as BuffInfoUpdate_RecvMsg_Arg;

            var effect = BattleSkillEffectManager_Client.Instance.FindSkillEffect(arg.buffInfo.guid);

            if (effect != null)
            {
                var buffInfo_client = BuffEffectInfo_Client.ToBuffClient(arg.buffInfo);
                effect.SetBuffInfo(buffInfo_client);
            }
        }
    }

    public class BuffInfoUpdate_RecvMsg_Arg : BaseClientRecvMsgArg
    {
        public BuffEffectInfo buffInfo;
    }
}