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
    public class CreateSkillEffect_RecvMsg : ClientRecvMsg
    {
        public override void Handle()
        {
            var arg = this.msgArg as CreateSkillEffect_RecvMsg_Arg;
            
            BattleSkillEffectManager_Client.Instance.CreateSkillEffect(arg.createEffectInfo);

        }
    }

    public class CreateSkillEffect_RecvMsg_Arg : BaseClientRecvMsgArg
    {
        public CreateEffectInfo createEffectInfo;
    }
}