using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

//using UnityEngine;

namespace Battle
{
    public class OperateModule
    {
        List<OperateNode> nodeList;

        BattleEntity entity;

        // public BaseEntityCtrl entityCtrl;
        public void Init(BattleEntity entity) //BaseEntityCtrl entityCtrl
        {
            // this.entity = entity;
            // this.entityCtrl = entityCtrl;
            nodeList = new List<OperateNode>();
            this.entity = entity;
        }

        // public void InitEntity(BattleEntity entity)
        // {
        //     this.entity = entity;
        // }

        public void AddOperate(OperateNode node,bool isAutoNormalAttack = false)
        {
            AddOperate(new List<OperateNode>() { node },isAutoNormalAttack);
        }

        public void AddOperate(List<OperateNode> addedNodeList,bool isAutoNormalAttack = false)
        {
            foreach (var item in addedNodeList)
            {
                item.Init(this.entity, this);
            }

            //Move NormalATK Skill
            if (0 == this.nodeList.Count)
            {
                this.nodeList.AddRange(addedNodeList);
            }
            else
            {
                if (isAutoNormalAttack)
                {
                    //自动攻击优先级最低
                    return;
                }

                var currExecuteNode = this.nodeList[0];
                if (currExecuteNode.type == OperateType.Move)
                {
                    this.nodeList.Clear();
                    this.nodeList.AddRange(addedNodeList);
                }
                else if (currExecuteNode.type == OperateType.ReleaseSkill)
                {
                    if (!currExecuteNode.IsCanBeBreak())
                    {
                        //从第二个操作开始全部移除 加入新的操作
                        for (int i = this.nodeList.Count - 1; i >= 1; i--)
                        {
                            this.nodeList.RemoveAt(i);
                        }

                        this.nodeList.AddRange(addedNodeList);
                    }
                    else
                    {
                        this.nodeList.Clear();

                        this.nodeList.AddRange(addedNodeList);
                    }
                }
            }
            // else
            // {
            //     //理论上不会出现
            //     Battle_Log.LogWarningZxy("OperateModule : AddOperate : this.nodeList.Count > 2");
            // }

            // if (this.entity.isPlayerCtrl)
            // {
            //     string s = "";
            //     foreach (var node in this.nodeList)
            //     {
            //         s += "   " +node.type;
            //     }
            //
            //     Debug.Log(s);
            // }

          
        }

        public void Update(float deltaTime)
        {
            UpdateNodes(deltaTime);
        }

        OperateNode currNode;

        public void UpdateNodes(float deltaTime)
        {
            // if (entity.isPlayerCtrl && nodeList.Count > 0)
            // {
            //     // Logx.Log(LogxType.Zxy," nodeList.Count : " + nodeList.Count);
            //     Logx.Log(LogxType.Zxy," nodeList[0] : " + nodeList[0].type + " ,len : " +
            //                           nodeList.Count);
            //
            // }

            if (nodeList.Count > 0)
            {
                currNode = nodeList[0];
                if (currNode.state == ExecuteState.Ready)
                {
                    currNode.Execute();
                }
                else if (currNode.state == ExecuteState.Doing)
                {
                    currNode.Update(deltaTime);
                }
            }
            else
            {
                //没有任何操作的话 那么检测自动攻击
            }
        }

        public void OnNodeExecuteFinish(int operateKey)
        {
            if (nodeList.Count > 0)
            {
                var currNode = nodeList[0];
                if (currNode.key == operateKey)
                {
                    //Debug.Log("zxy : test : Finish " + currNode.key);

                    currNode.Finish();
                    nodeList.RemoveAt(0);

                    //Handle();
                }
            }
        }

        internal bool IsHaveOperate()
        {
            return this.nodeList.Count > 0;
        }

        public void StopAllOperate()
        {
            this.ClearAllNodes();
            
            entity.ChangeToIdle();
        }

        void ClearAllNodes()
        {
            nodeList?.Clear();
            currNode = null;
        }
    }
}