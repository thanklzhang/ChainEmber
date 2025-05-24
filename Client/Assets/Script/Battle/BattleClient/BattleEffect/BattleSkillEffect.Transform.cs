using UnityEngine;

namespace Battle_Client
{
    public partial class BattleSkillEffect
    {
        //move
        Vector3 moveTargetPos;
        float moveSpeed;
        private int targetGuid;
        int followEntityGuid;
        Transform followEntityNode;

        public Vector3 initDir;

        //如果是链子的话
        private int linkTargetEntityGuid;
        private Transform linkTargetNode;

        //设置该特效为一直跟随实体
        internal void SetFollowEntityGuid(int followEntityGuid, string nodeName)
        {
            //StartMove(Vector3.zero, followEntityGuid, 9999999.0f);

            this.followEntityGuid = followEntityGuid;
            var entity = BattleEntityManager.Instance.FindEntity(followEntityGuid);
            if (null != entity)
            {
                followEntityNode = entity.FindModelNode(nodeName);
                this.SetPosition(followEntityNode.position);
            }
        }


        public void SetPosition(Vector3 pos)
        {
            //this.position = pos;
            gameObject.transform.position = pos;
        }

        private bool isFlyMaxRange;
        private Vector3 dirByFlyMax;
        internal void StartMove(Vector3 targetPos, int targetGuid, float moveSpeed, 
            bool isFlyMaxRange, UnityEngine.Vector3 dirByFlyMax)
        {
            //Logx.Log("skill effect start move : " + this.guid + " will move to : " + targetPos + " by speed : " + moveSpeed);
            state = BattleSkillEffectState.Move;

            this.moveTargetPos = targetPos;
            this.moveSpeed = moveSpeed;
            this.targetGuid = targetGuid;
            this.isFlyMaxRange = isFlyMaxRange;
            this.dirByFlyMax = dirByFlyMax;

            if (isFlyMaxRange)
            {
                //按照一个方向一直飞
                initDir = dirByFlyMax;
            }
            else
            {
                if (this.targetGuid <= 0)
                {
                    //非跟随
                    initDir = (targetPos - this.gameObject.transform.position).normalized;
                }
            }

         
        }

        public int GetFollowEntityGuid()
        {
            return this.followEntityGuid;
        }

        public void UpdateMove(float timeDelta)
        {
            var targetPos = moveTargetPos;

            var moveVector = Vector3.one;
            
            if (this.isFlyMaxRange)
            {
                moveVector = initDir;
            }
            else
            {
                if (targetGuid > 0)
                {
                    //跟随
                    var entity = BattleEntityManager.Instance.FindEntity(targetGuid);
                    if (entity != null)
                    {
                        targetPos = entity.GetPosition();
                        moveTargetPos = targetPos;
                    }
                    else
                    {
                        targetPos = moveTargetPos;
                    }

                    moveVector = targetPos - this.gameObject.transform.position;
                }
                else
                {
                    //非跟随 一直飞行
                    moveVector = initDir;
                }
            }

         
         


            var speed = this.moveSpeed;
            var dir = moveVector.normalized;
            //var dis = moveVector.magnitude;

            var currPos = this.gameObject.transform.position;

            var dirDis = dir * (speed * timeDelta);
            this.gameObject.transform.position = currPos + dirDis;

            if (this.followEntityGuid <= 0)
            {
                if (dir != Vector3.zero)
                {
                    this.gameObject.transform.forward = dir;
                }
            }

            var preDir = moveVector;
            var nowDir = this.gameObject.transform.position - targetPos;
            if (Vector3.Dot(preDir, nowDir) < 0)
            {
                //到了目的地 此时如果战斗结束 那么直接设置删除状态
                //否则等待服务器删除
                if (isBattleEnd)
                {
                    this.SetWillDestoryState();
                }
            }
        }

        void HandleFollowEntity()
        {
            //跟随的单位
            if (this.followEntityGuid > 0)
            {
                //完全跟随目标
                var followEntity = BattleEntityManager.Instance.FindEntity(this.followEntityGuid);
                if (followEntity != null)
                {
                    var followPos = new Vector3();
                    if (followEntityNode != null)
                    {
                        //跟随某一个挂点
                        followPos = followEntityNode.position;
                    }
                    else
                    {
                        followPos = followEntity.gameObject.transform.position;
                    }

                    this.gameObject.transform.position = followPos + Vector3.up * 0.03f;

                    //link 目标（如果是链子的话）
                    if (linkTargetNode != null)
                    {
                        var startNode = followEntity.FindModelNode(BattleClientConfig.TransformNodeName_hit);
                        if (startNode != null)
                        {
                            UpdateLinkPos(startNode.position,
                                linkTargetNode.position);
                        }
                        else
                        {
                            UpdateLinkPos(followPos,
                                linkTargetNode.position);
                        }
                    }
                }
            }
        }

        void UpdateLinkPos(Vector3 pos0, Vector3 pos1)
        {
            lineRender.SetPosition(0, pos0);
            lineRender.SetPosition(1, pos1);
        }

        public void StopMove(Vector3 endPos)
        {
            state = BattleSkillEffectState.Idle;

            this.SetPosition(endPos);
        }
    }
}