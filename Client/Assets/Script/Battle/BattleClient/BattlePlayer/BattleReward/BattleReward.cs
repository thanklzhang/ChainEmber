using System.Collections.Generic;
using Battle;
using UnityEngine;

namespace Battle_Client
{
    public class BattleReward_Client
    {
        public int guid;

        public int configId;
        //
        // public int intArg1;
        // public int intArg2;
        //
        // public List<int> intListArg1;
        // public List<int> intListArg2;

        public List<BattleRewardEffectOption_Client> effectOptionList = new List<BattleRewardEffectOption_Client>();
    }

    public class BattleRewardEffectOption_Client
    {
        public int guid;
        public int configId;

        public int intArg1;
        public int intArg2;

        public List<int> intListArg1;
        public List<int> intListArg2;
    }
}