using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using System.IO;



namespace Battle
{
    public class BattleWavePassResult
    {
        public int passTeam;
        public List<BattleCurrency> currencyItemList;
        public Dictionary<RewardQuality, List<BattleBoxBean>> boxDic;
    }

    public class BattleBoxBean
    {
        public int configId;
    }

   
}

