using System.Collections.Generic;
using System.Linq;

namespace Battle
{
    //init
    public class BattlePlayerInit
    {
        public int playerIndex;
        public long uid;
        public int team;

        public bool isPlayerCtrl;
    }

    public class BattlePlayerInitArg
    {
        public List<BattlePlayerInit> battlePlayerInitList;
    }

    //////////////////////// runtime

    public partial class BattlePlayer
    {
        public int playerIndex;
        public long uid;
        public int team;

        public int ctrlHeroGuid;

        //temp data
        private int progress; //thousand

        public int Progress
        {
            get => progress;
            set => progress = value;
        }

        private bool isReadyFinish;

        internal bool IsReadyFinish
        {
            get => isReadyFinish;
            set => isReadyFinish = value;
        }

        //客户端剧情播放完成
        private bool isPlotEnd;

        public bool IsPlotEnd
        {
            get => isPlotEnd;
            set => isPlotEnd = value;
        }

        private bool isPlayerCtrl;

        internal bool IsPlayerCtrl
        {
            get => isPlayerCtrl;
            set => isPlayerCtrl = value;
        }

        public Dictionary<int, BattleCurrency> currencyDic = new Dictionary<int, BattleCurrency>();

        // public BaseEntityCtrl EntityCtrl;

        public Battle battle;

        public BattleEntity entity;

        public void Init()
        {
            InitBuyInfo();
            InitBattlePlayerInput();
            InitEntityCtrl();
            InitCurrency();
            InitBoxShop();
            InitMyBox();
            InitBattleRewardList();
            InitItemWarehouseList();
        }

        void InitEntityCtrl()
        {
            // if (isPlayerCtrl)
            // {
            //     EntityCtrl = new BattlePlayerEntityCtrl();
            //     EntityCtrl.Init();
            // }
            
          
        }

        internal void InitCtrlHeroEntity(BattleEntity entity)
        {
            this.entity = entity;
            ctrlHeroGuid = entity.guid;
            this.battle = this.entity.GetBattle();

            // EntityCtrl?.InitEntity(entity);
        
        }


        //玩家请求进入战斗流程
        public void AskStartBattleProcess()
        {
            this.battle.battleProcess.OnPlayerAskStartBattle(playerIndex);
        }

        public void ReviveAllTeamMember()
        {
            
        }

        public void Revive(int entityGuid, bool isRevive)
        {
            if (this.entity.guid != entityGuid)
            {
                return;
            }

            if (isRevive)
            {
                this.entity.Revive();
                
                this.CostCurrency(BattleCurrency.reviveCoinId,1);
            }
            else
            {
                battle.battleProcess.OnPlayerGiveUpRevive();
            }
        }

        public void Update()
        {
            // EntityCtrl?.Update();
        }
    }
}