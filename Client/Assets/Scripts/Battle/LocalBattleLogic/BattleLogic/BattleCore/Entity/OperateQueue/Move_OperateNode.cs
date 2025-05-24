using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battle
{
    public class Move_OperateNode : OperateNode
    {
        //move param
        public Vector3 moveTargetPos;
        public int moveFollowTargetGuid;

        //如果目标是单位 那么指和目标差多少距离算作完成（释放技能的时候过远的时候指释放距离）
        public float finishDistance;

        private float findPathTargetTime = 0.60f;
        private float findPathTimer;

        protected override void OnInit()
        {
            
        }

        //第一次执行
        protected override void OnExecute()
        {
            findPathTimer = findPathTargetTime;
            
            if (moveFollowTargetGuid > 0)
            {
                var target = battle.FindEntity(moveFollowTargetGuid);
                if (target != null)
                {
                    // entityCtrl.FindPathAndMoveToPos(target.position);
                    entity.FindPathAndMoveToPos(target.position);
                }
                else
                {
                    //move 完成
                    //this.entity.ChangeToIdle();
                    this.operateModule.OnNodeExecuteFinish(this.key);
                }
            }
            else
            {
                // entityCtrl.FindPathAndMoveToPos(moveTargetPos);
                // Logx.Log(LogxType.Zxy,"one move");
                entity.FindPathAndMoveToPos(moveTargetPos);
            }
        }

        //update
        protected override void OnUpdate(float deltaTime)
        {
            if (!this.entity.IsCanMove())
            {
                return;
            }

            var isFind = false;
            Vector3 targetPos;
            if (moveFollowTargetGuid > 0)
            {
                UpdateMoveToTargetEntity(deltaTime);
            }
            else
            {
                UpdateMoveToTargetPos(deltaTime);
            }
        }


        //目标是实体的时候更新逻辑
        void UpdateMoveToTargetEntity(float deltaTime)
        {
            //有目标
            var target = battle.FindEntity(moveFollowTargetGuid);

            if (target != null)
            {
                var sqrDis = (this.entity.position - target.position).sqrMagnitude;
                if (sqrDis <= finishDistance * finishDistance)
                {
                    //move 完成
                    this.operateModule.OnNodeExecuteFinish(this.key);
                }
                else
                {
                    
                    if (findPathTimer >= findPathTargetTime)
                    {
                        var isSuccess = entity.FindPathAndMoveToPos(target.position);

                        if (isSuccess)
                        {
                            //直到成功移动才开始寻路 CD
                            findPathTimer = 0;
                        }
                    }
                    else
                    {
                        findPathTimer += deltaTime;
                    }
                }
            }
            else
            {
                //可能挂了 结束整个操作
                this.operateModule.StopAllOperate();
            }
        }

        //目标是点的时候更新逻辑
        void UpdateMoveToTargetPos(float deltaTime)
        {
            //地点
            var sqrDis = (this.entity.position - moveTargetPos).sqrMagnitude;
            if (sqrDis <= finishDistance * finishDistance)
            {
                //move 完成
                // this.entity.ChangeToIdle();
                this.operateModule.OnNodeExecuteFinish(this.key);
            }
            else
            {
                // if (findPathTimer >= findPathTargetTime)
                // {
                //     var isSuccess = entity.FindPathAndMoveToPos(moveTargetPos);
                //
                //     if (isSuccess)
                //     {
                //         //直到成功移动才开始寻路 CD
                //         findPathTimer = 0;
                //     }
                // }
                // else
                // {
                //     findPathTimer += deltaTime;
                // }
            }
        }

        public override int GenKey()
        {
            return (int)OperateKey.Move;
        }

    }

    public enum OperateKey
    {
        Move = 100
    }
}