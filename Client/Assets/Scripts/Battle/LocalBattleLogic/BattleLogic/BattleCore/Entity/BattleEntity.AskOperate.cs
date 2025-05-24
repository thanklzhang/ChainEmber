using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;



namespace Battle
{
    public partial class BattleEntity
    {
        OperateModule operateModule;

        public void InitOperateModule()
        {
            operateModule = new OperateModule();
            operateModule.Init(this);
        }

        //请求移动
        public virtual void AskMoveToPos(Vector3 targetPos)
        {
            Move_OperateNode move = new Move_OperateNode()
            {
                moveTargetPos = targetPos,
                type = OperateType.Move
            };
            operateModule.AddOperate(move);
            
            // ai.AskMoveToPos(targetPos);
        }

        //请求释放技能
        public virtual void AskReleaseSkill(int skillId, int targetGuid, Vector3 targetPos,Vector3 mousePos,
            bool isAutoNormalAttack = false)
        {
            
            // var isInRange = this.IsInSkillReleaseRange(skillId, targetGuid, targetPos);
            
            //先判断自身的状态是否能够释放技能
            // var isCanReleaseSkill = this.IsCanReleaseSkill();
            //判断释放的技能是否在释放范围内
            var isInRange = Skill.IsInSkillReleaseRange(this,skillId, targetGuid, targetPos);

            if (!isInRange)//!isCanReleaseSkill ||
            {
                var currSkill = this.FindSkillByConfigId(skillId);
                var range = currSkill.GetReleaseRange();

                //add move
                Move_OperateNode move = new Move_OperateNode()
                {
                    moveTargetPos = targetPos,
                    moveFollowTargetGuid = targetGuid,
                    type = OperateType.Move,
                    finishDistance = range
                };

                //add skill
                var skillBean = new ReleaseSkillBean()
                {
                    targetGuid = targetGuid,
                    targetPos = targetPos,
                    skillId = skillId,
                    mousePos = mousePos
                };

                ReleaseSkill_OperateNode skill = new ReleaseSkill_OperateNode()
                {
                    releaseSkill = skillBean,
                    type = OperateType.ReleaseSkill
                };

                operateModule.AddOperate(new List<OperateNode>() { move, skill },isAutoNormalAttack);
            }
            else
            {
                ReleaseSkill_OperateNode skill = new ReleaseSkill_OperateNode()
                {
                    releaseSkill = new ReleaseSkillBean()
                    {
                        targetGuid = targetGuid,
                        targetPos = targetPos,
                        skillId = skillId, 
                        mousePos = mousePos
                    },
                    type = OperateType.ReleaseSkill
                };
                operateModule.AddOperate(skill,isAutoNormalAttack);
            }
            
            // ai.AskReleaseSkill(skillId, targetGuid, targetPos);
        }

        //请求使用道具
        public virtual void AskUseItem(Battle_ItemUseArg arg)
        {
            this.UseItem(arg);
        }

        //请求使用技能道具
        public virtual void AskUseSkillItem(Battle_ItemUseArg arg)
        {
            // this.UseSkillItem(arg);
        }

        //请求打开宝箱
        public virtual void AskOpenBox(Battle_OpenBoxArg arg)
        {
            //this.OpenBox(arg.quality);
        }

        //请求选择宝箱奖励
        public virtual void AskSelectBoxReward(Battle_SelectBoxRewardArg arg)
        {
            //this.SelectBoxReward(arg.quality,arg.index);  
        }

        public void UpdateAskOperate(float deltaTime)
        {
            this.operateModule.Update(deltaTime);
        }

        public bool IsHaveOperate()
        {
            return this.operateModule.IsHaveOperate();
        }
    }
}