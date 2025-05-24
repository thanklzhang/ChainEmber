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
        //作为队长------
        private Dictionary<int, BattleEntity> teamMemberEntityDic = new Dictionary<int, BattleEntity>();
        private List<BattleEntity> teamMemberEntityList = new List<BattleEntity>();

        //作为队员-------
        public BattleEntity teamLeader;

        //设置为成员
        public void SetTeamMemberEntity(BattleEntity leader, float lastTime)
        {
            teamLeader = leader;
            roleType = BattleEntityRoleType.TeamMember;
        }

        public Dictionary<int, BattleEntity> GetTeamMemberDic()
        {
            return teamMemberEntityDic;
        }

        public void ReviveAllTeamMember()
        {
            foreach (var member in teamMemberEntityList)
            {
                if (member.IsDead())
                {
                    member.Revive();
                }
            }
        }

        public List<BattleEntity> GetTeamMemberList()
        {
            return teamMemberEntityList;
        }

        public BattleEntity GetMemberEntityByConfigId(int configId)
        {
            var entity = teamMemberEntityList.Find(e => e.configId == configId);
            return entity;
        }

        public void UpdateTeamMember(float deltaTime)
        {
            // if (summonMaxLastTime <= 0)
            // {
            //     return;
            // }
            //
            // currSummonLasterTimer -= deltaTime;
            // if (currSummonLasterTimer <= 0)
            // {
            //     SummonEntityTimeToDead();
            // }
        }

        // public void SummonEntityTimeToDead()
        // {
        //     this.OnBeHurt(0, null, DamageFromType.SummonTimeToDead);
        // }

        //作为队长 增加该实体的队员
        public void AddTeamMemberEntity(BattleEntity entity)
        {
            this.teamMemberEntityDic.TryAdd(entity.guid, entity);
            teamMemberEntityList.Add(entity);
            // RefreshSummonEntityDicState();
        }

        //作为队长 移除该实体的队员
        public void RemoveTeamMemberEntity(int entityGuid)
        {
            this.teamMemberEntityDic.Remove(entityGuid);
            teamMemberEntityList.RemoveAll(t => t.guid == entityGuid);
        }
    }
}