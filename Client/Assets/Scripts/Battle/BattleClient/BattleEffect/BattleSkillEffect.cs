using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Battle_Client
{
    public partial class BattleSkillEffect
    {
        public int guid;
        public GameObject gameObject;
        public BattleSkillEffectState state = BattleSkillEffectState.Idle;

        bool isAutoDestroy;
        float currAutoDestroyLastTime = 0.0f;
        float totalAutoDestroyTime;
        bool isLoop;

        public BuffEffectInfo_Client buffInfo;

        private bool isBattleEnd;

        public void Init(int guid, int resId)
        {
            this.guid = guid;
            this.resId = resId;

            // if (resId <= 0)
            // {
            //     this.SetWillDestoryState();
            //     return;
            // }

            //load
            //先拿一个简易的模型暂时放这 然后等真正模型下载好之后在替换即可
            var asset = GameMain.Instance.tempModelAsset;
            gameObject = GameObject.Instantiate(asset);

            // get path
            //var heroConfig = Config.ConfigManager.Instance.GetById<Config.EntityInfo>(this.configId);
            if (this.resId > 0)
            {
                var resConfig = Config.ConfigManager.Instance.GetById<Config.ResourceConfig>(this.resId);
                //临时组路径 之后会打进 ab 包
                path = "Assets/BuildRes/" + resConfig.Path + "/" + resConfig.Name + "." + resConfig.Ext;
            }
            else
            {
                gameObject.SetActive(false);
            }

            gameObject.transform.SetParent(GameMain.Instance.gameObjectRoot, false);


            isFinishLoad = false;

            //this.StartLoadModel();
        }

        internal void SetIsAutoDestroy(bool isAutoDestroy)
        {
            this.isAutoDestroy = isAutoDestroy;
        }

        private LineRenderer lineRender;

        internal void SetBuffInfo(BuffEffectInfo_Client buffInfo)
        {
            // this.buffInfo = new BuffEffectInfo_Client()
            // {
            //     guid = buffInfo.guid,
            //     targetEntityGuid = buffInfo.targetEntityGuid,
            //     currCDTime = buffInfo.currCDTime / 1000.0f,
            //     maxCDTime = buffInfo.maxCDTime / 1000.0f,
            //     stackCount = buffInfo.statckCount,
            //     linkTargetEntityGuid = buffInfo.linkTargetEntityGuid,
            //     configId = buffInfo.configId,
            //     //iconResId = buffInfo.iconResId
            // };
            this.buffInfo = buffInfo;

            //检查链接信息
            linkTargetEntityGuid = this.buffInfo.linkTargetEntityGuid;
            if (linkTargetEntityGuid > 0)
            {
                var entity = BattleEntityManager.Instance.FindEntity(linkTargetEntityGuid);
                if (null != entity)
                {
                    linkTargetNode = entity.FindModelNode(BattleClientConfig.TransformNodeName_hit);
                }
            }


            EventDispatcher.Broadcast(EventIDs.OnBuffInfoUpdate, this.buffInfo);
        }

        public void Update(float deltaTime)
        {
            if (this.state == BattleSkillEffectState.WillDestroy &&
                this.state == BattleSkillEffectState.Destroy)
            {
                return;
            }

            if (state == BattleSkillEffectState.Move)
            {
                UpdateMove(deltaTime);
            }

            HandleFollowEntity();

            if (isFinishLoad)
            {
                //if (!isLoop)
                //{
                //    currLastTime = currLastTime + timeDelta;
                //    if (currLastTime >= this.totalTotalTime)
                //    {
                //        this.SetWillDestoryState();
                //        //BattleSkillEffectManager.Instance.DestorySkillEffect(this.guid);
                //    }
                //}

                if (this.isAutoDestroy)
                {
                    currAutoDestroyLastTime += deltaTime;

                    if (currAutoDestroyLastTime >= this.totalAutoDestroyTime)
                    {
                        this.SetWillDestoryState();
                    }
                }

                //应该没有持续时间的概念 等待消息才销毁 这个时间可以做倒计时
                //currLastTime = currLastTime + timeDelta;
                //if (currLastTime >= this.totalTotalTime)
                //{
                //    this.SetWillDestoryState();
                //    //BattleSkillEffectManager.Instance.DestorySkillEffect(this.guid);
                //}
            }

            if (this.IsBuff())
            {
                this.buffInfo.currCDTime -= deltaTime;
            }
        }

        public void SetWillDestoryState()
        {
            this.state = BattleSkillEffectState.WillDestroy;
        }

        public void Destroy()
        {
            this.state = BattleSkillEffectState.Destroy;
            if (isFinishLoad)
            {
                if (this.resId > 0)
                {
                    ResourceManager.Instance.ReturnObject(resId, gameObject);
                }
                else
                {
                    GameObject.Destroy(gameObject);
                }
            }
            else
            {
                GameObject.Destroy(gameObject);
            }
        }

        public void OnBattleEnd()
        {
            this.isBattleEnd = true;
        }

        public bool IsBuff()
        {
            return this.buffInfo != null;
        }
    }
}