using System.Collections.Generic;
using UnityEngine;

namespace Battle_Client
{
    //模型相关 包括模型加载 动画播放等
    public partial class BattleEntity_Client
    {
        public GameObject tempModel;
        public GameObject model;
        public Animator animator;
        string modelRootName = "Model";
        public bool isFinishLoad = false;
        public string path;
        public List<string> triggerNames = new List<string>();
        private int aniAct = 0;
        
        public void InitModel()
        {   
            //load
            //先拿一个简易的模型暂时放这 然后等真正模型下载好之后在替换即可
            var asset = GameMain.Instance.tempModelAsset;
            gameObject = GameObject.Instantiate(asset);
            gameObject.transform.SetParent(GameMain.Instance.gameObjectRoot, false);
            tempModel = gameObject.transform.Find("Cube").gameObject;

            // get path
            var heroConfig = Config.ConfigManager.Instance.GetById<Config.EntityInfo>(this.configId);
            var heroResConfig = Config.ConfigManager.Instance.GetById<Config.ResourceConfig>(heroConfig.ModelId);
            //临时组路径 之后会打进 ab 包
            path = "Assets/BuildRes/" + heroResConfig.Path + "/" + heroResConfig.Name + "." + heroResConfig.Ext;
            this.gameObject.name = heroResConfig.Name;
        }
        
        internal Transform FindModelNode(string nodeName)
        {
            return this.model.transform.Find(modelRootName + "/" + nodeName);
        }
        
        
        //开始自行加载(主要用于创建 entity 的时候自己自行异步加载 )
        public void StartSelfLoadModel()
        {
            //Logx.Log("StartLoadModel");
            isFinishLoad = false;
            ResourceManager.Instance.GetObject<GameObject>(path, (obj) => { OnLoadModelFinish(obj); });
        }

        public void OnLoadModelFinish(GameObject obj)
        {
            isFinishLoad = true;
            tempModel.SetActive(false);

            model = obj;
            model.transform.SetParent(this.gameObject.transform);
            model.transform.localPosition = Vector3.zero;
            model.transform.localRotation = Quaternion.Euler(0, 0, 0);

            collider = gameObject.GetComponentInChildren<Collider>();
            animator = gameObject.GetComponentInChildren<Animator>();
        }
        
        
        public void PlayAnimation(string aniTriggerName, float speed = 1.0f)
        {
            var myEntityGuid = BattleManager.Instance.GetLocalCtrlHeroGuid();
            if (isFinishLoad && animator != null)
            {
                if (speed < 0)
                {
                    speed = 0.0001f;
                }

                animator.speed = speed;
                
                if ("idle" == aniTriggerName)
                {
                    aniAct = 1;
                }
                else if ("walk" == aniTriggerName)
                {
                    aniAct = 2;
                }
                else if ("attack" == aniTriggerName)
                {
                    var currStateInfo = animator.GetCurrentAnimatorStateInfo(0);
                    var hash = currStateInfo.shortNameHash;
           
                    if (currStateInfo.shortNameHash == hash)
                    {
                        animator.Play(hash, 0, 0f);     
                    }
                    aniAct = 3;
                }
                else if ("die" == aniTriggerName)
                {
                    aniAct = 4;
                }

                animator.SetInteger("Action", aniAct);
            }
        }

        
        public void SetAnimationSpeed(float speed)
        {
            if (speed < 0)
            {
                speed = 0.0001f;
            }

            if (isFinishLoad && animator != null)
            {
                animator.speed = speed;
            }
        }
    }
}