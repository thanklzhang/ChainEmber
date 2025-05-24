using UnityEngine;

namespace Battle_Client
{
    public partial class BattleSkillEffect
    {
        //加载相关
        public int resId;
        public bool isFinishLoad = false;
        public string path;
        
        
        //开始自行加载(主要用于创建 entity 的时候自己自行异步加载 )
        public void StartSelfLoadModel()
        {
            //Logx.Log("StartLoadModel");
            isFinishLoad = false;
            if (this.resId > 0)
            {
                ResourceManager.Instance.GetObject<GameObject>(path, (obj) => { OnLoadModelFinish(obj); });
            }
            else
            {
                isFinishLoad = true;
            }
        }

        
        public void OnLoadModelFinish(GameObject obj)
        {
            if (this.state == BattleSkillEffectState.Destroy)
            {
                ResourceManager.Instance.ReturnObject(path, gameObject);
                return;
            }

            //Logx.Log("BattleSkillEffect : OnLoadModelFinish");
            isFinishLoad = true;
            var position = gameObject.transform.position;
            GameObject.Destroy(gameObject);
            gameObject = obj;
            gameObject.transform.position = position;
            //gameObject = 

            lineRender = gameObject.GetComponentInChildren<LineRenderer>();
            if (lineRender != null)
            {
                lineRender.positionCount = 2;
                lineRender.useWorldSpace = true;
            }

          
            //获取持续时长
            var curParticle = obj.GetComponent<ParticleSystem>();
            var particles = obj.GetComponentsInChildren<ParticleSystem>();
            // if (gameObject.name.Contains("eft_skill_projectile"))
            // {
            //     Logx.Log(" particles : " + particles.Length);
            // }

            if (particles != null && particles.Length > 0)
            {
                var particle = particles[0];
                this.isLoop = particle.main.loop;
                totalAutoDestroyTime = particle.main.duration;
                // Logx.Log(this.gameObject.name + " totalAutoDestroyTime : " + totalAutoDestroyTime);

                for (int i = 0; i < particles.Length; i++)
                {
                    particle = particles[i];
                    particle.Play();
                }
            }

            if (curParticle != null)
            {
                curParticle.Play();
            }
        }

    }
}