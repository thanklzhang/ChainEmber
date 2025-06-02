using System;
using System.Collections.Generic;


namespace Battle
{
    //战斗流程
    public class BattleProcess
    {
        private int processId;
        private int currWaveIndex;
        List<BattleProcessWave> waveList = new List<BattleProcessWave>();

        private bool isStart = false;

        public Battle battle;

        // public Action OnWaveReadyProcessStartAction;
        // public Action OnWaveBattleProcessStartAction;

        public BattleProcessWave CurrWave
        {
            get
            {
                if (currWaveIndex < 0 || currWaveIndex >= waveList.Count)
                {
                    return null;
                }

                return waveList[currWaveIndex];
            }
        }

        public void Init(int processId, Battle battle)
        {
            this.processId = processId;
            this.battle = battle;

            //填充波次所有信息
            FillWaveList();
        }


        void FillWaveList()
        {
            var waveConfigs = GetWaveConfigs(this.processId);

            waveList = new List<BattleProcessWave>();

            for (int i = 0; i < waveConfigs.Count; i++)
            {
                var waveConfig = waveConfigs[i];
                BattleProcessWave wave = new BattleProcessWave();
                wave.Init(waveConfig, this);

                waveList.Add(wave);
            }
        }

        public List<IBattleProcessWave> GetWaveConfigs(int processId)
        {
            var list = BattleConfigManager.Instance.GetList<IBattleProcessWave>();

            list = list.FindAll(c => c.ProcessId == processId);

            list.Sort((a, b) => { return a.Id.CompareTo(b.Id); });

            return list;
        }

        public void Start()
        {
            BattleLog.Log("BattleProcess : start a new process");

            currWaveIndex = 0;
            isStart = true;

            // CurrWave.RegisterEndCallback(OnWaveEnd);
            CurrWave.Start();
        }

        //玩家开始请求进入战斗
        public void OnPlayerAskStartBattle(int playerIndex)
        {
            //TODO 这里先用单机 实际上应该是判断所有都准备好才行
            CurrWave.OnPlayerAskStartBattle();
        }

        public void OnPlayerGiveUpRevive()
        {
            OnWaveEnd(this.currWaveIndex, false);
        }

        public void OnWaveEnd(int index, bool isWin)
        {
            CurrWave.SetEndState();

            if (index == this.currWaveIndex)
            {
                //CurrWave.UnregisterEndCallbacck();
                if (isWin)
                {
                    currWaveIndex += 1;

                    if (currWaveIndex < this.waveList.Count)
                    {
                        //当前波通过 继续下一波
                        var reward = GenWavePassReward(currWaveIndex);
                        NotifyWavePass(reward);
                        CurrWave.Start();
                    }
                    else
                    {
                        //赢了 并且所有波数结束 所以整个战斗是赢了 
                        this.End(true);
                    }
                }
                else
                {
                    this.End(false);
                }
            }
            else
            {
                BattleLog.LogWarning("BattleProcess : OnWaveEnd : index is not equal to this.currWaveIndex , index : "
                                      + index + " , this.currWaveIndex : " + this.currWaveIndex);
            }
        }

        BattleWavePassResult GenWavePassReward(int waveIndex)
        {
            var passTeam = 0;
            var wave = waveList[waveIndex];
            var rewardConfig =
                BattleConfigManager.Instance.GetById<IBattleProcessWaveReward>(wave.waveConfig.PassRewardId);

            //货币（先按照只有货币来计算 后续如果有非货币的道具再说）
            var itemIds = rewardConfig.ItemIdList;
            var itemCounts = rewardConfig.ItemCountList;

            //箱子
            var boxIds = rewardConfig.FixedBoxIdList;
            var boxCounts = rewardConfig.FixedBoxCountList;

            //生成通过回合奖励
            var players = battle.GetAllPlayers();
            for (int i = 0; i < players.Count; i++)
            {
                var player = players[i];
                if (player.team != passTeam)
                {
                    continue;
                }

                player.GainCurrency(itemIds, itemCounts);
                player.GainBoxes(boxIds, boxCounts);
            }

            //组装回合结算数据 仅供客户端展示
            var rewardResult = new BattleWavePassResult();
            rewardResult.passTeam = passTeam;
            rewardResult.currencyItemList = new List<BattleCurrency>();
            rewardResult.boxDic = new Dictionary<RewardQuality, List<BattleBoxBean>>();
            //货币
            for (int i = 0; i < itemIds.Count; i++)
            {
                var id = itemIds[i];
                var count = 0;
                if (i < itemCounts.Count)
                {
                    count = itemCounts[i];
                }

                var t_item = new BattleCurrency()
                {
                    itemConfigId = id,
                    count = count
                };
                rewardResult.currencyItemList.Add(t_item);
            }

            //箱子
            for (int i = 0; i < boxIds.Count; i++)
            {
                var boxId = boxIds[i];
                var boxCount = i < boxCounts.Count ? boxCounts[i] : 0;
                var boxConfig = BattleConfigManager.Instance.GetById<IBattleBox>(boxId);
                var boxQuality = (RewardQuality)boxConfig.Quality;

                List<BattleBoxBean> boxList = null;
                if (!rewardResult.boxDic.ContainsKey(boxQuality))
                {
                    boxList = new List<BattleBoxBean>();
                    rewardResult.boxDic.Add(boxQuality, boxList);
                }
                else
                {
                    boxList = rewardResult.boxDic[boxQuality];
                }

                boxList.Add(new BattleBoxBean() { configId = boxId });
            }

            return rewardResult;
        }

        void NotifyWavePass(BattleWavePassResult reward)
        {
            //默认玩家队伍 team 为 0
            var passTeam = 0;
            battle.BattleWavePass(passTeam, reward);
        }

        public void Update(float deltaTime)
        {
            if (!isStart)
            {
                return;
            }

            if (CurrWave != null)
            {
                CurrWave.Update(deltaTime);
            }
        }


        public void OnWaveReadyProcessStart(BattleProcessWave battleProcessWave)
        {
            this.battle.OnWaveReadyProcessStart(battleProcessWave);
        }

        public void OnWaveBattleProcessStart(BattleProcessWave battleProcessWave)
        {
            this.battle.OnWaveBattleProcessStart(battleProcessWave);
        }

        public void End(bool isWin)
        {
            BattleLog.Log("BattleProcess : the whole process end");
            isStart = false;

            //目前只有两队（team） 玩家联盟队伍（0）和怪物队伍（-1）
            //如果之后有其他模式需要不同队伍 那么这里需要拓展 
            var winTeam = isWin ? 0 : -1;

            this.battle.BattleEnd(winTeam);
        }

        public void Release()
        {
            for (int i = 0; i < waveList.Count; i++)
            {
                var wave = waveList[i];
                wave.Release();
            }
        }
    }
}