using System.Collections.Generic;

namespace Battle
{
    //战斗波次节点
    public class BattleProcessWaveNode
    {
        //波次索引	初始延迟（*1000）	间隔时间（*1000）	触发次数	实体id	实体个数	生成位置类型

        public enum WaveNodeState
        {
            Null = 0,

            //延迟中
            InitDelay = 1,

            //出兵逻辑运行中
            Doing = 2,
            End = 3
        }


        public int waveIndex;

        private float currDelayTimer;
        private float maxDelayTime;

        private float currIntervalTimer;
        private float maxIntervalTime;
        private WaveNodeState state;

        private WaveNodeState State
        {
            get { return state; }

            set
            {
                // Logx.Log(LogxType.BattleProcess,
                //     string.Format("wave node state , configId : {0} , wave index : {1} ,  {2} -> {3}",
                //     config.Id,waveIndex,state,value));
                state = value;
            }
        }

        private int currTriggerCount;
        private int maxTriggerCount;

        private Battle battle;

        public IBattleProcessWaveNode config;

      
        private BattleProcessWave currWave;

        public void Init(IBattleProcessWaveNode nodeConfig, BattleProcessWave wave)
        {
            this.currWave = wave;
            config = nodeConfig;
            waveIndex = config.Index;
            maxDelayTime = config.DelayTime / 1000.0f;
            maxIntervalTime = config.IntervalTime / 1000.0f;
            battle = this.currWave.GetBattle();

            maxTriggerCount = config.TriggerCount;
        }

        void Reset()
        {
            State = WaveNodeState.InitDelay;
            currTriggerCount = 0;
            currDelayTimer = 0;
            currIntervalTimer = 0;
        }

        public void Start()
        {
            Reset();
        }

        public void Update(float deltaTime)
        {
            if (State == WaveNodeState.InitDelay)
            {
                //延迟状态
                currDelayTimer += deltaTime;
                if (currDelayTimer >= maxDelayTime)
                {
                    currDelayTimer = 0;

                    EnterDoingState();
                }
            }
            else if (State == WaveNodeState.Doing)
            {
                //当前运行状态
                currIntervalTimer += deltaTime;
                if (currIntervalTimer >= maxIntervalTime)
                {
                    currIntervalTimer = 0;

                    TriggerFunction();
                }
            }
            else if (State == WaveNodeState.End)
            {
                //结束状态
            }
        }

        void EnterDoingState()
        {
            this.State = WaveNodeState.Doing;
        }

        void TriggerFunction()
        {
            //触发出兵逻辑
            HandleTriggerLogic();

            currTriggerCount += 1;

            if (currTriggerCount >= maxTriggerCount)
            {
                currTriggerCount = 0;

                this.End();
            }
        }

        List<Vector3> ResetCustomPosList()
        {
            List<Vector3> mapCustomPosList = new List<Vector3>();
            var customPosList = battle.GetMap().enemyInitPosList;
            for (int i = 0; i < customPosList.Count; i++)
            {
                var customPos = customPosList[i];
                mapCustomPosList.Add(customPos);
            }

            return mapCustomPosList;
        }

        //出兵创建逻辑
        void HandleTriggerLogic()
        {
            var count = config.EntityCount;
            CreatePosType createPosType = CreatePosType.MapPosIndexRand;
            
            var insId = config.EntityInstanceId;
            var insConfig = BattleConfigManager.Instance.GetById<IEntityInstance>(insId);
            
            var configId = insConfig.EntityConfigId;

            List<EntityInit> entityInitList = new List<EntityInit>();

            // HashSet<int> hasPutPosValues = new HashSet<int>();
            List<Vector3> mapCustomPosList = ResetCustomPosList();
            //posIndexes.add

            for (int i = 0; i < count; i++)
            {
                //create entity
                // var battle = this.trigger.GetCurrActionContext().battle;


                var entityConfigId = configId;
                // var pos = createPosition.Get(context);

                //setPos
                var pos = new Vector3();
                if (createPosType == CreatePosType.MapPosIndexRand)
                {
                    var randIndex = BattleRandom.Next(0, mapCustomPosList.Count);
                    var randValue = mapCustomPosList[randIndex];

                    if (mapCustomPosList.Count <= 0)
                    {
                        //pos = 
                        pos = randValue;
                    }
                    else
                    {
                        mapCustomPosList.RemoveAt(randIndex);

                        if (mapCustomPosList.Count <= 0)
                        {
                            mapCustomPosList = ResetCustomPosList();
                        }

                        //pos =
                        pos = randValue;
                    }
                }

                var entityConfig = BattleConfigManager.Instance.GetById<IEntityInfo>(entityConfigId);
                EntityInit entityInit = new EntityInit()
                {
                    configId = entityConfigId,
                    playerIndex = -1, //应该是传过来的 现在全是敌人， 中立敌对 -1 ，中立无敌意 -2
                    position = pos,
                    isPlayerCtrl = false,
                    // level = entityConfig.Level,
                    level = insConfig.Level,
                    star = insConfig.Star
                };

                entityInit.skillInitList = new List<SkillInit>();

                var skillConfigList = insConfig.SkillLevels;
                // foreach (var skillId in entityConfig.SkillIds)
                for(int j = 0; j < entityConfig.SkillIds.Count; j++)
                {
                    
                    var skillId = entityConfig.SkillIds[j];
                    var skillLevel = 1;
                    if (j < skillConfigList.Count)
                    {
                        skillLevel = skillConfigList[j];
                    }
                    SkillInit skill = new SkillInit()
                    {
                        configId = skillId,
                        level = skillLevel
                    };
                    entityInit.skillInitList.Add(skill);
                }
                
                if (entityConfig.UltimateSkillId > 0)
                {
                    SkillInit ultimateSkill = new SkillInit()
                    {
                        configId = entityConfig.UltimateSkillId,
                        level = 1
                    };
                    entityInit.skillInitList.Add(ultimateSkill);
                }

                entityInitList.Add(entityInit);
            }

         
            //创建实体
            var entities = battle.CreateEntities(entityInitList);
            
            //计算宝箱奖励引起的怪物属性增加
            foreach (var enemy in entities)
            {
                var attrsGroup = battle.totalEnemyAddedAttrs;
                for (int i = 0; i < attrsGroup.Count; i++)
                {
                    var attrs = attrsGroup[i];
                    enemy.AddAttrs(EntityAttrGroupType.BattleReward, attrs);
                }
            }
         
            this.currWave.OnCreateEntity(entities);
        }

        public int GetTotalEnemyCount()
        {
            return this.config.TriggerCount * this.config.EntityCount;
        }

        public void End()
        {
            this.State = WaveNodeState.End;
        }

        public void Release()
        {
            Reset();
        }
    }
}