using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace Battle
{
    public partial class BattleEntity
    {
        #region Battle Var

        public int configId;
        public int guid;
        public int playerIndex;
        public ulong uid;
        public int level;
        public Vector3 position;
        public Vector3 dir = Vector3.forward;
        public float CurrHealth;
        public bool isPlayerCtrl;
        public IEntityInfo infoConfig;
        public Vector3 moveTargetPos;
        Battle battle;
        bool isShow;
        public BattleEntityRoleType roleType;


        public Action<EntityTriggerEventArg> attackStartEffectAction;
        public Action<EntityTriggerEventArg> normalAttackToOtherSuccessAction;
        public Action<EntityTriggerEventArg> beNormalAttackFromOtherSuccessAction;
        public Action<EntityTriggerEventArg> changeHealthAction;
        public Action<EntityTriggerEventArg> beforeDeadAction;
        public Action<EntityTriggerEventArg> afterDeadAction;
        public Action<EntityTriggerEventArg> avoidProjectileTriggerAction;
        public Action<EntityTriggerEventArg> avoidDamageTriggerAction;
        public Action<EntityTriggerEventArg> hurtToOtherAction;
        public Action<EntityTriggerEventArg> beHurtAction;
        public Action<EntityTriggerEventArg> summonEntityHurtToOtherAction;
        public Action<EntityTriggerEventArg> summonEntityBeHurtAction;
        public Action<EntityTriggerEventArg> collisionEntityActin;
        public Action<EntityTriggerEventArg> summonEntityAction;

        public int Team
        {
            get
            {
                var playerInfo = battle.FindPlayerByPlayerIndex(this.playerIndex);
                return playerInfo.team;
            }
        }

        private EntityState entityState;

        public EntityState EntityState
        {
            get => entityState;
            set
            {
                if (IsTrueDead())
                {
                    //死亡时不会再次改变状态(如果有需求再说)
                    return;
                }

                // if(this.isPlayerCtrl)
                //  Battle_Log.LogZxy("entity guid : " + this.guid + " : change state " + entityState + " -> " + value);
                entityState = value;
            }
        }

        public void ForceChangeState(EntityState state)
        {
            entityState = state;
        }

        #endregion

        private int willAddStarExp;
        public void Init(EntityInit entityInit, Battle battle)
        {
            this.battle = battle;

            //entityInit 填充
            this.configId = entityInit.configId;
            this.level = entityInit.level;

            //英雄星级处理
            var upgradeConfig = EntityUpgradeConfigHelper.GetConfig();
            var starExp = 0;
            var entityStarExp = 0;
            for (int i = 0; i < upgradeConfig.UpgradeExpPerStarLevel.Count; i++)
            {
                var exp = upgradeConfig.UpgradeExpPerStarLevel[i];
                if (entityInit.star >= i + 1)
                {
                    entityStarExp += exp;
                }
            }

             willAddStarExp = entityStarExp;
          
            // this.currStarExp = entityStarExp;
            
            this.position = entityInit.position;
            this.isPlayerCtrl = entityInit.isPlayerCtrl;
            if (entityInit.roleType == BattleEntityRoleType.Summon)
            {
                this.SetSummonEntity(entityInit.beSummonMaster, entityInit.summonLastTime);
            }
            else if (entityInit.roleType == BattleEntityRoleType.TeamMember)
            {
                this.SetTeamMemberEntity(entityInit.teamLeader, 0);
            }

            infoConfig = BattleConfigManager.Instance.GetById<IEntityInfo>(this.configId);

            if (null == infoConfig)
            {
                BattleLog.LogError("BattleEntity : Init : infoConfig is not found : configId : " + configId);
                return;
            }

            this.InitStar();

            this.InitAttrs();

            this.InitMovement();

            this.InitAbnormalState();

            this.InitOperateModule();

            SetCurrHp(this.MaxHealth);

            EntityState = EntityState.Idle;

            //初始化 skill
            this.InitSkill(entityInit.skillInitList);

            //初始化拥有的道具列表
            InitItemBar();

            //初始化技能书道具列表
            // InitSkillItemList();

            // this.InitBox();

            this.InitAI();

            SetMovementInfo();

            BattleLog.Log("entity init : guid : " + guid + " , name : " + this.infoConfig.Name);
            
            this.AddStarExp(entityStarExp);
        }


        public void Update(float deltaTime)
        {
            if (IsDead())
            {
                return;
            }

            //持续性的恢复状态的逻辑更新（如生命值恢复速度）
            RecoverStateUpdate(deltaTime);

            //技能更新
            UpdateSkills(deltaTime);

            //移动更新
            if (EntityState == EntityState.Move)
            {
                UpdateByMoveState(deltaTime);
            }

            //操作队列更新
            UpdateAskOperate(deltaTime);

            //ai 更新
            UpdateAI(deltaTime);


            //这里可以用继承 但是目前不想那么弄
            if (this.roleType == BattleEntityRoleType.Summon)
            {
                //如果是召唤物则更新召唤物逻辑
                UpdateSummon(deltaTime);
            }
            else if (this.roleType == BattleEntityRoleType.TeamMember)
            {
                UpdateTeamMember(deltaTime);
            }
        }

        public void ChangeToIdle(bool isSync = true)
        {
            var preState = this.EntityState;

            if (this.EntityState != EntityState.Idle)
            {
                this.EntityState = EntityState.Idle;

                this.OnStopMove();

                if (isSync)
                {
                    // if (this.isPlayerCtrl)
                    // Battle_Log.LogZxy("logic : stop move : " + this.position);
                    battle.OnEntityStopMove(this.guid, this.position);
                }
            }
        }
        //
        // public void GainReward(BaseBattleReward reward)
        // {
        //     reward.OnGain(this);
        // }

        internal void SetShowState(bool isShow)
        {
            this.isShow = isShow;
        }

        public void SetPosition(Vector3 targetPos)
        {
            this.position = targetPos;
            this.position.y = 0;
        }

        public void ToUnderstudy()
        {
        }


        public void SyncPos()
        {
            //先拿这个当位置的同步消息
            this.battle.OnEntityStopMove(this.guid, this.position);
        }

        public Battle GetBattle()
        {
            return this.battle;
        }

        public bool IsDead()
        {
            return this.entityState == EntityState.Dead ||
                   this.entityState == EntityState.WillDead;
        }

        public bool IsTrueDead()
        {
            return this.entityState == EntityState.Dead;
        }

        public void Revive()
        {
            this.EntityState = EntityState.Idle;
            battle.OnEntityRevive(this);

            SetCurrHp(this.MaxHealth);
            this.SyncBattleData();
        }

        //清理函数
        internal void Clear()
        {
            // this.itemBar.Clear();
        }

        // 普通攻击加成列表
        public Dictionary<int, AddedDamageGroup> normalAttackAddedDamageDic;

        public void AddNormalAttackAddedDamage(int id, DamageCalculate damageCalculate)
        {
            if (normalAttackAddedDamageDic == null)
            {
                normalAttackAddedDamageDic = new Dictionary<int, AddedDamageGroup>();
            }

            if (!normalAttackAddedDamageDic.ContainsKey(id))
            {
                normalAttackAddedDamageDic.Add(id, new AddedDamageGroup()
                {
                    id = id,
                    damageCalculateList = new List<DamageCalculate>() { damageCalculate }
                });
            }
            else
            {
                normalAttackAddedDamageDic[id].damageCalculateList.Add(damageCalculate);
            }
        }

        public void RemoveNormalAttackAddedDamage(int id)
        {
            if (normalAttackAddedDamageDic != null && normalAttackAddedDamageDic.ContainsKey(id))
            {
                normalAttackAddedDamageDic.Remove(id);
            }
        }
    }

    public class AddedDamageGroup
    {
        public int id;
        public List<DamageCalculate> damageCalculateList = new List<DamageCalculate>();
    }
}