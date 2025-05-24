using System.Collections.Generic;
using System.Linq;

namespace Battle
{
    //该玩家获得的战斗奖励列表 用于玩家观看已获得奖励信息列表
    public partial class BattlePlayer
    {
        private List<BaseBattleReward> battleRewardList;
        // 奖励记录器：用于追踪奖励获取次数和上限控制
        public BattlePlayerRewardRecorder rewardRecorder;

        public void InitBattleRewardList()
        {
            battleRewardList = new List<BaseBattleReward>();
            rewardRecorder = new BattlePlayerRewardRecorder();
        }

        public void AddBattleReward(BaseBattleReward battleReward)
        {
            // 记录奖励获取次数
            rewardRecorder?.RecordReward(battleReward.rewardConfig.Id);
            
            battleRewardList.Add(battleReward);

            // SyncBattleReward(battleReward);
        }
        
        public bool CheckBattleRewardExist(BaseBattleReward battleReward)
        {
            return battleRewardList.Contains(battleReward);
        }

        public void RemoveBattleReward(BaseBattleReward battleReward)
        {
            battleRewardList.Remove(battleReward);

            battleReward.isWillDelete = true;

            SyncBattleReward(battleReward);
        }

        public void SyncAllBattleRewards()
        {
            // this.battle.OnUpdateBoxInfo(this.playerIndex, this.myBoxDic);
        }

        public void SyncBattleReward(BaseBattleReward battleReward)
        {
            // //Logx.Log(LogxType.BattleItem,"entity OnSyncItemInfo : init item list : count : " + itemList.Count);
            //
            this.battle.OnNotifySyncBattleReward(this.playerIndex,
                battleReward);
        }

        public bool CheckRewardLimitReached(int rewardId)
        {
            return rewardRecorder?.IsRewardLimitReached(rewardId) ?? false;
        }

        public void Release()
        {
            battleRewardList?.Clear();
            rewardRecorder?.Reset();
        }
    }
}