using System.Collections.Generic;
using Battle;

namespace Battle_Client
{
    public partial class ClientPlayer
    {
        private List<BattleReward_Client> battleRewardList;

        public void InitBattleReward()
        {
            battleRewardList = new List<BattleReward_Client>();
        }

        public void AddBattleReward(BattleReward_Client battleReward)
        {
            var node = this.battleRewardList.Find(item => item.guid == battleReward.guid);

            if (null == node)
            {
                this.battleRewardList.Add(battleReward);

                EventDispatcher.Broadcast(EventIDs.OnUpdateBattleReward);
                
                //弹出一行字显示增加奖励的结果
                var nameStr = "";
                var desStr = "";
                AttrHelper_Client.GetBattleRewardContent(battleReward,true,true,out nameStr,
                    out desStr);
                
                // var tipsStr = $"{nameStr} , {desStr}";
                
                var tipsStr = $"{desStr}";
                
                
               UIManager.Instance.Open<CommonTipsUI>(new TipsUIArgs()
                {
                    tipStr = tipsStr
                });
            }

           
        }

        public List<BattleReward_Client> GetAllBattleRewards()
        {
            return this.battleRewardList;
        }
    }
}