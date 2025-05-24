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
    //按照固定给出的技能组 随机获得
    public class RewardEffect_Skill_Gain : BaseBattleRewardEffectOption
    {
        //最终结果奖励
        private int skillConfigId;

        public override void CalculateRealityReward()
        {
            // var idList = rewardConfig.ValueList;
            // var weightList = rewardConfig.WeightList;
            //
            // List<int> filterIdList = new List<int>();
            // List<int> filterWeightList = new List<int>();
            // //实体已经有的技能不能再随机出来
            // // var skills = player.entity.GetAllSkills();
            //
            // for (int i = 0; i < idList.Count; i++)
            // {
            //     var skillId = idList[i];
            //     var weight = weightList[i];
            //     // if (!skills.ContainsKey(skillId))
            //     {
            //         filterIdList.Add(skillId);
            //         filterWeightList.Add(weight);
            //     }
            // }
            //
            // Battle_Log.Log("BattleBox : CalculateRealityReward : filterIdList.count : " + filterIdList.Count);
            //
            // if (filterIdList.Count > 0)
            // {
            //     var rand = this.player.entity.GetBattle().rand;
            //     var index = BattleRandom.GetNextIndexByWeights(filterWeightList,rand);
            //     skillConfigId = filterIdList[index];
            //     
            //     Battle_Log.Log("BattleBox : CalculateRealityReward : skillConfigId : " + skillConfigId);
            // }

            skillConfigId = GetRandValueByWeights();

        }

        public override void OnGain(BattlePlayer gainer)
        {
            //根据 skillConfigId 获得实际技能奖励

            // Skill skill = new Skill();
            // skill.configId = skillConfigId;
            // skill.level = 1;

            var ret = gainer.entity.AddSkill(new CreateSkillBean()
            {
                releaser = gainer.entity,
                configId = skillConfigId,
                
            });

            if (ret.type == ResultCodeType.AddLeaderSkillFull)
            {
                //send msg to client : full
                this.battle.OnSyncReplaceSkillResult(gainer.playerIndex,ret);
            }
        }

        // public override List<int> GetIntValueList()
        // {
        //     return new List<int>()
        //         { skillConfigId };
        // }

        public override BattleRewardEffectDTO GetValues()
        {
            var dto = base.GetValues();
            dto.intArg1 = skillConfigId;
            return dto;
        }
    }
}