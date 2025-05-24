using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    public class PlayerAI : BaseAI
    {
        private float autoAttackTimer = 0f;
        private float autoAttackInterval = 1.5f; // 自动攻击间隔时间，将由技能CD决定
        private float noOperateTime = 0f;
        private float noOperateThreshold = 0.5f; // 玩家多久没操作才开始自动打怪
        
        // // 自动攻击检测范围
        // private float autoAttackDetectRange = 15.0f; // 自动攻击的检测范围
        
        // 当前攻击目标
        private int currentTargetGuid = -1;
        
        // 记录上一帧玩家是否有操作
        private bool previousHasOperation = false;
        
        public override void OnInit()
        {
            base.OnInit();
            isAutoAttack = true; // 默认开启自动攻击

        }


        public override void OnSetAISkillInfo()
        {
            normalAttackSkill = entity.GetNormalAttackSkill();
        }
         private Skill normalAttackSkill;
        
        public override void OnUpdate(float timeDelta)
        {
            base.OnUpdate(timeDelta);
            
            if (!isAutoAttack || entity == null || entity.IsDead())
            {
                return;
            }
            
            // 检查玩家是否有操作
            bool hasOperation = entity.IsHaveOperate();
            
            // 检测玩家是否刚从"有操作"变为"无操作"状态
            if (previousHasOperation && !hasOperation)
            {
                // 玩家刚操作完成，清除当前目标，以便重新选择最近的敌人
                currentTargetGuid = -1;
            }
            
            // 更新上一帧的操作状态
            previousHasOperation = hasOperation;
            
            if (hasOperation)
            {
                // 玩家有操作，重置无操作计时器
                noOperateTime = 0f;
            }
            else
            {
                // 玩家没有操作，累加无操作时间
                noOperateTime += timeDelta;
                
                // 如果玩家一段时间没有操作，开始自动打怪
                if (noOperateTime >= noOperateThreshold)
                {
                    // 获取普通攻击技能
                    var normalAttackSkill = this.normalAttackSkill;;
                    
                    if (normalAttackSkill != null && normalAttackSkill.state != ReleaseSkillState.CD)
                    {
                        AutoAttackNearestMonster(normalAttackSkill.configId);
                    }
                }
            }
        }


        // 自动攻击最近的怪物
        private void AutoAttackNearestMonster(int attackSkillId)
        {
            if (entity == null || entity.IsDead())
            {
                return;
            }
            
            // 获取战场中所有实体
            var allEntities = entity.GetBattle().GetAllEntities();
            
            if (allEntities.Count == 0)
            {
                return;
            }
            
            // 如果有当前目标，先检查它是否仍然有效
            BattleEntity currentTarget = null;
            if (currentTargetGuid > 0)
            {
                currentTarget = entity.GetBattle().FindEntity(currentTargetGuid);
                
                // 检查目标是否失效（死亡、超出范围等）
                if (currentTarget == null || currentTarget.IsDead())
                {
                    currentTargetGuid = -1;
                    currentTarget = null;
                }
                else
                {
                    // 检查目标是否超出范围
                    
                    float distance = Vector3.Distance(currentTarget.position, entity.position);
                    var attackRange = this.normalAttackSkill?.GetReleaseRange();
                    if (distance > attackRange)
                    {
                        currentTargetGuid = -1;
                        currentTarget = null;
                    }
                }
            }
            
            // 如果没有有效的当前目标，找一个新目标
            if (currentTarget == null)
            {
                // 筛选出敌方单位（非玩家控制的，且活着的，且在检测范围内的）
                List<BattleEntity> enemies = new List<BattleEntity>();
                foreach (var pair in allEntities)
                {
                    var e = pair.Value;
                    if (e == entity || e.IsDead())
                    {
                        continue;
                    }
                    
                    // 使用GetRelationWith获取实体关系，判断是否为敌人
                    var relations = entity.GetRelationWith(e);
                    bool isEnemy = false;
                    
                    foreach (var relationType in relations)
                    {
                        if (relationType == EntityRelationType.Enemy)
                        {
                            isEnemy = true;
                            break;
                        }
                    }
                    
                    if (isEnemy)
                    {
                        // 检查是否在检测范围内
                        float distance = Vector3.Distance(e.position, entity.position);
                        var attackRange = this.normalAttackSkill?.GetReleaseRange();
                        if (distance <= attackRange)
                        {
                            enemies.Add(e);
                        }
                    }
                }
                
                if (enemies.Count == 0)
                {
                    return; // 没有在范围内的敌人
                }
                
                // 找出最近的敌人
                BattleEntity nearestEnemy = null;
                float minDistance = float.MaxValue;
                
                foreach (var enemy in enemies)
                {
                    float distance = Vector3.Distance(enemy.position, entity.position);
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        nearestEnemy = enemy;
                    }
                }
                
                if (nearestEnemy != null)
                {
                    currentTarget = nearestEnemy;
                    currentTargetGuid = nearestEnemy.guid;
                }
            }
            
            // 攻击当前目标
            if (currentTarget != null)
            {
                // 使用实体的AskReleaseSkill方法来释放技能
                // 这个方法会自动处理移动和攻击的逻辑
                entity.AskReleaseSkill(attackSkillId, currentTarget.guid, currentTarget.position, 
                    entity.position, true);
            }
        }
    }
}


