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
    //战斗玩家相关
    public partial class Battle
    {
        internal void InitPlayerCtrlEntity(int playerIndex, BattleEntity entity)
        {
            var player = battlePlayerMgr.FindPlayerByPlayerIndex(playerIndex);
            player.InitCtrlHeroEntity(entity);
        }

        //op func

        public bool IsAllPlayerFinishLoading()
        {
            return battlePlayerMgr.IsAllPlayerFinishLoading();
        }

        public void SetAllPlayerLodingState()
        {
            this.battleState = BattleState.LoadingFinish;
        }

        public bool IsAllPlayerReadyFinish()
        {
            return battlePlayerMgr.IsAllPlayerReadyFinish();
        }

        public bool IsAllPlayerPlotEnd()
        {
            return battlePlayerMgr.IsAllPlayerPlotEnd();
        }


        //有玩家上传加载进度
        public void SetPlayerProgress(int playerIndex, int progress)
        {
            var player = FindPlayerByPlayerIndex(playerIndex);
            if (null == player)
            {
                BattleLog.LogError("Battle : SetPlayerProgress : the player is not found : playerIndex : " + playerIndex);
            }

            player.Progress = progress;

            //_G.Log("battle : SetPlayerProgress : uid : " + uid + " progress : " + progress);

            //判断是否都加载好了
            var isAllFinishLoading = IsAllPlayerFinishLoading();
            if (isAllFinishLoading)
            {
                ////notify msg (at presend only for remote battle )
                //csNotifyAllPlayerLoadFinish allFinish = new csNotifyAllPlayerLoadFinish();
                //NotifyAllPlayerMsg(ProtoIDs.NotifyAllPlayerLoadFinish, allFinish);
                this.PlayerMsgSender.NotifyAll_AllPlayerLoadFinish();
            }
        }

        //有玩家战斗准备好了
        internal void PlayerReadyFinish(int playerIndex)
        {
            var player = FindPlayerByPlayerIndex(playerIndex);
            player.IsReadyFinish = true;

            this.PlayerMsgSender.NotifyAll_PlayerReadyState((int)playerIndex, true);
            
            //_G.Log("battle : PlayerReadyFinish : uid : " + uid);

            //判断是否都准备好了
            var isAllReadyFinish = IsAllPlayerReadyFinish();
            if (isAllReadyFinish)
            {
                //战斗开始
                Start();
            }
        }

        // //添加一个玩家操作
        // public void AddPlayerAction(PlayerAction action)
        // {
        //     playerActionMgr.AddPlayerAction(action);
        // }

        
        public BattlePlayer FindPlayerByUid(long uid)
        {
            return battlePlayerMgr.FindPlayerByUid(uid);
        }
        
        public BattlePlayer FindPlayerByEntityGuid(int guid)
        {
            return battlePlayerMgr.FindPlayerByEntityGuid(guid);
        }

        public BattlePlayer FindPlayerByPlayerIndex(int index)
        {
            return battlePlayerMgr.FindPlayerByPlayerIndex(index);
        }
        
        public List<BattlePlayer> GetAllPlayers()
        {
            return this.battlePlayerMgr.battlePlayerList;
        }

        
        //有玩家剧情播放完事了
        internal void PlayerPlotEnd(int playerIndex)
        {
            var player = FindPlayerByPlayerIndex(playerIndex);
            player.IsPlotEnd = true;

            //_G.Log("battle : PlayerPlotEnd : uid : " + uid);

            //判断是否都播放完成了
            var isAllPlotEnd = IsAllPlayerReadyFinish();
            if (isAllPlotEnd)
            {
                this.OnAllPlayerPlotEnd("");
            }
        }

        //播放剧情
        public void PlayPlot(string name)
        {
            this.battlePlayerMgr.ResetAllPlayerPlotEndState();

            // OnPlotStartAction?.Invoke(null);

            //scNotifyPlayPlot playPlot = new scNotifyPlayPlot();
            //playPlot.PlotName = name;
            //NotifyAllPlayerMsg(ProtoIDs.NotifyPlayPlot, playPlot);
            this.PlayerMsgSender.NotifyAll_PlayPlot(name);
        }
        
        
    }
}