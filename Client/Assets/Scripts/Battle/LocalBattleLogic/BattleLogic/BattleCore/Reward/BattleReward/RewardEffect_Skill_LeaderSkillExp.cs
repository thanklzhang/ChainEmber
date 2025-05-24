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
    //随机给队长技能经验
    public class RewardEffect_Skill_LeaderSkillExp : BaseBattleRewardEffectOption
    {
        //经验
        private int skillExp;

        public override void CalculateRealityReward()
        {
            skillExp = GetRandValueByWeights();
        }

        public override void OnGain(BattlePlayer gainer)
        {
            var skills = gainer.entity.FindLeaderSkills();
            if (skills != null && skills.Count > 0)
            {
                var rand = this.player.entity.GetBattle().rand;
                
                var randIndex = BattleRandom.Next(0, skills.Count,rand);

                var randSkill = skills[randIndex];
                if (randSkill != null)
                {
                    gainer.entity.AddExpToSkill(randSkill.configId,skillExp);
                }   
            }
        }

        public override BattleRewardEffectDTO GetValues()
        {
            var dto = base.GetValues();
            dto.intArg1 = skillExp;
            return dto;
        }
    }
}