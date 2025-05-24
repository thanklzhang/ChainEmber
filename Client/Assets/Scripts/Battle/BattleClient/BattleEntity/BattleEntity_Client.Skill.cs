using System.Collections.Generic;
using Battle;
using Config;
using UnityEngine;

namespace Battle_Client
{
    //技能相关
    public partial class BattleEntity_Client
    {
        List<BattleSkillInfo> skills;

        internal void SetSkillList(List<BattleSkillInfo> skills)
        {
            this.skills = skills;
        }

        public List<BattleSkillInfo> GetSkills()
        {
            return skills;
        }


        public BattleSkillInfo FindSkill(int skillConfigId)
        {
            var skill = this.skills.Find((s) => { return s.configId == skillConfigId; });

            return skill;
        }

        public bool IsHeroCtrl()
        {
            var player = BattleManager.Instance.GetLocalPlayer();
            if (player.ctrlHeroGuid == this.guid)
            {
                return true;
            }

            return false;
        }

        internal void UpdateSkillInfo(SkillInfoUpdate_RecvMsg_Arg arg)
        {
            // if (arg.skillConfigId == 2100101 || 2100102 == arg.skillConfigId)
            // {
            //     Battle_Log.LogZxy("");
            // }

            var skillConfig = Config.ConfigManager.Instance.GetById<Config.Skill>(arg.skillConfigId);
            // if ((SkillCategory)skillConfig.SkillCategory == SkillCategory.MinorSkill)
            // {
            //     Logx.Log(arg.skillConfigId + " is minor skill");
            // }

            var skill = this.FindSkill(arg.skillConfigId);
            if (skill != null)
            {
               

                if (arg.isDelete)
                {
                    this.skills.Remove(skill);
                    if (IsHeroCtrl())
                    {
                        var player = BattleManager.Instance.GetLocalPlayer();
                        player.RemoveSkillInput(skill.configId);
                    }
                    
                    EventDispatcher.Broadcast(EventIDs.OnSkillRefreshAll);

                }
                
                skill?.UpdateInfo(arg);

                //player input remove skill

                // SortSkill();
            }
            else
            {
                skill = new BattleSkillInfo();
                skill.configId = arg.skillConfigId;
                skill.releaserGuid = this.guid;
                skill.exp = arg.exp;
                // skill.level = 1;

                this.skills.Add(skill);
                // SortSkill();

                //player input add skill by index
                if (IsHeroCtrl())
                {
                    var player = BattleManager.Instance.GetLocalPlayer();
                    player.TryToAddSkillInput(skill.configId);
                }

                skill?.UpdateInfo(arg);
                
                EventDispatcher.Broadcast(EventIDs.OnSkillRefreshAll);
            }
        }

        void SortSkill()
        {
            this.skills.Sort((a, b) => { return a.showIndex.CompareTo(b.showIndex); });
        }

        public void UpdateSkillInput()
        {
            var player = BattleManager.Instance.GetLocalPlayer();
            if (this.guid == player.ctrlHeroGuid)
            {
                player.UpdateSkillInput();
            }
        }


        internal void ReleaseSkill(int skillConfigId)
        {
            var skillConfig = Config.ConfigManager.Instance.GetById<Config.Skill>(skillConfigId);
            var normalAttackSkill = FindSkill(SkillCategory.NormalAttack);
            if (normalAttackSkill != null && normalAttackSkill.configId == skillConfig.Id)
            {
                //普通攻击
                var attackSpeed = this.attr.GetValue(EntityAttrType.AttackSpeed);
                var aniScale = attackSpeed * (skillConfig.AnimationSpeedScale / 1000.0f);

                PlayAnimation("attack", aniScale);
            }
            else
            {
                //技能
                var aniScale = skillConfig.AnimationSpeedScale / 1000.0f;
                PlayAnimation("attack", aniScale);
            }
            //play animation
            //Logx.Log(this.guid + " release skill : " + skillConfigId);
        }

        // public BattleSkillInfo GetNormalAttackSkill()
        // {
        //     if (this.skills.Count > 0)
        //     {
        //         return this.skills[0];
        //     }
        //
        //     return null;
        // }

        internal int GetSkillIdByIndex(int index)
        {
            if (skills.Count > 0)
            {
                return skills[index].configId;
            }
            else
            {
                Logx.LogWarning("the count of skills is 0 : index : " + index);
                return -1;
            }
        }

        // public void OnAbnormalEffect(AbnormalStateBean stateBean)
        // {
        //     EventDispatcher.Broadcast<BattleEntity_Client,AbnormalStateBean>(EventIDs.OnEntityAbnormalEffect,
        //         this,stateBean);
        // }

        public BattleSkillInfo FindSkill(SkillCategory category)
        {
            return skills.Find(skill =>
            {
                var skillConfig = ConfigManager.Instance.GetById<Config.Skill>(skill.configId);
                if (skillConfig != null)
                {
                    return (SkillCategory)skillConfig.SkillCategory == category;
                }

                return false;
            });
        }

        public List<BattleSkillInfo> FindSkills(SkillCategory category)
        {
            return skills.FindAll(skill =>
            {
                var skillConfig = ConfigManager.Instance.GetById<Config.Skill>(skill.configId);
                if (skillConfig != null)
                {
                    return (SkillCategory)skillConfig.SkillCategory == category;
                }

                return false;
            });
        }
    }


    public class BattleSkillInfo
    {
        public int releaserGuid;
        public int configId;
        public int exp;
        public float maxCDTime;
        public float currCDTime;
        public bool isDelete;
        public int showIndex;

        internal void UpdateInfo(SkillInfoUpdate_RecvMsg_Arg arg)
        {
            this.currCDTime = arg.currCDTime;
            this.maxCDTime = arg.maxCDTime;
            this.exp = arg.exp;
            this.isDelete = arg.isDelete;
            this.showIndex = arg.showIndex;

            EventDispatcher.Broadcast(EventIDs.OnSkillInfoUpdate, releaserGuid, this);
        }
    }
}