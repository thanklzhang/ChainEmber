// using System;
// using System.Collections.Generic;
//
// namespace Battle
// {
//     public class BaseAI
//     {
//         protected BattleEntity entity;
//
//         public bool isAutoAttack = true;
//
//         public void Init(BattleEntity entity)
//         {
//             this.entity = entity;
//
//             this.OnInit();
//         }
//
//         public virtual void OnInit()
//         {
//         }
//
//         public void SetAISkillInfo()
//         {
//             this.OnSetAISkillInfo();
//         }
//
//         public virtual void OnSetAISkillInfo()
//         {
//             
//         }
//
//         public void Update(float timeDelta)
//         {
//             CheckAutoAttack();
//             
//             this.OnUpdate(timeDelta);
//         }
//
//         public void CheckAutoAttack()
//         {
//             if (isAutoAttack)
//             {
//                 //没有任何操作的情况下 检测自动攻击
//                 var isHaveOperate = entity.IsHaveOperate();
//                 if (!isHaveOperate)
//                 {
//                     if (this.entity.EntityState == EntityState.Idle)
//                     {
//                         var battle = entity.GetBattle();
//                         //判断是否周围有在攻击范围内的的敌方单位
//                         BattleEntity nearestEntity = battle.FindNearestEnemyEntity(this.entity);
//
//                         if (nearestEntity != null)
//                         {
//                             var normalAttackSkill = this.entity.GetNormalAttackSkill();
//                             var sqrtDis = Vector3.SqrtDistance(nearestEntity.position, entity.position);
//                             var attackRange = normalAttackSkill.GetReleaseRange();
//                             if (sqrtDis <= attackRange * attackRange)
//                             {
//                                 var targetEntity = nearestEntity;
//                                 var normalSkillConfigId = this.entity.GetNormalAttackSkill().configId;
//                                 //this.entity.ReleaseSkill(normalSkillConfigId, targetEntity.guid, Vector3.one);
//                                 // AskReleaseSkill(normalSkillConfigId, targetEntity.guid, Vector3.one);
//                                 entity.AskReleaseSkill(normalSkillConfigId, targetEntity.guid, Vector3.one);
//                             }
//                         }
//                     }
//                 }
//             }
//         }
//
//         public virtual void OnUpdate(float timeDelta)
//         {
//         }
//
//         //请求移动
//         public virtual void AskMoveToPos(Vector3 targetPos)
//         {
//         }
//
//         public virtual void AskReleaseSkill(int skillId, int targetGuid, Vector3 targetPos)
//         {
//         }
//
//         public virtual void AskUseItem(Battle_ItemUseArg arg)
//         {
//         }
//
//         public virtual void AskUseSkillItem(Battle_ItemUseArg arg)
//         {
//         }
//
//         public virtual void AskOpenBox(Battle_OpenBoxArg arg)
//         {
//         }
//
//         public virtual void AskSelectBoxReward(Battle_SelectBoxRewardArg arg)
//         {
//         }
//
//
//         //
//
//
//         //entity 行为点------------------------
//
//         // //当实体到达当前的目标移动点的时候
//         // public virtual void OnMoveToCurrTargetPosFinish()
//         // {
//         //     //_Battle_Log.Log("find path test : BaseAI : OnMoveToCurrTargetPosFinish");
//         //     Vector3 nextStepPos;
//         //     if (this.pathFinder.TryToGetNextStepPos(out nextStepPos))
//         //     {
//         //         //_Battle_Log.Log("find path test : BaseAI : OnMoveToCurrTargetPosFinish : find pos , start move to : " + nextStepPos);
//         //         this.entity.MoveToPos(nextStepPos);
//         //     }
//         //     else
//         //     {
//         //         this.pathFinder.ClearPath();
//         //         this.entity.ChangeToIdle();
//         //         this.OnFinishAllMovePos();
//         //     }
//         // }
//         //
//         // public virtual void OnFinishAllMovePos()
//         // {
//         //
//         // }
//         //
//         // //当实体停止移动的时候
//         // public virtual void OnStopMove()
//         // {
//         //
//         // }
//         //
//         // //当实体状态变为 idle 的时候
//         // public virtual void OnChangeToIdle()
//         // {
//         //
//         // }
//         //
//         // //当实体的技能真正的释放出来的时候
//         // public virtual void OnFinishSkillEffect(Skill skill)
//         // {
//         //
//         // }
//
//
//         //------------------------
//
//         // //寻路相关---------------------------
//         // public List<Pos> GetCurrPathPosList()
//         // {
//         //     return pathFinder.GetCurrPosList();
//         // }
//         //
//         // //寻找路径并且开始移动
//         // public void FindPathAndMoveToPos(Vector3 targetPos)
//         // {
//         //     //_Battle_Log.Log("find path test : BaseAI : FindPathAndMoveToPos");
//         //
//         //     ////寻路
//         //     //Vector3 findPos;
//         //     //if (!pathFinder.FindPath(targetPos, out findPos))
//         //     //{
//         //     //    //_Battle_Log.Log(string.Format("find path test : BaseAI : StartToMoveToPos , not found path : targetPos : {0}", targetPos));
//         //     //    return;
//         //     //}
//         //
//         //     var pathList = pathFinder.FindPath(targetPos);
//         //
//         //     //_Battle_Log.Log(string.Format("find path test : BaseAI : findPos : " + "({0},{1})", findPos.x, findPos.z));
//         //
//         //     //this.entity.MoveToPos(findPos);
//         //     List<Vector3> vct3List = new List<Vector3>();
//         //     foreach (var pos in pathList)
//         //     {
//         //         Vector3 v = this.pathFinder.GetCenterPos(pos);
//         //         vct3List.Add(v);
//         //     }
//         //     this.entity.StartMoveByPath(vct3List);
//         //
//         // }
//         //
//         public virtual void OnBeHurt(float resultDamage, Skill fromSkill)
//         {
//         
//         }
//         //
//         // //根据当前路径的终点再进行寻路
//         // public void FindPathByCurrPath()
//         // {
//         //     var currPosList = this.pathFinder.GetCurrPosList();
//         //     if (currPosList.Count > 0)
//         //     {
//         //         var endPos = currPosList[currPosList.Count - 1];
//         //         var pos = this.pathFinder.GetCenterPos(endPos);
//         //         this.FindPathAndMoveToPos(pos);
//         //     }
//         // }
//         //
//         // //--------------------------------
//         //
//         // //释放技能时候因为距离不够等原因 需要移动到距离内
//     }
// }