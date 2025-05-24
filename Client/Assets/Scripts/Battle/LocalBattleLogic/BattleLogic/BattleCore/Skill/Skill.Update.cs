using System.Collections.Generic;

namespace Battle
{
    public partial class Skill
    {
        public void UpdateSkillProcess(float timeDelta)
        {
            var releaseType = (SkillReleaseType)this.infoConfig.SkillReleaseType;

            //CD
            if (state == ReleaseSkillState.CD)
            {
                currCDTimer -= timeDelta;
                if (currCDTimer <= 0)
                {
                    //currCDTimer = this.GetCDMaxTime();

                    OnCDEnd();
                    this.state = ReleaseSkillState.ReadyRelease;


                    return;
                }
            }

            if (state == ReleaseSkillState.SkillBefore)
            {
                currBeforeReleaseTimer -= timeDelta;
                if (currBeforeReleaseTimer <= 0)
                {
                    //前摇结束 释放技能效果
                    this.StartReleaseSkillEffect();

                    this.state = ReleaseSkillState.Releasing;

                    //currBeforeReleaseTimer = GetSkillBeforeTotalTime();
                    var isLastSkill = releaseType == SkillReleaseType.LastRelease;

                    if (isLastSkill)
                    {
                        //等待持续技能效果结束
                    }
                    else
                    {
                        //this.battle.OnSkillInfoUpdate(this);
                    }

                    return;
                }
            }

            if (state == ReleaseSkillState.Releasing)
            {
                //判断是否是持续技能 如果是持续技能 那么需要 effect 来进行结束该 releasing 状态

                // if (isNormalAttack && this.releaser.isPlayerCtrl)
                // {
                //     Logx.Log(LogxType.Zxy,"attack");
                // }

                var isLastSkill = releaseType == SkillReleaseType.LastRelease;
                if (isLastSkill)
                {
                    //等待持续技能效果结束
                }
                else
                {
                    OnChangeToSkillAfter();
                }
            }

            if (state == ReleaseSkillState.SkillAfter)
            {
                currAfterReleaseTimer -= timeDelta;
                if (currAfterReleaseTimer <= 0)
                {
                    //currAfterReleaseTimer = this.GetSkillAfterTotalTime();
                    this.FinishSkillRelease();
                    // this.currCDTimer = GetCDTimerTotalTime() + currAfterReleaseTimer;

                    this.currCDTimer = GetCDTotalTime();
                    return;
                }
            }
        }
    }
}