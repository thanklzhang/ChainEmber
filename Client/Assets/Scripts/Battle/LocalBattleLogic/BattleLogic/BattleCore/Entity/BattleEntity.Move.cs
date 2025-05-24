using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;


namespace Battle
{
    public partial class BattleEntity
    {
        BattleEntityPathFinder pathFinder;

        //移动碰撞等待标记
        public bool IsMoveCollisionWaiting;
        public float moveCollisionWaitTimer;
        public const float MoveCollisionWaitMaxTime = 0.15f;
        public float collisionCircle = 1.5f;

        void InitMovement()
        {
            pathFinder = new BattleEntityPathFinder();
            pathFinder.Init(this);
        }

        public void SetMovementInfo()
        {
            pathFinder.InitMap(this.GetBattle().GetMap());
        }

        //开始移动(按照路径移动)
        public bool StartMoveByPath(List<Vector3> posList)
        {
            bool isSuccess = true;
            if (posList.Count > 0)
            {
                //this.ChangeToIdle();

                var startPos = posList[0];
                isSuccess = MoveToPos(startPos);
                if (isSuccess)
                {
                    battle.OnEntityStartMoveByPath(this.guid, posList, this.MoveSpeed,false);
                }
            }

            return isSuccess;
        }

        //移动到某点 
        public bool MoveToPos(Vector3 targetPos)
        {
            var isCanMove = this.IsCanMove();

            if (!isCanMove)
            {
                //_Battle_Log.Log(string.Format("{0} StartMoveToPos move : no move ! ", this.infoConfig.Name));
                return false;
            }


            this.moveTargetPos = targetPos;

            SetDir((this.moveTargetPos - this.position).normalized,false);

            EntityState = EntityState.Move;

            // Logx.Log(LogxType.Zxy, "success to move");

            return true;
        }

        public void SetDir(Vector3 dir,bool isSync = true)
        {
            this.dir = dir;
            if (isSync)
            {
                SyncDir();
            }
        }

        public void SyncDir()
        {
            this.battle.OnChangeEntityDir(this.guid, dir);
        }

        //寻找路径并且开始移动
        public bool FindPathAndMoveToPos(Vector3 targetPos)
        {
            var ss = this.pathFinder.GetIntPos(this.position);
            var ss2 = this.pathFinder.GetCenterPos(ss);
            // if (this.ai is not CpuAI)
            // {
            //     Battle_Log.Log(200, $"{this.infoConfig.Name} : start pos : " + ss2);
            // }

          
            var pathList = pathFinder.FindPath(targetPos);
            List<Vector3> vct3List = new List<Vector3>();
            foreach (var pos in pathList)
            {
                Vector3 v = this.pathFinder.GetCenterPos(pos);
                vct3List.Add(v);
            }
            
            // if (this.ai is not CpuAI)
            // {
            //     Battle_Log.Log(200, "targetPos : " + targetPos);
            //     Battle_Log.LogVector3List(200, vct3List);
            // }

            if (this.playerIndex == 0)
            {
                // Battle_Log.LogZxy("move ---->>");
                // foreach (var VARIABLE in vct3List)
                // {
                //     Battle_Log.LogZxy("VARIABLE : " + VARIABLE);
                // }
                // Battle_Log.LogZxy("move <<----");
            }

            if (vct3List.Count <= 1)
            {
                this.ai.OnMoveNoWay();
            }

            return this.StartMoveByPath(vct3List);
        }

        //到达了当前移动路径节点
        public virtual void OnMoveToCurrTargetPosFinish()
        {
            //_Battle_Log.Log("find path test : BaseAI : OnMoveToCurrTargetPosFinish");
            Vector3 nextStepPos;
            if (this.pathFinder.TryToGetNextStepPos(out nextStepPos))
            {
                //_Battle_Log.Log("find path test : BaseAI : OnMoveToCurrTargetPosFinish : find pos , start move to : " + nextStepPos);
                this.MoveToPos(nextStepPos);
            }
            else
            {
                this.pathFinder.ClearPath();
                this.ChangeToIdle();
                this.OnFinishAllMovePos();
            }
        }

        //完成移动的整个路径
        public void OnFinishAllMovePos()
        {
            this.operateModule.OnNodeExecuteFinish((int)OperateKey.Move);
        }

        public void UpdateByMoveState(float deltaTime)
        {
            var isCanMove = this.IsCanMove();

            if (!isCanMove)
            {
                //_Battle_Log.Log(string.Format("{0} Update move : no move ! ", this.infoConfig.Name));
                return;
            }

            // Logx.Log(LogxType.Zxy, "success to move update");

            var vector = moveTargetPos - this.position;
            var dir = vector.normalized;
            var speed = this.MoveSpeed;

            var currFramePos = this.position;
            var nextFramePos = this.position + dir * speed * battle.TimeDelta;

            var dotValue = Vector3.Dot(nextFramePos - moveTargetPos, moveTargetPos - currFramePos);

            if (dotValue >= 0)
            {
                //到达目的地
                this.SetPosition(moveTargetPos);

                //_G.Log("battle", "entity of guid : " + this.guid + " , reach to target pos : " + moveTargetPos);

                //battle.OnMoveToCurrTargetPosFinish(this.guid);
                OnMoveToCurrTargetPosFinish();
            }
            else
            {
                // //碰撞检测(这里是移动的碰撞检测)-----------
                // BattleEntity collisionEntity;
                // // bool isNeedCollisionWait;
                // var isCollision = battle.CheckMoveCollision(this, out collisionEntity);
                //
                // var isMove = false;
                // if (!IsMoveCollisionWaiting)
                // {
                //     if (isCollision)
                //     {
                //         if (!collisionEntity.IsMoveCollisionWaiting)
                //         {
                //             //都没有碰撞 那么自己就开始等待
                //             IsMoveCollisionWaiting = true;
                //         }
                //         else
                //         {
                //             //碰撞目标已经在等待中了 自己继续寻路即可
                //             isMove = true;
                //             //battle.EntityContinueFindPath(this);
                //              this.FindPathByCurrPath();
                //              return;
                //         }
                //     }
                //     else
                //     {
                //         isMove = true;
                //     }
                // }
                // else
                // {
                //     moveCollisionWaitTimer += deltaTime;
                //     if (moveCollisionWaitTimer >= MoveCollisionWaitMaxTime)
                //     {
                //         moveCollisionWaitTimer = 0;
                //         IsMoveCollisionWaiting = false;
                //         isMove = true;
                //         // battle.EntityContinueFindPath(this);
                //         //this.FindPathByCurrPath();
                //         //return;
                //     }
                // }

                //碰撞检测(这里是移动的碰撞检测)-----------
                BattleEntity collisionEntity = null;
                var currPos = this.pathFinder.GetIntPos(currFramePos);
                var nextPos = this.pathFinder.GetIntPos(nextFramePos);

                if (currPos.x != nextPos.x || currPos.y != nextPos.y)
                {
                    //判断哪个目标在这个格子上（TODO：这里碰撞体积先按照 1 个格子来算）
                    var allEntityList = battle.GetAllEntities();
                    foreach (var kv in allEntityList)
                    {
                        var currEntity = kv.Value;
                        if (currEntity == this)
                        {
                            continue;
                        }

                        // //判断前方射线
                        // var explorePos = currPos + dir * RayLength;
                        //
                        // var isRayCollision = IsCollisionCircle(explorePos, 0, currEntity.position, ExploreEndR);
                        // if (isRayCollision)
                        // {
                        //     collisionEntity = currEntity;
                        //     break;
                        // }
                        
                        var currEntityPos = this.pathFinder.GetIntPos(currEntity.position);
                        if (currEntityPos.x == nextPos.x && currEntityPos.y == nextPos.y)
                        {
                            collisionEntity = currEntity;
                            break;
                        }
                    }
                }

                var isCollision = collisionEntity != null;//battle.CheckMoveCollision(this, out collisionEntity);
                if (isCollision)
                {
                    this.FindPathByCurrPath();
                }
                else
                {
                    var moveDelta = dir * speed * battle.TimeDelta;
                    var _nextPos = this.position + moveDelta;
                    this.SetPosition(_nextPos);
                }

                //没有动态碰撞检测
                // var moveDelta = dir * speed * battle.TimeDelta;
                // var nextPos = this.position + moveDelta;
                // this.SetPosition(nextPos);
            }
        }

        void IsForwardCollision(Vector3 currPos, Vector3 targetPos)
        {
            
        }

        public void FindPathByCurrPath()
        {
            var currPosList = this.pathFinder.GetCurrPosList();
            if (currPosList.Count > 0)
            {
                var endPos = currPosList[currPosList.Count - 1];
                var pos = this.pathFinder.GetCenterPos(endPos);
                this.FindPathAndMoveToPos(pos);
            }
        }

        public void OnStopMove()
        {
        }

        public bool IsCanMoveByAbnormalState()
        {
            if (this.IsDead())
            {
                return false;
            }
            
            
            var checkAbnormal = this.abnormalStateMgr.IsCanMove();

            if (!checkAbnormal)
            {
                return false;
            }

            // Logx.Log(LogxType.Zxy,"move 3");
            return true;
        }

        public bool IsCanMove()
        {
            if (this.IsDead())
            {
                return false;
            }

            // Logx.Log(LogxType.Zxy,"move 1");

            if (this.EntityState == EntityState.UseSkill)
            {
                return false;
            }

            // Logx.Log(LogxType.Zxy,"move 2");

            //检测异常状态
            if (!IsCanMoveByAbnormalState())
            {
                return false;
            }

            // Logx.Log(LogxType.Zxy,"move 3");
            return true;
        }

        public List<Pos> GetCurrPathPosList()
        {
            return pathFinder.GetCurrPosList();
        }
    }
}