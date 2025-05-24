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
    public class EffectMoveArg
    {
        public int effectGuid;
        public Vector3 startPos;
        public Vector3 targetPos;
        public int targetGuid;
        public bool isFollow;
        public float moveSpeed;
        
        public bool isFlyMaxRange;
        public Vector3 dirByFlyMaxRange;
    }
}