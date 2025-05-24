using System.Collections.Generic;
using System.Linq;
namespace Battle
{
    public class BattlePlayerMgr
    {
        public List<BattlePlayer> battlePlayerList;
        public void Init(BattlePlayerInitArg battlePlayerInitArg)
        {
            battlePlayerList = new List<BattlePlayer>();

            foreach (var item in battlePlayerInitArg.battlePlayerInitList)
            {
                BattlePlayer player = new BattlePlayer();
                var battlePlayer = item;
                //这个 index 可以理解为 guid 受战斗内查找使用
                player.playerIndex = battlePlayer.playerIndex;
                player.team = battlePlayer.team;
                //这个也可认为是 guid 但是战斗中逻辑不应该依靠这个 uid 
                player.uid = battlePlayer.uid;

                if (!battlePlayer.isPlayerCtrl)
                {
                    //非自己的电脑玩家(电脑)
                    player.Progress = 1000;
                    player.IsReadyFinish = true;
                }
                player.IsPlayerCtrl = battlePlayer.isPlayerCtrl;

                battlePlayerList.Add(player);
                player.Init();
            }

            //增加中立敌对玩家（敌对电脑）
            BattlePlayer playerEnemy = new BattlePlayer();
            playerEnemy.playerIndex = -1;
            playerEnemy.team = -1;
            playerEnemy.uid = -1;
            playerEnemy.Progress = 1000;
            playerEnemy.IsReadyFinish = true;
            battlePlayerList.Add(playerEnemy);
            playerEnemy.Init();

        }

        public void Init_Relation()
        {
            for (int i = 0; i < battlePlayerList.Count; i++)
            {
                var player = battlePlayerList[i];
                if (player.IsPlayerCtrl)
                {
                    player.InitMembers_Relation();
                }
            }
        }
        
        public BattlePlayer FindPlayerByUid(long uid)
        {
            var player = battlePlayerList.Find((p) => p.uid == uid);
            return player;
        }

        public BattlePlayer FindPlayerByPlayerIndex(int playerIndex)
        {
            return battlePlayerList.Find((player) => player.playerIndex == playerIndex);
        }

        public BattlePlayer FindPlayerByEntityGuid(int entityGuid)
        {
            var player = battlePlayerList.Find((player) =>
            {
                return player.ctrlHeroGuid == entityGuid;
            });

            return player;
        }

        public bool IsAllPlayerFinishLoading()
        {
            return battlePlayerList.All((player) =>
            {
                return player.Progress >= 1000;
            });

        }

        internal bool IsAllPlayerReadyFinish()
        {
            return battlePlayerList.All((player) =>
            {
                return player.IsReadyFinish;
            });
        }

        internal bool IsAllPlayerPlotEnd()
        {
            return battlePlayerList.All((player) =>
            {
                return player.IsPlotEnd;
            });
        }

        public void ResetAllPlayerPlotEndState()
        {
            battlePlayerList.ForEach((player) =>
            {
                player.IsPlotEnd = false;
            });
        }

        public void Update()
        {
            for (int i = 0; i < battlePlayerList.Count; i++)
            {
                var player = battlePlayerList[i];
                player.Update();
            }
        }

    }

}

