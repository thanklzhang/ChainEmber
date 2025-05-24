using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Battle
{
    //区域点位
    public class EntityLocation
    {
        // public BattleEntity battleEntity;
        public int entityGuid;
    }

    public partial class BattlePlayer
    {
        //最大队伍成员数目（包括自己）
        public int maxTeamMemberCount
        {
            get { return this.GetCurrency(BattleCurrency.PopulationId).count; }
        }

        private int willAddMemberConfigId;

        //由引用关系的初始化
        public void InitMembers_Relation()
        {
            SyncTeamMemberRelationInfo();
        }

        public void AddPopulation(int count = 1)
        {
            this.GainCurrency(BattleCurrency.PopulationId, count, true);
        }

        //人口是否满了
        public bool IsPopulationFull()
        {
            //+1 是加上玩家自己
            return entity.GetTeamMemberDic().Count + 1 >= maxTeamMemberCount;
        }

        //得到替补空位
        public EntityLocation GetEmptyUnderstudyLocation()
        {
            return null;
        }

        public List<BattleEntity> GetAllMemberAndSelf()
        {
            var list = new List<BattleEntity>();

            if (null == this.entity)
            {
                return new List<BattleEntity>();
            }

            var dic = this.entity.GetTeamMemberDic();
            foreach (var kv in dic)
            {
                var entity = kv.Value;
                list.Add(entity);
            }

            list.Add(this.entity);
            return list;
        }

        public ResultCode TryToAddTeamMember(int configId, int level = 1, int star = 1)
        {
            var entityConfigId = configId;
            BattleLog.LogZxy("zxy : add member : entityConfigId : " + entityConfigId);
            var allEntity = GetAllMemberAndSelf();
            //判断自己和队伍是否有相同实体
            var findEntity = allEntity.Find(e => e.configId == entityConfigId);
            bool isHaveSameConfigId = findEntity != null;
            if (isHaveSameConfigId)
            {
                //判断升星操作

                //根据 entityConfigId 对应的星级分解成经验
                var entityConfig = BattleConfigManager.Instance.GetById<IEntityInfo>(entityConfigId);
                var decomposeStarExp = EntityUpgradeConfigHelper.GetDecomposeStarExp(star);

                // BattleEntity findEntity = null;
                findEntity.AddStarExp(decomposeStarExp);
            }
            else
            {
                //TODO 可能不会满 因为最多可能就 5 种类型（不算自己）
                //判断战场上是否有空位（人口是否还没满）
                bool isFullPopulation = IsPopulationFull();
                if (isFullPopulation)
                {
                    //人口满了 需要用户选择替换哪个英雄
                    willAddMemberConfigId = entityConfigId;
                    //TODO:发给客户端一个选择替换协议消息

                    return new ResultCode
                        { type = ResultCodeType.AddTeamMemberFull, intArg0 = willAddMemberConfigId };
                }
                else
                {
                    AddTeamMember(configId, level, star);
                }

                SyncTeamMemberRelationInfo();
            }

            return ResultCode.Success;
        }

        public void SyncTeamMemberRelationInfo()
        {
            //增加或者删除队友 ， 所以同步队长和队友关系信息
            var entities = GetAllMemberAndSelf();
            var guids = entities.Select(e => e.guid).ToList();
            this.battle.PlayerMsgSender.UpdatePlayerTeamMembersInfo(this.playerIndex, guids);
        }

        public void SelectReplaceHero(int beReplaceConfigId)
        {
            if (beReplaceConfigId > 0)
            {
                //玩家选择了替换的英雄
                if (willAddMemberConfigId > 0)
                {
                    ForceKillTeamMember(beReplaceConfigId);

                    // TryToAddTeamMember(willAddMemberConfigId);

                    AddTeamMember(willAddMemberConfigId);

                    SyncTeamMemberRelationInfo();

                    willAddMemberConfigId = -1;
                }
            }
            else
            {
                //玩家选择了放弃替换
                willAddMemberConfigId = -1;
            }
        }

        private void ForceKillTeamMember(int configId)
        {
            BattleLog.LogZxy("ForceKillTeamMember : configId : " + configId);

            this.entity.ForceRemoveTeamMember(configId, DamageFromType.TeamMemberOverLimitCount);
        }

        //布阵：移动某个成员
        public void MoveMemberByArraying(OperateHeroArg opHeroArg)
        {
            var srcGuid = opHeroArg.opHeroGuid;
            var targetPos = opHeroArg.targetPos;
            var understudyIndex = opHeroArg.toUnderstudyIndex;
            bool isToUnderstudy = understudyIndex > 0;


            BattleLog.LogZxy(
                $"backend : recv msg : srcGuid:{srcGuid} targetPos:{targetPos} understudyIndex:{understudyIndex}");

            var srcEntity = battle.FindEntity(srcGuid);


            if (srcEntity.playerIndex != this.playerIndex)
            {
                return;
            }

            if (this.battle.battleProcess.CurrWave.State != BattleProcessWave.BattleWaveState.Ready)
            {
                return;
            }

            srcEntity.SetPosition(targetPos);
            //发送源单位消息：战场 -> 战场
            this.battle.PlayerMsgSender.NotifyHeroOperationByArraying(this.playerIndex, srcGuid, targetPos, -1);
        }

        // 将所有适用于新队友的全队效果应用到新队友上
        private void ApplyNewTeamMemberEffects(BattleEntity newTeamMember)
        {
            if (newTeamMember == null || battleRewardList == null || battleRewardList.Count <= 0)
                return;

            foreach (var reward in battleRewardList)
            {
                // 检查该奖励是否需要应用到新队友上
                // if (!reward.applyToNewTeamMembers)
                //     continue;

                var effects = reward.rewardEffectList;


                if (effects == null || effects.Count <= 0)
                    continue;
                foreach (var effect in effects)
                {
                    if (effect.applyToNewTeamMembers)
                    {
                        //目前应该获得队友的时候都是准备阶段
                        if (effect.IsCanGainEffect(BattleRewardEffectGainTimingType.OnReadyProcessStart))
                        {
                            effect.ApplyToNewTeamMembers(newTeamMember);
                        }

                        //TODO 按照不同的 rewardEffect 进行拓展
                        // // 处理全队属性效果
                        // if (effect is BattleReward_TeamMember_AllTeammateAddRandAttr allTeammateAttrReward)
                        // {
                        //     // 直接给新队友应用相同的属性
                        //     newTeamMember.AddAttrs(EntityAttrGroupType.BattleReward, allTeammateAttrReward.attrs);
                        //     Battle_Log.LogZxy($"应用全队属性效果到新队友: {newTeamMember.guid}, 属性组ID: {allTeammateAttrReward.attrGroupId}");
                        // }
                        // // 处理全队buff效果
                        // else if (effect is BattleReward_TeamMember_AllAddBuff allAddBuffReward)
                        // {
                        //     // 为新队友添加相同的buff
                        //     SkillEffectContext context = new SkillEffectContext();
                        //     context.battle = this.battle;
                        //     context.fromSkill = null;
                        //     context.selectEntities = new List<BattleEntity> { newTeamMember };
                        //     context.selectPositions = new List<Vector3>();
                        //
                        //     battle.AddSkillEffectGroup(new List<int>() { allAddBuffReward.buffConfigId }, context);
                        //     Battle_Log.LogZxy($"应用全队buff效果到新队友: {newTeamMember.guid}, BuffID: {allAddBuffReward.buffConfigId}");
                        // }
                        // // 如果有其他类型的队友效果，也可以在这里添加处理
                    }
                }
            }
        }

        private BattleEntity AddTeamMember(int configId, int level = 1, int star = 1)
        {
            var memberConfigId = configId;
            var posList = battle.GetAroundEmptyPos(this.entity, 1);

            if (posList.Count <= 0)
            {
                BattleLog.LogZxy("BattlePlayer : AddTeamMember : the count of posList is 0");
                return null;
            }

            if (entity.GetTeamMemberDic().Count + 1 > maxTeamMemberCount)
            {
                BattleLog.LogZxy("BattlePlayer : AddTeamMember : reach the max : " + maxTeamMemberCount);
                return null;
            }

            List<EntityInit> entityInitList = new List<EntityInit>();

            var pos = posList[0];
            var entityConfig = BattleConfigManager.Instance.GetById<IEntityInfo>(memberConfigId);

            var dir = (pos - this.entity.position).normalized;
            EntityInit entityInit = new EntityInit()
            {
                configId = memberConfigId,
                playerIndex = this.entity.playerIndex,
                position = pos,
                isPlayerCtrl = false,
                level = level,
                dir = dir,
                roleType = BattleEntityRoleType.TeamMember,
                teamLeader = this.entity,
                star = star,
            };

            entityInit.skillInitList = new List<SkillInit>();

            foreach (var skillId in entityConfig.SkillIds)
            {
                SkillInit skill = new SkillInit()
                {
                    configId = skillId,
                    level = 1
                };
                entityInit.skillInitList.Add(skill);
            }
            
            if (entityConfig.UltimateSkillId > 0)
            {
                SkillInit ultimateSkill = new SkillInit()
                {
                    configId = entityConfig.UltimateSkillId,
                    level = 1
                };
                entityInit.skillInitList.Add(ultimateSkill);
            }

            entityInitList.Add(entityInit);

            var entityList = battle.CreateEntities(entityInitList);
            for (int i = 0; i < entityList.Count; i++)
            {
                var entity = entityList[i];
                this.entity.AddTeamMemberEntity(entity);
            }

            if (entityList.Count > 0)
            {
                var entity = entityList[0];
                this.battle.PlayerMsgSender.NotifyHeroOperationByArraying(this.playerIndex, entity.guid,
                    entity.position, -1);

                // 为新队友应用所有全队效果
                ApplyNewTeamMemberEffects(entity);

                return entityList[0];
            }

            return null;
        }
    }
}