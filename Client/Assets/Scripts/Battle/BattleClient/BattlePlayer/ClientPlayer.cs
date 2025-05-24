using System.Collections.Generic;
using UnityEngine;

namespace Battle_Client
{
    public partial class ClientPlayer
    {
        public int playerIndex;
        public int team;
        public int uid;
        public int ctrlHeroGuid;

        public void Init()
        {
            InitCurrency();
            InitItemWarehouse();
            InitBoxShop();
            InitMyBox();
            InitBattleReward();
            InitPreference();
        }

    }
}