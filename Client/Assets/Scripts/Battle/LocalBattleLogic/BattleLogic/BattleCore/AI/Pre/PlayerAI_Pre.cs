// using System.Collections.Generic;
//
// namespace Battle
// {
//     //玩家 AI
//     //1 ：技能释放距离不够时候可以自动移动
//     //2 ：在攻击范围内可以自动普通攻击
//     //3 : 普通攻击可以一直追击目标 TODO:增加个巡逻范围 超过了就不追了
//     public class PlayerAI : BaseAI
//     {
//         //OperateModule operateModule;
//
//         public override void OnInit()
//         {
//             // operateModule = new OperateModule();
//             // operateModule.Init(null);
//             //
//             // operateModule.InitEntity(this.entity);
//         }
//
//         //玩家请求移动
//         public override void AskMoveToPos(Vector3 targetPos)
//         {
//             // Move_OperateNode move = new Move_OperateNode()
//             // {
//             //     moveTargetPos = targetPos,
//             //     type = OperateType.Move
//             // };
//             // //operateModule.AddOperate(move);
//
//         }
//
//         public override void AskReleaseSkill(int skillId, int targetGuid, Vector3 targetPos)
//         {
//          
//             // var isInRange = this.entity.IsInSkillReleaseRange(skillId, targetGuid, targetPos);
//             //
//             // if (!isInRange)
//             // {
//             //     var currSkill = this.entity.FindSkillByConfigId(skillId);
//             //     var range = currSkill.GetReleaseRange();
//             //
//             //     //add move
//             //     Move_OperateNode move = new Move_OperateNode()
//             //     {
//             //         moveTargetPos = targetPos,
//             //         moveFollowTargetGuid = targetGuid,
//             //         type = OperateType.Move,
//             //         finishDistance = range
//             //     };
//             //
//             //     //add skill
//             //     var skillBean = new ReleaseSkillBean()
//             //     {
//             //         targetGuid = targetGuid,
//             //         targetPos = targetPos,
//             //         skillId = skillId
//             //     };
//             //
//             //     ReleaseSkill_OperateNode skill = new ReleaseSkill_OperateNode()
//             //     {
//             //         releaseSkill = skillBean,
//             //         type = OperateType.ReleaseSkill
//             //     };
//             //
//             //     operateModule.AddOperate(new List<OperateNode>() { move, skill });
//             // }
//             // else
//             // {
//             //     ReleaseSkill_OperateNode skill = new ReleaseSkill_OperateNode()
//             //     {
//             //         releaseSkill = new ReleaseSkillBean()
//             //         {
//             //             targetGuid = targetGuid,
//             //             targetPos = targetPos,
//             //             skillId = skillId
//             //         },
//             //         type = OperateType.ReleaseSkill
//             //     };
//             //     operateModule.AddOperate(skill);
//             // }
//         }
//
//         public override void AskUseItem(Battle_ItemUseArg arg)
//         {
//             //道具目前不走操作列表 直接使用
//             this.entity.UseItem(arg);
//         }
//         
//         public override void AskUseSkillItem(Battle_ItemUseArg arg)
//         {
//             // this.entity.UseSkillItem(arg);
//         }
//
//         // public override void OnFinishAllMovePos()
//         // {
//         //     this.operateModule.OnNodeExecuteFinish((int)OperateKey.Move);
//         // }
//         //
//         // public override void OnFinishSkillEffect(Skill skill)
//         // {
//         //     this.operateModule.OnNodeExecuteFinish(skill.configId);
//         //
//         // }
//
//         public override void AskOpenBox(Battle_OpenBoxArg arg)
//         {
//             //this.entity.OpenBox(arg.quality);
//         }
//
//         public override void AskSelectBoxReward(Battle_SelectBoxRewardArg arg)
//         {
//             //this.entity.SelectBoxReward(arg.quality,arg.index);  
//         }
//
//         public override void OnUpdate(float timeDelta)
//         {
//             // this.operateModule.Update(timeDelta);
//
//             //没有任何操作的情况下 检测自动攻击
//             // var isHaveOperate = this.operateModule.IsHaveOperate();
//             // if (!isHaveOperate)
//             // {
//             //     if (this.entity.EntityState == EntityState.Idle)
//             //     {
//             //         //判断是否周围有在攻击范围内的的敌方单位
//             //         var battle = entity.GetBattle();
//             //         BattleEntity nearestEntity = battle.FindNearestEnemyEntity(this.entity);
//             //
//             //         if (nearestEntity != null)
//             //         {
//             //             var normalAttackSkill = this.entity.GetNormalAttackSkill();
//             //             var sqrtDis = Vector3.SqrtDistance(nearestEntity.position, entity.position);
//             //             var attackRange = normalAttackSkill.GetReleaseRange();
//             //             if (sqrtDis <= attackRange * attackRange)
//             //             {
//             //                 var targetEntity = nearestEntity;
//             //                 var normalSkillConfigId = this.entity.GetNormalAttackSkill().configId;
//             //                 //this.entity.ReleaseSkill(normalSkillConfigId, targetEntity.guid, Vector3.one);
//             //                 AskReleaseSkill(normalSkillConfigId, targetEntity.guid, Vector3.one);
//             //             }
//             //         }
//             //     }
//             //
//             // }
//
//         }
//     }
// }
//
//
