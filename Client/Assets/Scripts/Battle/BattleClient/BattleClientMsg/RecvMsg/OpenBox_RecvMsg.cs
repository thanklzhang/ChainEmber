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
    public class OpenBox_RecvMsg : ClientRecvMsg
    {
        public override void Handle()
        {
            var arg = this.msgArg as OpenBox_RecvMsg_Arg;

            var box = arg.box;

            // var entity = BattleEntityManager.Instance.FindEntity(box.openEntityGuid);
            // if (entity != null)
            // {
            //     var hero = BattleManager.Instance.GetLocalCtrlHero();
            //     if (hero != null)
            //     {
            //         if (hero.guid == entity.guid)
            //         {
            //             hero.OnOpenBox(box);
            //         }
            //     }
            // }


            var localPlayer = BattleManager.Instance.GetLocalPlayer();
            if (arg.box.playerIndex == localPlayer.playerIndex)
            {
                localPlayer.OnOpenBox(box);
            }
        }
    }

    public class OpenBox_RecvMsg_Arg : BaseClientRecvMsgArg
    {
        public BattleClientMsg_BattleBox box;
    }
}