using System.Collections.Generic;
using Battle;
using Vector3 = UnityEngine.Vector3;

namespace Battle_Client
{
    //队伍相关
    public partial class ClientPlayer
    {
        
        public List<int> teamMemberGuids = new List<int>();
        public void UpdatePlayerTeamMembersInfo(List<int> entityGuids)
        {
            teamMemberGuids = entityGuids;
            EventDispatcher.Broadcast(EventIDs.OnUpdatePlayerTeamMembersInfo,
                teamMemberGuids);

        }
        
        public void OnOperateHero(OperateHeroByArraying_RecvMsg_Arg arg)
        {
          
            Vector3 pos = arg.targetPos; //
            var opHeroGuid = arg.opHeroGuid; //
            int toUnderstudyIndex = arg.toUnderstudyIndex;

            Logx.Log(LogxType.Zxy,$"client : recv msg : pos:{pos} opHeroGuid:{opHeroGuid} toUnderstudyIndex:{toUnderstudyIndex}");
            
            if (opHeroGuid > 0)
            {
                var targetEntity = BattleEntityManager.Instance.FindEntity(opHeroGuid);
                if (targetEntity != null)
                {
                    if (toUnderstudyIndex >= 0)
                    {
                        // /
                        //没有替补了
                        //UnderstudyManager_Client.Instance.SetLocation(targetEntity, toUnderstudyIndex);
                    }
                    else
                    {
                        //去战场
                        var pre = targetEntity.GetPosition();
                        targetEntity.SetPosition(new Vector3(
                            pos.x, pre.y, pos.z));
                    }
                }
            }
            else
            {
                if (toUnderstudyIndex >= 0)
                {
                    //没有替补了
                    //UnderstudyManager_Client.Instance.SetLocation(null, toUnderstudyIndex);
                }
            }

           
        }
        
        //本地玩家和目标单位的关系
        public EntityRelationType GetRelationFromEntity(BattleEntity_Client entity)
        {
            
            var selfPlayerIndex = this.playerIndex;
            bool isSelf = selfPlayerIndex == entity.playerIndex;
            bool isSameTeam = BattleManager.Instance.IsSameTeam(selfPlayerIndex,entity.playerIndex);
            EntityRelationType relationType = EntityRelationType.Friend;
            if (isSelf)
            {
                relationType = EntityRelationType.Self;
            }
            else if (isSameTeam)
            {
                relationType = EntityRelationType.Friend;
            }
            else
            {
                relationType = EntityRelationType.Enemy;
            }
            
            return relationType;
        }
    }
}