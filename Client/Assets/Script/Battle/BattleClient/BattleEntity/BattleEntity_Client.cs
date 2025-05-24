using System.Collections;
using System.Collections.Generic;
using Battle;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

namespace Battle_Client
{
    public partial class BattleEntity_Client
    {
        public int guid;
        public int configId;
        public int playerIndex;
        public int level;
        public float CurrHealth;
        public int Team => BattleManager.Instance.GetTeamByPlayerIndex(this.playerIndex);
        public float MaxHealth => attr.GetValue(EntityAttrType.MaxHealth);

        private BattleEntityState state = BattleEntityState.Idle;

        public BattleEntityState State
        {
            get => state;
            set
            {
                var pre = state;
                state = value;

                
               // Logx.Log($"entity client : {this.guid} state change : {pre} -> {value}");
            }
        }

        public GameObject gameObject;
        public Collider collider;
        
        public float deadDisappearCurrTimer = 0;
        public float deadDisappearTotalTime = 2.0f;

        public int starLv;
        public int starExp;
        
        //初始化
        public void Init(int guid, int configId)
        {
            this.guid = guid;
            this.configId = configId;

            InitModel();
            
            isFinishLoad = false;
            
            InitItemList();

            InitAttr();

            InitBuffs();

            var dir = this.gameObject.transform.forward;
            dirTarget = new Vector3(dir.x,0,dir.z);
        }


        public void Update(float timeDelta)
        {
            SetDirTarget(new Vector3(dirTarget.x, 0, dirTarget.z));
            SetTrueDir(timeDelta);

            //Battle._Battle_Log.Log("zxy path test : dir : " + this.gameObject.transform.forward.x + "," + this.gameObject.transform.forward.z + " -> " + dirTarget);
            if (State == BattleEntityState.Move || isForceSkillMove)
            {
                UpdateMove(timeDelta);
            }

            if (triggerNames.Count > 0)
            {
                var str = triggerNames[^1];
                this.animator.SetTrigger(str);
                triggerNames.Clear();
            }

            if (this.State == BattleEntityState.Dead)
            {
                deadDisappearCurrTimer -= timeDelta;
                if (deadDisappearCurrTimer <= 0)
                {
                    this.State = BattleEntityState.Destroy;


                    //这里应该是设置成标志 然后删除
                    // BattleEntityManager.Instance.DestoryEntity(this.guid);
                }
            }
        }

        
        internal void SetPlayerIndex(int playerIndex)
        {
            this.playerIndex = playerIndex;
        }

        public static EntityRelationType GetRelation(BattleEntity_Client aEntity, BattleEntity_Client bEntity)
        {
            if (null == aEntity || null == bEntity)
            {
                return EntityRelationType.Self;
            }

            if (aEntity.guid == bEntity.guid)
            {
                return EntityRelationType.Self;
            }
            else
            {
                if (aEntity.Team == bEntity.Team)
                {
                    return EntityRelationType.Friend;
                }
                else
                {
                    return EntityRelationType.Enemy;
                }
            }
        }
        
        internal void SetShowState(bool isShow)
        {
            gameObject.SetActive(isShow);
        }
        
        public IEnumerator RemoveSelf()
        {
            yield return new WaitForSeconds(2);

            //这里应该是设置成标志 然后删除
            BattleEntityManager.Instance.DestoryEntity(this.guid);
        }

        public void Dead(bool isTrueDead )
        {
            PlayAnimation("die");
            // state = BattleEntityState.Dead;
            if (isTrueDead)
            {
                State = BattleEntityState.Dead;
            }
            else
            {
                State = BattleEntityState.WillDead;
            }
            
            deadDisappearCurrTimer = deadDisappearTotalTime;

            EventDispatcher.Broadcast(EventIDs.OnEntityDead, this);


            // CoroutineManager.Instance.StartCoroutine(RemoveSelf());
        }
        
        
        public void OnRevive()
        {
            State = BattleEntityState.Idle;
            PlayAnimation("idle");
        }
        
        public void Destroy()
        {
            this.State = BattleEntityState.Destroy;
            if (isFinishLoad)
            {
                ResourceManager.Instance.ReturnObject(path, model);
                //model.transform.SetParent(GameMain.Instance.gameObjectRoot);
            }

            GameObject.Destroy(gameObject);

            EventDispatcher.Broadcast(EventIDs.OnEntityDestroy, this);
        }

        
    }
}