using System;
using System.Collections.Generic;
using System.Linq;
using Config;

namespace Battle
{
    public partial class CpuAI : BaseAI
    {
        // public AIAnalyseResult GetSkillPriority_Attack(Skill skill)
        // {
        //     
        //     
        //     var resultPri = 0.0f;
        //     BattleEntity expectTarget = null;
        //     Vector3 expectPos = Vector3.zero;
        //
        //     //是否是区域技能
        //     var areaId = Skill_Tool.GetKeyAreaEft(skill.configId);
        //     var isAreaSkill = areaId > 0;
        //
        //     var releaseTargetType = (SkillReleaseTargetType)skill.infoConfig.SkillReleaseTargeType;
        //     if (releaseTargetType == SkillReleaseTargetType.Entity)
        //     {
        //         var _info = GetSingleTargetEntityPriority(skill, SkillActionTendencyType.Attack);
        //         resultPri = _info.Item1;
        //         expectTarget = _info.Item2;
        //     }
        //     else if (releaseTargetType == SkillReleaseTargetType.Point)
        //     {
        //         if (isAreaSkill)
        //         {
        //             //区域技能
        //             var _info = GetAreaPriority(areaId, skill);
        //             resultPri = _info.Item1;
        //             expectPos = _info.Item2;
        //         }
        //         else
        //         {
        //             //点 先简单处理 ，应该是按照技能效果来区分 例如 投掷物需要计算飞行途径的敌人数量和生命值百分比等这种 待定吧
        //             //释放范围内所有单位点进行计算
        //             var _info = GetSinglePointPriority(skill);
        //             resultPri = _info.Item1;
        //             expectPos = _info.Item2;
        //         }
        //     }
        //     else if (releaseTargetType == SkillReleaseTargetType.NoTarget)
        //     {
        //         if (isAreaSkill)
        //         {
        //             //区域技能
        //             var _info = GetAreaPriority(areaId, skill);
        //             resultPri = _info.Item1;
        //             expectPos = _info.Item2;
        //         }
        //         else
        //         {
        //             //这种情况一般都是给自己加状态，或者是以自身为中心的技能
        //             //周围有怪就有可以释放
        //             var selectEntities = entity.GetBattle().GetEntitiesInCircleAtEntity(entity,
        //                 entity.position,
        //                 autoReleaseNoTargetSkillRange, EntityRelationFilterType.Enemy);
        //
        //             if (selectEntities.Count > 0)
        //             {
        //                 //有怪
        //                 resultPri = GetTotalSelectEntitiesPriority(selectEntities);
        //             }
        //         }
        //     }
        //
        //     var analyseResult = new AIAnalyseResult()
        //     {
        //         priority = resultPri,
        //         skill = skill,
        //         expectTarget = expectTarget,
        //         expectPos = expectPos,
        //         isEscape = false
        //     };
        //
        //     return analyseResult;
        // }


        //攻击倾向（攻击技能和普攻）
        // public AIAnalyseResult GetSkillPriority_Attack(Skill skill)
        // {
        //     BattleEntity expectTarget = null;
        //     Vector3 expectPos = Vector3.zero;
        //     List<SkillTagType> tags = skill.infoConfig.TagList.Select(t => (SkillTagType)t).ToList();
        //
        //     if (skill.isNormalAttack)
        //     {
        //         tags.Add(SkillTagType.Hurt);
        //     }
        //
        //     //选择目标筛选类型
        //     var selectEntityType = (EntityRelationFilterType)skill.infoConfig.EntityRelationFilterType;
        //
        //     //释放目标类型
        //     var releaseTargetType = (SkillReleaseTargetType)skill.infoConfig.SkillReleaseTargeType;
        //
        //     //技能效果目标类型
        //     var effectTarget = (SkillEffectTargetType)skill.infoConfig.SkillEffectTargetType;
        //
        //     //是否是区域技能
        //     var areaId = Skill_Tool.GetKeyAreaEft(skill.configId);
        //     bool isAreaSkill = areaId > 0;
        //     bool isAreaPointSkill = areaId > 0 && releaseTargetType == SkillReleaseTargetType.Point;
        //
        //     float attackPriority = 0;
        //     for (int i = 0; i < tags.Count; i++)
        //     {
        //         var tag = tags[i];
        //         if (!skill.isNormalAttack && skill.state != ReleaseSkillState.ReadyRelease)
        //         {
        //             //技能的不能释放就跳过
        //             attackPriority = 0.0f;
        //         }
        //         else
        //         {
        //             if (tag == SkillTagType.Hurt || tag == SkillTagType.DeBuff_Defence)
        //             {
        //                 if (selectEntityType == EntityRelationFilterType.Enemy ||
        //                     selectEntityType == EntityRelationFilterType.FriendAndEnemy ||
        //                     selectEntityType == EntityRelationFilterType.All)
        //                 {
        //                     //攻击向倾向
        //                     if (isAreaPointSkill)
        //                     {
        //                         var _info = GetAreaPriority_Hurt_Skill(areaId, skill);
        //                         attackPriority = _info.Item1;
        //                         expectPos = _info.Item2;
        //                     }
        //                     else if (releaseTargetType == SkillReleaseTargetType.Entity)
        //                     {
        //                         //单个实体
        //
        //                         var _info = GetSingleTargetEntityPriority_Hurt_Skill(skill);
        //                         attackPriority += _info.Item1;
        //                         expectTarget = _info.Item2;
        //                     }
        //                     else if (releaseTargetType == SkillReleaseTargetType.Point)
        //                     {
        //                         //点 先简单处理 ，应该是按照技能效果来区分 例如 投掷物需要计算飞行途径的敌人数量和生命值百分比等这种 待定吧
        //                         //释放范围内所有单位点进行计算
        //                         var _info = GetSinglePointPriority_Hurt_Skill(skill);
        //                         attackPriority += _info.Item1;
        //                         expectPos = _info.Item2;
        //                     }
        //                 }
        //                 else if (selectEntityType == EntityRelationFilterType.Self)
        //                 {
        //                     var battle = this.entity.GetBattle();
        //                     if (isAreaSkill)
        //                     {
        //                         //自己周围区域伤害
        //                         var selectEntities = battle.GetEntitiesInCircleAtEntity(skill.releaser,
        //                             skill.releaser.position,
        //                             skill.infoConfig.ReleaseRange / 1000.0f, selectEntityType);
        //
        //                         var total = GetTotalPriority_Attack(selectEntities, 6.0f, 2.0f);
        //
        //                         attackPriority = total;
        //                         expectPos = Vector3.zero;
        //                     }
        //                 }
        //             }
        //         }
        //     }
        //
        //     var analyseResult = new AIAnalyseResult()
        //     {
        //         priority = attackPriority,
        //         skill = skill,
        //         expectTarget = expectTarget,
        //         expectPos = expectPos
        //     };
        //
        //     return analyseResult;
        // }
    }
}