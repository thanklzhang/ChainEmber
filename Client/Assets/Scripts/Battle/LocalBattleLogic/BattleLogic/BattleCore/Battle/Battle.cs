using System;
using System.Collections.Generic;

// 
// 

namespace Battle
{
    //战斗逻辑流程
    public partial class Battle
    {
        public BattlePlayerMgr battlePlayerMgr = new BattlePlayerMgr();
        BattleMapMgr battleMapMgr = new BattleMapMgr();

        public BattleEntityMgr battleEntityMgr = new BattleEntityMgr();

        //PlayerActionMgr playerActionMgr = new PlayerActionMgr();
        SkillEffectMgr skillEffectMgr = new SkillEffectMgr();

        private BattleRecvMsgManager battleRecvMsgManager = new BattleRecvMsgManager();
        // private BattleSendMsgManager battleSendMsgManager = new BattleSendMsgManager();

        // AIMgr aiMgr = new AIMgr();
        public CollisionMgr collisionMgr = new CollisionMgr();

        // public TriggerMgr triggerMgr = new TriggerMgr();
        public BattleProcess battleProcess = new BattleProcess();
        public BattleItemMgr battleItemMgr = new BattleItemMgr();

        public int battleConfigId;
        private BattleState battleState = BattleState.Null;
        public int guid;
        public int roomId;

        // private float timeDelta = 0.033f;
        public float TimeDelta;
        public float totalTime;
        int randSeek;
        public Random rand;
        public int currFrame = 1;
        public int stageId;
        public int funcId;

        // public Action<TriggerArg> OnTimePassAction;
        // public Action<TriggerArg> OnEntityEventAction;
        // public Action<TriggerArg> OnBattleStartAction;
        // public Action<TriggerArg> OnPlotStartAction;
        // public Action<TriggerArg> OnPlotEndAction;

        public Action<BattleEntity> OnEntityDeadAction;
        public Action<Battle, int> OnBattleEnd;
        public Action OnWaveReadyProcessStartAction;
        public Action OnWaveBattleProcessStartAction;

        //public ITriggerReader TriggerReader;
        public IBattleMsgSender PlayerMsgSender;
        // public IPlayerMsgReceiver PlayerMsgReceiver;
        // public IConfigManager ConfigManager;

        //怪物增强属性
        public List<List<AttrOption>> totalEnemyAddedAttrs = new List<List<AttrOption>>();

        public void AddEnemyAddedAttr(List<AttrOption> attrs)
        {
            totalEnemyAddedAttrs.Add(attrs);
        }

        public void RemoveEnemyAddedAttr(List<AttrOption> attrs)
        {
            totalEnemyAddedAttrs.Remove(attrs);
        }

        //初始化
        internal void Init(int id, BattleCreateArg battleArg)
        {
            BattleLog.Log("Init");

            this.guid = id;

            randSeek = unchecked((int)DateTime.Now.Ticks);
            rand = new Random(randSeek);
            this.roomId = battleArg.roomId;
            this.battleConfigId = battleArg.configId;

            battleState = BattleState.CanUse;

            this.Load(battleArg);
        }

        //战斗加载
        public void Load(BattleCreateArg battleArg)
        {
            BattleLog.Log("start load");
            //---------------初始化各个模块--------------------------
            //配置管理器
            // this.ConfigManager.Init();

            //战斗消息管理初始化
            battleRecvMsgManager.Init(this);
            // battleSendMsgManager.Init(this);

            var battleConfig = BattleConfigManager.Instance.GetById<IBattle>(battleConfigId);
            this.funcId = battleArg.funcId;

            var battlePlayerArg = battleArg.battlePlayerInitArg;
            var mapInitArg = battleArg.mapInitArg;
            var entityInitArg = battleArg.entityInitArg;
            //var stageLogicArg = battleArg.stageLogicArg;

            //地图数据初始化
            battleMapMgr.Init(mapInitArg);

            //TODO 初始化触发器 去除
            // this.InitTrigger(battleArg.triggerSrcData);

            //玩家初始化
            battlePlayerMgr.Init(battlePlayerArg);

            //单位初始化
            battleEntityMgr.Init(entityInitArg, this);

            //AI初始化
            // aiMgr.Init(this);

            //道具管理器初始化
            battleItemMgr.Init();

            //test
            // battleItemMgr.GenerateItems(100001, 1);
            // battleItemMgr.GenerateItems(100001, 1);

            //玩家行为初始化
            // playerActionMgr.Init(this);

            //技能效果初始化
            skillEffectMgr.Init(this);

            //碰撞器管理初始化
            collisionMgr.Init(this);

            //战斗流程初始化
            battleProcess.Init(battleConfig.ProcessId, this);

            // //模块初始化完毕 开始业务层初始化
            // //InitEntitySkills();
            // InitEntityItems();


            //-----------------功能初始化结束 相互关联的信息可以设置了----------------------------------
            SetRelativeInfo();

            TestCreateEntityItems();

            //同步所有实体的数据
            SyncAllEntityData();

            battleState = BattleState.Loading;

            BattleLog.Log("finish load");
        }

        void SyncAllEntityData()
        {
            var entities = battleEntityMgr.GetAllEntity();
            foreach (var item in entities)
            {
                var entity = item.Value;
                entity.SyncBattleData();
            }
        }

        void SetRelativeInfo()
        {
            battleEntityMgr.SetRelativeInfo();
            battlePlayerMgr.Init_Relation();
        }

        //战斗开始
        public void Start()
        {
            //_G.Log("battle start !");

            BattleLog.Log("battle logic start");

            battleState = BattleState.Battling;
            this.battleProcess.Start();

            this.PlayerMsgSender.NotifyAll_BattleStart();

            // this.OnBattleStartAction?.Invoke(null);
        }

        public void Update()
        {
            if (battleState == BattleState.Null ||
                battleState == BattleState.CanUse ||
                battleState == BattleState.End)
            {
                return;
            }

            battleRecvMsgManager.Update();
            // battleSendMsgManager.Update();
            this.battlePlayerMgr.Update();

            if (battleState == BattleState.Battling)
            {
                this.collisionMgr.Update(TimeDelta);
                skillEffectMgr.Update(TimeDelta);
                battleEntityMgr.Update(TimeDelta);
                battleMapMgr.Update();
                this.battleProcess.Update(TimeDelta);
                battleItemMgr.Update(TimeDelta);

                currFrame = currFrame + 1;

                totalTime = currFrame * TimeDelta;

                //OnTimePassAction?.Invoke(new EventTimePassArg { currTime = totalTime });
            }
        }


        public void OnWaveReadyProcessStart(BattleProcessWave battleProcessWave)
        {
            this.OnWaveReadyProcessStartAction?.Invoke();
            
            
        }

        public void OnWaveBattleProcessStart(BattleProcessWave battleProcessWave)
        {
            this.OnWaveBattleProcessStartAction?.Invoke();
        }

        public void BattleWavePass(int passTeam, BattleWavePassResult reward)
        {
            this.PlayerMsgSender.Notify_BattleWavePass(passTeam, reward);
        }

        public void BattleEnd(int teamIndex)
        {
            battleState = BattleState.End;
            OnBattleEnd?.Invoke(this, teamIndex);
        }

        public void BattleEnd_Pre(int teamIndex) //, BattleEndType endType
        {
            battleState = BattleState.End;

            //var str = bIsWin ? "Win" : "Fail";

            ////test 先只有两队
            //var winTeam = 0;
            //if (bIsWin)
            //{
            //    winTeam = 0;
            //}
            //else
            //{
            //    winTeam = 1;
            //}

            // OnBattleEnd?.Invoke(this, teamIndex, endType);

            //this.PlayerMsgSender.NotifyAll_BattleEnd(winTeam);
            //_G.Log("BattleEnd : " + str);

            ////战斗结束监听触发 todo

            //scNotifyBattleEnd2S batleEnd = new scNotifyBattleEnd2S();
            //var allPlayers = GetAllPlayers();
            //foreach (var player in allPlayers)
            //{
            //    var uid = player.uid;
            //    batleEnd.PlayerEndInfoList.Add(new PlayerBattleEndInfo()
            //    {
            //        Uid = (int)uid,
            //        IsWin = bIsWin ? 1 : 0
            //    });
            //}
            //batleEnd.RoomId = this.roomId;
            //batleEnd.StageId = stageId;
            //batleEnd.BattleTableId = this.tableId;


            //ProxyMgr.CenterProxy.RequestAync((int)ProtoIDs.NotifyBattleEnd2S, batleEnd.ToByteArray());

            ////清理本次战斗 (最好是 battleMgr 监听 然后调用 clear)
            //ServerMgr.DestroyBattle(this.guid);
        }

        public void Destroy()
        {
            // OnTimePassAction = null;
            // OnEntityEventAction = null;
            //
            OnBattleEnd = null;
            //TODO Clear
            //battlePlayerMgr.Clear();
            //battleMapMgr.Clear();
            //battleEntityMgr.Clear();
            //playerActionMgr.Clear();
            //skillEffectMgr.Clear();

            //注意 目前是删除 没有复用战斗

            // triggerMgr.Clear();
            this.battleRecvMsgManager.Clear();
            // this.battleSendMsgManager.Clear();


            battleProcess.Release();
            battleItemMgr.Release();
        }
    }
}