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
    //给队长加buff
    public class RewardEffect_Leader_AddBuff : BaseBattleRewardEffectOption
    {
        //最终结果奖励
        public int buffConfigId;

        public override void CalculateRealityReward()
        {
            buffConfigId = GetRandValueByWeights();
        }

        public override void OnGain(BattlePlayer gainer)
        {
            // 只给队长(玩家自己)添加buff
            var leaderEntity = gainer.entity;
            
            if (leaderEntity != null)
            {
                SkillEffectContext context = new SkillEffectContext();
                context.battle = this.battle;
                context.fromSkill = null;
                context.selectEntities = new List<BattleEntity> { leaderEntity };
                context.selectPositions = new List<Vector3>();

                battle.AddSkillEffectGroup(new List<int>() { buffConfigId }, context);
                
                BattleLog.LogZxy($"给队长添加buff效果: {leaderEntity.guid}, BuffID: {buffConfigId}");
            }
        }

        public override BattleRewardEffectDTO GetValues()
        {
            var dto = base.GetValues();
            dto.intArg1 = buffConfigId;
            return dto;
        }
    }
} 