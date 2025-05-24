using System.Collections.Generic;
using Battle;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

namespace Battle_Client
{
    //实体转换部分 如位移旋转等
    public partial class BattleEntity_Client
    {
        Vector3 moveTargetPos;
        public List<Vector3> movePosList;
        Vector3 dirTarget;
        int movePosIndex;
        float normalAnimationMoveSpeed = 4.0f;
        float rotateSpeed = 540;
        int lastDir = 1;
        public EntityLocationType localtionType;
        private bool isForceSkillMove = false;

        private float forceMoveSpeed;

        internal void StartMoveByPath(List<Vector3> pathList, float moveSpeed, bool isSkillForce)
        {
            if (pathList.Count > 0)
            {
                isForceSkillMove = isSkillForce;
                State = BattleEntityState.Move;

                this.movePosList = pathList;
                // this.attr.moveSpeed = moveSpeed;
                if (!isForceSkillMove)
                {
                    this.attr.SetAttr(EntityAttrType.MoveSpeed, moveSpeed);
                }
                else
                {
                    forceMoveSpeed = moveSpeed;
                }


                var aniScale = this.attr.GetValue(EntityAttrType.MoveSpeed) / normalAnimationMoveSpeed;

                PlayAnimation("walk", aniScale);

                movePosIndex = 0;
                this.moveTargetPos = pathList[0];

                //设置朝向
                SetDirTarget((moveTargetPos - this.gameObject.transform.position).normalized);
                // dirTarget = (moveTargetPos - this.gameObject.transform.position).normalized;
            }
        }

        public void UpdateMove(float timeDelta)
        {
            var moveVector = moveTargetPos - this.gameObject.transform.position;
            var speed = this.attr.GetValue(EntityAttrType.MoveSpeed);


            if (isForceSkillMove)
            {
                speed = forceMoveSpeed;
            }

            if (0 == this.playerIndex)
            {
                //Logx.Log("client : speed : " + speed);
            }


            var dir = moveVector.normalized;
            //var dis = moveVector.magnitude;

            var currPos = this.gameObject.transform.position;
            var moveDistance = (dir * speed * timeDelta).magnitude;

            var currFramePos = currPos;
            //预测下一帧位置
            var moveDelta = dir * speed * timeDelta;
            var nextFramePos = currPos + moveDelta;

            var dotValue = Vector3.Dot(currFramePos - moveTargetPos, moveTargetPos - nextFramePos);
            if (dotValue >= 0)
            {
                this.movePosIndex = this.movePosIndex + 1;
                if (this.movePosIndex >= this.movePosList.Count)
                {
                    //所有路径走完了
                    this.FakeStopMove(moveTargetPos);
                }
                else
                {
                    //前往下一个路径点
                    this.moveTargetPos = this.movePosList[this.movePosIndex];

                    //设置朝向
                    SetDirTarget((moveTargetPos - this.gameObject.transform.position).normalized);
                    // dirTarget = (moveTargetPos - this.gameObject.transform.position).normalized;
                }

                //this.gameObject.transform.forward = dirTarget;
            }
            else
            {
                moveDelta = dir * (speed * timeDelta);
                //Logx.Log("moveTest : battleClient : moveDis : " + moveDelta.x + " " + moveDelta.z);
                this.gameObject.transform.position = currPos + moveDelta;

                var prePos = this.gameObject.transform.position;
                this.gameObject.transform.position = new Vector3(prePos.x, 0, prePos.z);

                //this.gameObject.transform.forward = Vector3.Lerp(this.gameObject.transform.forward, dirTarget, timeDelta * 15);
            }
        }

        //根据消息来真实的终止移动
        public void StopMove(Vector3 endPos)
        {
            if (State != BattleEntityState.Dead)
            {
                State = BattleEntityState.Idle;
                PlayAnimation("idle");
            }

           

            isForceSkillMove = false;

           

            if (CheckPosIsForcePullBack(endPos))
            {
                this.SetPosition(endPos);
            }

            if (0 == this.playerIndex)
            {
                //Logx.Log("client : StopMove : " + endPos);
            }

            // isForceSkillMove = false;
        }

        //检查当前点和目标的距离是否需要强制拉回
        public bool CheckPosIsForcePullBack(Vector3 checkPos)
        {
            var len = (checkPos - this.GetPosition()).magnitude;

            if (len <= 0.5)
            {
                return false;
            }

            return true;
        }

        public void FakeStopMove(Vector3 endPos)
        {
            if (State != BattleEntityState.Dead)
            {
                State = BattleEntityState.Idle;
                PlayAnimation("idle");
            }


            //Logx.Log("moveTest : battleClient : fake reach : " + endPos.x + " " + endPos.y);

            

            this.SetPosition(endPos);
        }


        internal void SetDirTarget(Vector3 dir)
        {
            if (dir == Vector3.zero)
            {
                return;
            }

            this.dirTarget = dir;
        }

        public void SetPosition(Vector3 pos)
        {
            //this.position = pos;
            gameObject.transform.position = pos;
        }


        public void SetLocationType(EntityLocationType type)
        {
            localtionType = type;
        }

        public void SetLevel(int level)
        {
            this.level = level;
        }

        internal Vector3 GetPosition()
        {
            return this.gameObject.transform.position;
        }


        public void SetTrueDir(float deltaTime)
        {
            float speed = 30;
            var subDir = this.gameObject.transform.forward + dirTarget;
            if (subDir == Vector3.zero)
            {
                //矢量和为 0 的情况
                var filterDir = Quaternion.Euler(0, 1, 0) * this.gameObject.transform.forward;
                this.gameObject.transform.forward = Vector3.Lerp(filterDir, dirTarget, speed * deltaTime);
            }
            else
            {
                this.gameObject.transform.forward =
                    Vector3.Lerp(this.gameObject.transform.forward, dirTarget, speed * deltaTime);
            }
        }
    }
}