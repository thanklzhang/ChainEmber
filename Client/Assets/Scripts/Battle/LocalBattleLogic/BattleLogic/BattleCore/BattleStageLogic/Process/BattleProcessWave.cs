using System;
using System.Collections.Generic;
using Battle_Client;

namespace Battle
{
    //战斗波次
    public class BattleProcessWave
    {
        public enum BattleWaveState
        {
            Null = 0,

            //玩家准备阶段
            Ready = 1,

            //战斗阶段
            Battle = 2,

            End = 3
        }

        public int waveIndex;
        private List<BattleProcessWaveNode> waveNodeList = new List<BattleProcessWaveNode>();

        private float limitTimer;
        private float maxLimitTime;
        private float readyTimer;
        private float maxReadyTime;

        // private bool isEnd = false;
        // private Action<int> endCallback;

        private BattleProcess battleProcess;


        private BattleWaveState state;

        public BattleWaveState State
        {
            get { return state; }

            set
            {
                if (state != value)
                {
                    BattleLog.Log(
                        string.Format(
                            "BattleProcess : wave state , configId : {0} , wave index(from 0) : {1} ,  {2} -> {3}",
                            waveConfig.Id, waveIndex, state, value));
                }

                state = value;
            }
        }

        public IBattleProcessWave waveConfig;

        public int currKillCount = 0;
        public int maxKillCount = 0;
        private Battle battle;

        public void Init(IBattleProcessWave waveConfig, BattleProcess battleProcess)
        {
            this.battleProcess = battleProcess;
            this.battleProcess.battle.OnEntityDeadAction += this.OnEntityDead;
            this.battle = this.battleProcess.battle;
            this.waveConfig = waveConfig;

            waveIndex = waveConfig.WaveIndex - 1;

            currCreateEntities = new List<BattleEntity>();

            maxLimitTime = waveConfig.LimitTime / 1000.0f;
            maxReadyTime = waveConfig.ReadyTime / 1000.0f;

            //填充波次中的每一个波次节点（波次中的每一个出兵逻辑）
            FillNodeList();

            State = BattleWaveState.Null;
        }

        public Battle GetBattle()
        {
            return this.battle;
        }

        void FillNodeList()
        {
            waveNodeList = new List<BattleProcessWaveNode>();

            var nodeIdList = this.waveConfig.WaveNodeIdList;
            for (int i = 0; i < nodeIdList.Count; i++)
            {
                var nodeId = nodeIdList[i];
                var nodeConfig = BattleConfigManager.Instance.GetById<IBattleProcessWaveNode>(nodeId);
                //var nodeConfig =
                BattleProcessWaveNode node = new BattleProcessWaveNode();

                node.Init(nodeConfig, this);
                waveNodeList.Add(node);
            }
        }

        public void Start()
        {
            BattleLog.Log("BattleProcess : start a new wave : waveIndex(from 0 start) : " + waveIndex);

            limitTimer = 0;
            readyTimer = 0;

            //计算一共会出多少敌人
            this.currKillCount = 0;
            this.maxKillCount = GetTotalEnemyCount();

            for (int i = 0; i < waveNodeList.Count; i++)
            {
                var node = waveNodeList[i];
                node.Start();
            }

            State = BattleWaveState.Ready;

            int time = (int)(maxReadyTime * 1000.0f);

            //复活所有死亡队友
            ReviveAllTeamMember();

            //准备阶段的奖励
            GainReadyProcessRewards();

            //回合结束的时候 所有单位满血
            RestoreAllUnitsHealth();
            
            battleProcess.battle.PlayerMsgSender.NotifyAll_EnterProcessState(BattleProcessState.Ready, waveIndex, time);

            this.OnWaveReadyProcessStart();
        }

        // 回合结束时恢复所有单位满血
        private void RestoreAllUnitsHealth()
        {
            foreach (var player in this.battle.GetAllPlayers())
            {
                if (player.entity != null)
                {
                    player.entity.SetCurrHp(player.entity.MaxHealth);
                    
                    // 恢复所有队员满血
                    var teamMembers = player.entity.GetTeamMemberList();
                    if (teamMembers != null)
                    {
                        foreach (var member in teamMembers)
                        {
                            if (member != null)
                            {
                                member.SetCurrHp(member.MaxHealth);
                            }
                        }
                    }
                }
            }
        }
        
        public void OnWaveReadyProcessStart()
        {
            // //准备结算奖励
            // GainReadyProcessRewards();

            this.battleProcess.OnWaveReadyProcessStart(this);
        }

        public void OnWaveBattleProcessStart()
        {
            this.battleProcess.OnWaveBattleProcessStart(this);
        }

        public void ReviveAllTeamMember()
        {
            foreach (var player in this.battle.GetAllPlayers())
            {
                if (player.IsPlayerCtrl)
                {
                    player.entity.ReviveAllTeamMember();
                }
            }
        }

        public void GainReadyProcessRewards()
        {
            var players = this.battle.GetAllPlayers();

            //人口奖励
            var population = waveConfig.ReadyAddPopulationCount;
            if (population > 0)
            {
                foreach (var player in players)
                {
                    if (player.IsPlayerCtrl)
                    {
                        player.AddPopulation(population);
                    }
                }
            }

            //道具奖励
            var rewardConfig =
                BattleConfigManager.Instance.GetById<IBattleProcessWaveReward>(this.waveConfig.ReadyRewardId);

            var itemIds = rewardConfig.ItemIdList;
            var itemCounts = rewardConfig.ItemCountList;

            for (int i = 0; i < itemIds.Count; i++)
            {
                var itemId = itemIds[i];
                var itemCount = itemCounts[i];
                var items = battle.battleItemMgr.GenerateItems(itemId, itemCount);

                foreach (var item in items)
                {
                    foreach (var player in players)
                    {
                        if (player.IsPlayerCtrl)
                        {
                            player.GainItem(item);
                        }
                    }
                }
            }

            //固定宝箱奖励
            var boxIds = rewardConfig.FixedBoxIdList;
            var boxCounts = rewardConfig.FixedBoxCountList;
            foreach (var player in players)
            {
                if (player.IsPlayerCtrl)
                {
                    player.GainBoxes(boxIds, boxCounts, true);
                }
            }
        }

        public int GetTotalEnemyCount()
        {
            int totalCount = 0;
            for (int i = 0; i < waveNodeList.Count; i++)
            {
                var node = waveNodeList[i];
                var count = node.GetTotalEnemyCount();
                totalCount += count;
            }

            return totalCount;
        }

        // public void RegisterEndCallback(Action<int> callback)
        // {
        //     endCallback = callback;
        // }
        //
        // public void UnregisterEndCallbacck()
        // {
        //     endCallback = null;
        // }


        //玩家开始请求进入战斗
        public void OnPlayerAskStartBattle()
        {
            if (State == BattleWaveState.Ready)
            {
                readyTimer = 0;
                EnterDoingState();
            }
        }

        public void Update(float deltaTime)
        {
            if (State == BattleWaveState.Null || State == BattleWaveState.End)
            {
                return;
            }


            if (State == BattleWaveState.Ready)
            {
                //玩家准备阶段
                readyTimer += deltaTime;
                if (readyTimer >= maxReadyTime)
                {
                    readyTimer = 0;
                    EnterDoingState();
                }
            }
            else if (State == BattleWaveState.Battle)
            {
                //战斗阶段
                for (int i = 0; i < waveNodeList.Count; i++)
                {
                    var node = waveNodeList[i];
                    node.Update(deltaTime);
                }

                limitTimer += deltaTime;

                if (limitTimer >= maxLimitTime)
                {
                    //目前这里到时间就失败（之后要根据通过类型去判断）
                    this.End(false);
                }
            }
            else if (State == BattleWaveState.End)
            {
                //结束
            }
        }

        private void EnterDoingState()
        {
            State = BattleWaveState.Battle;

            var waveType = (BattleWaveType)waveConfig.WaveType;
            int time = (int)(maxLimitTime * 1000.0f);
            if (waveType == BattleWaveType.Boss)
            {
                battleProcess.battle.PlayerMsgSender.NotifyAll_EnterProcessState(BattleProcessState.Boss,
                    this.waveIndex, time);
            }
            else
            {
                battleProcess.battle.PlayerMsgSender.NotifyAll_EnterProcessState(BattleProcessState.Battle,
                    this.waveIndex, time);
            }


            battleProcess.battle.PlayerMsgSender.NotifyAll_UpdateProcessStateInfo(this.currKillCount, maxKillCount);

            OnWaveBattleProcessStart();
        }

        public List<BattleEntity> currCreateEntities;

        public void OnCreateEntity(List<BattleEntity> entities)
        {
            currCreateEntities.AddRange(entities);
        }

        void OnEntityDead(BattleEntity entity)
        {
            if (this.state != BattleWaveState.Battle)
            {
                return;
            }

            //先按照测试规则：只要有一个玩家控制的实体阵亡 游戏就失败
            if (entity.isPlayerCtrl)
            {
                var player = this.battle.FindPlayerByPlayerIndex(entity.playerIndex);
                var revive = player.GetCurrency(BattleCurrency.reviveCoinId);
                if (revive.count <= 0)
                {
                    this.End(false);
                    return;
                }
            }

            var passType = (BattleWavePassType)waveConfig.PassType;
            if (passType == BattleWavePassType.KillBoss)
            {
                var bossEntityConfigId = FindWaveEndEntityConfigId();
                if (bossEntityConfigId > 0)
                {
                    if (entity.infoConfig.Id == bossEntityConfigId)
                    {
                        this.End(true);
                    }
                }
            }
            else if (passType == BattleWavePassType.KillAllInTime)
            {
                HandleDeadEntity(entity);

                if (this.currKillCount >= maxKillCount)
                {
                    this.End(true);
                }
            }
        }

        private void HandleDeadEntity(BattleEntity entity)
        {
            // var nodeList = this.waveNodeList;
            // for (int i = 0; i < nodeList.Count; i++)
            // {
            //     var node = nodeList[i];
            //     if (node.IsContainEntity(entity))
            //     {
            //         node.RemoveCurrEntity(entity);
            //     }
            // }

            if (this.currCreateEntities.Contains(entity))
            {
                this.currCreateEntities.Remove(entity);
                this.currKillCount += 1;

                //sync
                battleProcess.battle.PlayerMsgSender.NotifyAll_UpdateProcessStateInfo(this.currKillCount, maxKillCount);
            }
        }

        //找到当前波次最后需要打败的怪物（需要在波胜利条件是怪物死亡的前提下，目前应该都是按照这个）
        int FindWaveEndEntityConfigId()
        {
            var nodeList = this.waveNodeList;
            for (int i = 0; i < nodeList.Count; i++)
            {
                var node = nodeList[i];
                var isEndNode = 1 == node.config.IsEndNode;
                if (isEndNode)
                {
                    var insId = node.config.EntityInstanceId;
                    var insConfig = BattleConfigManager.Instance.GetById<IEntityInstance>(insId);
                    return insConfig.EntityConfigId;
                }
            }

            return 0;
        }


        public void End(bool isWin)
        {
            // 波次结束时恢复所有单位满血
            RestoreAllUnitsHealth();
            
            this.battleProcess.OnWaveEnd(waveIndex, isWin);
        }

        public void SetEndState()
        {
            limitTimer = 0;
            State = BattleWaveState.End;
        }


        public void Release()
        {
            for (int i = 0; i < waveNodeList.Count; i++)
            {
                var node = waveNodeList[i];
                node.Release();
            }

            this.battleProcess.battle.OnEntityDeadAction -= this.OnEntityDead;
        }
    }
}