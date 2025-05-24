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
        Dictionary<int, BuffEffect> buffDic = new Dictionary<int, BuffEffect>();
        Dictionary<int, PassiveEffect> passiveEffectDic = new Dictionary<int, PassiveEffect>();


        #region active skill

        //释放技能 带有是否释放成功判断
        public bool ReleaseSkill(int skillId, int targetGuid, Vector3 targetPos,Vector3 mousePos,
            bool isForce = false)
        {
            var skill = FindSkillByConfigId(skillId);
            bool isCanRelease = Skill.IsCanRelease(skill, targetGuid, targetPos);

            if (!isCanRelease)
            {
                return false;
            }
            SuccessToReleaseSkill(skill, targetGuid, targetPos,mousePos);

            return true;
        }

        //成功释放技能 (条件符合释放 开始技能)
        void SuccessToReleaseSkill(Skill skill, int targetGuid, Vector3 targetPos, Vector3 mousePos)
        {
            // _Battle_Log.Log(string.Format("{0} success release skill : {1}", this.infoConfig.Name,
            //     skill.infoConfig.Name));

            //瞬发技能不走技能前后摇逻辑
            bool isImmediatelyRelease = (SkillReleaseType)skill.infoConfig.SkillReleaseType ==
                                        SkillReleaseType.ImmediatelyRelease;
            if (!isImmediatelyRelease)
            {
                this.ChangeToIdle();

                this.EntityState = EntityState.UseSkill;
            }

            //设置朝向
            var releaseTargetType = (SkillReleaseTargetType)skill.infoConfig.SkillReleaseTargeType;
            if (releaseTargetType == SkillReleaseTargetType.Entity || releaseTargetType == SkillReleaseTargetType.Point)
            {
                var dir = this.dir;
                if (targetGuid > 0)
                {
                    var skillTargetEntity = this.battle.FindEntity(targetGuid);
                    if (skillTargetEntity != null)
                    {
                        dir = (skillTargetEntity.position - this.position).normalized;
                    }
                }
                else
                {
                    dir = (targetPos - this.position).normalized;
                }

                SetDir(dir);
            }

            var logStr = string.Format("{0}({1}) release  skill : {2}({3})", this.infoConfig.Name, this.guid,
                skill.infoConfig.Name, skill.infoConfig.Id);
            BattleLog.Log(logStr);

            //开始释放技能
            skill.Start(targetGuid, targetPos,mousePos);

            // this.operateModule.OnNodeExecuteFinish(skill.configId);

            battle.OnEntityReleaseSkill(this.guid, skill, targetGuid, targetPos);
        }


        // //增加技能
        // public void AddSkill(Skill skill)
        // {
        //     if (this.skillDic.ContainsKey(skill.configId))
        //     {
        //         return;
        //     }
        //
        //     skill.Init(this);
        //     this.skillDic.Add(skill.configId, skill);
        //
        //     battle.OnSkillInfoUpdate(skill);
        //
        //     // OnSyncSkills();
        // }

        public void RemoveSkill(Skill skill)
        {
            skill.SetWillDelete();
            skill.Remove();
            this.skillDic.Remove(skill.configId);
            wilDeleteSkillList.Add(skill);
            battle.OnSkillInfoUpdate(skill);
        }

        public void OnSyncSkills()
        {
            //battle.OnSyncSkills(this.guid, this.skillDic);
        }

        public bool IsCanReleaseSkill()
        {
            //判断是否释放者死亡
            if (this.IsDead())
            {
                return false;
            }

            //检查异常状态
            var checkAbnormal = IsSkillCanReleaseByAbnoraml();
            return checkAbnormal;
        }

        public bool IsSkillCanReleaseByAbnoraml()
        {
            var checkAbnormal = this.abnormalStateMgr.IsAbnormalForSkill();

            return !checkAbnormal;
        }

        //目前 configId 当作 key , 所以不要有一个英雄用相同的技能
        //目前用 group 概念
        public Skill FindSkillByConfigId(int configId)
        {
            var groupId = SkillUpgradeConfigHelper.GetSkillGroupId(configId);
            
            foreach (var kv in skillDic)
            {
                var _groupId = SkillUpgradeConfigHelper.GetSkillGroupId(kv.Value.configId);
                if (_groupId == groupId)
                {
                    return kv.Value;
                }
            }
            return null;
        }

        public Dictionary<int, Skill> GetAllSkills()
        {
            return this.skillDic;
        }

        public List<Skill> GetSkillList()
        {
            return this.skillDic.Select(s => s.Value).ToList();
        }

        public Skill GetNormalAttackSkill()
        {
            foreach (var item in this.skillDic)
            {
                var skill = item.Value;
                if (skill.isNormalAttack)
                {
                    return skill;
                }
            }

            return null;
        }

        public void BreakSkills()
        {
            foreach (var kv in skillDic)
            {
                var skill = kv.Value;
                skill.Break();
            }
        }

        #endregion

        #region buff

        //-------------------- buff
        public void AddBuff(BuffEffect buff)
        {
            if (!buffDic.ContainsKey(buff.guid))
            {
                this.buffDic.Add(buff.guid, buff);
                battle.OnEntityAddBuff(this.guid, buff);
            }
            else
            {
                BattleLog.LogZxy("entity : AddBuff : the guid is exist : " + buff.guid);
            }
        }

        public void ChangeBuffLayer(int buffConfigId, int changeValue)
        {
            foreach (var kv in buffDic)
            {
                var buff = kv.Value;
                if (buff.configId == buffConfigId)
                {
                    buff.AddLayer(changeValue);
                }
            }
        }

        public void RemoveBuff(BuffEffect buff)
        {
            if (buffDic.ContainsKey(buff.guid))
            {
                this.buffDic.Remove(buff.guid);
            }
            else
            {
                BattleLog.LogWarningZxy("entity : RemoveBuff : the guid is not exist : " + buff.guid);
            }
        }

        //只是移除引用
        public void RemoveBuffsByConfigId(List<int> configIds)
        {
            foreach (var configId in configIds)
            {
                this.RemoveBuffByConfigId(configId);
            }
        }

        //只是移除引用
        public void RemoveBuffByConfigId(int configId)
        {
            //性能待定
            foreach (var kv in buffDic)
            {
                var buff = kv.Value;
                if (buff.configId == configId)
                {
                    buffDic.Remove(configId);
                }
            }
        }


        internal Dictionary<int, BuffEffect> GetBuffs()
        {
            return this.buffDic;
        }

        //根据 config 获得 buff
        //isIncludeInvalid ： 是否包含失效的 buff
        internal BuffEffect GetBuffByConfigId(int effectConfigId, bool isIncludeInvalid = false)
        {
            foreach (var item in this.buffDic)
            {
                var buff = item.Value;
                if (!isIncludeInvalid && !buff.IsValid())
                {
                    //不包括失效的 buff
                    continue;
                }

                if (item.Value.configId == effectConfigId)
                {
                    return item.Value;
                }
            }

            return null;
        }

        #endregion

        #region 被动技能

        public void AddPassiveEffect(PassiveEffect passiveEffect)
        {
            if (!passiveEffectDic.ContainsKey(passiveEffect.guid))
            {
                this.passiveEffectDic.Add(passiveEffect.guid, passiveEffect);
                //battle.OnEntityAddBuff(this.guid, buff);
            }
            else
            {
                BattleLog.LogWarning("entity : AddPassiveEffect : the guid is exist : " + passiveEffect.guid);
            }
        }

        //移除被动技能引用（只是删除引用 注意）
        public void RemovePassiveEffect(PassiveEffect passiveEffect)
        {
            if (passiveEffectDic.ContainsKey(passiveEffect.guid))
            {
                this.passiveEffectDic.Remove(passiveEffect.guid);
            }
            else
            {
                //Battle_Log.LogWarning("entity : RemovePassiveEffect : the guid is not exist : " + passiveEffect.guid);
            }
        }

        public PassiveEffect FindPassiveSkillByConfigId(int configId)
        {
            foreach (var item in this.passiveEffectDic)
            {
                if (configId == item.Value.configId)
                {
                    return item.Value;
                }
            }

            return null;
        }

        //强制移除被动技能(走整个流程)
        public void ForceRemovePassiveEffect(int configId)
        {
            var eft = FindPassiveSkillByConfigId(configId);
            if (eft != null)
            {
                eft.ForceDelete();
            }
        }

        // //目前只用 configId 进行删除 ， 如果有需要可以改成
        // //创建的时候存 effect 的 guid ， 然后删除的时候按照索引删除等
        // //看需求
        // internal void DeletePassiveSkill(int configId)
        // {
        //     var passiveSkill = FindPassiveSkillByConfigId(configId);
        //     if (passiveSkill != null)
        //     {
        //         passiveSkill.ForceDelete();
        //     }
        // }

        #endregion
    }
}