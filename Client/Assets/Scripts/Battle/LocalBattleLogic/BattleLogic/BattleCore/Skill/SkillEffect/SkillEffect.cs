using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Battle
{
    //public enum SkillEffectType
    //{
    //    Null = 0,
    //    Projectile = 1,
    //    Calculate = 2
    //}

    public enum SkillEffectState
    {
        Null = 0,
        InitFinish = 1,
        Running = 2,
        WillEnd = 3,
        End = 4,
        Destroy = 5
    }

    public class SkillEffectContext
    {
        //上下文中选取的单位（目前这个 list 的 长度只储存 1 个，因为多个选取的情况在之前已经就有逻辑了，而这里只处理一个的情况 ， TODO：改为非列表）
        public List<BattleEntity> selectEntities;

        //上下文中选取的点
        public List<Vector3> selectPositions;

        //归属技能
        public Skill fromSkill;

        //当前战斗
        public Battle battle;

        //碰撞组
        public CollisionGroupEffect collisonGroupEffect;

        //造成的伤害
        public float damage;

        //输出伤害变化的百分比
        public float damageChangeRate;

        // //队长
        // public BattleEntity teamLeader;
        public LinkGroupEffect linkGroupEffect;

        //当前的鼠标位置
        public Vector3 mousePos;

        //初始释放者坐标点
        public Vector3 initReleaserPos;

        //初始目标坐标点
        public Vector3 initTargetPos;

        public SkillEffectContext Copy()
        {
            var src = this;
            SkillEffectContext newContext = new SkillEffectContext();
            newContext.selectEntities = src.selectEntities?.ToList();
            newContext.selectPositions = src.selectPositions?.ToList();
            newContext.fromSkill = src.fromSkill;
            newContext.battle = src.battle;
            newContext.collisonGroupEffect = src.collisonGroupEffect;
            newContext.damage = src.damage;
            newContext.damageChangeRate = src.damageChangeRate;
            newContext.linkGroupEffect = src.linkGroupEffect;
            newContext.mousePos = src.mousePos;
            newContext.initReleaserPos = src.initReleaserPos;
            newContext.initTargetPos = src.initTargetPos;

            return newContext;
        }
    }

    public class SkillEffect
    {
        public int guid;

        public int configId;
        //SkillEffectType type;

        //从哪个技能释放出来的
        //protected Skill fromSkill;
        protected SkillEffectContext context;

        //public void SetFromSkill(Skill skill)
        //{
        //    this.fromSkill = skill;
        //}
        public bool isAutoDestroy;
        private SkillEffectState _state = SkillEffectState.Null;

        public SkillEffectState state
        {
            get { return _state; }
            set
            {
                var type = (SkillEffectType)(configId / 1000000);
                if (type == SkillEffectType.BuffEffect)
                {
                    //Battle_Log.LogZxy("change state : " + _state + " -> " + value);
                }

                _state = value;
            }
        }

        public void Init(int configId, int guid, SkillEffectContext context) //Skill skill
        {
            //this.fromSkill = skill;
            this.guid = guid;
            this.configId = configId;
            this.context = context;
            state = SkillEffectState.InitFinish;
            this.OnInit();
        }

        //创建信息 供客户端使用
        public virtual CreateEffectInfo ToCreateEffectInfo()
        {
            CreateEffectInfo createInfo = new CreateEffectInfo();
            createInfo.guid = guid;
            createInfo.resId = -1;
            createInfo.effectPosType = EffectPosType.Custom_Pos;
            createInfo.createPos = Vector3.zero;
            createInfo.followEntityGuid = -1;
            createInfo.isAutoDestroy = isAutoDestroy;

            return createInfo;
        }


        public bool IsValid()
        {
            var buff = this;
            var result = !(buff.state == SkillEffectState.Null ||
                           buff.state == SkillEffectState.WillEnd ||
                           buff.state == SkillEffectState.End ||
                           buff.state == SkillEffectState.Destroy);

            return result;
        }

        public void Start()
        {
            //on start effect
            state = SkillEffectState.Running;

            //_G.Log(string.Format("start effect configId({0}) , from skill {1}", this.configId,
            //  this.context.fromSkill.configId));

            this.OnStart();
        }

        public void Update(float timeDelta)
        {
            //update pos

            //triggerTimer over , effect

            if (this.state != SkillEffectState.Running)
            {
                return;
            }

            this.OnUpdate(timeDelta);
        }

        public void End()
        {
            //on end effect
            state = SkillEffectState.End;
            this.OnEnd();
        }

        public void Destroy()
        {
            state = SkillEffectState.Destroy;
            this.OnDestroy();
        }

        /////////////////////////////////////////

        public virtual void OnInit()
        {
        }

        public virtual void OnStart()
        {
        }

        public virtual void OnUpdate(float timeDelta)
        {
        }

        public virtual void OnEnd()
        {
        }

        public virtual void OnDestroy()
        {
        }

        protected virtual void SetWillEndState()
        {
            state = SkillEffectState.WillEnd;
            this.context.battle.DeleteSkillEffect(this);
        }

        public void ForceDelete()
        {
            this.SetWillEndState();
        }


        internal void SetContext(SkillEffectContext context)
        {
            this.context = context;
        }

        //被打断
        public virtual void Break()
        {
        }
    }
}