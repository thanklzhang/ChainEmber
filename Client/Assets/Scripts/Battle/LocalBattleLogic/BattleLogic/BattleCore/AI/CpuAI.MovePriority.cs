using System;
using System.Collections.Generic;
using System.Linq;
using Config;

namespace Battle
{
    public partial class CpuAI : BaseAI
    {
        //获得移动逃跑的优先级
        public AIAnalyseResult GetMovePriority()
        {
            var brave = dimensionMgr.GetValue(CharacterDimensionType.Bravery);
            var love = dimensionMgr.GetValue(CharacterDimensionType.Compassion);
            var tenacity = dimensionMgr.GetValue(CharacterDimensionType.Tenacity);

            var escapeMaxP = 5.0f;

            if (null == this.recentlyDamageSourceEntity)
            {
                escapeMaxP = escapeMaxP * 0.5f;
            }
            
            var escapePriority = 0.0F;
            var expectPos = Vector3.zero;
            var hpRatio = this.entity.GetCurrHpRatio();

            if (recentlyDamageSourceEntity != null)
            {
                var reflectPos = MathTool.GetMidpointReflection(recentlyDamageSourceEntity.position,
                    this.entity.position);
                escapePriority += (1 - hpRatio) * escapeMaxP;
                expectPos = reflectPos;
            }
            else
            {
                if (this.entity.teamLeader != null)
                {
                    //往队长处跑
                    escapePriority += (1 - hpRatio) * escapeMaxP;
                    expectPos = this.entity.teamLeader.position;
                }
                else
                {
                    //这种情况下随机即可（也可以往人少敌方跑）
                    var rand = this.entity.GetBattle().rand;
                    var range = 3.00f;
                    var center = this.entity.position;
                    var endPos = MathTool.GetRandPosAroundCircle(rand,range,center);
                    
                    escapePriority += (1 - hpRatio) * escapeMaxP;
                    expectPos = endPos;
                }
            }

            escapePriority *= MathTool.Clamp(1 - brave, 0.0f, 1.0f);

            var dangerousRatio = 0.0f;
            if (recentlyDamageSourceEntity != null && 
                !recentlyDamageSourceEntity.IsDead())
            {
                dangerousRatio = 1 - recentlyDamageSourceTimer / recentlyDamageSourceTotalTime;
            }

            escapePriority *= dangerousRatio;
            
            var analyseResult = new AIAnalyseResult()
            {
                priority = escapePriority,
                // skill = skill,
                // expectTarget = expectTarget,
                expectPos = expectPos
            };

            return analyseResult;
        }

    }
}