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
    //随机给一个队友加Buff
    public class RewardEffect_TeamMember_RandAddBuff : BaseBattleRewardEffectOption
    {
        //最终结果奖励
        public int buffConfigId;
        public int randMemberGuid;

        // private int skillConfigId;
        // private List<AttrOption> attrs;

        public override void CalculateRealityReward()
        {
            buffConfigId = GetRandValueByWeights();

            // //根据配置创建属性
            // AttrHelper.AttrCreateArg arg = new AttrHelper.AttrCreateArg();
            // arg.guid = this.guid;
            // arg.attrGroupType = EntityAttrGroupType.BattleReward;
            // arg.attrGroupConfigId = this.attrGroupId;
            //
            // attrs = AttrHelper.GetAttrOptions(arg,
            //     this.player.entity.GetBattle());
        }

        public override void OnGain(BattlePlayer gainer)
        {
            // AttrHelper.AttrCreateArg arg = new AttrHelper.AttrCreateArg();
            // arg.guid = this.guid;
            // arg.attrGroupType = EntityAttrGroupType.BattleReward;
            // arg.attrGroupConfigId = this.attrGroupId;
            //
            // attrs = this.player.AddAttrToRandMemeber(arg);

            var memberList = this.player.entity.GetTeamMemberDic().Values.ToList();
            memberList.Add(this.player.entity);
            if (memberList.Count > 0)
            {
                var index = BattleRandom.GetRandInt(0, memberList.Count, this.player.entity.GetBattle());
                var randMember = memberList[index];

                randMemberGuid = randMember.guid;;
                
                SkillEffectContext context = new SkillEffectContext();
                context.battle = this.battle;
                context.fromSkill = null;
                context.selectEntities = new List<BattleEntity>();
                context.selectPositions = new List<Vector3>();

                context.selectEntities.Add(randMember);


                battle.AddSkillEffectGroup(new List<int>((buffConfigId)), context);
                
                
                
                // randMember.AddStarExp(starExp);


                // randMember.AddAttrs(EntityAttrGroupType.BattleReward, attrs);
            }


            // player.entity.AddAttrs(EntityAttrGroupType.BattleReward, attrs);
        }

        public override BattleRewardEffectDTO GetValues()
        {
            var dto = base.GetValues();
            dto.intArg1 = buffConfigId;
            dto.intArg2 = randMemberGuid;
            // dto.intListArg1 = attrs.Select(attr => AttrHelper.ToDtoAttrValue(
            //     attr.entityAttrType, attr.initValue)).ToList();
            return dto;
        }

        // public override List<int> GetIntValueList()
        // {
        //     return new List<int>()
        //         { attrGroupId };
        // }
    }
}