using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GameData
{
    public class GameDataManager : BaseGameDataManager //Singleton<GameDataManager>
    {
        private static GameDataManager instance;

        public static GameDataManager Instance
        {
            get
            {
                if (null == instance)
                {
                    instance = new GameDataManager();
                }

                return instance;
            }
        }

        public HeroGameData HeroData;
        public BattleGameData BattleData;
        public UserGameData UserData;
        public MainTaskGameData MainTaskData;
        public BagGameData BagData;
        public TeamGameDataStore TeamData;

        internal override void Init()
        {
            HeroData = AddGameData<HeroGameData>();
            BattleData = AddGameData<BattleGameData>();
            UserData = AddGameData<UserGameData>();
            MainTaskData = AddGameData<MainTaskGameData>();
            BagData = AddGameData<BagGameData>();
            TeamData = AddGameData<TeamGameDataStore>();
        }
    }
}