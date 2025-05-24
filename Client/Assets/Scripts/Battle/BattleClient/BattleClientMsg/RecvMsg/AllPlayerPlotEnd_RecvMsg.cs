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
    public class AllPlayerPlotEnd_RecvMsg : ClientRecvMsg
    {
        public override void Handle()
        {
            var arg = this.msgArg as AllPlayerPlotEnd_RecvMsg_Arg;
            
            BattleEntityManager.Instance.SetAllEntityShowState(true);
            CameraManager.Instance.GetCameraUI().SetUICameraShowState(true);

            PlotManager.Instance.ClosePlot();
        }
    }

    public class AllPlayerPlotEnd_RecvMsg_Arg : BaseClientRecvMsgArg
    {
    }
}