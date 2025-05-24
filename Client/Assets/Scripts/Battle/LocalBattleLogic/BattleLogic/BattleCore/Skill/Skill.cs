using System.Collections.Generic;

namespace Battle
{
    public class CreateSkillBean
    {
        //需要填充-----
        public BattleEntity releaser;
        public int configId;
        public int currExp;

        //可忽略
        public int showIndex;
    }


    public partial class Skill
    {
        public int configId;

        // public int level;
        public int currExp = 1;
        public int maxExp;
        public int showIndex;
        public int targetGuid;
        public Vector3 targetPos;
        public Vector3 mousePos;
        public BattleEntity releaser;
        public ISkill infoConfig;
        internal bool isNormalAttack => (SkillCategory)infoConfig.SkillCategory == SkillCategory.NormalAttack;
        public ReleaseSkillState state = ReleaseSkillState.ReadyRelease;
        float currBeforeReleaseTimer = 0;
        float currAfterReleaseTimer = 0;
        float currCDTimer = 0;
        protected Battle battle;
        public Dictionary<int, SkillEffect> immediatelyTriggerEftList = new Dictionary<int, SkillEffect>();
        private SkillEffect endEffect = null;

        public virtual void Init(CreateSkillBean createInfo)
        {
            //infoConfig = ConfigManager.Instance.GetById<Config.Skill>(configId);
            this.configId = createInfo.configId;
            this.releaser = createInfo.releaser;
            // this.currExp = createInfo.currExp;

            //技能经验初始就是 1
            this.currExp = 1;

            //skillEffectList = new List<SkillEffect>();
            this.battle = releaser.GetBattle();
            this.showIndex = createInfo.showIndex;
            this.infoConfig = BattleConfigManager.Instance.GetById<ISkill>(this.configId);

            TriggerSkillEffectOnInit();
        }

        public void TriggerSkillEffectOnInit()
        {
            //被动技能(其实可以判断出来 不过为了突出被动 增加了该字段)
            //TODO 可以用立即触发效果列表代替这个
            SkillEffectContext context = new SkillEffectContext();
            if (1 == this.infoConfig.IsPassiveSkill)
            {
                context.battle = this.GetBattle();
                context.fromSkill = this;

                // foreach (var item in infoConfig.EffectList)
                // {
                //     var effectId = item;
                //     SkillEffectContext context = new SkillEffectContext();
                //     context.battle = this.GetBattle();
                //     context.fromSkill = this;
                //     battle.AddSkillEffect(effectId, context);
                // }

                battle.AddSkillEffectGroup(infoConfig.EffectList, context);
            }

            //立即触发效果列表
            context = new SkillEffectContext();
            context.battle = this.GetBattle();
            context.fromSkill = this;
            //这里默认施法者
            context.selectEntities = new List<BattleEntity>()
            {
                this.releaser
            };
            foreach (var item in infoConfig.EffectListOnGain)
            {
                // var effectId = item;
                // SkillEffectContext context = new SkillEffectContext();
                // context.battle = this.GetBattle();
                // context.fromSkill = this;
                // //这里默认施法者
                // context.selectEntities = new List<BattleEntity>()
                // {
                //     this.releaser
                // };
                // var eft = battle.AddSkillEffect(effectId, context);
                // immediatelyTriggerEftList.Add(eft.guid,eft);
            }

            battle.AddSkillEffectGroup(infoConfig.EffectListOnGain, context);
        }

        public bool Start(int targetGuid, Vector3 targetPos, Vector3 mousePos)
        {
            //_G.Log(string.Format("{0} release skill({1}) to target({2})", this.releser.guid,
            //    this.configId, targetGuid));

            this.targetGuid = targetGuid;
            this.targetPos = targetPos;
            this.mousePos = mousePos;

            var battle = releaser.GetBattle();

            bool isImmediatelyRelease = (SkillReleaseType)this.infoConfig.SkillReleaseType ==
                                        SkillReleaseType.ImmediatelyRelease;
            if (isImmediatelyRelease)
            {
                //瞬发技能直接释放 无需多言
                this.StartReleaseSkillEffect();
                state = ReleaseSkillState.CD;
                // this.currCDTimer = GetCDTimerTotalTime();
            }
            else
            {
                state = ReleaseSkillState.SkillBefore;
            }

            this.currBeforeReleaseTimer = GetSkillBeforeTotalTime();

            // this.currCDTimer = GetCDMaxTime();

            //先设置好 CD 时间 , 释放后开始计时
            this.currCDTimer = GetCDTotalTime();

            NotifySkillTrackStart(SkillTrackStartTimeType.SkillBeforeStart);

            return true;
        }

        public void Update(float timeDelta)
        {
            UpdateSkillProcess(timeDelta);
        }

        public void StartReleaseSkillEffect()
        {
            //前摇结束 释放技能

            SkillEffectContext context = new SkillEffectContext();
            context.battle = this.GetBattle();
            context.fromSkill = this;
            context.selectEntities = new List<BattleEntity>();
            context.selectPositions = new List<Vector3>();
            context.mousePos = mousePos;

            //技能目标 这里目标现在这个列表中所有效果统一用一个目标,如果需要独立需要该逻辑
            var skillTargetType = (SkillEffectTargetType)this.infoConfig.SkillEffectTargetType;
            if (skillTargetType == SkillEffectTargetType.SkillTargetEntity)
            {
                var targetEntity = this.GetBattle().FindEntity(this.targetGuid);
                if (targetEntity != null)
                {
                    context.selectEntities.Add(targetEntity);
                }
            }
            else if (skillTargetType == SkillEffectTargetType.SkillReleaserEntity)
            {
                context.selectEntities.Add(this.releaser);
            }
            else if (skillTargetType == SkillEffectTargetType.SkillTargetPos)
            {
                context.selectPositions.Add(this.targetPos);
            }

            //设置初始相关带点
            context.initReleaserPos = this.releaser.position;
            context.initTargetPos = this.targetPos;
            // if (targetGuid > 0)
            // {
            //     var targetEntity = this.GetBattle().FindEntity(this.targetGuid);
            //     if (targetEntity != null)
            //     {
            //         context.initTargetPos = targetEntity.position;
            //     }
            // }

            battle.AddSkillEffectGroup(infoConfig.EffectList, context);


            if (this.isNormalAttack)
            {
                var targetEntity = this.GetBattle().FindEntity(this.targetGuid);
                releaser.OnNormalAttackStartEffect(targetEntity);
            }
        }

        public void Break()
        {
            if (this.endEffect != null)
            {
                //由子效果决定技能结束
                this.endEffect.Break();
                this.endEffect = null;
                return;
            }

            var isConfigSuit = 0 == this.infoConfig.IsNoBreak;
            var isStateSuit = this.state == ReleaseSkillState.SkillBefore ||
                              this.state == ReleaseSkillState.Releasing ||
                              this.state == ReleaseSkillState.SkillAfter;

            var isCanBeBreak = isConfigSuit && isStateSuit;

            if (isCanBeBreak)
            {
                FinishSkillRelease();
            }
        }

        //释放技能完成（后摇也完成了）
        public void FinishSkillRelease()
        {
            //this.releser.ChangeToIdle();

            this.OnFinishSkillEffect();
            //this.releser.ChangeToIdle();

            if (this.state == ReleaseSkillState.CD)
            {
                return;
            }

            this.state = ReleaseSkillState.CD;

            // var releaseType = (SkillReleaseType)this.infoConfig.SkillReleaseType;
            // if (releaseType == SkillReleaseType.LastRelease)
            // {
            //     this.battle.OnSkillInfoUpdate(this);
            // }
            // if ((SkillCategory)this.infoConfig.SkillCategory == SkillCategory.MinorSkill)
            // {
            //     Battle_Log.LogZxy("");
            // }

            this.battle.OnSkillInfoUpdate(this);


            //track
            NotifySkillTrackEnd(SkillTrackEndTimeType.SkillFinishAllProcess);

            this.releaser.OnSkillReleaseEnd(this);
        }

        //设置是否为子效果决定技能结束的标志
        public void SetSubEndffect(SkillEffect endEffect)
        {
            this.endEffect = endEffect;
        }

        //失去效果
        public void Remove()
        {
            // if (1 == this.infoConfig.IsPassiveSkill)
            {
                //移除 buff 和 被动技能
                var eftIdList = infoConfig.EffectList;
                for (int i = 0; i < eftIdList.Count; i++)
                {
                    var eftId = eftIdList[i];
                    battle.DeleteBuffFromEntity(releaser.guid, eftId);
                    this.releaser.ForceRemovePassiveEffect(eftId);
                }

                eftIdList = infoConfig.EffectListOnGain;
                for (int i = 0; i < eftIdList.Count; i++)
                {
                    var eftId = eftIdList[i];
                    battle.DeleteBuffFromEntity(releaser.guid, eftId);
                    this.releaser.ForceRemovePassiveEffect(eftId);
                }


                // foreach (var kv in immediatelyTriggerEftList)
                // {
                //     var eft = kv.Value;
                //     eft.ForceDelete();
                // }
            }

            // //立即触发效果列表
            // foreach (var kv in immediatelyTriggerEftList)
            // {
            //     var eft = kv.Value;
            //     eft.ForceDelete();
            // }
        }
    }
}