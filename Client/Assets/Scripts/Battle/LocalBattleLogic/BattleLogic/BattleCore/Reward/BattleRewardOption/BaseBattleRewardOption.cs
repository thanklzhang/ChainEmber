using System.Collections.Generic;

namespace Battle
{
    public class BaseBattleRewardEffectOption
    {
        protected Battle battle;

        protected BattlePlayer player;

        // 是否应用于后续获得的队员
        public bool applyToNewTeamMembers;

        protected IBattleRewardEffectOption rewardOptionConfig;

        //每一个奖励效果都会生成唯一guid
        public int guid = 1;
        public static int maxGuid = 1;

        public static int GenRewardEffectGuid()
        {
            //防止出界  但是还是不对 待定
            if (maxGuid >= int.MaxValue)
            {
                maxGuid = 1;
            }

            return maxGuid++;
        }

        public int hasGainEffectTimes = 0;
        public int maxGainEffectTimes = 0;

        public void Init(IBattleRewardEffectOption rewardOption, BattlePlayer player)
        {
            this.player = player;
            this.rewardOptionConfig = rewardOption;

            this.applyToNewTeamMembers = 1 == rewardOption.ApplyToNewTeamMembers;

            battle = this.player.entity.GetBattle();

            // battle.OnWaveReadyProcessStartAction += OnWaveReadyProcessStart;
            // battle.OnWaveBattleProcessStartAction += OnWaveBattleProcessStart;

            hasGainEffectTimes = 0;
            maxGainEffectTimes = this.rewardOptionConfig.MaxGainTimesType;
        }

        //计算出最后确定的实际奖励
        public virtual void CalculateRealityReward()
        {
        }

        public bool IsCanGainEffect(BattleRewardEffectGainTimingType timeType)
        {
            if (!CheckGainTiming(timeType))
            {
                return false;
            }

            if (maxGainEffectTimes > 0)
            {
                //有次数限制
                if (hasGainEffectTimes >= maxGainEffectTimes)
                {
                    return false;
                }
            }
            else if (maxGainEffectTimes < 0)
            {
                return false;
            }
            else if (0 == maxGainEffectTimes)
            {
                //无限触发
            }

            return true;
        }

        public virtual void OnWaveReadyProcessStart()
        {
            
        }

        public void GainRewardEffectsByTime(BattleRewardEffectGainTimingType timeType)
        {
            // if (!CheckGainTiming(timeType))
            // {
            //     return;
            // }
            //
            // if (maxGainEffectTimes > 0)
            // {
            //     //有次数限制
            //     if (hasGainEffectTimes >= maxGainEffectTimes)
            //     {
            //         return;
            //     }
            // }
            // else if (maxGainEffectTimes < 0)
            // {
            //     return;
            // }
            // else if (0 == maxGainEffectTimes)
            // {
            //     //无限触发
            // }

            var isCanGain = IsCanGainEffect(timeType);

            if (isCanGain)
            {
                this.GainRewardEffect(this.player);
            }

          
        }

        // public int gainRewardTimes = 0;

        //获得奖励效果（实际奖励）
        public void GainRewardEffect(BattlePlayer gainer)
        {
            OnGain(gainer);

            // gainRewardTimes += 1;

            hasGainEffectTimes += 1;
        }

        //获得相关数值参数 根据奖励类型不同意义也不同
        public virtual BattleRewardEffectDTO GetValues()
        {
            return new BattleRewardEffectDTO()
            {
                guid = this.guid,
                configId = this.rewardOptionConfig.Id,
            };
        }

        public virtual void OnGain(BattlePlayer gainer)
        {
        }

        public int GetRandValueByWeights()
        {
            var randValue = 0;
            var idList = rewardOptionConfig.ValueList;
            var weightList = rewardOptionConfig.WeightList;

            //如果权重没有填写 那么就是均等
            if (null == weightList || 0 == weightList.Count)
            {
                weightList = new List<int>();
                for (int i = 0; i < idList.Count; i++)
                {
                    weightList.Add(1);
                }
            }


            List<int> filterIdList = new List<int>();
            List<int> filterWeightList = new List<int>();
            //实体已经有的技能不能再随机出来
            // var skills = player.entity.GetAllSkills();

            for (int i = 0; i < idList.Count; i++)
            {
                var skillId = idList[i];
                var weight = weightList[i];
                // if (!skills.ContainsKey(skillId))
                {
                    filterIdList.Add(skillId);
                    filterWeightList.Add(weight);
                }
            }

            //Battle_Log.Log("BattleBox : CalculateRealityReward : filterIdList.count : " + filterIdList.Count);

            if (filterIdList.Count > 0)
            {
                var rand = this.player.entity.GetBattle().rand;
                var index = BattleRandom.GetNextIndexByWeights(filterWeightList, rand);
                randValue = filterIdList[index];

                //Battle_Log.Log("BattleBox : CalculateRealityReward : skillConfigId : " + skillConfigId);
            }

            return randValue;
        }

        public bool CheckGainTiming(BattleRewardEffectGainTimingType timingType)
        {
            var needGainTimingType = (BattleRewardEffectGainTimingType)this.rewardOptionConfig.GainTimingType;
            return timingType == needGainTimingType;
        }


        public virtual void ApplyToNewTeamMembers(BattleEntity newTeamMember)
        {
        }

        // public void OnWaveReadyProcessStart()
        // {
        //     if (CheckGainTiming(BattleRewardEffectGainTimingType.OnReadyProcessStart))
        //     {
        //         this.GainRewardEffect(this.player);
        //     }
        // }
        //
        // public void OnWaveBattleProcessStart()
        // {
        //     if (CheckGainTiming(BattleRewardEffectGainTimingType.OnBattleProcessStart))
        //     {
        //         this.GainRewardEffect(this.player);
        //     }
        // }

        public void Release()
        {
            // battle.OnWaveReadyProcessStartAction -= OnWaveReadyProcessStart;
            // battle.OnWaveBattleProcessStartAction -= OnWaveBattleProcessStart;
        }
    }
}