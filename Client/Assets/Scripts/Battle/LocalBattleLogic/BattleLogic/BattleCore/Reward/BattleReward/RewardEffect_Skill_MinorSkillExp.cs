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
    //随机给小招经验
    public class RewardEffect_Skill_MinorSkillExp : BaseBattleRewardEffectOption
    {
        //技能经验
        private int skillExp;

        public override void CalculateRealityReward()
        {
            skillExp = GetRandValueByWeights();
        }

        public override void OnGain(BattlePlayer gainer)
        {
            var skill = gainer.entity.FindMinorSkill();
            if (skill != null)
            {
                BattleLog.LogZxy( $"add minor skill exp : the skillExp is {skillExp} , skillId is {skill.configId}");
                gainer.entity.AddExpToSkill(skill.configId,skillExp);
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