using System.Collections.Generic;

namespace Battle
{
    public enum MoveProcessType
    {
        //正常移动
        Normal = 0,
        //瞬间移动
        Immediately = 1
    }

    public class MoveEffect : SkillEffect
    {
        public IMoveEffect tableConfig;
        Battle battle;
        Vector3 moveTargetPos;

        public override void OnInit()
        {
            battle = this.context.battle;
            tableConfig = BattleConfigManager.Instance.GetById<IMoveEffect>(this.configId);

            //是否以此效果结束为结束的标记
            var isForEndSkill = this.tableConfig.IsThisEndForSkillEnd;
            if (1 == isForEndSkill)
            {
                this.context.fromSkill.SetSubEndffect(this);
            }
        }

        public override CreateEffectInfo ToCreateEffectInfo()
        {
            var resId = tableConfig.EffectResId;
            var selectEntity = this.context.selectEntities[0];
            // var followEntityGuid = context.fromSkill.releaser.guid;
            var followEntityGuid = selectEntity.guid;

            CreateEffectInfo createInfo = new CreateEffectInfo();
            createInfo.guid = guid;
            createInfo.resId = resId;
            createInfo.effectPosType = EffectPosType.Custom_Pos;
            createInfo.createPos = Vector3.zero;

            createInfo.followEntityGuid = followEntityGuid;
            createInfo.isAutoDestroy = isAutoDestroy;

            return createInfo;
        }

        public override void OnStart()
        {
            base.OnStart();

            //_Battle_Log.Log("MoveEffect OnStart");

            //tableConfig = ConfigManager.Instance.GetById<Config.MoveEffect>(this.configId);

            var battle = this.context.battle;
            //默认为技能释放者
             var moveTargetEntity = this.context.selectEntities[0];
            // SkillEffectContext context = new SkillEffectContext()
            // {
            //     selectEntities = new List<BattleEntity>() { moveTargetEntity },
            //     battle = battle,
            //     fromSkill = this.context.fromSkill
            // };

            TriggerStartEffect();

            //默认是技能目标点
            moveTargetPos = this.context.fromSkill.targetPos;
            //默认是技能释放者(可拓展)
            //注意 这里技能释放者不一定是自身 有可能是 A 给 B 一个位移效果
            // var skillReleaser = this.context.fromSkill.releaser;
            var skillReleaser = this.context.fromSkill.releaser;
            var speed = this.tableConfig.MoveSpeed / 1000.0f;
            var lastTime = this.tableConfig.LastTime / 1000.0f;
            var dir = (moveTargetPos - moveTargetEntity.position).normalized;
            var selfEntity = this.context.selectEntities[0];
            
            if ((MoveEndPosType)this.tableConfig.EndPosType == MoveEndPosType.SkillTargetPos)
            {
                moveTargetPos = this.context.fromSkill.targetPos;
                moveTargetPos.y = 0;
            }
            else if ((MoveEndPosType)this.tableConfig.EndPosType == MoveEndPosType.SelfToSkillTargetPos_DirMaxDistancePos)
            {
                moveTargetPos = selfEntity.position + dir * (speed * lastTime);
                moveTargetPos.y = 0;
            }
            else if ((MoveEndPosType)this.tableConfig.EndPosType == MoveEndPosType.ReleaserToSelf_DirMaxDistancePos)
            {
              
                dir = (selfEntity.position - skillReleaser.position).normalized;
                moveTargetPos = selfEntity.position + dir * (speed * lastTime);
                moveTargetPos.y = 0;
            }
            else if ((MoveEndPosType)this.tableConfig.EndPosType == MoveEndPosType.SelfToMousePos_DirMaxDistancePos)
            {
              
                dir = (context.mousePos - selfEntity.position).normalized;
                moveTargetPos = selfEntity.position + dir * (speed * lastTime);
                moveTargetPos.y = 0;
            }
            else if ((MoveEndPosType)this.tableConfig.EndPosType == MoveEndPosType.ReleaserPos)
            {
                moveTargetPos = skillReleaser.position;
                moveTargetPos.y = 0;
            }
            

            // Battle_Log.LogZxy("moveTargetPos : " + moveTargetPos);
            // Battle_Log.LogZxy("speed : " + speed);
            
            battle.OnEntityStartMoveByOnePos_Skill(selfEntity.guid, moveTargetPos, speed);
        }

        //触发开始effect
        public void TriggerStartEffect()
        {
            //_G.Log("battle", string.Format("AreaEffect effect of guid : {0} TriggerEffect", this.guid));

            battle.AddSkillEffectGroup(tableConfig.StartEffectList,this.context);

        }


        public override void OnUpdate(float timeDelta)
        {
            var battle = this.context.battle;
            //强制移动
            //默认是技能目标点
            //var moveTargetPos = this.context.fromSkill.targetPos;
            //默认是技能释放者(可拓展)
            // var entity = this.context.fromSkill.releaser;
            var entity = this.context.selectEntities[0];

            var vector = moveTargetPos - entity.position;
            var dir = vector.normalized;
            var speed = this.tableConfig.MoveSpeed / 1000.0f;

            var currFramePos = entity.position;
            var nextFramePos = entity.position + dir * speed * timeDelta;

            //BattleLog.LogZxy("init dd: " + currFramePos + " " + nextFramePos);
            
            // //检测下一帧是否出界
            // var isOut = battle.IsOutOfMap((int)nextFramePos.x, (int)nextFramePos.z);
            // var isObstacle = battle.IsObstacle((int)nextFramePos.x, (int)nextFramePos.z);
            // if (isObstacle || isOut)
            // {
            //     BattleLog.LogZxy("currFramePos : " + currFramePos);
            //     BattleLog.LogZxy("nextFramePos : " + nextFramePos);
            //     
            //     battle.OnEntityStopMove(entity.guid, entity.position);
            //     this.SetWillEndState();
            //     return;
            // }
            // //

            var dotValue = Vector3.Dot(nextFramePos - moveTargetPos, moveTargetPos - currFramePos);


            if (dotValue >= 0)
            {
                //到达目的地
                entity.SetPosition(moveTargetPos);
                battle.OnEntityStopMove(entity.guid, moveTargetPos);
                
                battle.AddSkillEffectGroup(tableConfig.ReachEffectList,this.context);
                
                this.SetWillEndState();
            }
            else
            {
                // var prePos = entity.position;;
                // var nextPos = entity.position + dir * speed * timeDelta;
                var nextPos = nextFramePos;
                var moveType = (MoveProcessType)this.tableConfig.MoveProcessType;
                
                if (moveType == MoveProcessType.Immediately)
                {
                    nextPos = moveTargetPos;

                    var battleEntityMgr = battle.battleEntityMgr;
                    if (battleEntityMgr.IsHaveEntityOnPos((int)nextPos.x, (int)nextPos.z))
                    {
                        nextPos = battleEntityMgr.FindEmptyPosAround(nextPos);
                        
                        entity.SetPosition(nextPos);
                        // BattleLog.LogZxy("update pos : " + nextPos);
                        battle.AddSkillEffectGroup(tableConfig.ReachEffectList,this.context);
                        battle.OnEntityStopMove(entity.guid, nextPos);
                        this.SetWillEndState();
                        return;
                    }
                    
                }
                
                //检测下一帧是否出界
                var isOut = battle.IsOutOfMapF(nextPos.x, nextPos.z);
                var isObstacle = battle.IsObstacle((int)nextPos.x, (int)nextPos.z);
                if (isObstacle || isOut)
                {
                    // BattleLog.LogZxy("currFramePos : " + currFramePos);
                    // BattleLog.LogZxy("nextFramePos : " + nextFramePos);
                
                    //获取 (int)nextPos.x, (int)nextPos.z 这个坐标的在地图内的合理坐标
                    
                    
                    // battle.OnEntityStopMove(entity.guid, entity.position);
                    // battle.OnEntityStopMove(entity.guid, moveTargetPos);
                    var validPos = battle.GetValidMapPos(nextPos);
                    entity.SetPosition(validPos);
                    BattleLog.LogZxy("validPos : " + validPos);
                    battle.AddSkillEffectGroup(tableConfig.ReachEffectList,this.context);
                    battle.OnEntityStopMove(entity.guid, validPos);
                    this.SetWillEndState();
                    return;
                }
                //
                
                //BattleLog.LogZxy("update xx: " + prePos + " " + nextPos);
                
                //判断是否出界 如果出界 那么就设置点为合适的点 并且停止技能

                // if (!battle.IsOutOfMap((int)prePos.x, (int)prePos.z) && 
                //     battle.IsOutOfMap((int)nextPos.x, (int)nextPos.z))
                // {
                //     //到达目的地
                //     entity.SetPosition(prePos);
                //     battle.OnEntityStopMove(entity.guid, prePos);
                //
                //     battle.AddSkillEffectGroup(tableConfig.ReachEffectList,this.context);
                //
                //     BattleLog.LogZxy("update xx end: " + prePos + " " + nextPos);
                //     
                //     this.SetWillEndState();
                //     return;
                // }

                entity.SetPosition(nextPos);
            }
        }

        public override void Break()
        {
            var entity = this.context.fromSkill.releaser;
            battle.OnEntityStopMove(entity.guid, entity.position);
            this.SetWillEndState();
        }

        public override void OnEnd()
        {
            foreach (var item in tableConfig.EndRemoveEffectList)
            {
                var configId = item;
                //this.context.fromSkill.releser.RemoveBuffByConfigId(id);
                //默认为技能释放者 之后拓展

                //检查 buff(这个可以直接用 entity 进行删除 buff)
                var releaser = this.context.fromSkill.releaser;
                battle.DeleteBuffFromEntity(releaser.guid, configId);

                //检查 被动
                releaser.ForceRemovePassiveEffect(configId);
            }

            //持续施法结束引起技能释放结束 转到后摇状态
            //var isForEndSkill = this.tableConfig.IsThisEndForSkillEnd == 1;
            //if (isForEndSkill)
            //{
            //    this.context.fromSkill.OnChangeToSkillAfter();
            //}

            var isForEndSkill = 1 == this.tableConfig.IsThisEndForSkillEnd;
            if (isForEndSkill)
            {
                this.context.fromSkill.OnChangeToSkillAfter();
            }
        }
    }
}