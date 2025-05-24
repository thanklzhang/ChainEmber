// using System;
// using System.Collections.Generic;
//
// namespace Battle
// {
//     //电脑通用 AI
//     //1 : 一直跟随玩家
//     //2 ： 技能按照顺序释放
//     public class CpuAI2 : BaseAI
//     {
//         float timer;
//         BattleEntity targetEntity;
//         float maxCatchCDTime = 0.35f;
//         float catchCDTimer;
//
//         List<int> skillSequence;
//         int currSkillSeqIndex;
//
//         //仇恨值
//         Dictionary<int, HateObject> hateObjectDic = new Dictionary<int, HateObject>();
//         public float maxHateCDTime = 5;
//         public float currHateCDTime;
//
//         public override void OnInit()
//         {
//             catchCDTimer = maxCatchCDTime;
//           
//         }
//
//         public override void OnSetAISkillInfo()
//         {
//             //默认按照 id 排序
//             skillSequence = new List<int>();
//             var skill = this.entity.GetAllSkills();
//             foreach (var item in skill)
//             {
//                 var skillConfig = BattleConfigManager.Instance.GetById<ISkill>(item.Value.configId);
//                 // var skillCOnfig = this.BattleConfigManager.Instance.GetById<ISkillConfig>(item.Value.configId);
//                 if (skillConfig.IsPassiveSkill <= 0)
//                 {
//                     skillSequence.Add(item.Value.configId);
//                 }
//             }
//
//             skillSequence.Sort((a, b) => { return a > b ? 1 : 0; });
//
//             //普通攻击优先级最低
//             if (skillSequence.Count > 0)
//             {
//                 var normalAttackId = skillSequence[0];
//                 skillSequence.RemoveAt(0);
//                 skillSequence.Add(normalAttackId);
//                 currSkillSeqIndex = 0;
//             }
//             else
//             {
//                 Battle_Log.LogWarning("the entity hasnt normalAttack : id of entity : " + this.entity.infoConfig.Id);
//             }
//         }
//
//         void CheckHate(float timeDelta)
//         {
//             if (this.currHateCDTime <= 0)
//             {
//                 //检测憎恨值最高的单位
//                 int guid = 0;
//                 int maxValue = -1;
//                 HateObject maxHateObj = null;
//                 foreach (var item in this.hateObjectDic)
//                 {
//                     var hateObj = item.Value;
//                     if (hateObj.value >= maxValue)
//                     {
//                         maxValue = hateObj.value;
//                         maxHateObj = hateObj;
//                     }
//                 }
//
//                 if (maxHateObj != null)
//                 {
//                     //改变攻击对象
//                     //maxHateObj
//                     var battle = this.entity.GetBattle();
//                     var entity = battle.FindEntity(maxHateObj.entityGuid);
//                     if (entity != null)
//                     {
//                         targetEntity = entity;
//                     }
//                     else
//                     {
//                         //清除死亡单位或者无效的引用
//                         this.hateObjectDic.Remove(maxHateObj.entityGuid);
//                     }
//                 }
//             }
//             else
//             {
//                 this.currHateCDTime -= timeDelta;
//             }
//         }
//
//         public override void OnUpdate(float timeDelta)
//         {
//             if (null == targetEntity || targetEntity.IsDead())
//             {
//                 //TODO 找敌方单位
//                 //随即找一个玩家单位
//                 targetEntity = FindPlayerEntity();
//                 if (null == targetEntity)
//                 {
//                     //没有找到可攻击的玩家单位了
//                     return;
//                 }
//             }
//             else
//             {
//                 //一直追玩家单位
//
//                 //检测仇恨值 检测是否改变目标
//                 CheckHate(timeDelta);
//
//                 //根据当前序列索引来找到可以释放的技能
//                 if (skillSequence.Count > 0)
//                 {
//                     Skill findSkill = null;
//                     for (int i = 0; i < skillSequence.Count; ++i)
//                     {
//                         var currIndex = (currSkillSeqIndex + i) % skillSequence.Count;
//                         var skillId = skillSequence[currIndex];
//                         var skill = this.entity.FindSkillByConfigId(skillId);
//                         if (skill.IsReadyRelease())
//                         {
//                             findSkill = skill;
//                             break;
//                         }
//                     }
//
//                     if (findSkill != null)
//                     {
//                         //找到一个能释放的技能并开始释放
//                         var skillRange = 0.0f;
//                         var battle = this.entity.GetBattle();
//                         var skillConfig = BattleConfigManager.Instance.GetById<ISkill>(findSkill.configId);
//                         if ((SkillReleaseTargetType)skillConfig.SkillReleaseTargeType == SkillReleaseTargetType.Entity ||
//                             (SkillReleaseTargetType)skillConfig.SkillReleaseTargeType == SkillReleaseTargetType.Point)
//                         {
//                             skillRange = findSkill.GetReleaseRange();
//                         }
//                         else if ((SkillReleaseTargetType)skillConfig.SkillReleaseTargeType ==
//                                  SkillReleaseTargetType.NoTarget)
//                         {
//                             //先默认是 3
//                             skillRange = 3;
//                         }
//
//                         var sqrtDis = Vector3.SqrtDistance(targetEntity.position, entity.position);
//
//                         if (sqrtDis <= skillRange * skillRange)
//                         {
//                             //在释放距离里
//                             if ((SkillReleaseTargetType)skillConfig.SkillReleaseTargeType ==
//                                 SkillReleaseTargetType.Entity)
//                             {
//                                 // this.entity.ReleaseSkill(findSkill.configId, targetEntity.guid, targetEntity.position);
//
//                                 this.entity.AskReleaseSkill(findSkill.configId, targetEntity.guid,
//                                     targetEntity.position);
//                             }
//                             else if ((SkillReleaseTargetType)skillConfig.SkillReleaseTargeType ==
//                                      SkillReleaseTargetType.Point)
//                             {
//                                 // this.entity.ReleaseSkill(findSkill.configId, 0, targetEntity.position);
//                                 this.entity.AskReleaseSkill(findSkill.configId, 0, targetEntity.position);
//                             }
//                             else if ((SkillReleaseTargetType)skillConfig.SkillReleaseTargeType ==
//                                      SkillReleaseTargetType.NoTarget)
//                             {
//                                 // this.entity.ReleaseSkill(findSkill.configId, 0, targetEntity.position);
//
//                                 this.entity.AskReleaseSkill(findSkill.configId, 0, targetEntity.position);
//                             }
//
//
//                             //_Battle_Log.Log("this.entity.ReleaseSkill : " + this.entity.infoConfig.Name + " " + skillConfig.Name);
//                             currSkillSeqIndex = (currSkillSeqIndex + 1) % skillSequence.Count;
//                         }
//                         else
//                         {
//                             // //没有在目标里 追寻目标 有冷却时间的追寻
//                             if (this.entity.EntityState == EntityState.Idle)
//                             {
//                                 // entity.FindPathAndMoveToPos(targetEntity.position);
//
//                                 entity.AskMoveToPos(targetEntity.position);
//                             }
//                             else
//                             {
//                                 // Battle_Log.Log(200,
//                                 //     $"{this.entity.guid} : ${this.entity.infoConfig.Name}" + this.entity.EntityState);
//                                 if (catchCDTimer <= 0)
//                                 {
//                                     // entity.FindPathAndMoveToPos(targetEntity.position);
//                                     //this.entity.StartMoveToPos(targetEntity.position);
//                                     entity.AskMoveToPos(targetEntity.position);
//                                     catchCDTimer = maxCatchCDTime;
//                                 }
//                                 else
//                                 {
//                                     catchCDTimer = catchCDTimer - timeDelta;
//                                 }
//                             }
//                         }
//                     }
//                 }
//                 else
//                 {
//                     //啥技能都没有 连普通攻击都没有
//                 }
//             }
//         }
//
//         BattleEntity FindPlayerEntity()
//         {
//             var battle = this.entity.GetBattle();
//             var allEntities = battle.GetAllEntities();
//             List<BattleEntity> playerEntities = new List<BattleEntity>();
//             foreach (var item in allEntities)
//             {
//                 var entity = item.Value;
//                 if (entity.playerIndex != this.entity.playerIndex &&
//                     entity.Team != this.entity.Team)
//                 {
//                     playerEntities.Add(entity);
//                 }
//             }
//
//             //这里应该是找最近的单位
//             if (playerEntities.Count > 0)
//             {
//                 BattleEntity nearestEntity = null;
//                 float nearestSqrDis = 9999999.0f;
//                 foreach (var entity in playerEntities)
//                 {
//                     var sqrDis = (entity.position - this.entity.position).sqrMagnitude;
//
//
//                     if (sqrDis <= nearestSqrDis)
//                     {
//                         nearestSqrDis = sqrDis;
//                         nearestEntity = entity;
//                     }
//                 }
//
//                 //int seekSeek = unchecked((int)DateTime.Now.Ticks);
//                 //Random r = new Random(seekSeek);
//                 //var rand = MyRandom.Next(0, playerEntities.Count - 1, r);
//                 //var randPlayerEntity = playerEntities[rand];
//
//                 UpdateHateEntity(nearestEntity.guid, 0);
//
//                 return nearestEntity;
//             }
//
//             return null;
//         }
//
//         public void UpdateHateEntity(int guid, int value)
//         {
//             if (!this.hateObjectDic.ContainsKey(guid))
//             {
//                 HateObject hate = new HateObject()
//                 {
//                     entityGuid = guid,
//                     priority = 1,
//                     value = value
//                 };
//                 hateObjectDic.Add(guid, hate);
//             }
//             else
//             {
//                 var hateObj = this.hateObjectDic[guid];
//                 hateObj.value = hateObj.value + value;
//             }
//         }
//
//         public override void OnBeHurt(float resultDamage, Skill fromSkill)
//         {
//             if (resultDamage > 0)
//             {
//                 //伤害
//                 UpdateHateEntity(fromSkill.releaser.guid, (int)resultDamage);
//             }
//         }
//     }
//
//     public class HateObject
//     {
//         public int entityGuid;
//         public int priority;
//         public int value;
//     }
// }