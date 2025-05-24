using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Config;
using NetProto;
using UnityEngine;

namespace Battle_Client
{
    public class BattleTool_Client
    {
        public static int GetMaxWave(int battleConfigId)
        {
            var battleConfig = ConfigManager.Instance.GetById<Config.Battle>(battleConfigId);
            var processId = battleConfig.ProcessId;
            var allProcessWaveConfig = ConfigManager.Instance.GetList<Config.BattleProcessWave>();
            var list = allProcessWaveConfig.Where(x => x.ProcessId == processId).ToList();

            return list.Count;
        }
    }
}