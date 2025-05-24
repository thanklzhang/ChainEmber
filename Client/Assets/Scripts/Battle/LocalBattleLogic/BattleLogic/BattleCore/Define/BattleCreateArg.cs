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
   public class BattleCreateArg
   {
      //guid
      //public int guid;
      //对应 center server 的房间 id 
      public int roomId;

      //配置 id , 表格中的 id
      public int configId;

      //public BattleType battleType;
      public BattlePlayerInitArg battlePlayerInitArg;
      public MapInitArg mapInitArg;
      public BattleEntityInitArg entityInitArg;
      public int funcId;

      //public TriggerSourceResData triggerSrcData;
      //public int stageId;//业务层的关卡 id
      ////地图尺寸
      //public int mapSizeX;
      //public int mapSizeY;
   }

}

