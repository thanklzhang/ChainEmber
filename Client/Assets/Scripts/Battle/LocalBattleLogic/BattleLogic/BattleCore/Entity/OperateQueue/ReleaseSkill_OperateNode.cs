using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battle
{
    public class ReleaseSkill_OperateNode : OperateNode
    {
        public ReleaseSkillBean releaseSkill;

        protected override void OnExecute()
        {
            TryToRelaseSkill();
        }

        protected override void OnUpdate(float deltaTime)
        {
            if (releaseSkill.targetGuid > 0)
            {
                //判断目标是否符合技能的目标单位关系类型
                if (!Skill.CheckSkillTargetRelation(this.entity, this.entity.FindSkillByConfigId(releaseSkill.skillId), releaseSkill.targetGuid))
                {
                    this.operateModule.OnNodeExecuteFinish(releaseSkill.skillId);
                    return;
                }
                
                //如果目标是单位的话
                
                //缺失目标后直接结束技能
                var findEntity = this.battle.FindEntity(releaseSkill.targetGuid);
                if (null == findEntity)
                {
                    this.operateModule.OnNodeExecuteFinish(releaseSkill.skillId);
                    return;
                }
            }

            TryToRelaseSkill();

        }

        bool TryToRelaseSkill()
        {
            var isSuccess = this.entity.ReleaseSkill(releaseSkill.skillId, releaseSkill.targetGuid,
                releaseSkill.targetPos,releaseSkill.mousePos);
            // if (isSuccess)
            // {
            //     //释放成功直接结束
            //     this.operateModule.OnNodeExecuteFinish(releaseSkill.skillId);
            // }

            return isSuccess;
        }

        public override int GenKey()
        {
            return releaseSkill.skillId;
        }

        public override bool IsCanBeBreak()
        {
            var skill = this.entity.FindSkillByConfigId(releaseSkill.skillId);
            //skill.state == RelaseSkillState.ReadyRelease
            //这里去掉 ReadyRelease 条件 ， 因为可能在一帧中 A 技能在队列的第一个，但没有开始，正要释放 ， 但是 B 技能再这一帧也要释放 ， A 就会被覆盖
            //return skill.state == RelaseSkillState.CD;
            
            return skill.state == ReleaseSkillState.ReadyRelease || skill.state == ReleaseSkillState.CD;
        }
    }

    public class ReleaseSkillBean
    {
        public int skillId;
        public int targetGuid;
        public Vector3 targetPos;
        public Vector3 mousePos;
    }



}
