using System.Collections.Generic;

namespace Battle
{
    public partial class Skill
    {
        float GetSkillBeforeTotalTime()
        {
            var scale = 1.0f;
            if (this.isNormalAttack)
            {
                var attackSpeed = this.releaser.AttackSpeed;
                if (0 == attackSpeed)
                {
                    attackSpeed = 1;
                }

                scale = 1.0f / attackSpeed;
            }

            var result = this.infoConfig.BeforeTime * scale / 1000.0f;
            return result;
        }

        float GetSkillAfterTotalTime()
        {
            var scale = 1.0f;
            if (this.isNormalAttack)
            {
                var attackSpeed = this.releaser.AttackSpeed;
                if (0 == attackSpeed)
                {
                    attackSpeed = 1;
                }

                scale = 1.0f / attackSpeed;
            }

            var rsult = this.infoConfig.AfterTime * scale / 1000.0f;
            return rsult;
        }


        public float GetReleaseRange()
        {
            if (this.isNormalAttack)
            {
                return this.releaser.AttackRange;
            }
            else
            {
                return this.infoConfig.ReleaseRange / 1000.0f;
            }
        }

        // //CD 计时器总时间 （总时间 - 前摇 - 后摇） ， 供逻辑用
        // float GetCDTimerTotalTime()
        // {
        //     var currCDTimer = 0.0f;
        //     if (this.isNormalAttack)
        //     {
        //         if (0 == this.releser.AttackSpeed)
        //         {
        //             return 0.01f;
        //         }
        //
        //         //普通攻击需要保证 1 s 内输出固定几次 , 所以按照配置 - 前摇 - 后摇
        //         var cdMax = 1.0f / this.releser.AttackSpeed;
        //         var resultCD = cdMax - this.GetSkillBeforeTotalTime() - this.GetSkillAfterTotalTime();
        //         if (resultCD < 0)
        //         {
        //             resultCD = 0;
        //         }
        //
        //         currCDTimer = resultCD;
        //     }
        //     else
        //     {
        //         currCDTimer = this.infoConfig.CdTime / 1000.0f - this.GetSkillBeforeTotalTime() -
        //                       this.GetSkillAfterTotalTime();
        //     }
        //
        //     return currCDTimer;
        // }

        //CD 时长
        public float GetCDTotalTime()
        {
            if (this.isNormalAttack)
            {
                //普通攻击需要保证 1 s 内输出固定几次 , 所以按照配置 - 前摇 - 后摇

                if (0 == this.releaser.AttackSpeed)
                {
                    return 0.01f;
                }

                var cdMax = 1.0f / this.releaser.AttackSpeed;
                var resultCD = cdMax - this.GetSkillBeforeTotalTime() -
                               this.GetSkillAfterTotalTime();
                if (resultCD < 0)
                {
                    resultCD = 0;
                }

                return resultCD;
            }
            else
            {
                var ratio = 0.0f;

           
                if (IsCanReduceSkillCD())
                {
                    var skillCDAttr = this.releaser.SkillCD;
                    var factor = 100.0f;
                    ratio = (skillCDAttr / (skillCDAttr + factor));
                    var max = maxSkillCDRatio;
                    if (ratio >= max)
                    {
                        ratio = max;
                    }
                }

                var resultCD = this.infoConfig.CdTime * (1 - ratio) / 1000.0f;
                return resultCD;
            }
        }

        public bool IsCanReduceSkillCD()
        {
            var category = (SkillCategory)this.infoConfig.SkillCategory;
            if (category == SkillCategory.LeaderSkill
                || category == SkillCategory.MinorSkill
                || category == SkillCategory.UltimateSkill
                || category == SkillCategory.TalentSkill)
            {
                return true;
            }

            return false;
        }

        // //CD 时长 （总时间 - 前摇） , 不是真正的 CD 时间 ， 因为包括了后摇的时间 ， 这里待定
        // public float GetCDMaxTime()
        // {
        //     if (this.isNormalAttack)
        //     {
        //         if (0 == this.releser.AttackSpeed)
        //         {
        //             return 0.01f;
        //         }
        //
        //         var cdMax = 1.0f / this.releser.AttackSpeed;
        //         var resultCD = cdMax - this.GetSkillBeforeTotalTime();
        //         if (resultCD < 0)
        //         {
        //             resultCD = 0;
        //         }
        //
        //         return resultCD;
        //     }
        //     else
        //     {
        //         //技能只减去前摇
        //         var resultCD = this.infoConfig.CdTime / 1000.0f - this.GetSkillBeforeTotalTime();
        //         return resultCD;
        //     }
        // }


        public float GetCurrCDTimer()
        {
            return this.currCDTimer;
        }

        internal Battle GetBattle()
        {
            return releaser.GetBattle();
        }

        public bool IsReadyRelease()
        {
            return this.state == ReleaseSkillState.ReadyRelease;
        }
    }
}