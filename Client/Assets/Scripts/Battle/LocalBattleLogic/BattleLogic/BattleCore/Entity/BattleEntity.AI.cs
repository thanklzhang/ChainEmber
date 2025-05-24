using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;



namespace Battle
{
    public partial class BattleEntity
    {
        public  BaseAI ai;

        public void InitAI()
        {
            if (this.isPlayerCtrl)
            {
                ai = new PlayerAI();
            }
            else
            {
                ai = new CpuAI();
            }

            ai.Init(this);
        }

        public void UpdateAI(float deltaTime)
        {
            ai.Update(deltaTime);
        }
    }
}