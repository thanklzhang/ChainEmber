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
    public enum BattleRewardEffectGainTimingType
    {
        //立即获得
        Immediately = 0,

        //准备阶段到来时
        OnReadyProcessStart = 1,

        //战斗阶段到来时
        OnBattleProcessStart = 2,
    }

    public class BattleRewardDTO
    {
        public int configId;
        public int guid;

        // public int intArg1;
        // public int intArg2;
        // public List<int> intListArg1;
        // public List<int> intListArg2;
        // public bool applyToNewTeamMembers;
        public List<BattleRewardEffectDTO> rewardEffectDTOs = new List<BattleRewardEffectDTO>();
    }

    public class BattleRewardEffectDTO
    {
        public int guid;
        //奖励的配置 id
        public int configId;
        
        public int intArg1;
        public int intArg2;
        public List<int> intListArg1;
        public List<int> intListArg2;
    }

    public class BaseBattleReward
    {
        // 奖励效果工厂字典 - 存储枚举类型到对应构造函数的映射
        private static readonly Dictionary<BattleRewardEffectType, Func<BaseBattleRewardEffectOption>> RewardEffectFactories = 
            new Dictionary<BattleRewardEffectType, Func<BaseBattleRewardEffectOption>>
            {
                { BattleRewardEffectType.Skill_Gain, () => new RewardEffect_Skill_Gain() },
                { BattleRewardEffectType.Item_Gain, () => new RewardEffect_Item_Gain() },
                { BattleRewardEffectType.Skill_MinorSkillExp, () => new RewardEffect_Skill_MinorSkillExp() },
                { BattleRewardEffectType.Skill_LeaderSkillExp, () => new RewardEffect_Skill_LeaderSkillExp() },
                { BattleRewardEffectType.Item_Copy, () => new RewardEffect_Item_Copy() },
                { BattleRewardEffectType.TeamMember_Gain, () => new RewardEffect_TeamMember_Gain() },
                { BattleRewardEffectType.TeamMember_TeammateAddRandAttr, () => new RewardEffect_TeamMember_TeammateAddRandAttr() },
                { BattleRewardEffectType.TeamMember_AllTeammateAddRandAttr, () => new RewardEffect_TeamMember_AllTeammateAddRandAttr() },
                { BattleRewardEffectType.TeamMember_RandAddStarExp, () => new RewardEffect_TeamMember_RandAddStarExp() },
                { BattleRewardEffectType.TeamMember_AllAddStarExp, () => new RewardEffect_TeamMember_AllAddStarExp() },
                { BattleRewardEffectType.TeamMember_RandAddBuff, () => new RewardEffect_TeamMember_RandAddBuff() },
                { BattleRewardEffectType.TeamMember_AllAddBuff, () => new RewardEffect_TeamMember_AllAddBuff() },
                { BattleRewardEffectType.Leader_RandAttr, () => new RewardEffect_Leader_LeaderRandAttr() },
                { BattleRewardEffectType.Leader_AddBuff, () => new RewardEffect_Leader_AddBuff() },
                { BattleRewardEffectType.Currency_BattleCoin, () => new RewardEffect_Currency_BattleCoin() },
                { BattleRewardEffectType.Currency_Population, () => new RewardEffect_Currency_Population() },
                { BattleRewardEffectType.Currency_AddCoinGainRate, () => new RewardEffect_Currency_AddCoinGainRate() },
                { BattleRewardEffectType.EnemyAddAttr, () => new RewardEffect_EnemyAddAttr() }
            };
            
        public IBattleReward rewardConfig;

        //选择者(不一定是获得者，但目前确实选择者就是获得者。。。)
        public BattlePlayer player;

        public int guid;


        public static int maxGuid = 1;


        public static int GenGuid()
        {
            //防止出界  但是还是不对 待定
            if (maxGuid >= int.MaxValue)
            {
                maxGuid = 1;
            }

            return maxGuid++;
        }

        public bool isWillDelete;
        public Battle battle;

        public List<BaseBattleRewardEffectOption> rewardEffectList = new List<BaseBattleRewardEffectOption>();

        public virtual void Init(IBattleReward rewardConfig, BattlePlayer player)
        {
            this.guid = GenGuid();
            this.rewardConfig = rewardConfig;
            this.player = player;
            // this.applyToNewTeamMembers = 1 == rewardConfig.ApplyToNewTeamMembers;

            battle = this.player.entity.GetBattle();

            battle.OnWaveReadyProcessStartAction += OnWaveReadyProcessStart;
            battle.OnWaveBattleProcessStartAction += OnWaveBattleProcessStart;

            var effectIds = rewardConfig.RewardEffectOptionIds;
            for (int i = 0; i < effectIds.Count; i++)
            {
                var effectId = effectIds[i];
                var config = BattleConfigManager.Instance.GetById<IBattleRewardEffectOption>(effectId);

                var type = (BattleRewardEffectType)config.Type;

                var effectOption = GetRewardEffectByType(type);
                effectOption.Init(config, this.player);

                rewardEffectList.Add(effectOption);
            }
        }

        public BaseBattleRewardEffectOption GetRewardEffectByType(BattleRewardEffectType type)
        {
            // 从工厂字典中获取对应类型的构造函数并创建实例
            if (RewardEffectFactories.TryGetValue(type, out var factory))
            {
                return factory();
            }
            
            // 如果找不到对应的类型，记录错误并返回null
            BattleLog.LogError($"未找到对应的奖励效果类型: {type}");
            return null;
        }


        bool IsTriggerEffectValid()
        {
            //判断该奖励是否在对应玩家已获得的奖励列表中
            // if (!this.player.CheckBattleRewardExist(this))
            // {
            //     return false;
            // }
            //
            // return true;

            return this.player.CheckBattleRewardExist(this);
        }

        public void ImmediatelyGainRewardEffect()
        {
            if (IsTriggerEffectValid())
            {
                this.GainRewardEffects(BattleRewardEffectGainTimingType.Immediately);
            }
        }

        void OnWaveReadyProcessStart()
        {
            if (IsTriggerEffectValid())
            {
                this.GainRewardEffects(BattleRewardEffectGainTimingType.OnReadyProcessStart);
            }
        }

        void OnWaveBattleProcessStart()
        {
            if (IsTriggerEffectValid())
            {
                foreach (var effect in rewardEffectList)
                {
                    effect.OnWaveReadyProcessStart();
                }
                this.GainRewardEffects(BattleRewardEffectGainTimingType.OnBattleProcessStart);
            }
        }

        void GainRewardEffects(BattleRewardEffectGainTimingType timeType)
        {
            foreach (var effect in rewardEffectList)
            {
                effect.GainRewardEffectsByTime(timeType);
            }
        }


        public void CalculateRealityReward()
        {
            foreach (var effect in rewardEffectList)
            {
                effect.CalculateRealityReward();
            }
        }

        public virtual BattleRewardDTO GetValues()
        {
            return new BattleRewardDTO()
            {
                guid = this.guid,
                configId = this.rewardConfig.Id,
                rewardEffectDTOs = rewardEffectList.Select(effect => effect.GetValues()).ToList(),
                //applyToNewTeamMembers = this.applyToNewTeamMembers
            };
        }
        
        
        
        // public bool CheckGainTiming(BattleRewardEffectGainTimingType timingType)
        // {
        //     //判断该奖励是否在对应玩家已获得的奖励列表中
        //     if (!this.player.CheckBattleRewardExist(this))
        //     {
        //         return false;
        //     }
        //     
        //
        //     var needGainTimingType = (BattleRewardEffectGainTimingType)this.rewardOptionConfig.GainTimingType;
        //     return timingType == needGainTimingType;
        // }

        // //获得相关数值参数 根据奖励类型不同意义也不同
        // public virtual BattleRewardDTO GetValues()
        // {
        //     return new BattleRewardDTO()
        //     {
        //         guid = this.guid,
        //         configId = this.rewardConfig.Id,
        //         applyToNewTeamMembers = this.applyToNewTeamMembers
        //     };
        // }

        // //获得 int 数值列表 供显示用
        // public virtual List<int> GetIntValueList()
        // {
        //     return new List<int>();
        // }

        // public int GetRandValueByWeights()
        // {
        //     var randValue = 0;
        //     var idList = rewardConfig.ValueList;
        //     var weightList = rewardConfig.WeightList;
        //
        //     //如果权重没有填写 那么就是均等
        //     if (null == weightList || 0 == weightList.Count)
        //     {
        //         weightList = new List<int>();
        //         for (int i = 0; i < idList.Count; i++)
        //         {
        //             weightList.Add(1);
        //         }
        //     }
        //
        //
        //     List<int> filterIdList = new List<int>();
        //     List<int> filterWeightList = new List<int>();
        //     //实体已经有的技能不能再随机出来
        //     // var skills = player.entity.GetAllSkills();
        //
        //     for (int i = 0; i < idList.Count; i++)
        //     {
        //         var skillId = idList[i];
        //         var weight = weightList[i];
        //         // if (!skills.ContainsKey(skillId))
        //         {
        //             filterIdList.Add(skillId);
        //             filterWeightList.Add(weight);
        //         }
        //     }
        //
        //     //Battle_Log.Log("BattleBox : CalculateRealityReward : filterIdList.count : " + filterIdList.Count);
        //
        //     if (filterIdList.Count > 0)
        //     {
        //         var rand = this.player.entity.GetBattle().rand;
        //         var index = BattleRandom.GetNextIndexByWeights(filterWeightList, rand);
        //         randValue = filterIdList[index];
        //
        //         //Battle_Log.Log("BattleBox : CalculateRealityReward : skillConfigId : " + skillConfigId);
        //     }
        //
        //     return randValue;
        // }


        public virtual void OnDiscard()
        {
        }

        public void Release()
        {
            battle.OnWaveReadyProcessStartAction -= OnWaveReadyProcessStart;
            battle.OnWaveBattleProcessStartAction -= OnWaveBattleProcessStart;
        }
    }
}