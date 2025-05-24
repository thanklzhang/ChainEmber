using System;
using System.Collections.Generic;


namespace Battle
{
    //宝箱
    public class BattleBox
    {
        public List<RewardSelection> selectionGroup;
        // public BattleEntity entity;
        public BattlePlayer player;

        public IBattleBox boxConfig;

        // public void Init(int boxConfigId, BattleEntity entity)
        // {
        //     this.entity = entity;
        //
        //     boxConfig = BattleConfigManager.Instance.GetById<IBattleBox>(boxConfigId);
        // }
        
        public void Init(int boxConfigId, BattlePlayer player)
        {
            this.player = player;

            boxConfig = BattleConfigManager.Instance.GetById<IBattleBox>(boxConfigId);
        }

        //生成奖励选项组 一旦生成不能改变
        public bool GenSelectionReward()
        {
            if (IsOpened())
            {
                return false;
            }

            selectionGroup = new List<RewardSelection>();

            //填充奖励组
            //selectionGroup.Add
            GenRewards();

            return true;
            // //初始化
            // for (int i = 0; i < selectionGroup.Count; i++)
            // {
            //     var selection = selectionGroup[i];
            //     selection.Init(this.entity);
            //   
            // }

            // entity.OnOpenBox(this);
        }

        public void GenRewards()
        {
            //生成奖励逻辑
            int poolId = boxConfig.PoolId;
            //pool = find

            var pp = BattleConfigManager.Instance.GetById<IBattleRewardPool>(poolId);

            var pool = BattleConfigManager.Instance.GetById<IBattleRewardPool>(poolId);
            int selectionCount = boxConfig.SelectionCount;

            //处理固定奖励
            List<int> fixedRewards = pool.FixedRewardList;
            List<int> rewardIdList = new List<int>();
            
            // 添加固定奖励，但也要检查上限
            if (fixedRewards != null && fixedRewards.Count > 0)
            {
                foreach (var fixedRewardId in fixedRewards)
                {
                    // 检查固定奖励是否已达到上限
                    if (!player.CheckRewardLimitReached(fixedRewardId))
                    {
                        rewardIdList.Add(fixedRewardId);
                    }
                    else
                    {
                        BattleLog.LogWarning($"固定奖励 ID={fixedRewardId} 已达到上限，跳过");
                    }
                }
            }

            // 计算还需要随机生成多少个奖励
            int remainingCount = Math.Max(0, selectionCount - rewardIdList.Count);
            
            //从池子中随机出几个奖励id
            for (int i = 0; i < remainingCount; i++)
            {
                var idList = pool.RewardIdList;
                var weightList = pool.RewardWeightList;
                
                // 过滤掉已达到获取上限的奖励
                List<int> availableIdList = new List<int>();
                List<int> availableWeightList = new List<int>();
                
                for (int j = 0; j < idList.Count; j++)
                {
                    int rewardId = idList[j];
                    // 检查是否达到上限
                    if (!player.CheckRewardLimitReached(rewardId))
                    {
                        availableIdList.Add(rewardId);
                        if (j < weightList.Count)
                        {
                            availableWeightList.Add(weightList[j]);
                        }
                        else
                        {
                            // 默认权重为1
                            availableWeightList.Add(1);
                        }
                    }
                }
                
                // 如果没有可用奖励(全部达到上限)，则使用原始列表
                if (availableIdList.Count == 0)
                {
                    BattleLog.LogWarning($"所有奖励都已达到上限，使用原始奖励池");
                    availableIdList = idList;
                    availableWeightList = weightList;
                }

                var index = BattleRandom.GetNextIndexByWeights(availableWeightList);

                var id = availableIdList[index];
                rewardIdList.Add(id);
            }

            //根据奖励id在进行获得实际奖励项（先按照没选择之前就确定奖励项来）
            for (int i = 0; i < rewardIdList.Count; i++)
            {
                var rewardId = rewardIdList[i];

                RewardSelection selection = new RewardSelection();
                selection.Init(rewardId, this.player);
                selection.GenRealityReward();

                selectionGroup.Add(selection);
            }
        }

        public bool IsOpened()
        {
            return selectionGroup != null;
        }

        public void Select(int index)
        {
            if (index < selectionGroup.Count)
            {
                var selection = selectionGroup[index];
                var rewardId = selection.rewardConfig.Id;
                
                BattleLog.Log(114, $"玩家选择了奖励选项：索引={index}，奖励ID={rewardId}");

                //触发效果
                selection.Trigger();
                
                // 记录宝箱选择完成
                BattleLog.Log(114, $"宝箱奖励选择完成，剩余可选数：{selectionGroup.Count - 1}");
            }
            else
            {
                BattleLog.LogError($"选择的奖励索引超出范围：{index}，最大索引：{selectionGroup.Count - 1}");
            }
        }
    }

  

}