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
        //作为召唤者------
        public Dictionary<int, BattleEntity> summonEntityDic = new Dictionary<int, BattleEntity>();

        //作为召唤物-------
        public BattleEntity beSummonMaster;
        // public bool isSummonEntity;
        public float summonMaxLastTime;
        public float currSummonLasterTimer;

        //设置为召唤物
        public void SetSummonEntity(BattleEntity master, float lastTime)
        {
            beSummonMaster = master;
            roleType = BattleEntityRoleType.Summon;

            summonMaxLastTime = lastTime;
            currSummonLasterTimer = summonMaxLastTime;
        }

        public void UpdateSummon(float deltaTime)
        {
            if (summonMaxLastTime <= 0)
            {
                return;
            }

            currSummonLasterTimer -= deltaTime;
            if (currSummonLasterTimer <= 0)
            {
                SummonEntityTimeToDead();
            }
        }

        public void SummonEntityTimeToDead()
        {
            this.OnBeHurt(0, null, DamageFromType.SummonTimeToDead);
        }

        public void AddSummonEntity(BattleEntity entity)
        {
            this.summonEntityDic.TryAdd(entity.guid, entity);
            RefreshSummonEntityDicState();
        }

        public void RefreshSummonEntityDicState()
        {
            List<BattleEntity> willDelList = new List<BattleEntity>();
            foreach (var kv in summonEntityDic)
            {
                var currEntity = kv.Value;

                if (currEntity.IsDead())
                {
                    willDelList.Add(currEntity);
                }
            }

            for (int i = 0; i < willDelList.Count; i++)
            {
                var delEntity = willDelList[i];
                summonEntityDic.Remove(delEntity.guid);
            }
        }

        public int GetSummonEntityCount(int entityConfigId)
        {
            int count = 0;
            foreach (var kv in summonEntityDic)
            {
                var entity = kv.Value;
                if (!entity.IsDead())
                {
                    if (EntityTool.IsSameOriginConfigId(entity.configId,entityConfigId))
                    {
                        count += 1;
                    }
                }
            }

            return count;
        }

        public void ForceSummonEntityDead(int entityConfigId, int count)
        {
            List<BattleEntity> willDelList = new List<BattleEntity>();
            foreach (var kv in summonEntityDic)
            {
                var entity = kv.Value;
                if (!entity.IsDead())
                {
                    if (EntityTool.IsSameOriginConfigId(entity.configId, entityConfigId))
                    {
                        willDelList.Add(entity);
                    }
                }
            }

            //优先选择血量百分比低的
            willDelList.Sort((a, b) =>
            {
                var aPercent = a.GetCurrHp() / a.MaxHealth;
                var bPercent = b.GetCurrHp() / b.MaxHealth;

                return aPercent.CompareTo(bPercent);
            });

            for (int i = 0; i < count; i++)
            {
                if (i >= willDelList.Count)
                {
                    break;
                }

                var entity = willDelList[i];
                entity.OnBeHurt(0, null, DamageFromType.SummonOverLimitCount);
            }
        }

        public bool IsMySummonEntity(BattleEntity other)
        {
            if (this.summonEntityDic != null && this.summonEntityDic.ContainsKey(other.guid))
            {
                return true;
            }

            return false;
        }

        public bool IsSummonEntity()
        {
            return this.beSummonMaster != null;
        }
    }
}