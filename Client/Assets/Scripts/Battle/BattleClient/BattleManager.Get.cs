using System.Collections.Generic;
using UnityEngine;

namespace Battle_Client
{
    public partial class BattleManager
    {
         
        public int GetCtrlHeroSkillIdByIndex(int index)
        {
            return this.localCtrlEntity.GetSkillIdByIndex(index);
        }

        public GameObject GetLocalCtrlHeroGameObject()
        {
            return this.localCtrlEntity?.gameObject;
        }

        public BattleEntity_Client GetLocalCtrlHero()
        {
            var localCtrlHeroGameObject = BattleManager.Instance.GetLocalCtrlHeroGameObject();
            var localInstanceID = localCtrlHeroGameObject.GetInstanceID();
            var localEntity = BattleEntityManager.Instance.FindEntityByInstanceId(localInstanceID);

            return localEntity;
        }

        public int GetLocalCtrlHeroGuid()
        {
            return this.localCtrlEntity.guid;
        }

        public BattleEntityAttr GetLocalCtrlHeroAttrs()
        {
            return this.localCtrlEntity.attr;
        }

        public List<BattleSkillInfo> GetLocalCtrlHeroSkills()
        {
            return this.localCtrlEntity.GetSkills();
        }

        // public List<BattleItemInfo> GetLocalCtrlHeroItems()
        // {
        //     return this.localCtrlEntity.GetItems();
        // }
        //
        // public List<BattleItemInfo> GetLocalCtrlHeroSkillItems()
        // {
        //     return this.localCtrlEntity.GetSkillItems();
        // }

        public BattleSkillInfo FindLocalHeroSkill(int skillId)
        {
            var skills = BattleManager.Instance.GetLocalCtrlHeroSkills();
            foreach (var skill in skills)
            {
                if (skill.configId == skillId)
                {
                    return skill;
                }
            }

            return null;
        }

        public ClientPlayer GetLocalPlayer()
        {
            return this.localPlayer;
        }
        
        internal Battle.Battle GetBattle()
        {
            return this.localBattleExecuter.GetBattle();
        }

        public int GetTeamByPlayerIndex(int playerIndex)
        {
            foreach (var item in playerDic)
            {
                var player = item.Value;
                if (player.playerIndex == playerIndex)
                {
                    return player.team;
                }
            }

            return -1;
        }

        public bool IsSameTeam(int index0, int index1)
        {
            var team = GetTeamByPlayerIndex(index0);
            var team2 = GetTeamByPlayerIndex(index1);

            return team == team2;
        }

        public List<ClientPlayer> GetAllPlayerList()
        {
            return this.playerList;
        }

        public bool IsLocalBattle()
        {
            return localBattleExecuter != null;
        }

       
    }
}