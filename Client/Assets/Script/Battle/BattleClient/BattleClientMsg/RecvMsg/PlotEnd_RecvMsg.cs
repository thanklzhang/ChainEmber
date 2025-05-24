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
    public class PlotEnd_RecvMsg : ClientRecvMsg
    {
        public override void Handle()
        {
            var arg = this.msgArg as PlotEnd_RecvMsg_Arg;
            var name = arg.plotName;

            BattleEntityManager.Instance.SetAllEntityShowState(true);
            CameraManager.Instance.GetCameraUI().SetUICameraShowState(true);

            PlotManager.Instance.ClosePlot();
        }
    }

    public class PlotEnd_RecvMsg_Arg : BaseClientRecvMsgArg
    {
        public string plotName;
    }
}