using System.Collections.Generic;
using Battle;
using Config;
using UnityEngine;

namespace Battle_Client
{
    //玩家偏好设置相关
    public partial class ClientPlayer
    {
        public Dictionary<PlayerInputType, PlayerCommandType> inputMappingDic;
        public Dictionary<PlayerCommandType, PlayerInputCommandModel> inputCommandDic;

        //TODO 技能初始化之后调用
        public void InitPreference()
        {
            inputMappingDic = new Dictionary<PlayerInputType, PlayerCommandType>();
            inputCommandDic = new Dictionary<PlayerCommandType, PlayerInputCommandModel>();
        }

        public void SetPreference()
        {
            SetSkillPreference();
        }

        public void SetSkillPreference()
        {
            var hero = BattleManager.Instance.GetLocalCtrlHero();
            var normalAttackSkill = hero.FindSkill(SkillCategory.NormalAttack);
            var minorSkill = hero.FindSkill(SkillCategory.MinorSkill);
            var leaderSkills = hero.FindSkills(SkillCategory.LeaderSkill);


            var leaderSkill1 = leaderSkills.Count > 0 ? leaderSkills[0] : null;
            var leaderSkill2 = leaderSkills.Count > 1 ? leaderSkills[1] : null;
            var ultimateSkill = hero.FindSkill(SkillCategory.UltimateSkill);

            inputMappingDic.Add(PlayerInputType.KeyCode_A, PlayerCommandType.NormalAttack);
            inputMappingDic.Add(PlayerInputType.KeyCode_Q, PlayerCommandType.Skill_Minor);
            inputMappingDic.Add(PlayerInputType.KeyCode_W, PlayerCommandType.Skill_Leader_1);
            inputMappingDic.Add(PlayerInputType.KeyCode_E, PlayerCommandType.Skill_Leader_2);
            inputMappingDic.Add(PlayerInputType.KeyCode_R, PlayerCommandType.Skill_Ultimate);


            AddInputCommand(PlayerCommandType.NormalAttack, new SkillCommandModel
            {
                commandType = PlayerCommandType.NormalAttack,
                skillConfigId = normalAttackSkill?.configId ?? 0
            });
            AddInputCommand(PlayerCommandType.Skill_Minor, new SkillCommandModel
            {
                commandType = PlayerCommandType.Skill_Minor,
                skillConfigId = minorSkill?.configId ?? 0
            });
            AddInputCommand(PlayerCommandType.Skill_Leader_1, new SkillCommandModel
            {
                commandType = PlayerCommandType.Skill_Leader_1,
                skillConfigId = leaderSkill1?.configId ?? 0
            });
            AddInputCommand(PlayerCommandType.Skill_Leader_2, new SkillCommandModel
            {
                commandType = PlayerCommandType.Skill_Leader_2,
                skillConfigId = leaderSkill2?.configId ?? 0
            });
            AddInputCommand(PlayerCommandType.Skill_Ultimate, new SkillCommandModel
            {
                commandType = PlayerCommandType.Skill_Ultimate,
                skillConfigId = ultimateSkill?.configId ?? 0
            });
        }

        public void AddInputCommand(PlayerCommandType commanType, PlayerInputCommandModel model)
        {
            model.commandType = commanType;
            if (!this.inputCommandDic.ContainsKey(commanType))
            {
                this.inputCommandDic.Add(commanType, model);
            }
            else
            {
                Logx.LogWarning(LogxType.Game, "has exist commanType : " + commanType);
            }
        }

        public PlayerCommandType GetCommandByInputType(PlayerInputType inputType)
        {
            if (inputMappingDic.ContainsKey(inputType))
            {
                return inputMappingDic[inputType];
            }

            return PlayerCommandType.Null;
        }

        public PlayerInputCommandModel GetCommandModelByInputType(PlayerInputType inputType)
        {
            var commandType = GetCommandByInputType(inputType);
            return GetInputCommand(commandType);
        }

        public PlayerInputCommandModel GetInputCommand(PlayerCommandType commandType)
        {
            if (this.inputCommandDic.ContainsKey(commandType))
            {
                return this.inputCommandDic[commandType];
            }

            return null;
        }

        public SkillCommandModel GetCommandModelBySkillId(int skillId)
        {
            foreach (var kv in this.inputCommandDic)
            {
                var commandModel = kv.Value;
                if (commandModel is SkillCommandModel)
                {
                    var skillCM = commandModel as SkillCommandModel;
                    if (skillCM.skillConfigId == skillId)
                    {
                        return skillCM;
                    }
                }
            }

            return null;
        }

        public void RemoveSkillInput(int skillId)
        {
            foreach (var kv in this.inputCommandDic)
            {
                var commandModel = kv.Value;
                if (commandModel is SkillCommandModel)
                {
                    var skillCM = commandModel as SkillCommandModel;
                    if (skillCM.skillConfigId == skillId)
                    {
                        skillCM.skillConfigId = 0;
                        break;
                    }
                }
            }
        }

        public void TryToAddSkillInput(int skillId)
        {
            var skillConfig = ConfigManager.Instance.GetById<Config.Skill>(skillId);
            var skillCategory = (SkillCategory)skillConfig.SkillCategory;

            if (skillCategory == SkillCategory.MinorSkill)
            {
                var cm = inputCommandDic[PlayerCommandType.Skill_Minor];
                if (cm is SkillCommandModel)
                {
                    (cm as SkillCommandModel).skillConfigId = skillId;
                }
            }
            else if(skillCategory == SkillCategory.UltimateSkill)
            {
                var cm = inputCommandDic[PlayerCommandType.Skill_Ultimate];
                if (cm is SkillCommandModel)
                {
                    (cm as SkillCommandModel).skillConfigId = skillId;
                }
            }
            else if(skillCategory == SkillCategory.LeaderSkill)
            {
                bool isHasSet = false;
                var cm = inputCommandDic[PlayerCommandType.Skill_Leader_1];
                if (cm is SkillCommandModel)
                {
                    var skillCM = (cm as SkillCommandModel);
                    var isEmpty = skillCM.skillConfigId <= 0;
                    if (isEmpty)
                    {
                        skillCM.skillConfigId = skillId;
                        isHasSet = true;
                    }
                }

                if (!isHasSet)
                {
                    cm = inputCommandDic[PlayerCommandType.Skill_Leader_2];
                    if (cm is SkillCommandModel)
                    {
                        var skillCM = (cm as SkillCommandModel);
                        var isEmpty = skillCM.skillConfigId <= 0;
                        if (isEmpty)
                        {
                            skillCM.skillConfigId = skillId;
                        }
                    }
                }
            }

            
            
            // foreach (var kv in this.inputCommandDic)
            // {
            //     var commandModel = kv.Value;
            //     if (commandModel is SkillCommandModel)
            //     {
            //         var skillCM = commandModel as SkillCommandModel;
            //
            //         if (skillCategory == SkillCategory.MinorSkill && 
            //             skillCM.commandType == PlayerCommandType.Skill_Minor)
            //         {
            //             skillCM.skillConfigId = skillId;
            //             return;
            //         }
            //         
            //         if (skillCategory == SkillCategory.UltimateSkill && 
            //             skillCM.commandType == PlayerCommandType.Skill_Ultimate)
            //         {
            //             skillCM.skillConfigId = skillId;
            //             return;
            //         }
            //
            //         if (skillCM.skillConfigId <= 0)
            //         {
            //             skillCM.skillConfigId = skillId;
            //             break;
            //         }
            //     }
            // }
        }

        // public bool TryToAddInputCommand(PlayerInputType inputType, PlayerInputCommandModel model)
        // {
        //     if (!this.inputCommandDic.ContainsKey(inputType))
        //     {
        //         AddInputCommand(inputType, model);
        //         return true;
        //     }
        //
        //     return false;
        // }

        // public SkillCommandModel GetInputCommand(PlayerCommandType commandType)
        // {
        //     
        // }

        public void UpdateSkillInput()
        {
        }
    }

    public class PlayerInputCommandModel
    {
        // public PlayerInputType inputType;
        public PlayerCommandType commandType;
    }

    public class SkillCommandModel : PlayerInputCommandModel
    {
        public int skillConfigId;
    }
}