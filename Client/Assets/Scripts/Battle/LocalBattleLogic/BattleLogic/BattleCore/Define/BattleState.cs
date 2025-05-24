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
   public enum BattleState
   {
      Null = 0,
      CanUse = 1,
      Loading = 2,
      LoadingFinish = 3,
      Battling = 4,
      End = 5,
   }

}

