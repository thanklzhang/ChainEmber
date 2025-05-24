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
        Dictionary<int, Skill> skillDic = new Dictionary<int, Skill>();

        private List<Skill> wilDeleteSkillList = new List<Skill>();

        private List<SkillInit> skillInitList;

        void InitSkill(List<SkillInit> skillInitList)
        {
            this.skillDic = new Dictionary<int, Skill>();

            this.skillInitList = skillInitList;
        }

        void AddInitSkill()
        {
            if (this.playerIndex >= 0)
            {
                //玩家控制的实体:
                var index = 0;
                foreach (var initSkill in skillInitList)
                {
                    this.AddSkill(new CreateSkillBean()
                    {
                        releaser = this,
                        configId = initSkill.configId,
                    });
                }
            }
            else
            {
                //非玩家控制实体 从表里取
                var index = 0;
                //普通攻击 和 小招技能
                foreach (var skillId in infoConfig.SkillIds)
                {
                    this.AddSkill(new CreateSkillBean()
                    {
                        releaser = this,
                        configId = skillId
                    });
                }

                //大招
                if (infoConfig.UltimateSkillId > 0)
                {
                    this.AddSkill(new CreateSkillBean()
                    {
                        releaser = this,
                        configId = infoConfig.UltimateSkillId
                    });
                }
            }
        }

        public void SetSkillInitinfo()
        {
            AddInitSkill();

            // foreach (var kv in skillDic)
            // {
            //     var skill = kv.Value;
            //     skill.TriggerSkillEffectOnInit();
            // }
        }

        public void SetAISkillInfo()
        {
            this.ai.SetAISkillInfo();
        }

        //包括普通攻击
        private int maxSkillCount = 5;

        private int maxLeaderSkillCount = 2;

        public bool IsLeaderSkillFull()
        {
            int count = GetLeaderSkillCount();
            return count >= maxLeaderSkillCount;
        }

        public int GetLeaderSkillCount()
        {
            int count = 0;
            foreach (var kv in this.skillDic)
            {
                var skill = kv.Value;
                if ((SkillCategory)skill.infoConfig.SkillCategory
                    == SkillCategory.LeaderSkill)
                {
                    count += 1;
                }
            }

            return count;
        }

        public ResultCode AddSkill(CreateSkillBean createSkillBean)
        {
            var incomeConfigId = createSkillBean.configId;
            var incomeConfig = BattleConfigManager.Instance.GetById<ISkill>(incomeConfigId);
            var incomeGroupId = SkillUpgradeConfigHelper.GetSkillGroupId(incomeConfigId);

            // var existConfigId = 0;
            //找到同组的技能
            Skill existSkill = null;
            foreach (var kv in this.skillDic)
            {
                var configId = kv.Key;
                var groupId = SkillUpgradeConfigHelper.GetSkillGroupId(configId);
                if (groupId == incomeGroupId)
                {
                    existSkill = kv.Value;
                    break;
                }
            }

            if (null == existSkill)
            {
                // //添加新技能
                // if (this.skillDic.Count >= maxSkillCount)
                // {
                //     return new ResultCode { type = ResultCodeType.AddSkillFull, intArg0 = createSkillBean.configId };
                // }

                //判断队长技能数量
                if ((SkillCategory)incomeConfig.SkillCategory == SkillCategory.LeaderSkill)
                {
                    if (IsLeaderSkillFull())
                    {
                        willAddSkillBean = new CreateSkillBean();
                        willAddSkillBean.configId = incomeConfigId;
                        willAddSkillBean.releaser = this;

                        return new ResultCode
                            { type = ResultCodeType.AddLeaderSkillFull, intArg0 = createSkillBean.configId };
                    }
                }

                // createSkillBean.showIndex = showIndex;
                Skill skill = new Skill();
                skill.Init(createSkillBean);
                this.skillDic.Add(createSkillBean.configId, skill);
                battle.OnSkillInfoUpdate(skill);
            }
            else
            {
                //技能增加等级经验
                var skill = existSkill;
                var decomposeExp = SkillUpgradeConfigHelper.GetDecomposeExp(incomeConfig.Level);
                AddExpToSkill(skill.infoConfig.Id, decomposeExp);
            }

            return ResultCode.Success;
        }

        //将要获得的技能 等待玩家替换
        private CreateSkillBean willAddSkillBean;

        public void SelectReplaceSkill(int skillId)
        {
            if (null == willAddSkillBean)
            {
                BattleLog.LogError("the willAddSkillBean is null , please check the process");
                return;
            }

            if (skillId > 0)
            {
                //替换技能
                var preSkill = FindSkillByConfigId(skillId);
                RemoveSkill(preSkill);

                Skill skill = new Skill();
                skill.Init(willAddSkillBean);
                this.skillDic.Add(willAddSkillBean.configId, skill);
                battle.OnSkillInfoUpdate(skill);
            }
            else
            {
                //放弃
            }

            willAddSkillBean = null;
        }

        public Skill GetSkillByGroupId(int groupId)
        {
            foreach (var kv in this.skillDic)
            {
                var skill = kv.Value;
                var _groupId = SkillUpgradeConfigHelper.GetSkillGroupId(skill.configId);
                if (_groupId == groupId)
                {
                    return skill;
                }
            }

            return null;
        }

        public void AddExpToSkill(int skillConfigId, int exp)
        {
            var groupId = SkillUpgradeConfigHelper.GetSkillGroupId(skillConfigId);

            var skill = GetSkillByGroupId(groupId);

            if (null == skill || null == skill.infoConfig)
            {
                BattleLog.Log("");
            }

            var preLevel = skill.infoConfig.Level;

            var preExp = skill.currExp;

            var nowExp = preExp + exp;
            var nowLevel = SkillUpgradeConfigHelper.GetLevelByExp(nowExp);

            //根据现有技能和添加的技能 算出是否升级
            var isUpgrade = nowLevel > preLevel;
            if (!isUpgrade)
            {
                skill.SetExp(nowExp);
                battle.OnSkillInfoUpdate(skill);

                BattleLog.LogZxy("not upgrade , still pre level : " + preLevel);
            }
            else
            {
                var nextId = SkillUpgradeConfigHelper.GetUpgradeSkillConfigId(skill.configId);

                if (nextId <= 0)
                {
                    // Battle_Log.Log("perhaps  ： " + skill.configId);
                    BattleLog.LogZxy("the skill perhaps max level");
                    return;
                }

                var upSkillConfig = BattleConfigManager.Instance.GetById<ISkill>(
                    nextId);
                if (upSkillConfig != null)
                {
                    //技能升级（新的技能代替旧的技能）
                    skill.Upgrade();
                    //先移除以前的技能
                    RemoveSkill(skill);

                    //新技能
                    var createSkillBean = new CreateSkillBean();
                    createSkillBean.configId = nextId;
                    Skill upSkill = new Skill();

                    createSkillBean.releaser = this;
                    createSkillBean.showIndex = skill.showIndex;
                    upSkill.Init(createSkillBean);
                    upSkill.SetExp(nowExp);
                    this.skillDic.Add(createSkillBean.configId, upSkill);
                    battle.OnSkillInfoUpdate(upSkill);

                    BattleLog.LogZxy("upgrade ： now level : " + upSkill.infoConfig.Level);
                }
                else
                {
                    BattleLog.LogZxy("the upSkillConfig is null , please check the process ： " + skill.configId);
                }

                // //新技能
                // var createSkillBean = new CreateSkillBean();
                //
                // Skill upSkill = new Skill();
                // createSkillBean.configId =
                //     SkillUpgradeConfigHelper.GetUpgradeSkillConfigId(skill.configId);
                // var upSkillConfig = BattleConfigManager.Instance.GetById<ISkill>(
                //     createSkillBean.configId);
                //
                // if (upSkillConfig != null)
                // {
                //     createSkillBean.releaser = this;
                //     createSkillBean.showIndex = skill.showIndex;
                //     upSkill.Init(createSkillBean);
                //     upSkill.SetExp(nowExp);
                //     this.skillDic.Add(createSkillBean.configId, upSkill);
                //     battle.OnSkillInfoUpdate(upSkill);
                // }
            }
        }

        public void UpdateSkills(float deltaTime)
        {
            //更新技能
            foreach (var item in this.skillDic)
            {
                var skill = item.Value;
                skill.Update(deltaTime);
            }

            //处理将要删除的技能
            if (wilDeleteSkillList != null &&
                wilDeleteSkillList.Count > 0)
            {
                List<Skill> delSkills = new List<Skill>();
                for (int i = 0; i < wilDeleteSkillList.Count; i++)
                {
                    var delSkill = wilDeleteSkillList[i];
                    delSkill.Update(deltaTime);

                    if (delSkill.state == ReleaseSkillState.ReadyRelease
                        || delSkill.state == ReleaseSkillState.CD)
                    {
                        delSkills.Add(delSkill);
                    }
                }

                for (int i = 0; i < delSkills.Count; i++)
                {
                    var willDestroySkill = delSkills[i];
                    wilDeleteSkillList.Remove(willDestroySkill);
                }
            }
        }

        public Skill FindMinorSkill()
        {
            foreach (var kv in skillDic)
            {
                var skill = kv.Value;
                if ((SkillCategory)skill.infoConfig.SkillCategory == SkillCategory.MinorSkill)
                {
                    return skill;
                }
            }

            return null;
        }

        public List<Skill> FindLeaderSkills()
        {
            List<Skill> list = new List<Skill>();

            foreach (var kv in skillDic)
            {
                var skill = kv.Value;
                if ((SkillCategory)skill.infoConfig.SkillCategory ==
                    SkillCategory.LeaderSkill)
                {
                    list.Add(skill);
                }
            }

            return list;
        }

        public Skill FindUltimateSkill()
        {
            foreach (var kv in skillDic)
            {
                var skill = kv.Value;
                if ((SkillCategory)skill.infoConfig.SkillCategory ==
                    SkillCategory.UltimateSkill)
                {
                    return skill;
                }
            }

            return null;
        }

        public int GetUltimateSkillConfigId()
        {
            return this.infoConfig.UltimateSkillId;
        }
    }
}