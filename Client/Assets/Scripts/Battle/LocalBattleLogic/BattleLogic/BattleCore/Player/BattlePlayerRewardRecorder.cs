using System.Collections.Generic;

namespace Battle
{
    /// <summary>
    /// 战斗玩家奖励记录器：用于跟踪玩家获得的奖励次数，实现奖励产出上限限制
    /// </summary>
    public class BattlePlayerRewardRecorder
    {
        // 奖励ID => 已获得次数的映射
        private Dictionary<int, int> rewardCounts = new Dictionary<int, int>();
        
        /// <summary>
        /// 记录玩家获得了某个奖励
        /// </summary>
        /// <param name="rewardId">奖励ID</param>
        /// <param name="count">获得次数</param>
        public void RecordReward(int rewardId, int count = 1)
        {
            if (!rewardCounts.ContainsKey(rewardId))
            {
                rewardCounts[rewardId] = 0;
            }
            
            rewardCounts[rewardId] += count;
            
            // 使用正确的战斗日志方式，LogxType.BattleBox对应114
            BattleLog.Log(114, $"记录奖励获取：ID={rewardId}，当前次数={rewardCounts[rewardId]}");
        }
        
        /// <summary>
        /// 获取玩家已获得某奖励的次数
        /// </summary>
        /// <param name="rewardId">奖励ID</param>
        /// <returns>已获得的次数</returns>
        public int GetRewardCount(int rewardId)
        {
            if (!rewardCounts.ContainsKey(rewardId))
            {
                return 0;
            }
            
            return rewardCounts[rewardId];
        }
        
        /// <summary>
        /// 检查奖励是否达到了产出上限
        /// </summary>
        /// <param name="rewardId">奖励ID</param>
        /// <returns>是否达到上限</returns>
        public bool IsRewardLimitReached(int rewardId)
        {
            // 获取奖励配置
            var rewardConfig = BattleConfigManager.Instance.GetById<IBattleReward>(rewardId);
            if (rewardConfig == null)
            {
                // 配置不存在，视为无限制
                return false;
            }
            
            // MaxAcquireCount为0表示无上限限制
            if (rewardConfig.MaxAcquireCount <= 0)
            {
                return false;
            }
            
            // 检查当前获取次数是否达到上限
            int currentCount = GetRewardCount(rewardId);
            return currentCount >= rewardConfig.MaxAcquireCount;
        }
        
        /// <summary>
        /// 重置所有奖励记录
        /// </summary>
        public void Reset()
        {
            rewardCounts.Clear();
        }
    }
} 