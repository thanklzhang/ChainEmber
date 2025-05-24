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
    public enum BattleRewardEffectType
    {
        // 100 固定技能组随机 （valueList：1,2） 表示 技能id组随机 weightList ： 权重
        //
        // 200 升级武器 （valueList：1,2）表示 升1级和 升2级 中随机 weightList ： 权重
        //
        // 300 获得装备 （valueList：1,2） 表示 1 2 级中随机 weightList ： 权重
        //
        // 400 获得队友 （valueList：1,2） 队友实体 id 组随机 weightList ： 权重
        // 401 随机一个队友获得属性加成 （valueList：1,2） 属性加成buff id 随机 weightList ： 权重
        // 402 所有队友获得属性加成
        //
        // 500 队长获得属性加成

        Skill_Gain = 100,
        Skill_MinorSkillExp = 101,
        Skill_LeaderSkillExp = 102,

        WeaponUpgrade = 200,

        Item_Gain = 300,
        Item_Copy = 301,

        TeamMember_Gain = 400,
        TeamMember_TeammateAddRandAttr = 401,
        TeamMember_AllTeammateAddRandAttr = 402,
        TeamMember_RandAddStarExp = 403,
        TeamMember_AllAddStarExp = 404,
        TeamMember_RandAddBuff = 405,
        TeamMember_AllAddBuff = 406,

        Leader_RandAttr = 500,
        Leader_AddBuff = 501,

        Currency_BattleCoin = 600,
        Currency_Population = 601,
        Currency_AddCoinGainRate = 602,
        
        // 700 战斗相关效果
        EnemyAddAttr = 700, // 敌人属性增强
    }

    public class RewardSelection
    {
        public BattlePlayer player;

        // private object config;

        public IBattleReward rewardConfig;

        //实际奖励
        public BaseBattleReward realityReward;

        public void Init(int rewardId, BattlePlayer player)
        {
            this.player = player;

            rewardConfig = BattleConfigManager.Instance.GetById<IBattleReward>(rewardId);

            // //生成确定奖励
            // GenRealityReward();
        }

        public void GenRealityReward()
        {
            // var type = rewardConfig.Type;
            //根据实际奖励类型来进行不同的奖励产出算法
            //可以用继承实现
            //BattleRewardType -> class
            // realityReward = GetRewardByType((BattleRewardEffectType)type);
            
            realityReward = new BaseBattleReward();
            realityReward.Init(rewardConfig, player);
            realityReward.CalculateRealityReward();
        }

     

        public BattleRewardDTO GetBattleRewardValues()
        {
            return this.realityReward.GetValues();
        }

        public void SetReward()
        {
        }

        //触发选项效果
        public void Trigger()
        {
            //获得相应的奖励
            var rewardId = this.rewardConfig.Id;
            var count = player.rewardRecorder != null ? player.rewardRecorder.GetRewardCount(rewardId) : 0;
            
            BattleLog.Log(114, $"触发奖励：ID={rewardId}，获取次数={count + 1}，最大次数={this.rewardConfig.MaxAcquireCount}");
            
            this.player.GainReward(realityReward);
        }
    }
}